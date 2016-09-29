DELETE FROM [Fuel].[Voyage];

UPDATE [Fuel].VesselInCompany SET VesselStateCode = 3 WHERE VesselStateCode = 4;
UPDATE [Fuel].VesselInCompany SET VesselStateCode = 2 WHERE VesselStateCode = 1;

INSERT INTO [Fuel].[Voyage]
           ([VoyageNumber]
           ,[Description]
           ,[VesselInCompanyId]
           ,[CompanyId]
           ,[StartDate]
           ,[EndDate]
           ,[IsActive])
SELECT * FROM (

SELECT
	[VoyageNumber],
	[VoyageNumber] AS Description,
	FVinC.Id as VesselInCompanyId,
	FVinC.CompanyId,
	StartDateTime AS StartDate,
	EndDateTime AS EndDate,
	IsActive
FROM [StorageSpace].[dbo].[HAFIZVoyagesView] HV inner join [StorageSpace].Fuel.Vessel FVs ON 
HV.VesselCode COLLATE Arabic_CI_AS = FVs.[Code]
INNER JOIN [StorageSpace].Fuel.VesselInCompany FVinC ON FVinC.VesselId = FVs.Id AND FVinC.VesselStateCode IN (2,4) --Charter In / Owned
WHERE 
HV.IsActive = 1

UNION
---------------------------------------------------------------
SELECT
	[VoyageNumber],
	[VoyageNumber] AS Description,
	FVinC.Id as VesselInCompanyId,
	FVinC.CompanyId,
	StartDateTime AS StartDate,
	EndDateTime AS EndDate,
	IsActive
FROM [StorageSpace].[dbo].[SAPIDVoyagesView] SV inner join [StorageSpace].Fuel.Vessel FVs ON 
SV.VesselCode COLLATE Arabic_CI_AS = FVs.[Code]
INNER JOIN [StorageSpace].Fuel.VesselInCompany FVinC ON FVinC.VesselId = FVs.Id AND FVinC.VesselStateCode IN (2,4) --Charter In / Owned
WHERE 
SV.IsActive = 1
) Q
ORDER BY CompanyId, VesselInCompanyId

UPDATE [Fuel].VesselInCompany SET VesselStateCode = 4 WHERE VesselStateCode = 3;
UPDATE [Fuel].VesselInCompany SET VesselStateCode = 1 WHERE VesselStateCode = 2;