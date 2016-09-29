using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(6)]
    public class Migration_V6 : Migration
    {
        public const string OFFHIRE_SCHEMA = "Offhire";
        public const string OFFHIRE_FUEL_TYPE_FUEL_GOOD_CODE_MAPPING_TABLE_NAME = "OffhireFuelTypeFuelGoodCode";
        public const string OFFHIRE_MEASURE_TYPE_FUEL_MEASURE_CODE_MAPPING_TABLE_NAME = "OffhireMeasureTypeFuelMeasureCode";



        public override void Up()
        {
            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop Inventory BasicInfo Views.sql");

            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\BasicInfoViews_Drop.sql");

            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create MiniStock BasicInfo Views.sql");


            if (!Schema.Schema(OFFHIRE_SCHEMA).Exists())
            {
                Create.Schema(OFFHIRE_SCHEMA);
            }

            Create.Table(OFFHIRE_FUEL_TYPE_FUEL_GOOD_CODE_MAPPING_TABLE_NAME).InSchema(OFFHIRE_SCHEMA)
                  .WithColumn("Id").AsInt64().Identity().PrimaryKey().Indexed().NotNullable()
                  .WithColumn("OffhireFuelType").AsString(50).NotNullable()
                  .WithColumn("FuelGoodCode").AsString(50).NotNullable()
                  .WithColumn("ActiveFrom").AsDateTime().Nullable()
                  .WithColumn("ActiveTo").AsDateTime().Nullable();

            var fuelTypeMappingFullName = string.Format("[{0}].[{1}]", OFFHIRE_SCHEMA,OFFHIRE_FUEL_TYPE_FUEL_GOOD_CODE_MAPPING_TABLE_NAME);

            Execute.Sql("ALTER TABLE " + fuelTypeMappingFullName + " WITH CHECK ADD  CONSTRAINT [CK_OffhireFuelTypeFuelGoodCodeDatesCheck] CHECK  (([ActiveFrom] IS NULL OR [ActiveTo] IS NULL OR [ActiveFrom]<=[ActiveTo]));");
            Execute.Sql("ALTER TABLE " + fuelTypeMappingFullName + " CHECK CONSTRAINT [CK_OffhireFuelTypeFuelGoodCodeDatesCheck];");


            Create.Table(OFFHIRE_MEASURE_TYPE_FUEL_MEASURE_CODE_MAPPING_TABLE_NAME).InSchema(OFFHIRE_SCHEMA)
                  .WithColumn("Id").AsInt64().Identity().PrimaryKey().Indexed().NotNullable()
                  .WithColumn("OffhireMeasureType").AsString(50).NotNullable()
                  .WithColumn("FuelMeasureCode").AsString(50).NotNullable()
                  .WithColumn("ActiveFrom").AsDateTime().Nullable()
                  .WithColumn("ActiveTo").AsDateTime().Nullable();

            var measureTypeMappingFullName = string.Format("[{0}].[{1}]", OFFHIRE_SCHEMA, OFFHIRE_MEASURE_TYPE_FUEL_MEASURE_CODE_MAPPING_TABLE_NAME);

            Execute.Sql("ALTER TABLE " + measureTypeMappingFullName + " WITH CHECK ADD  CONSTRAINT [CK_OffhireMeasureTypeFuelMeasureCodeDatesCheck] CHECK  (([ActiveFrom] IS NULL OR [ActiveTo] IS NULL OR [ActiveFrom]<=[ActiveTo]));");
            Execute.Sql("ALTER TABLE " + measureTypeMappingFullName + " CHECK CONSTRAINT [CK_OffhireMeasureTypeFuelMeasureCodeDatesCheck];");

            insertOffhireSystemMappingData();
        }
        
        public override void Down()
        {
            Delete.Table("OffhireMeasureTypeFuelMeasureCode").InSchema(OFFHIRE_SCHEMA);
            Delete.Table("OffhireFuelTypeFuelGoodCode").InSchema(OFFHIRE_SCHEMA);

            Delete.Schema(OFFHIRE_SCHEMA);

            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop MiniStock BasicInfo Views.sql");
        }

        private void insertOffhireSystemMappingData()
        {
            if (Schema.Schema(Migration_V6.OFFHIRE_SCHEMA).Exists())
            {
                if (Schema.Schema(Migration_V6.OFFHIRE_SCHEMA).Table(Migration_V6.OFFHIRE_FUEL_TYPE_FUEL_GOOD_CODE_MAPPING_TABLE_NAME).Exists())
                {
                    Insert.IntoTable(Migration_V6.OFFHIRE_FUEL_TYPE_FUEL_GOOD_CODE_MAPPING_TABLE_NAME)
                        .InSchema(Migration_V6.OFFHIRE_SCHEMA)
                            .Row(new
                            {
                                OffhireFuelType = "HO",
                                FuelGoodCode = "HFO",
                                ActiveFrom = (DateTime?)null,
                                ActiveTo = (DateTime?)null,
                            });

                    Insert.IntoTable(Migration_V6.OFFHIRE_FUEL_TYPE_FUEL_GOOD_CODE_MAPPING_TABLE_NAME)
                        .InSchema(Migration_V6.OFFHIRE_SCHEMA)
                            .Row(new
                            {
                                OffhireFuelType = "DO",
                                FuelGoodCode = "MDO",
                                ActiveFrom = (DateTime?)null,
                                ActiveTo = (DateTime?)null,
                            });

                    //INSERT INTO [Offhire].[OffhireFureTypeFuelGoodCode] ([OffhireFuelType], [FuelGoodCode], [ActiveFrom], [ActiveTo]) VALUES (N'HO', N'HFO', NULL, NULL)
                    //INSERT INTO [Offhire].[OffhireFureTypeFuelGoodCode] ([OffhireFuelType], [FuelGoodCode], [ActiveFrom], [ActiveTo]) VALUES (N'DO', N'MDO', NULL, NULL)
                }

                if (Schema.Schema(Migration_V6.OFFHIRE_SCHEMA).Table(Migration_V6.OFFHIRE_MEASURE_TYPE_FUEL_MEASURE_CODE_MAPPING_TABLE_NAME).Exists())
                {
                    Insert.IntoTable(Migration_V6.OFFHIRE_MEASURE_TYPE_FUEL_MEASURE_CODE_MAPPING_TABLE_NAME)
                        .InSchema(Migration_V6.OFFHIRE_SCHEMA)
                            .Row(new
                            {
                                OffhireMeasureType = "TON",
                                FuelMeasureCode = "TON",
                                ActiveFrom = (DateTime?)null,
                                ActiveTo = (DateTime?)null,
                            });

                    //INSERT INTO [Offhire].[OffhireMeasureTypeFuelMeasureCode] ([Id], [OffhireMeasureType], [FuelMeasureCode], [ActiveFrom], [ActiveTo]) VALUES (1, N'TON', N'MT', NULL, NULL)
                }
            }
        }

    }
}
