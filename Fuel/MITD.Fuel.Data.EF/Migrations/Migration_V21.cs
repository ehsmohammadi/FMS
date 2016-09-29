using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(21)]
    public class Migration_V21 : Migration
    {
        public override void Up()
        {
            Create.Table("ApproveFlowConfigValidFuelUsers").InSchema(Migration_Initial.FUEL_SCHEMA)
                  .WithColumn("ApproveFlowConfigId").AsInt64().NotNullable()
                  .ForeignKey("FK_ApproveFlowConfigValidFuelUsers_ApproveFlowConfigId_ApproveFlowConfig_Id", Migration_Initial.FUEL_SCHEMA, "ApproveFlowConfig", "Id")
                  .WithColumn("FuelUserId").AsInt64().NotNullable();

            Create.PrimaryKey("PK_ApproveFlowConfigValidFuelUsers_ApproveFlowConfigId_FuelUserId").OnTable("ApproveFlowConfigValidFuelUsers")
                .WithSchema(Migration_Initial.FUEL_SCHEMA).Columns(new []{"ApproveFlowConfigId", "FuelUserId"});

            Execute.Sql(@"INSERT INTO Fuel.ApproveFlowConfigValidFuelUsers
                            SELECT [Id], [ActorUserId] FROM [Fuel].[ApproveFlowConfig];");
        }

        public override void Down()
        {
            Delete.Table("ApproveFlowConfigValidFuelUsers").InSchema(Migration_Initial.FUEL_SCHEMA);
        }
    }
}
