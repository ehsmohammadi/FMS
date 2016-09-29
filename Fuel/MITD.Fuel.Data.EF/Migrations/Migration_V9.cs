using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(9)]
    public class Migration_V9 : Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {

            Alter.Table("VoucherLogs").InSchema("Fuel")
                .AlterColumn("StackTrace").AsString()
                .AlterColumn("RefrenceNo").AsString(100);

            Alter.Table("Segments").InSchema("Fuel")
                .AlterColumn("FreeAccountId").AsInt64().Nullable()
                .AlterColumn("EffectiveFactorId").AsInt64().Nullable();

        }
    }
}
