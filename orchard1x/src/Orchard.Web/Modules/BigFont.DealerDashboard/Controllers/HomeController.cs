using BigFont.DealerDashboard.ViewModels;
using Orchard.ContentManagement;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Models;
using Orchard.Core.Contents.Settings;
using Orchard.DisplayManagement;
using Orchard.Settings;
using Orchard.Themes;
using Orchard.UI.Navigation;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Orchard;
using Orchard.Core.Contents;
using Orchard.Localization;
using Orchard.Core.Containers.Models;

namespace BigFont.DealerDashboard.Controllers
{
    [Themed]
    public class HomeController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        dynamic Shape { get; set; }
        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }

        public HomeController(
            ISiteService siteService,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            IOrchardServices orchardServices,
            IShapeFactory shapeFactory)
        {
            _siteService = siteService;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            Services = orchardServices;
            Shape = shapeFactory;
            T = NullLocalizer.Instance;
        }
        public ActionResult Index()
        {
            return View("HelloWorld");
        }
        public ActionResult List(ListContentsViewModel model, PagerParameters pagerParameters)
        {
            Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var query = _contentManager.Query(VersionOptions.Latest, GetCreatableTypes(false).Select(ctd => ctd.Name).ToArray());

            #region TODO Turn this code block into its own method
            GetOwnedContentItems(query);
            #endregion

            if (!string.IsNullOrEmpty(model.TypeName))
            {
                var contentTypeDefinition = _contentDefinitionManager.GetTypeDefinition(model.TypeName);
                if (contentTypeDefinition == null)
                    return HttpNotFound();

                model.TypeDisplayName = !string.IsNullOrWhiteSpace(contentTypeDefinition.DisplayName)
                                            ? contentTypeDefinition.DisplayName
                                            : contentTypeDefinition.Name;
                query = query.ForType(model.TypeName);
            }

            switch (model.Options.OrderBy)
            {
                case ContentsOrder.Modified:
                    //query = query.OrderByDescending<ContentPartRecord, int>(ci => ci.ContentItemRecord.Versions.Single(civr => civr.Latest).Id);
                    query = query.OrderByDescending<CommonPartRecord>(cr => cr.ModifiedUtc);
                    break;
                case ContentsOrder.Published:
                    query = query.OrderByDescending<CommonPartRecord>(cr => cr.PublishedUtc);
                    break;
                case ContentsOrder.Created:
                    //query = query.OrderByDescending<ContentPartRecord, int>(ci => ci.Id);
                    query = query.OrderByDescending<CommonPartRecord>(cr => cr.CreatedUtc);
                    break;
            }

            model.Options.SelectedFilter = model.TypeName;
            model.Options.FilterOptions = GetCreatableTypes(false)
                .Select(ctd => new KeyValuePair<string, string>(ctd.Name, ctd.DisplayName))
                .ToList().OrderBy(kvp => kvp.Value);

            var pagerShape = Shape.Pager(pager).TotalItemCount(query.Count());
            var pageOfContentItems = query.Slice(pager.GetStartIndex(), pager.PageSize).ToList();

            var list = Shape.List();
            list.AddRange(pageOfContentItems.Select(ci => _contentManager.BuildDisplay(ci, "SummaryAdmin")));

            dynamic viewModel = Shape.ViewModel()
                .ContentItems(list)
                .Pager(pagerShape)
                .Options(model.Options)
                .TypeDisplayName(model.TypeDisplayName ?? "");

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)viewModel);
        }
        public ActionResult Create(string id, int? containerId)
        {
            if (string.IsNullOrEmpty(id))
                return CreatableTypeList(containerId);

            var contentItem = _contentManager.New(id);

            if (!Services.Authorizer.Authorize(Permissions.EditContent, contentItem, T("Cannot create content")))
                return new HttpUnauthorizedResult();

            if (containerId.HasValue && contentItem.Is<ContainablePart>())
            {
                var common = contentItem.As<CommonPart>();
                if (common != null)
                {
                    common.Container = _contentManager.Get(containerId.Value);
                }
            }

            dynamic model = _contentManager.BuildEditor(contentItem);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }
        private IEnumerable<ContentTypeDefinition> GetCreatableTypes(bool andContainable)
        {
            return _contentDefinitionManager.ListTypeDefinitions().Where(ctd => ctd.Settings.GetModel<ContentTypeSettings>().Creatable && (!andContainable || ctd.Parts.Any(p => p.PartDefinition.Name == "ContainablePart")));
        }
        private ActionResult CreatableTypeList(int? containerId)
        {
            dynamic viewModel = Shape.ViewModel(ContentTypes: GetCreatableTypes(containerId.HasValue), ContainerId: containerId);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View("CreatableTypeList", (object)viewModel);
        }
        private IContentQuery<ContentItem> GetOwnedContentItems(IContentQuery<ContentItem> query)
        {
            // limit the content items to those that the current user owns
            Orchard.Security.IUser currentUser = Services.WorkContext.CurrentUser;
            query = query.Where<CommonPartRecord>(cpr => cpr.OwnerId == currentUser.Id);
            return query;
        }
    }
}