using Orchard.ContentManagement.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigFont.DashboardUsability.Handlers
{
    public class DashboardUsabilityHandler : IContentHandler
    {
        public DashboardUsabilityHandler() { }
        public void Activating(ActivatingContentContext context) { }
        public void Activated(ActivatedContentContext context) { }
        public void Initializing(InitializingContentContext context) { }
        public void Creating(CreateContentContext context) { }
        public void Created(CreateContentContext context) { }
        public void Loading(LoadContentContext context) { }
        public void Loaded(LoadContentContext context) { }
        public void Updating(UpdateContentContext context) { }
        public void Updated(UpdateContentContext context) { }
        public void Versioning(VersionContentContext context) { }
        public void Versioned(VersionContentContext context) { }
        public void Publishing(PublishContentContext context) { }
        public void Published(PublishContentContext context) { }
        public void Unpublishing(PublishContentContext context) { }
        public void Unpublished(PublishContentContext context) { }
        public void Removing(RemoveContentContext context) { }
        public void Removed(RemoveContentContext context) { }
        public void Indexing(IndexContentContext context) { }
        public void Indexed(IndexContentContext context) { }
        public void Importing(ImportContentContext context) { }
        public void Imported(ImportContentContext context) { }
        public void Exporting(ExportContentContext context) { }
        public void Exported(ExportContentContext context) { }

        public void GetContentItemMetadata(GetContentItemMetadataContext context) { }
        public void BuildDisplay(BuildDisplayContext context) { }
        public void BuildEditor(BuildEditorContext context) { }
        public void UpdateEditor(UpdateEditorContext context) { }
    }
}