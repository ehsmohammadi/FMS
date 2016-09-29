using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(22)]
    public class Migration_V22 : Migration
    {
        public override void Up()
        {
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Fuel StoredProcedures.sql");
        }

        public override void Down()
        {
            
        }
    }
}
