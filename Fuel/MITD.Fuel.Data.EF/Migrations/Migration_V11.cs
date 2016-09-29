using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(11)]
    public class Migration_V11:Migration
    {
        public override void Down()
        {
            Delete.Column("UserId").FromTable("Vouchers").InSchema("Fuel");
        }

        public override void Up()
        {
            Alter.Table("Vouchers").InSchema("Fuel")
                .AddColumn("UserId").AsInt64().Nullable();

            Execute.Sql("UPDATE Fuel.Vouchers SET UserId = (SELECT TOP 1 [Id] FROM [StorageSpace].[BasicInfo].[UserView] WHERE CompanyId = Fuel.Vouchers.[CompanyId]) WHERE UserId IS NULL;");

            Alter.Table("Vouchers").InSchema("Fuel")
                .AlterColumn("UserId").AsInt64().NotNullable();
        }
    }
}
