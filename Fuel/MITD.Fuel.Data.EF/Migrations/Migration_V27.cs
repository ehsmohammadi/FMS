using System.Data;
using System.Linq;
using FluentMigrator;
using MITD.Fuel.Data.EF.Context;
using MITD.Fuel.Data.EF.Migrations;

namespace MITD.Data.EF.Migrations
{
    [Migration(27)]
    public class Migration_V27 : Migration
    {
        public override void Down()
        {

        }

        public override void Up()
        {
            this.Alter.Table("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA).AddColumn("IsCorrectionPricingTypeRevised").AsBoolean().WithDefaultValue(false).NotNullable();

            using (DataContainer context = new DataContainer())
            {
                DataConfigurationProvider.ModifyFuelReportWorkflowConfigForFinancialSubmit_9404041400(context);
            }
        }
    }
}
