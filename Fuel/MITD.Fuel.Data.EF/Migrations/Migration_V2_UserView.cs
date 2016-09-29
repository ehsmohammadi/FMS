using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(2)]
    public class Migration_V2_UserView : Migration
    {
        public override void Up()
        {
            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create MiniStock BasicInfo Views.sql");

            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Create BasicInfo.UserView.sql");            
        }

        public override void Down()
        {
            Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop BasicInfo.UserView.sql");

            //Execute.Script(@"Fuel\MITD.Fuel.Data.EF\DBQueries\Drop MiniStock BasicInfo Views.sql");
        }
    }
}
