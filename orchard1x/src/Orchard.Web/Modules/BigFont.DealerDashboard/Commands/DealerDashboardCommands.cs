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

namespace BigFont.DealerDashboard.Commands
{
    public class DealerDashboardCommands : DefaultOrchardCommandHandler
    {
        private readonly IContentManager _contentManager;
        private readonly IMembershipService _membershipService;

        public DealerDashboardCommands(
            IContentManager contentManager,
            IMembershipService membershipService)
        {
            _contentManager = contentManager;
            _membershipService = membershipService;
        }

        [CommandName("dealerdashboard create")]
        public void Create()
        {

            var owner = _membershipService.GetUser("dealer1");

            if (owner == null)
            {
                Context.Output.WriteLine(T("Invalid username: {0}", owner));
                return;
            }

            for (int i = 0; i < 100; ++i)
            {
                var dealerProduct = _contentManager.New("DealerProduct");
                dealerProduct.As<ICommonPart>().Owner = owner;
                dealerProduct.As<TitlePart>().Title = "Test" + i.ToString();

                _contentManager.Create(dealerProduct);
            }

            Context.Output.WriteLine(T("DealerProduct created successfully"));
        }
    }
}