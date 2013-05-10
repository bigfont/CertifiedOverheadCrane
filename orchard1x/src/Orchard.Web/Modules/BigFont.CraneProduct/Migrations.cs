using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using BigFont.CraneProduct.Models;

namespace BigFont.CraneProduct
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            // Creating table CraneProductRecord
            SchemaBuilder.CreateTable("CraneProductRecord", table => table
                .ContentPartRecord()
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(CraneProductPart).Name, cfg => cfg
                    .Attachable()
                    .WithField("Image 1", field => field.OfType("ImageField"))
                    .WithField("Image 2", field => field.OfType("ImageField"))
                    .WithField("Part #", field => field.OfType("TextField"))
                    .WithField("Model #", field => field.OfType("TextField"))
                    );

            ContentDefinitionManager.AlterTypeDefinition("CraneProduct", cfg => cfg
                .WithPart("CraneProductPart")
                .WithPart("CommonPart", cp => cp
                    .WithSetting("OwnerEditorSettings.ShowOwnerEditor", "false")
                    .WithSetting("DateEditorSettings.ShowDateEditor", "false"))

                .WithPart("BodyPart")
                .WithPart("TitlePart")
                .Creatable());

            return 1;
        }

        public int UpdateFrom1()
        {
            ContentDefinitionManager.AlterTypeDefinition("CraneProduct", cfg => cfg
                .Draftable());
            return 2;
        }
    }
}