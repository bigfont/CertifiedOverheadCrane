using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using BigFont.OpenDashboard.Models;
using System.Web.Routing;

namespace BigFont.OpenDashboard
{
    public class DealerProductHandler : ContentHandler
    {
        public DealerProductHandler(IRepository<OpenDashboardContentRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            var dealerPart = context.ContentItem.As<OpenDashboardContentPart>();

            if (dealerPart == null)
                return;

            /*
             * These ensure that the Create, Edit, View, and Delete buttons
             * point to the OpenDashboard instead of to TheAdmin.
             */
            context.Metadata.CreateRouteValues = new RouteValueDictionary {
                    {"Area", "BigFont.OpenDashboard"},
                    {"Controller", "Home"}, // this one was Admin
                    {"Action", "Create"},
                    {"Id", context.ContentItem.ContentType}
                };
            context.Metadata.EditorRouteValues = new RouteValueDictionary {
                    {"Area", "BigFont.OpenDashboard"},
                    {"Controller", "Home"}, // this one was Admin
                    {"Action", "Edit"},
                    {"Id", context.ContentItem.Id}
                };
            context.Metadata.DisplayRouteValues = new RouteValueDictionary {
                    {"Area", "BigFont.OpenDashboard"},
                    {"Controller", "Home"}, // this one was Item
                    {"Action", "Display"},
                    {"Id", context.ContentItem.Id}
                };
            context.Metadata.RemoveRouteValues = new RouteValueDictionary {
                    {"Area", "BigFont.OpenDashboard"},
                    {"Controller", "Home"}, // this one was Admin
                    {"Action", "Remove"},
                    {"Id", context.ContentItem.Id}
                };                        
        }
    }
}