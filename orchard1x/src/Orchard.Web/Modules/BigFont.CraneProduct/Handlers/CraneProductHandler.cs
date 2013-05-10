using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Handlers;
using Orchard.Data;
using BigFont.CraneProduct.Models;
using System.Web.Routing;

namespace BigFont.CraneProduct.Handlers
{
    public class CraneProductHandler : ContentHandler
    {
        public CraneProductHandler(IRepository<CraneProductRecord> repository)
        {
            Filters.Add(StorageFilter.For(repository));
        }

        protected override void GetItemMetadata(GetContentItemMetadataContext context)
        {
            var cranePart = context.ContentItem.As<CraneProductPart>();

            if (cranePart == null)
                return;

                context.Metadata.CreateRouteValues = new RouteValueDictionary {
                    {"Area", "BigFont.DealerDashboard"},
                    {"Controller", "Home"}, // this one was Admin
                    {"Action", "Create"},
                    {"Id", context.ContentItem.ContentType}
                };          
                context.Metadata.EditorRouteValues = new RouteValueDictionary {
                    {"Area", "BigFont.DealerDashboard"},
                    {"Controller", "Home"}, // this one was Admin
                    {"Action", "Edit"},
                    {"Id", context.ContentItem.Id}
                };
                context.Metadata.DisplayRouteValues = new RouteValueDictionary {
                    {"Area", "BigFont.DealerDashboard"},
                    {"Controller", "Home"}, // this one was Item
                    {"Action", "Display"},
                    {"Id", context.ContentItem.Id}
                };           
                context.Metadata.RemoveRouteValues = new RouteValueDictionary {
                    {"Area", "BigFont.DealerDashboard"},
                    {"Controller", "Home"}, // this one was Admin
                    {"Action", "Remove"},
                    {"Id", context.ContentItem.Id}
                };            
        }
    }
}