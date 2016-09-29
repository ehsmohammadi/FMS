using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using MITD.Fuel.Domain.Model.DomainObjects.VoucherAggregate;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(7)]
    public class Migration_V7 : Migration
    {
        public override void Up()
        {
            Create.Table("UserInCompany").InSchema(Migration_Initial.FUEL_SCHEMA)
                  .WithColumn("Id").AsInt64().PrimaryKey().NotNullable()
                  .WithColumn("UserId").AsInt64().NotNullable()
                        .ForeignKey("FK_UserInCompany_UserId_SecurityUser_Id", Migration_Initial.SECURITY_SCHEMA, "Users", "Id")
                  .WithColumn("CompanyId").AsInt64().NotNullable();


            //Alter.Table("Users").InSchema(Migration_Initial.SECURITY_SCHEMA)
            //     .AddColumn("CompanyId").AsInt64().Nullable();


            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop BasicInfo.UserView.sql");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create BasicInfo.UserView.V2.sql");
        }

        
        public override void Down()
        {
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop BasicInfo.UserView.V2.sql");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create BasicInfo.UserView.sql");


            //Delete.Column("CompanyId").FromTable("Users").InSchema(Migration_Initial.SECURITY_SCHEMA);

            Delete.Table("UserInCompany").InSchema(Migration_Initial.FUEL_SCHEMA);
        }
    }
}
