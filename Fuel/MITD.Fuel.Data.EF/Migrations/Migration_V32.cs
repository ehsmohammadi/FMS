using System.ComponentModel;
using System.Linq;
using FluentMigrator;
using MITD.Fuel.Data.EF.Context;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(32)]
    [Description("Adding OrderItemBalance.InventoryOperationId field;")]
    public class Migration_V32 : Migration
    {
        public override void Up()
        {
            using (DataContainer context = new DataContainer())
            {
                DataConfigurationProvider.ModifyFuelReportWorkflowConfigForCancel_9408071200(context);
            }
        }

        public override void Down()
        {
        }
    }
}
