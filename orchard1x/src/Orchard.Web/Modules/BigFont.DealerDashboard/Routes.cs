using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.Mvc.Routes;

namespace BigFont.DealerDashboard
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
                    Priority = 5,
                    Route = new Route(
                        "DealerDashboard",
                        new RouteValueDictionary {
                            {"area", "BigFont.DealerDashboard"},
                            {"controller", "Home"},
                            {"action", "Index"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "BigFont.DealerDashboard"}
                        },
                        new MvcRouteHandler())
                },
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "DealerDashboard/List",
                        new RouteValueDictionary {
                            {"area", "BigFont.DealerDashboard"},
                            {"controller", "Home"},
                            {"action", "List"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "BigFont.DealerDashboard"}
                        },
                        new MvcRouteHandler())
                                },
                new RouteDescriptor {
                    Priority = 5,
                    Route = new Route(
                        "DealerDashboard/Create",
                        new RouteValueDictionary {
                            {"area", "BigFont.DealerDashboard"},
                            {"controller", "Home"},
                            {"action", "Create"}
                        },
                        new RouteValueDictionary(),
                        new RouteValueDictionary {
                            {"area", "BigFont.DealerDashboard"}
                        },
                        new MvcRouteHandler())
                }
            };
        }
    }
}