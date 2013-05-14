using Orchard.ContentManagement;
using Orchard.ContentManagement.Records;
using Orchard.Environment.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigFont.DealerDashboard.Models {
    public class DealerProductRecord : ContentPartRecord {
    }
    public class DealerProductPart : ContentPart<DealerProductRecord> {
    }
    [OrchardFeature("BigFont.DealerDashboard.StandardTypes")]
    public class CraneProductRecord : ContentPartRecord {
    }
    [OrchardFeature("BigFont.DealerDashboard.StandardTypes")]
    public class CraneProductPart : ContentPart<CraneProductRecord> {
    }
}