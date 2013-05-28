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
using Orchard.Core.Settings.Metadata;

namespace BigFont.OpenDashboard.Commands {
    public class OpenDashboardCommands : DefaultOrchardCommandHandler {
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
            IMembershipService membershipService) {
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
        public void Create() {

            IUser owner = null;
            int amount = 0;
            ContentItem contentItem = null;

            // instantiate the owner of the product
            if (String.IsNullOrEmpty(Owner)) {
                Owner = _siteService.GetSiteSettings().SuperUser;
            }
            owner = _membershipService.GetUser(Owner);
            if (owner == null) {
                Context.Output.WriteLine(T("Invalid username: {0}", Owner));
                return;
            }            

            // determine the amount of products to create
            amount = Amount > 0 ? Amount : 30;

            // create that amount of products
            for (int i = 0; i < amount; ++i) {
                // make a new DemoProduct
                contentItem = _contentManager.New("Product");

                // get the owner, title, and body
                ICommonPart commonPart = contentItem.As<ICommonPart>();
                TitlePart titlePart = contentItem.As<TitlePart>();
                BodyPart bodyPart = contentItem.As<BodyPart>();                

                // populate the owner, title, and body
                commonPart.Owner = owner;
                titlePart.Title = string.Format("Test Product {0} for {1} at {2}", i.ToString(), owner.UserName, DateTime.Now);
                bodyPart.Text = "This is some test body text.";

                // TODO Populate fields here, though this seems non-trivial for now
                // so we are doing it with T-SQL for now.

                // create the new DealerProduct
                _contentManager.Create(contentItem);
            }
            Context.Output.WriteLine(T("{0} Products created successfully for owner {1}", amount.ToString(), owner.UserName));
        }
    }
}