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
            var blog = context.ContentItem.As<CraneProductPart>();

            if (blog == null)
                return;


            //context.Metadata.DisplayRouteValues = new RouteValueDictionary {
            //    {"Area", "Orchard.Blogs"},
            //    {"Controller", "Blog"},
            //    {"Action", "Item"},
            //    {"blogId", context.ContentItem.Id}
            //};
            //context.Metadata.CreateRouteValues = new RouteValueDictionary {
            //    {"Area", "Orchard.Blogs"},
            //    {"Controller", "BlogAdmin"},
            //    {"Action", "Create"}
            //};
            context.Metadata.EditorRouteValues = new RouteValueDictionary {
                {"Area", "BigFont.DealerDashboard"},
                {"Controller", "Home"},
                {"Action", "Edit"},
                {"id", context.ContentItem.Id}
            };
            //context.Metadata.EditorRouteValues = new RouteValueDictionary {
            //    {"Area", "Orchard.Blogs"},
            //    {"Controller", "BlogAdmin"},
            //    {"Action", "Edit"},
            //    {"blogId", context.ContentItem.Id}
            //};
            //context.Metadata.RemoveRouteValues = new RouteValueDictionary {
            //    {"Area", "Orchard.Blogs"},
            //    {"Controller", "BlogAdmin"},
            //    {"Action", "Remove"},
            //    {"blogId", context.ContentItem.Id}
            //};
            //context.Metadata.AdminRouteValues = new RouteValueDictionary {
            //    {"Area", "Orchard.Blogs"},
            //    {"Controller", "BlogAdmin"},
            //    {"Action", "Item"},
            //    {"blogId", context.ContentItem.Id}
            //};
        }
    }
}