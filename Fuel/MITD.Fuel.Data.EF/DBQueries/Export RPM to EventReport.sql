--DECLARE @RPMInfoIdsTable AS TABLE (ID INT)
----WHERE id IN (20356, 20378, 20396, 20411, 20424)
--INSERT @RPMInfoIdsTable (ID) VALUES()
--so so123
INSERT INTO /*[10.9.80.15].*/[VoyageCost].[dbo].[EventReport]
           ([DraftID]
           ,[ShipCode]
           ,[ConsNo]
           ,[ShipName]
           ,[VoyageNo]
           ,[Year]
           ,[Month]
           ,[Day]
           ,[PortName]
           ,[PortTime]
           ,[AtSeaLatitudeDegree]
           ,[AtSeaLatitudeMinute]
           ,[AtSeaLongitudeDegree]
           ,[AtSeaLongitudeMinute]
           ,[ObsDist]
           ,[EngDist]
           ,[SteamTime]
           ,[AvObsSpeed]
           ,[AvEngSpeed]
           ,[RPM]
           ,[Slip]
           ,[WindDir]
           ,[WindForce]
           ,[SeaDir]
           ,[SeaForce]
           ,[ROBHO]
           ,[ROBDO]
           ,[ROBMGO]
           ,[ROBFW]
           ,[ConsInPortHO]
           ,[ConsInPortDO]
           ,[ConsInPortMGO]
           ,[ConsInPortFW]
           ,[ConsAtSeaHO]
           ,[ConsAtSeaDO]
           ,[ConsAtSeaMGO]
           ,[ConsAtSeaFW]
           ,[ReceivedHO]
           ,[ReceivedDO]
           ,[ReceivedMGO]
           ,[ReceivedFW]
           ,[ETAPort]
           ,[ETADate]
           --,[Date]
           ,[DateIn]
           --,[DailyFuelCons]
           --,[Speed]
           ,[IsSM]
           ,[InPortOrAtSea]
           ,[ImportDate]
           ,[TransferHo]
           ,[TransferDo]
           ,[TransferFW]
           ,[TransferMGOLS]
           ,[CorrectionHo]
           ,[CorrectionDo]
           ,[CorrectionFW]
           ,[CorrectionMGOLS]
           ,[CorrectionTypeHo]
           ,[CorrectionTypeDo]
           ,[CorrectionTypeFW]
           ,[CorrectionTypeMGOLS]
           ,[Time]
           ,[FuelReportType]
           ,[State])
     SELECT 
           --rpm.[ID],
           rpm.[DraftID]
           ,s.[ShipID]
           ,rpm.[ConsNo]
           ,rpm.[ShipName]
           ,rpm.[VoyageNo]
           ,rpm.[Year]
           ,rpm.[Month]
           ,rpm.[Day]
           ,rpm.[PortName]
           ,rpm.[PortTime]
           ,rpm.[AtSeaLatitudeDegree]
           ,rpm.[AtSeaLatitudeMinute]
           ,rpm.[AtSeaLongitudeDegree]
           ,rpm.[AtSeaLongitudeMinute]
           ,rpm.[ObsDist]
           ,rpm.[EngDist]
           ,rpm.[SteamTime]
           ,rpm.[AvObsSpeed]
           ,rpm.[AvEngSpeed]
           ,rpm.[RPM]
           ,rpm.[Slip]
           ,rpm.[WindDir]
           ,rpm.[WindForce]
           ,rpm.[SeaDir]
           ,rpm.[SeaForce]
           ,rpm.[ROBHO]
           ,rpm.[ROBDO]
           ,rpm.[ROBMGO]
           ,rpm.[ROBFW]
           ,rpm.[ConsInPortHO]
           ,rpm.[ConsInPortDO]
           ,rpm.[ConsInPortMGO]
           ,rpm.[ConsInPortFW]
           ,rpm.[ConsAtSeaHO]
           ,rpm.[ConsAtSeaDO]
           ,rpm.[ConsAtSeaMGO]
           ,rpm.[ConsAtSeaFW]
           ,rpm.[ReceivedHO]
           ,rpm.[ReceivedDO]
           ,rpm.[ReceivedMGO]
           ,rpm.[ReceivedFW]
           ,rpm.[ETAPort]
           ,rpm.[ETADate]
           --,rpm.[Date]
           ,rpm.[DateIn]
           --,rpm.[DailyFuelCons]
           --,rpm.[Speed]
           ,rpm.[IsSM]
           ,rpm.[InPortOrAtSea]
           ,rpm.[ImportDate]
           ,rpm.[TransferHo]
           ,rpm.[TransferDo]
           ,rpm.[TransferFW]
           ,rpm.[TransferMGOLS]
           ,rpm.[CorrectionHo]
           ,rpm.[CorrectionDo]
           ,rpm.[CorrectionFW]
           ,rpm.[CorrectionMGOLS]
           ,CASE WHEN rpm.[CorrectionTypeHo]     IS NULL OR rpm.[CorrectionTypeHo]       = 0 THEN '+' ELSE '-' END AS [CorrectionTypeHo]
           ,CASE WHEN rpm.[CorrectionTypeDo]     IS NULL OR rpm.[CorrectionTypeDo]       = 0 THEN '+' ELSE '-' END AS [CorrectionTypeDo]
           ,CASE WHEN rpm.[CorrectionTypeFW]     IS NULL OR rpm.[CorrectionTypeFW]       = 0 THEN '+' ELSE '-' END AS [CorrectionTypeFW]       
           ,CASE WHEN rpm.[CorrectionTypeMGOLS]	 IS NULL OR rpm.[CorrectionTypeMGOLS]	 = 0 THEN '+' ELSE '-' END AS [CorrectionTypeMGOLS]	 
           ,rpm.[Time]
           ,rpm.[FuelReportType]
           ,rpm.[State]
     FROM /*[10.9.80.15].*/[VoyageCost].[dbo].[RPMInfo] rpm
		INNER JOIN /*[10.9.80.15].*/[VoyageCost].[dbo].[Ships] s ON rpm.ShipID = s.ID
     WHERE rpm.[ID] IN (20356, 20378, 20396, 20411, 20424)
GO


