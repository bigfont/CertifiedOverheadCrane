using System.Web.Mvc;
using System.Linq;
using Orchard.Themes;
using Orchard.UI.Navigation;
using BigFont.DealerDashboard.ViewModels;
using Orchard.Settings;
using Orchard.ContentManagement;
using System.Collections.Generic;
using Orchard.DisplayManagement.Shapes;
using Orchard.ContentManagement.MetaData.Models;
using Orchard.ContentManagement.MetaData;

using System;
using System.Reflection;
using System.Web.Routing;
using Orchard.ContentManagement.Aspects;

using Orchard.Core.Common.Models;
using Orchard.Core.Containers.Models;
using Orchard.Core.Contents.Settings;

using Orchard.Data;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Logging;
using Orchard.Mvc.Extensions;
using Orchard.Mvc.Html;
using Orchard.UI.Notify;
using Orchard.Utility.Extensions;

namespace BigFont.DealerDashboard.Controllers
{
    [Themed]
    public class HomeController : Controller
    {
        private readonly ISiteService _siteService;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;

        dynamic Shape { get; set; }

        public HomeController(
            ISiteService siteService,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            IShapeFactory shapeFactory)
        {
            _siteService = siteService;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            Shape = shapeFactory;

        }
        public ActionResult Index()
        {
            return View("HelloWorld");
        }
        public ActionResult List(ListContentsViewModel model, PagerParameters pagerParameters)
        {
            Pager pager = new Pager(_siteService.GetSiteSettings(), pagerParameters);

            var query = _contentManager.Query(VersionOptions.Latest, GetCreatableTypes(false).Select(ctd => ctd.Name).ToArray());

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
        private IEnumerable<ContentTypeDefinition> GetCreatableTypes(bool andContainable)
        {
            return _contentDefinitionManager.ListTypeDefinitions().Where(ctd => ctd.Settings.GetModel<ContentTypeSettings>().Creatable && (!andContainable || ctd.Parts.Any(p => p.PartDefinition.Name == "ContainablePart")));
        }
    }
}