using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
        [Migration(12)]
   public class Migration_V12:Migration
    {
        public override void Down()
        {
            Execute.Sql("DROP SEQUENCE Fuel.LocalVoucherNoGenerator;");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop VoucherReportView.sql");
        }

        public override void Up()
        {
           
            ////Alter.Table("Vouchers").InSchema("Fuel")
            ////.AlterColumn("LocalVoucherNo").AsInt64().Identity();
            //Alter.Column("LocalVoucherNo").OnTable("Vouchers").InSchema("Fuel").AsInt64().Identity();

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create VoucherReportView.sql");

            Execute.Sql("CREATE SEQUENCE Fuel.LocalVoucherNoGenerator as BIGINT START WITH 1 INCREMENT BY 1;");

            Alter.Table("VoucherLogs").InSchema("Fuel")
                .AlterColumn("StackTrace").AsCustom("ntext");
        }
    
    }
}
