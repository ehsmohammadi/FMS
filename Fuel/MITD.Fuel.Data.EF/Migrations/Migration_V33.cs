using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(33)]
     [Description("Adding FinancialVoucherNo,FinancialVoucherState,FinancialVoucherSendingUserId and change FinancialVoucherDate in Vouchers")]
    public class Migration_V33:Migration
    {
     
           public override void Up()
           {
               Alter.Table("Vouchers").InSchema("Fuel")
                   .AddColumn("FinancialVoucherNo").AsString(50).Nullable()
                   .AddColumn("FinancialVoucherState").AsInt32().WithDefaultValue(2)
                   .AddColumn("FinancialVoucherSendingUserId").AsInt64().Nullable();

              Alter.Column("FinancialVoucherDate").OnTable("Vouchers").InSchema("Fuel").AsString();

              Execute.Sql(
@"ALTER VIEW [BasicInfo].[CurrencyView]
AS
	SELECT u.Id, u.Abbreviation, u.Name, u.Code
	FROM [Inventory].Units u
	WHERE u.IsCurrency=1 AND u.IsActive=1 

GO
");

               Create.Table("VoucherTransferLog").InSchema("Fuel")
                   .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                   .WithColumn("FinancialExceptionMessage").AsCustom("nvarchar(MAX)").NotNullable()
                   .WithColumn("UserId").AsInt64().NotNullable()
                   .WithColumn("VoucherIds").AsString(50).NotNullable()
                   .WithColumn("SendDate").AsDateTime().NotNullable()
                   .WithColumn("ConfigDate").AsString(50).Nullable()
                   .WithColumn("ConfigCode").AsString(50).Nullable();



           }

           public override void Down()
           {

           }
     
    }
}
