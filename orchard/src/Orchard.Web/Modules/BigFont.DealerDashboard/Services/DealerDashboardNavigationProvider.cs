using System.Web;
using System.Linq;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Aspects;
using Orchard.Core.Navigation.Models;
using Orchard.Localization;
using Orchard.UI.Navigation;
using Orchard.Mvc.Routes;
using System.Collections.Generic;
using Orchard.Security.Permissions;
using Orchard;

namespace BigFont.DealerDashboard.Services
{
    public class DealerDashboardMenuProvider : IMenuProvider
    {
        private readonly IContentManager _contentManager;
        public IOrchardServices Services { get; private set; }
        public Localizer T { get; set; }

        public DealerDashboardMenuProvider(
            IContentManager contentManager,
            IOrchardServices orchardServices)
        {
            _contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public void GetMenu(IContent menu, NavigationBuilder builder)
        {           
            /*
             * TODO Add this menu item in code instead of through the UI.
             * Currently, we are additing it as follows:
             * 1 Install and enable content item permissions
             * 2 Navigation > Add > Custom Link > to Main Menu
             * 3 Edit the custom link
             * 4 Give the Dealer role "View this item" permission explicitly             
             */
            // builder.Add(T("Dealers"), "2", subMenu => subMenu
            //    .Url("~/Dealers"));
        }
    }
}