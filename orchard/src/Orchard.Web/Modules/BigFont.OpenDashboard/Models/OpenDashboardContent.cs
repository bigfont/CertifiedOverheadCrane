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
    [OrchardFeature("BigFont.OpenDashboard.OpenTypes")]
    public class ProductCodesRecord : ContentPartRecord {
    }
    [OrchardFeature("BigFont.OpenDashboard.OpenTypes")]
    public class ProductCodesPart : ContentPart<ProductCodesRecord> {
    }
    [OrchardFeature("BigFont.OpenDashboard.OpenTypes")]
    public class ProductImagesRecord : ContentPartRecord {
    }
    [OrchardFeature("BigFont.OpenDashboard.OpenTypes")]
    public class ProductImagesPart : ContentPart<ProductImagesRecord> {
    }
}