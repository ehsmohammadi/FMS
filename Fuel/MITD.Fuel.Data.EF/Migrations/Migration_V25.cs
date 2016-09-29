using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(25)]
   public class Migration_V25:Migration
    {


        public override void Down()
        {
            Delete.Column("InventoryItemId").FromTable("JournalEntries").InSchema("Fuel");
        }

        public override void Up()
        {
            Alter.Table("JournalEntries").InSchema("Fuel")
                .AddColumn("InventoryItemId").AsInt64().Nullable();
        }
    }
}
