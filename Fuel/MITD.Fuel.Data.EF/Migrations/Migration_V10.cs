using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(10)]
    public class Migration_V10 : Migration
    {
        public override void Down()
        {
            Delete.Column("SegmentTypeCode").FromTable("Segments").InSchema("Fuel");
            Delete.Column("SegmentTypeName").FromTable("Segments").InSchema("Fuel");
        }

        public override void Up()
        {
            Alter.Table("Segments").InSchema("Fuel")
                .AddColumn("SegmentTypeCode").AsString(50).Nullable()
                .AddColumn("SegmentTypeName").AsString(200).Nullable();

        }
    }
}
