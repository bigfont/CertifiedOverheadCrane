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
using BigFont.DealerDashboard.Models;
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

namespace BigFont.DealerDashboard.Commands
{
    public class DealerDashboardCommands : DefaultOrchardCommandHandler
    {
        private readonly ISiteService _siteService;
        private readonly IContentManager _contentManager;
        private readonly IContentDefinitionManager _contentDefinitionManager;
        private readonly ITransactionManager _transactionManager;
        private readonly IOrchardServices _orchardServices;
        private readonly IShapeFactory _shapeFactory;
        private readonly IMembershipService _membershipService;

        public DealerDashboardCommands(
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

        [CommandName("dealerdashboard create")]
        public void Create()
        {
            // get a previously created user
            var owner = _membershipService.GetUser("dealer1");
            // make sure the user is not null
            if (owner == null)
            {
                Context.Output.WriteLine(T("Invalid username: {0}", owner));
                return;
            }
            // create a schwack of DealerProducts
            for (int i = 0; i < 100; ++i)
            {
                // make a new DealerProduct
                var dealerProduct = _contentManager.New("DealerProduct");
                // populate the owner
                dealerProduct.As<ICommonPart>().Owner = owner;
                // populate the title
                dealerProduct.As<TitlePart>().Title = "Test" + i.ToString();                
                // create the new DealerProduct
                _contentManager.Create(dealerProduct);
            }
            Context.Output.WriteLine(T("DealerProduct created successfully"));
        }
    }
}