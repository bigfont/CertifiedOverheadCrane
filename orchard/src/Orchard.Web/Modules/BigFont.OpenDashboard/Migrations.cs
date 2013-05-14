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
         * Create some optional but useful other ContentParts
         * to illustrate the usefulness of the open dashboard.
         */
        public int UpdateFrom1() {

            // CONTENT PART - ProductCodes
            SchemaBuilder.CreateTable("ProductCodesRecord", table => table
                .ContentPartRecord()
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ProductCodesPart).Name, cfg => cfg
                    .WithField("Part #", field => field.OfType("TextField"))
                    .WithField("Model #", field => field.OfType("TextField"))
                    .Attachable());

            // CONTENT PART - ProductImages
            SchemaBuilder.CreateTable("ProductImagesRecord", table => table
                .ContentPartRecord()
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(ProductImagesPart).Name, cfg => cfg
                    .WithField("Image 1", field => field.OfType("ImageField"))
                    .WithField("Image 2", field => field.OfType("ImageField"))
                    .Attachable());            

            return 2;
        }

        /*
         * Create an optional demo product 
         * that will display in the OpenDashboard.
         */
        public int UpdateFrom2() {

            // CONTENT TYPE - OverstockProduct
            ContentDefinitionManager.AlterTypeDefinition("OverstockProduct",
                cfg => cfg
                    .WithPart("OpenDashboardContentPart") // required for display in the OpenDashboard
                    .WithPart("ProductCodesPart")
                    .WithPart("ProductImagesPart")
                    .WithPart("CommonPart", cp => cp
                        .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                        .WithSetting("DateEditorSettings.ShowDateEditor", "false"))
                    .WithPart("BodyPart")
                    .WithPart("TitlePart")
                        .Draftable()
                        .Creatable());

            return 3;
        }
    }
}