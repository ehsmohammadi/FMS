
/****** Object:  View [dbo].[EOVReportsView]    Script Date: 9/14/2014 12:01:10 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Script for SelectTopNRows command from SSMS  ******/
CREATE VIEW Fuel.[EOVReportsView]
AS
/****** Script for SelectTopNRows command from SSMS  ******/
SELECT --fvslco.Id,
      2 as [DraftID]
      ,(SELECT TOP 1 ID FROM [10.9.80.15].[VoyageCost].[dbo].Ships s where s.ShipID = VRShipCodes.Code)AS [ShipID]
	  
	  --Ships.ID AS [ShipID]
      ,null AS [ConsNo]
	  ,(SELECT TOP 1 Name FROM [10.9.80.15].[VoyageCost].[dbo].Ships s where s.ShipID = VRShipCodes.Code) AS [ShipName]
      --,Ships.Name AS [ShipName]
      ,fvg.VoyageNumber AS [VoyageNo]
      ,DATEPART(YEAR, fvg.EndDate) AS [Year]
      ,DATEPART(MONTH, fvg.EndDate) AS [Month]
      ,DATEPART(DAY, fvg.EndDate) AS [Day]
	  --,DATEPART(HOUR, fvg.EndDate) AS [Hour]
   --   ,DATEPART(Minute, fvg.EndDate) AS [Minute]
   --   ,DATEPART(Second, fvg.EndDate) AS [Second]
	  ,CAST(fvg.EndDate AS TIME) AS [Time]
      ,2 AS [FuelReportType]
      ,'EOV Port' AS [PortName]
      ,0 AS [PortTime]	 
      ,27 AS [AtSeaLatitudeDegree]
      ,50 AS [AtSeaLatitudeMinute]
      ,121 AS [AtSeaLongitudeDegree]
      ,58 AS [AtSeaLongitudeMinute]
      ,0 AS [ObsDist]
      ,0 AS [EngDist]
      ,0 AS [SteamTime]
      ,0 AS [AvObsSpeed]
      ,0 AS [AvEngSpeed]
      ,0 AS [RPM]
      ,0 AS [Slip]
      ,0 AS [WindDir]
      ,0 AS [WindForce]
      ,0 AS [SeaDir]
      ,0 AS [SeaForce]
      ,66 AS [ROBHO]
      ,66 AS [ROBDO]
      ,66 AS [ROBMGO]
      ,66 AS [ROBFW]
      ,33 AS [ConsInPortHO]
      ,33 AS [ConsInPortDO]
      ,33 AS [ConsInPortMGO]
      ,33 AS [ConsInPortFW]
      ,33 AS [ConsAtSeaHO]
      ,33 AS [ConsAtSeaDO]
      ,33 AS [ConsAtSeaMGO]
      ,33 AS [ConsAtSeaFW]
      ,0 AS [ReceivedHO]
      ,0 AS [ReceivedDO]
      ,0 AS [ReceivedMGO]
      ,0 AS [ReceivedFW]
      ,'EOV Port' AS [ETAPort]
	  ,CONVERT (nvarchar(15), fvg.EndDate, 111) AS [ETADate]
      ,GETDATE() AS [DateIn]
      ,0 AS [IsSM]
      ,1 AS [InPortOrAtSea]
      ,CONVERT (nvarchar(15), GETDATE(), 111) AS [ImportDate],
	  VRShipCodes.Code AS ShipCode,
	  fvg.Id AS VoyageId
FROM [StorageSpace].BasicInfo.VoyagesView fvg 
	inner join [StorageSpace].Fuel.VesselInCompany fvslco ON fvg.VesselInCompanyId = fvslco.Id
	inner join [StorageSpace].Fuel.Vessel fvsl ON fvsl.Id = fvslco.VesselId
	INNER JOIN (SELECT DISTINCT ShipID AS Code FROM [10.9.80.15].[VoyageCost].[dbo].Ships s) VRShipCodes ON VRShipCodes.Code = fvsl.Code
WHERE 
	fvg.EndDate IS NOT NULL AND 
	fvg.EndDate >= '2014-06-01 00:00:00.000'
GO

GRANT SELECT ON Fuel.[EOVReportsView] TO [public]
GO


--select * from [StorageSpace].BasicInfo.VoyagesView