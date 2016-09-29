﻿--USE [StorageSpace]
--GO

/****** Object:  View [BasicInfo].[VoyagesView]    Script Date: 27/4/2014 11:25:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[BasicInfo].[VoyagesView]'))
DROP VIEW [BasicInfo].[VoyagesView]
GO

CREATE VIEW [BasicInfo].[VoyagesView]
	AS	SELECT   CAST(VoyagesQuery.Id AS BIGINT) AS Id, VoyagesQuery.VoyageNumber COLLATE Arabic_CI_AS AS VoyageNumber, VoyagesQuery.VoyageNumber  COLLATE Arabic_CI_AS AS Description, CAST(VoyagesQuery.CompanyId AS BIGINT) 
            AS CompanyId, Fuel.VesselInCompany.Id AS VesselInCompanyId, VoyagesQuery.StartDateTime AS StartDate, VoyagesQuery.EndDateTime AS EndDate, 
            VoyagesQuery.IsActive
	FROM (SELECT Id, VesselCode, 1 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, 
				CASE WHEN [TripType] IN (5, 6, 7) THEN 1 -- As ShipOwner
					ELSE 2  --As Operation Company
				END AS CompanyId
            FROM dbo.SAPIDVoyagesView
				WHERE EndDateTime > '2014-01-01 00:00:00.000'
            UNION
            SELECT 10000000 + Id AS Id, VesselCode, 1 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, 
				CASE WHEN [TripType] IN (5, 6, 7) THEN 1 -- As ShipOwner
					ELSE 3   --As Operation Company
				END AS CompanyId
			FROM dbo.HAFIZVoyagesView 
				WHERE EndDateTime > '2014-01-01 00:00:00.000'
		) AS VoyagesQuery INNER JOIN
                Fuel.Vessel ON VoyagesQuery.VesselCode = Fuel.Vessel.Code COLLATE SQL_Latin1_General_CP1256_CI_AS INNER JOIN
                Fuel.VesselInCompany ON Fuel.VesselInCompany.VesselId = Fuel.Vessel.Id AND Fuel.VesselInCompany.CompanyId = VoyagesQuery.CompanyId
		UNION
			SELECT   Id, VoyageNumber, Description, CompanyId, VesselInCompanyId,  StartDate, EndDate, IsActive
			FROM      Fuel.Voyage
		
GO

--CREATE VIEW [BasicInfo].[VoyagesView]
--	AS	SELECT   * FROM Fuel.Voyage
--GO
