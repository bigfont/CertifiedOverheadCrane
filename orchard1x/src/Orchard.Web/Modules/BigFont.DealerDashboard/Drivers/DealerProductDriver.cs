using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BigFont.DealerDashboard.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace BigFont.DealerDashboard
{
    public class DealerProduct : ContentPartDriver<DealerProductPart>
    {        
        protected override DriverResult Display(
            DealerProductPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_DealerProduct_Publish_SummaryAdmin", () => shapeHelper.Parts_DealerProduct_Publish_SummaryAdmin());
        }
        //GET
        protected override DriverResult Editor(
            DealerProductPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_DealerProduct_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/DealerProduct",
                    Model: part,
                    Prefix: Prefix));
        }
        //POST
        protected override DriverResult Editor(
            DealerProductPart part, IUpdateModel updater, dynamic shapeHelper)
        {

            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}