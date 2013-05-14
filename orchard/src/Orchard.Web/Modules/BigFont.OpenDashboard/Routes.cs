using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace BigFont.OpenDashboard
{
    public class Routes : IRouteProvider
    {
        public void GetRoutes(ICollection<RouteDescriptor> routes)
        {
            foreach (var routeDescriptor in GetRoutes())
                routes.Add(routeDescriptor);            
        }

        public IEnumerable<RouteDescriptor> GetRoutes()
        {
            return new[] {
                new RouteDescriptor {
                    Name = "OpenDashboard",
                    Priority = 5,
                    Route = new Route(
                        "Dashboard",
                        new RouteValueDictionary {
                            {"area", "BigFont.OpenDashboard"},
                            {"controller", "Home"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "BigFont.OpenDashboard"}
                        },
                        new MvcRouteHandler())
                }          
            };
        }
    }
}