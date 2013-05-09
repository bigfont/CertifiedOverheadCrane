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

namespace BigFont.DealerDashboard.Services
{
    public class DealerDashboardMenuProvider : IMenuProvider
    {
        private readonly IContentManager _contentManager;
        public Localizer T { get; set; }

        public DealerDashboardMenuProvider(
            IContentManager contentManager)
        {
            _contentManager = contentManager;
            T = NullLocalizer.Instance;
        }

        public void GetMenu(IContent menu, NavigationBuilder builder)
        {
            builder.Add(T("Dealers"), "2", subMenu => subMenu
                .Url("~/Dealers")
                // TODO Add permissions once we know how they work.
                // .Permission(Orchard.Core.Contents.Permissions.EditOwnContent));
        }
    }
}