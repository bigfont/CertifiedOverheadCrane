using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BigFont.CraneProduct.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace BigFont.CraneProduct.Drivers
{
    public class CraneProductDriver : ContentPartDriver<CraneProductPart>
    {        
        protected override DriverResult Display(
            CraneProductPart part, string displayType, dynamic shapeHelper)
        {
            return ContentShape("Parts_CraneProduct", () => shapeHelper.Parts_CraneProduct());
        }
        //GET
        protected override DriverResult Editor(
            CraneProductPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_CraneProduct_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/CraneProduct",
                    Model: part,
                    Prefix: Prefix));
        }
        //POST
        protected override DriverResult Editor(
            CraneProductPart part, IUpdateModel updater, dynamic shapeHelper)
        {

            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}