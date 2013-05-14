using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigFont.OpenDashboard.Models {
    public class OpenDashboardContentRecord : ContentPartRecord {
    }
    public class OpenDashboardContentPart : ContentPart<OpenDashboardContentRecord> {
    }
    [OrchardFeature("BigFont.OpenDashboard.StandardContentTypes")]
    public class ProductRecord : ContentPartRecord {
    }
    [OrchardFeature("BigFont.OpenDashboard.StandardContentTypes")]
    public class ProductPart : ContentPart<ProductRecord> {
    }
}