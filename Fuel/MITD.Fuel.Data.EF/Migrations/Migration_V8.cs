using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(8)]
    public class Migration_V8 : Migration
    {
        public override void Down()
        {
            Delete.ForeignKey("FK_Segment_FreeAccount").OnTable("Segments").InSchema("Fuel");
            Delete.ForeignKey("FK_Segment_EffectiveFactor").OnTable("Segments").InSchema("Fuel");

            Delete.Column("FreeAccountId").FromTable("Segments").InSchema("Fuel");
            Delete.Column("EffectiveFactorId").FromTable("Segments").InSchema("Fuel");

            Delete.Column("VoucherTypeId").FromTable("Vouchers").InSchema("Fuel");
            Delete.Column("VoucherDetailTypeId").FromTable("Vouchers").InSchema("Fuel");
            Delete.Column("CompanyId").FromTable("Vouchers").InSchema("Fuel");


            Delete.Table("VoucherLogs").InSchema("Fuel");
            Delete.Table("FreeAccounts").InSchema("Fuel");
        }

        public override void Up()
        {
            Create.Table("VoucherLogs").InSchema("Fuel")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("ExceptionMessage").AsString(1000)
                .WithColumn("StackTrace").AsString(500)
                .WithColumn("VoucherType").AsString(30)
                .WithColumn("RefrenceNo").AsString(10);

            Create.Table("FreeAccounts").InSchema("Fuel")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("Name").AsString(50)
                .WithColumn("Code").AsString(50);


           
            Alter.Table("Segments").InSchema("Fuel")
                .AddColumn("FreeAccountId").AsInt64()
                  .ForeignKey("FK_Segment_FreeAccount", "Fuel", "FreeAccounts", "Id")
                .AddColumn("EffectiveFactorId").AsInt64()
                  .ForeignKey("FK_Segment_EffectiveFactor", "Fuel", "EffectiveFactor", "Id");


            Alter.Table("Vouchers").InSchema("Fuel")
                .AddColumn("CompanyId").AsInt64()
                .AddColumn("VoucherDetailTypeId").AsInt32()
                .AddColumn("VoucherTypeId").AsInt32();


        }
    }
}
