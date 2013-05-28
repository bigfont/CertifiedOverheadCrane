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
         * Create the OpenDashboardContentPart.
         */
        public int Create() {

            SchemaBuilder.CreateTable("OpenDashboardContentRecord", table => table
                .ContentPartRecord()
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(OpenDashboardContentPart).Name, cfg => cfg
                    .WithDescription("Attaching this part to any ContentType will make it appear in the OpenDashboard.")
                    .Attachable());

            // Create a new Product part, and
            // add a couple of fields to it. 
            // This Product part will not show up in the admin list of Content Parts.
            ContentDefinitionManager.AlterPartDefinition("Product",
                builder => builder
                    .Attachable()
                    .WithDescription("Adds some fields to the Product type")
                    .WithField("PartNumber", fieldBuilder => fieldBuilder.OfType("TextField").WithDisplayName("Part Number").WithSetting("FieldIndexing.Included", "true"))
                    .WithField("ModelNumber", fieldBuilder => fieldBuilder.OfType("TextField").WithDisplayName("Model Number").WithSetting("FieldIndexing.Included", "true"))
                    .WithField("Image1", fieldBuilder => fieldBuilder.OfType("ImageField").WithDisplayName("Image1"))
                    .WithField("Image2", fieldBuilder => fieldBuilder.OfType("ImageField").WithDisplayName("Image2"))
                    );

            // Create a new Product type, and
            // attach the OpenDashboardContentPart as well as the Product part.           
            ContentDefinitionManager.AlterTypeDefinition("Product",
                cfg => cfg

                    // shows in admin > Content Definition > Content Types
                    .DisplayedAs("Product") 
                    .Named("Product")

                    // add the product part, created above, to the product type
                    .WithPart("Product")

                    // add the open dashboard content part to make Product display in the OpenDashboard
                    .WithPart("OpenDashboardContentPart") 

                    // add other more generic parts
                    .WithPart("BodyPart")
                    .WithPart("TitlePart")
                    .WithPart("CommonPart", cp => cp
                        .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                        .WithSetting("DateEditorSettings.ShowDateEditor", "false"))
                    .WithSetting("TypeIndexing.Indexes", "OpenDashboardIndex")
                    .Creatable()
                    .Draftable());

            return 1;
        }
    }
}