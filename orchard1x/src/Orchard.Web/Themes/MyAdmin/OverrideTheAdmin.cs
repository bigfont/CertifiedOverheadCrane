using System.Web.Routing;
using Orchard.Themes;
using Orchard.UI.Admin;

namespace MyAdmin
{
    public class OverrideTheAdmin : IThemeSelector
    {
        public ThemeSelectorResult GetTheme(RequestContext context)
        {
            if (AdminFilter.IsApplied(context))
            {
                return new ThemeSelectorResult { Priority = 110, ThemeName = "MyAdmin" };
            }

            return null;
        }
    }

}