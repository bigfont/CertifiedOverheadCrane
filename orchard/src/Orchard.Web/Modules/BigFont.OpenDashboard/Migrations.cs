using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using BigFont.OpenDashboard.Models;

namespace BigFont.OpenDashboard {
    public class Migrations : DataMigrationImpl {

        /*
         * Create the required OpenDashboardContentPart.
         * The OpenDashboard will display ContentTypes
         * that have the OpenDashboardContentPart attached.
         */
        public int Create() {

            // CONTENT PART - OpenDashboardContentPart
            SchemaBuilder.CreateTable("OpenDashboardContentRecord", table => table
                .ContentPartRecord()                
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(OpenDashboardContentPart).Name, cfg => cfg
                    .Attachable());

            return 1;
        }

        /*
         * Create an optional demo product 
         * that will display in the OpenDashboard.
         */
        public int UpdateFrom1() {

            // CONTENT TYPE - DemoProduct
            ContentDefinitionManager.AlterTypeDefinition("DemoProduct",
                cfg => cfg
                    .WithPart("OpenDashboardContentPart") // required for display in the OpenDashboard
                    .WithPart("CommonPart", cp => cp
                        .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                        .WithSetting("DateEditorSettings.ShowDateEditor", "false"))
                    .WithPart("BodyPart")
                    .WithPart("TitlePart")
                        .Draftable()
                        .Creatable());

            return 2;
        }
    }
}