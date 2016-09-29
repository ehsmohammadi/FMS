using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;
using MITD.Fuel.Domain.Model.Enums;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(23)]
    public class Migration_V23 : Migration
    {
        public override void Up()
        {
            Create.Column("CorrectionPricingType").OnTable("FuelReportDetail").InSchema(Migration_Initial.FUEL_SCHEMA).AsInt32().Nullable();
                //.WithDefaultValue((int) CorrectionPricingTypes.Default);

            Execute.Sql(@"UPDATE [Fuel].[FuelReportDetail] 
                            SET CorrectionPricingType = 
                                CASE 
                                    WHEN CorrectionPriceCurrencyId IS NOT NULL THEN 4 
                                    WHEN CorrectionPriceCurrencyId IS NULL AND CorrectionReference_ReferenceType IS NOT NULL AND CorrectionReference_ReferenceType = 2 THEN 3
                                    ELSE 2 END 
                                WHERE Correction IS NOT NULL AND Correction <> 0 AND CorrectionType = 1;");

            Execute.Sql(@"UPDATE [Fuel].[FuelReportDetail] 
                            SET CorrectionPricingType = 
                                CASE 
                                    WHEN CorrectionPriceCurrencyId IS NULL AND CorrectionReference_ReferenceType IS NOT NULL AND CorrectionReference_ReferenceType = 2 THEN 3
                                    ELSE 1 END
                                WHERE Correction IS NOT NULL AND Correction <> 0 AND CorrectionType = 2;");
        }

        public override void Down()
        {
            
        }
    }
}
