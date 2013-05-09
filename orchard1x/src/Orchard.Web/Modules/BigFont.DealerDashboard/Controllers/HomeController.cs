using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Core.Contents.Settings;
using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc.Extensions;
using Orchard.Mvc.Html;
using Orchard.UI.Navigation;
using Orchard.UI.Notify;
using Orchard.Settings;
using Orchard.Utility.Extensions;
// These are namespaces that we have to resolve here
// but that we do not need to resolve if we are already in Orchard.Core.Content.Controllers
using Orchard;
using Orchard.Mvc.Routes;
using Orchard.Themes;
using BigFont.DealerDashboard.ViewModels;

namespace BigFont.DealerDashboard.Controllers
{
    [Themed]
    [ValidateInput(false)]
    public class HomeController : Controller, IUpdateModel
    {
        private readonly ISiteService _siteService;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITransactionManager _transactionManager;

        dynamic Shape { get; set; }
        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }

        public HomeController(
            ISiteService siteService,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            ITransactionManager transactionManager,
            IOrchardServices orchardServices,
            IShapeFactory shapeFactory)
        {
            _siteService = siteService;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _transactionManager = transactionManager;
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

            GetOwnedContentItems(query);

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
            list.AddRange(pageOfContentItems.Select(ci => 
                _contentManager.BuildDisplay(ci, "SummaryAdmin")));

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
        [HttpPost, ActionName("Create")]
        [FormValueRequired("submit.Save")]
        public ActionResult CreatePOST(string id, string returnUrl)
        {
            return CreatePOST(id, returnUrl, contentItem =>
            {
                if (!contentItem.Has<IPublishingControlAspect>() && !contentItem.TypeDefinition.Settings.GetModel<ContentTypeSettings>().Draftable)
                    _contentManager.Publish(contentItem);
            });
        }
        private ActionResult CreatePOST(string id, string returnUrl, Action<ContentItem> conditionallyPublish)
        {
            var contentItem = _contentManager.New(id);
            
            if (!Services.Authorizer.Authorize(Permissions.EditContent, contentItem, T("Couldn't create content")))
                return new HttpUnauthorizedResult();

            _contentManager.Create(contentItem, VersionOptions.Draft);

            dynamic model = _contentManager.UpdateEditor(contentItem, this);
            if (!ModelState.IsValid)
            {
                _transactionManager.Cancel();
                // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
                return View((object)model);
            }

            conditionallyPublish(contentItem);

            Services.Notifier.Information(string.IsNullOrWhiteSpace(contentItem.TypeDefinition.DisplayName)
                ? T("Your content has been created.")
                : T("Your {0} has been created.", contentItem.TypeDefinition.DisplayName));
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return this.RedirectLocal(returnUrl);
            }
           
            //var adminRouteValues = _contentManager.GetItemMetadata(contentItem).AdminRouteValues;
            //return RedirectToRoute(adminRouteValues);

            var redirectRoute = new Routes().GetRoutes().First<RouteDescriptor>(rd => rd.Name.Equals("DealerDashboard"));
            return RedirectToRoute(redirectRoute);
        }
        public ActionResult Edit(int id)
        {
            var contentItem = _contentManager.Get(id, VersionOptions.Latest);

            if (contentItem == null)
                return HttpNotFound();

            if (!Services.Authorizer.Authorize(Permissions.EditContent, contentItem, T("Cannot edit content")))
                return new HttpUnauthorizedResult();

            dynamic model = _contentManager.BuildEditor(contentItem);
            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View((object)model);
        }
        private ActionResult CreatableTypeList(int? containerId)
        {
            dynamic viewModel = Shape.ViewModel(ContentTypes: GetDealerDashboardTypes(containerId.HasValue), ContainerId: containerId);

            // Casting to avoid invalid (under medium trust) reflection over the protected View method and force a static invocation.
            return View("CreatableTypeList", (object)viewModel);
        }
        private IEnumerable<ContentTypeDefinition> GetDealerDashboardTypes(bool andContainable)
        {
            IEnumerable<ContentTypeDefinition> creatableTypes = GetCreatableTypes(andContainable);
            IEnumerable<ContentTypeDefinition> dealerDashboardTypes = creatableTypes.Where(ctd => ctd.Name.Contains("Crane"));
            return dealerDashboardTypes;
        }
        private IEnumerable<ContentTypeDefinition> GetCreatableTypes(bool andContainable)
        {
            return _contentDefinitionManager.ListTypeDefinitions().Where(ctd => 
                ctd.Settings.GetModel<ContentTypeSettings>().Creatable && 
                (!andContainable || ctd.Parts.Any(p => p.PartDefinition.Name == "ContainablePart")) &&
                ctd.Name.Contains("Crane"));
        }
        private IContentQuery<ContentItem> GetOwnedContentItems(IContentQuery<ContentItem> query)
        {
            // limit the content items to those that the current user owns
            Orchard.Security.IUser currentUser = Services.WorkContext.CurrentUser;
            query = query.Where<CommonPartRecord>(cpr => cpr.OwnerId == currentUser.Id);
            return query;
        }
        bool IUpdateModel.TryUpdateModel<TModel>(TModel model, string prefix, string[] includeProperties, string[] excludeProperties)
        {
            return TryUpdateModel(model, prefix, includeProperties, excludeProperties);
        }
        void IUpdateModel.AddModelError(string key, LocalizedString errorMessage)
        {
            ModelState.AddModelError(key, errorMessage.ToString());
        }
    }

    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        private readonly string _submitButtonName;

        public FormValueRequiredAttribute(string submitButtonName)
        {
            _submitButtonName = submitButtonName;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            var value = controllerContext.HttpContext.Request.Form[_submitButtonName];
            return !string.IsNullOrEmpty(value);
        }
    }
}