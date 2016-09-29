using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(5)]
    public class Migration_V5 : Migration
    {
        public override void Down()
        {

            Delete.ForeignKey("FK_EffectiveFactor_Account").OnTable("EffectiveFactor").InSchema(Migration_Initial.FUEL_SCHEMA);

            Delete.Column("AccountId").FromTable("EffectiveFactor").InSchema(Migration_Initial.FUEL_SCHEMA);
            Delete.Column("VoucherDescription").FromTable("EffectiveFactor").InSchema(Migration_Initial.FUEL_SCHEMA);
            Delete.Column("VoucherRefDescription").FromTable("EffectiveFactor").InSchema(Migration_Initial.FUEL_SCHEMA);

            Delete.Table("AsgnVoucherAconts").InSchema("Fuel");
            Delete.Table("AsgnSegmentTypeVoucherSetingDetail").InSchema("Fuel");



            Delete.Table("Segments").InSchema("Fuel");
            Delete.Table("JournalEntries").InSchema("Fuel");
            Delete.Table("Vouchers").InSchema("Fuel");
            Delete.Table("VoucherSetingDetails").InSchema("Fuel");
            Delete.Table("VoucherSetings").InSchema("Fuel");
            Delete.Table("Accounts").InSchema("Fuel");
            

        }

        public override void Up()
        {


            Create.Table("Accounts").InSchema("Fuel")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(50)
                .WithColumn("TimeStamp").AsCustom("RowVersion")
                .WithColumn("Code").AsString(50);

                          

            Create.Table("VoucherSetings").InSchema("Fuel")
                .WithColumn("Id").AsInt64().Identity().NotNullable().PrimaryKey()
                .WithColumn("VoucherMainRefDescription").AsString(250)
                .WithColumn("VoucherMainDescription").AsString(250)
                .WithColumn("CompanyId").AsInt64().NotNullable()
                .WithColumn("VoucherDetailTypeId").AsInt32()
                .WithColumn("TimeStamp").AsCustom("RowVersion")
                .WithColumn("VoucherTypeId").AsInt32();

            Create.Table("VoucherSetingDetails").InSchema("Fuel")
                .WithColumn("Id").AsInt64().Identity().NotNullable().PrimaryKey()
                .WithColumn("VoucherSetingId").AsInt64()
                  .ForeignKey("FK_VoucherSeting", "Fuel", "VoucherSetings", "Id")
                .WithColumn("VoucherCeditRefDescription").AsString(250)
                .WithColumn("VoucherDebitDescription").AsString(250)
                .WithColumn("VoucherDebitRefDescription").AsString(250)
                .WithColumn("VoucherCreditDescription").AsString(250)
                .WithColumn("GoodId").AsInt64().NotNullable();



            Create.Table("AsgnVoucherAconts").InSchema("Fuel")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("VoucherSetingDetailId").AsInt64()
                  .ForeignKey("FK_VoucherSetingDetail_AsgnVoucherAconts", "Fuel", "VoucherSetingDetails", "Id")
                .WithColumn("AccountId").AsInt32()
                  .ForeignKey("FK_Account_AsgnVoucherAcont", "Fuel", "Accounts", "Id")
                .WithColumn("Type").AsInt32()
                 .WithColumn("TimeStamp").AsCustom("RowVersion");

          

            Create.Table("AsgnSegmentTypeVoucherSetingDetail").InSchema("Fuel")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Type").AsInt32()
                .WithColumn("VoucherSetingDetailId").AsInt64()
                .ForeignKey("FK_VoucherSetingDetail_AsgnSegmentTypeVoucherSetingDetail", "Fuel", "VoucherSetingDetails",
                    "Id")
                .WithColumn("SegmentTypeId").AsInt32();


            Create.Table("Vouchers").InSchema("Fuel")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("Description").AsString(250)
                .WithColumn("FinancialVoucherDate").AsDateTime()
                .WithColumn("LocalVoucherDate").AsDateTime()
                .WithColumn("LocalVoucherNo").AsString(50)
                .WithColumn("ReferenceNo").AsString(50)
                .WithColumn("VoucherRef").AsString(50)
                .WithColumn("ReferenceTypeId").AsInt32()
                .WithColumn("TimeStamp").AsCustom("RowVersion");


            Create.Table("JournalEntries").InSchema("Fuel")
                .WithColumn("Id").AsInt64().Identity().NotNullable().PrimaryKey()
                .WithColumn("VoucherId").AsInt64().NotNullable()
                    .ForeignKey("FK_Voucher_JournalEntries_", "Fuel", "Vouchers", "Id")
                .WithColumn("AccountNo").AsString(250)
                .WithColumn("CurrencyId").AsInt64().NotNullable()
                .WithColumn("Description").AsString(250)
                .WithColumn("TimeStamp").AsCustom("RowVersion")
                .WithColumn("VoucherRef").AsString(250)
                .WithColumn("ForeignAmount").AsDecimal()
                .WithColumn("IrrAmount").AsDecimal()
                .WithColumn("Typ").AsInt32();


            Create.Table("Segments").InSchema("Fuel")
               .WithColumn("Id").AsInt64().Identity().PrimaryKey()
               .WithColumn("Name").AsString(50)
               .WithColumn("Code").AsString(50)
               .WithColumn("TimeStamp").AsCustom("RowVersion")
               .WithColumn("SegmentTypeId").AsInt32()
               .WithColumn("JournalEntryId").AsInt64()
                .ForeignKey("FK_Segment_JournalEntries", "Fuel", "JournalEntries", "Id");

            Alter.Table("EffectiveFactor").InSchema(Migration_Initial.FUEL_SCHEMA)
                 .AddColumn("AccountId").AsInt32().Nullable()
                    .ForeignKey("FK_EffectiveFactor_Account", Migration_Initial.FUEL_SCHEMA, "Accounts", "Id")
                 .AddColumn("VoucherDescription").AsString().Nullable()
                 .AddColumn("VoucherRefDescription").AsString().Nullable();

        }
    }
}
