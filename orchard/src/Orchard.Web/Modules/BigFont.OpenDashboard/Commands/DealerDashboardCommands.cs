using System;
using System.Linq;
using System.Xml.Linq;
using Orchard.Commands;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Common.Models;
using Orchard.Core.Navigation.Models;
using Orchard.Security;
using Orchard.Core.Navigation.Services;
using Orchard.Settings;
using Orchard.Core.Title.Models;
using Orchard.UI.Navigation;
using Contrib.ImageField.Fields;
using BigFont.OpenDashboard.Models;
using Contrib.ImageField.Settings;
using Orchard.Core.Common.Fields;
using Orchard.Core.Common.ViewModels;
using Orchard.Core.Common.Settings;
using System.Web.Mvc;
using Orchard.ContentManagement.MetaData;
using Orchard.Data;
using Orchard;
using Orchard.DisplayManagement;
using Orchard.Localization;
using Orchard.Environment.Extensions;

namespace BigFont.OpenDashboard.Commands
{
    [OrchardFeature("BigFont.OpenDashboard.OpenTypes")]
    public class OpenDashboardCommands : DefaultOrchardCommandHandler
    {
        private readonly ISiteService _siteService;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITransactionManager _transactionManager;
        private readonly IOrchardServices _orchardServices;
        private readonly IShapeFactory _shapeFactory;
        private readonly IMembershipService _membershipService;

        public OpenDashboardCommands(
            ISiteService siteService,
            IContentManager contentManager,
            IContentDefinitionManager contentDefinitionManager,
            ITransactionManager transactionManager,
            IOrchardServices orchardServices,
            IShapeFactory shapeFactory,
            IMembershipService membershipService)
        {
            _siteService = siteService;
            _contentManager = contentManager;
            _contentDefinitionManager = contentDefinitionManager;
            _transactionManager = transactionManager;
            _orchardServices = orchardServices;
            _shapeFactory = shapeFactory;
            _membershipService = membershipService;
        }

        [OrchardSwitch]
        public string Owner { get; set; }

        [OrchardSwitch]
        public int Amount { get; set; }

        [CommandName("openDashboard populate")]
        [CommandHelp("openDashboard populate [/Owner:<username>] [/Amount:<integer>] \r\n\t" + "Creates specified Amount (default is 30) of content items for the Owner (default is admin)")]
        [OrchardSwitches("Owner,Amount")]
        public void Create()
        {
            if (String.IsNullOrEmpty(Owner)) {
                Owner = _siteService.GetSiteSettings().SuperUser;
            }
            var owner = _membershipService.GetUser(Owner);

            if (owner == null) {
                Context.Output.WriteLine(T("Invalid username: {0}", Owner));
                return;
            }

            var amount = Amount > 0 ? Amount : 30;

            // create a schwack of DemoProduct
            for (int i = 0; i < amount; ++i)
            {
                // make a new DealerProduct
                var demoContent = _contentManager.New("DemoProduct");

                // populate the owner
                demoContent.As<ICommonPart>().Owner = owner;

                // populate the title
                demoContent.As<TitlePart>().Title = "My Demo Product " + i.ToString();

                // try to populate a simple text field - doesn't work
                //DealerProductPart dealerProductPart =
                //    dealerProduct.As<DealerProductPart>();
                //TextField partNum =
                //    (TextField)dealerProductPart.Get(typeof(TextField), "Part #");
                //partNum.Value = "123";

                // create the new DealerProduct
                _contentManager.Create(demoContent);
            }
            Context.Output.WriteLine(T("{0} DemoProduct created successfully for owner {1}", amount.ToString(), owner.UserName));
        }
    }
}