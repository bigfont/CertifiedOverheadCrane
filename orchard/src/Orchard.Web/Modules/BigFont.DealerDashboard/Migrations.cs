using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using BigFont.DealerDashboard.Models;

namespace BigFont.DealerDashboard {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            // Part - DealerProductPart
            SchemaBuilder.CreateTable("DealerProductRecord", table => table
                .ContentPartRecord()
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(DealerProductPart).Name, cfg => cfg
                    .Attachable());

            return 1;
        }

        public int UpdateFrom1() {

            // Part - CraneProductPart
            SchemaBuilder.CreateTable("CraneProductRecord", table => table
                .ContentPartRecord()
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(CraneProductPart).Name, cfg => cfg
                    .WithField("Image 1", field => field.OfType("ImageField"))
                    .WithField("Image 2", field => field.OfType("ImageField"))
                    .WithField("Part #", field => field.OfType("TextField"))
                    .WithField("Model #", field => field.OfType("TextField"))
                    .Attachable());

            // Type - CraneProduct
            ContentDefinitionManager.AlterTypeDefinition("UsedCraneStock",
                cfg => cfg
                    .WithPart("DealerProductPart")
                    .WithPart("CraneProductPart")
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