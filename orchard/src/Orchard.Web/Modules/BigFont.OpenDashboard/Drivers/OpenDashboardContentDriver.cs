using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BigFont.OpenDashboard.Models;
using Orchard.ContentManagement;
using Orchard.ContentManagement.Drivers;

namespace BigFont.OpenDashboard
{
    public class OpenDashboardContent : ContentPartDriver<OpenDashboardContentPart>
    {
        protected override DriverResult Display(
            OpenDashboardContentPart part, string displayType, dynamic shapeHelper)
        {
            return Combined(
                    ContentShape("Parts_OpenDashboardContent", 
                        () => shapeHelper.Parts_OpenDashboardContent()),
                    ContentShape("Parts_OpenDashboardContent_Publish_OpenDashboard",
                        () => shapeHelper.Parts_OpenDashboardContent_Publish_OpenDashboard()),
                    ContentShape("Parts_OpenDashboardContent_Clone_OpenDashboard",
                        () => shapeHelper.Parts_OpenDashboardContent_Clone_OpenDashboard()));
        }
        //GET
        protected override DriverResult Editor(
            OpenDashboardContentPart part, dynamic shapeHelper)
        {

            return ContentShape("Parts_OpenDashboardContent_Edit",
                () => shapeHelper.EditorTemplate(
                    TemplateName: "Parts/OpenDashboardContent",
                    Model: part,
                    Prefix: Prefix));
        }
        //POST
        protected override DriverResult Editor(
            OpenDashboardContentPart part, IUpdateModel updater, dynamic shapeHelper)
        {
            updater.TryUpdateModel(part, Prefix, null, null);
            return Editor(part, shapeHelper);
        }
    }
}