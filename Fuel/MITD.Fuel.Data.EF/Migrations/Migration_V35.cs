using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(35)]
    public class Migration_V35 : Migration
    {
        public override void Up()
        {
            this.Alter.Table("Voyage").InSchema(Migration_Initial.FUEL_SCHEMA).AddColumn("IsLocked").AsBoolean().WithDefaultValue(false);

            modifyUpdateVoyagesFromRotationDataStoredProcedure();

        }

        private void modifyUpdateVoyagesFromRotationDataStoredProcedure()
        {
            this.Execute.Sql(@"
ALTER PROCEDURE [Fuel].[UpdateVoyagesFromRotationData]  
(@VoyageId BIGINT = NULL)
AS
BEGIN
	SET NOCOUNT ON;
    
    IF(@VoyageId IS NULL)
    BEGIN
		MERGE Fuel.Voyage AS target
		USING (SELECT Id, VoyageNumber, [Description], CompanyId,
		   VesselInCompanyId, StartDate, EndDate, IsActive FROM BasicInfo.RotationVoyagesView vv ) AS source (Id, VoyageNumber, [Description], CompanyId,
		   VesselInCompanyId, StartDate, EndDate, IsActive)
		ON (target.Id = source.Id)
		WHEN MATCHED THEN 
			UPDATE SET 
				VoyageNumber = CASE WHEN TARGET.IsLocked = 0 THEN source.VoyageNumber ELSE Target.VoyageNumber END, 
				[Description] =  CASE WHEN TARGET.IsLocked = 0 THEN source.[Description] ELSE Target.[Description] END, 
				CompanyId =  CASE WHEN TARGET.IsLocked = 0 THEN source.CompanyId ELSE Target.CompanyId END,
				VesselInCompanyId =  CASE WHEN TARGET.IsLocked = 0 THEN source.VesselInCompanyId ELSE Target.VesselInCompanyId END, 
				StartDate =  CASE WHEN TARGET.IsLocked = 0 THEN source.StartDate ELSE Target.StartDate END,
				EndDate =  CASE WHEN TARGET.IsLocked = 0 THEN source.EndDate ELSE Target.EndDate END,
				IsActive =  CASE WHEN TARGET.IsLocked = 0 THEN source.IsActive ELSE Target.IsActive END   
		WHEN NOT MATCHED THEN
		INSERT (Id, VoyageNumber, [Description], CompanyId,
		   VesselInCompanyId, StartDate, EndDate, IsActive, IsLocked)
		VALUES (source.Id, source.VoyageNumber, source.[Description], source.CompanyId,
		   source.VesselInCompanyId, source.StartDate, source.EndDate, source.IsActive, 0);
    END
    ELSE
    BEGIN
    	IF(EXISTS(SELECT 1 FROM Fuel.Voyage v WHERE v.Id = @VoyageId AND v.IsLocked = 0))
    		UPDATE fuelVoyages 
    		SET 
					VoyageNumber = sourceVoyages.VoyageNumber, 
					[Description] =  sourceVoyages.[Description], 
					CompanyId =  sourceVoyages.CompanyId,
					VesselInCompanyId =  sourceVoyages.VesselInCompanyId, 
					StartDate =  sourceVoyages.StartDate,
					EndDate =  sourceVoyages.EndDate,
					IsActive =  sourceVoyages.IsActive
    		FROM Fuel.Voyage fuelVoyages
    		INNER JOIN BasicInfo.RotationVoyagesView AS sourceVoyages ON fuelVoyages.Id = sourceVoyages.Id
    		WHERE fuelVoyages.Id = @VoyageId
    END
END
GO
");
        }

        public override void Down()
        {

        }

    }
}

