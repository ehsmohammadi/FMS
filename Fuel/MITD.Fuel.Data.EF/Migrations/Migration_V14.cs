using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(14)]
    public class Migration_V14 : Migration
    {
        public override void Down()
        {
            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Recreate Voyage Identity Id.sql");
        }

        public override void Up()
        {
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\SAPIDAccountListView.sql");
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\HAFIZAccountListView.sql");
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\AccountListView.sql");
        }

    }
}
