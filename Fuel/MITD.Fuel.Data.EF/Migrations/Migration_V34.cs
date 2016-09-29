using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(34)]
    public class Migration_V34 : Migration
    {
        public override void Up()
        {
            createBasicInfoCurrencyExchangeView();

            if (!Schema.Schema("Finance").Exists())
                Create.Schema("Finance");


            createFinanceUpdateCurrenciesFromBasis();

            //"103-Add IsMemberOfHolding column to Company Table.sql" must be run.
            
            modifyBasicInfoCompanyViewToAddIsMemberOfHolding();

            createNGSCurrencyRatesView();

        }

        private void createNGSCurrencyRatesView()
        {
            Execute.Sql(@"
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE VIEW [Finance].[NGSCurrencyRatesView]
                AS
                SELECT cr.[Code] AS Abbreviation
                        ,[EffectiveDate] AS PersianEffectiveDate
                        ,[Rate] AS ExchangeRateToMainCurrency
                    FROM [NGSQLHDA,2433].[HDABasis].[dbo].[AllCurrencyRate] cr INNER JOIN BasicInfo.CurrencyView cv ON cr.Code = cv.Abbreviation COLLATE Arabic_CI_AS
                WHERE cr.EffectiveDate >= '13930101'
                GO

                GRANT SELECT ON [Finance].[NGSCurrencyRatesView] TO [public]
                GO
            ");
        }

        private void modifyBasicInfoCompanyViewToAddIsMemberOfHolding()
        {
            Execute.Sql(@"
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                ALTER VIEW [BasicInfo].[CompanyView]
                AS
	                SELECT c.Id, c.Code, c.Name, c.IsMemberOfHolding
	                FROM [Inventory].Companies c
                GO
            ");
        }

        private void createFinanceUpdateCurrenciesFromBasis()
        {
            Execute.Sql(@"
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE PROCEDURE [Finance].[UpdateCurrenciesFromBasis]
                AS
                BEGIN
	                SET NOCOUNT ON;

                    MERGE Inventory.Units AS target
                    USING (SELECT c.Code AS Abbreviation, c.Name, RIGHT('000' + c.CodeN, 3) AS Code
		                FROM [NGSQLHDA,2433].[HDABasis].[dbo].[Currency] c
			                WHERE c.Code NOT IN (SELECT Abbreviation COLLATE Arabic_CI_AS FROM [BasicInfo].[CurrencyView])
			                AND c.Code IN (SELECT Code  FROM [NGSQLHDA,2433].[HDABasis].[dbo].[AllCurrencyRate])
		                ) AS source 
                        (Abbreviation, Name, Code)
                    ON (target.Abbreviation COLLATE Arabic_CI_AS = source.Abbreviation)
                    WHEN MATCHED THEN 
                        UPDATE SET Name = source.Name     
	                WHEN NOT MATCHED THEN
		                INSERT 
                            ([Abbreviation]
                            ,[Name]
                            ,[IsCurrency]
                            ,[IsBaseCurrency]
                            ,[IsActive]
                            ,[UserCreatorId]
                            ,[CreateDate]
                            ,[Code])
		                VALUES
                            (source.Abbreviation
                            ,source.Name
                            ,1
                            ,CASE WHEN source.Abbreviation = 'IRR' THEN 1 ELSE 0 END
                            ,1
                            ,100000
                            ,GETDATE()
                            ,source.Code);
                END
                GO
                GRANT EXECUTE ON [Finance].[UpdateCurrenciesFromBasis] TO [public] AS [dbo]
                GO
            ");
        }

        private void createBasicInfoCurrencyExchangeView()
        {
            Execute.Sql(
              @"
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                CREATE VIEW [BasicInfo].[CurrencyExchangeView]
                AS
                SELECT   Inventory.UnitConverts.Id, Inventory.UnitConverts.UnitId AS FromCurrencyId, Inventory.UnitConverts.SubUnitId AS ToCurrencyId, Inventory.UnitConverts.Coefficient, 
                                Inventory.UnitConverts.EffectiveDateStart, Inventory.UnitConverts.EffectiveDateEnd, Inventory.UnitConverts.CreateDate
                FROM      BasicInfo.CurrencyView AS CurrencyView_FromCurrency INNER JOIN
                                Inventory.UnitConverts ON CurrencyView_FromCurrency.Id = Inventory.UnitConverts.UnitId INNER JOIN
                                BasicInfo.CurrencyView AS CurrencyView_ToCurrency ON Inventory.UnitConverts.SubUnitId = CurrencyView_ToCurrency.Id
                GO
                ");
        }

        public override void Down()
        {

        }

    }
}
