/*USE [StorageSpace]
GO*/

/****** Object:  View [BasicInfo].[VoyagesView]    Script Date: 27/4/2014 11:25:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/*IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[BasicInfo].[VoyagesView]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [BasicInfo].[VoyagesView]
	AS	SELECT   CAST(VoyagesQuery.Id AS BIGINT) AS Id, VoyagesQuery.VoyageNumber, VoyagesQuery.VoyageNumber AS Description, CAST(VoyagesQuery.CompanyId AS BIGINT) 
                AS CompanyId, Fuel.VesselInCompany.Id AS VesselInCompanyId, VoyagesQuery.StartDateTime AS StartDate, VoyagesQuery.EndDateTime AS EndDate, 
                VoyagesQuery.IsActive
		FROM      (SELECT   Id, VesselCode, 4 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, 1 AS CompanyId
                 FROM      dbo.SAPIDVoyagesView
                 UNION
                 SELECT   10000000 + Id AS Id, VesselCode, 4 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, 2 AS CompanyId
                 FROM      dbo.HAFIZVoyagesView) AS VoyagesQuery INNER JOIN
                Fuel.Vessel ON VoyagesQuery.VesselCode = Fuel.Vessel.Code COLLATE SQL_Latin1_General_CP1256_CI_AS INNER JOIN
                Fuel.VesselInCompany ON Fuel.VesselInCompany.VesselId = Fuel.Vessel.Id AND Fuel.VesselInCompany.CompanyId = VoyagesQuery.CompanyId'

GO*/

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[BasicInfo].[VoyagesView]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [BasicInfo].[VoyagesView]
	AS	SELECT   * FROM Fuel.Voyage'

GO
