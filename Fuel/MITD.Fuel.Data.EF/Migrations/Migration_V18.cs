using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
   [Migration(18)]
   public class Migration_V18:Migration
    {

     
       public override void Up()
       {
           Alter.Table("VoucherSetingDetails").InSchema(Migration_Initial.FUEL_SCHEMA).AddColumn("IsDelete").AsBoolean().WithDefaultValue(false);

       }

       public override void Down()
       {
           
       }
    }
}
