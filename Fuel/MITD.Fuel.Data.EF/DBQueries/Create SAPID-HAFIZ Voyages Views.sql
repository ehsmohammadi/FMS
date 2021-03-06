--USE [StorageSpace]
--GO

/****** Object:  View [dbo].[SAPIDVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[SAPIDVoyagesView]'))
DROP VIEW [dbo].[SAPIDVoyagesView]
GO

/****** Object:  View [dbo].[HAFIZVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[HAFIZVoyagesView]'))
DROP VIEW [dbo].[HAFIZVoyagesView]
GO

/****** Object:  View [dbo].[HAFIZVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
CREATE VIEW [dbo].[HAFIZVoyagesView]
AS
SELECT   Project_1.ProjectID AS Id, Ship1.Code AS VesselCode, Ship1.Owner AS ShipOwnerId, Project_1.Code AS VoyageNumber, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.StartDateTime, 5, 0, '/'), 8, 0, '/'), 11, 0, ' '), 14, 0, ':') + ':00' AS DATETIME) AS StartDateTime, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.EndDateTime, 5, 0, '/'), 8, 0, '/'), 11, 0, ' '), 14, 0, ':') + ':00' AS DATETIME) AS EndDateTime, 
				ProjectEx_1.[TripType],
                ~ Project_1.Disabled AS IsActive
FROM    [NGSQLCNT,2433].CNTBasis.dbo.Project AS Project_1 
		INNER JOIN [NGSQLCNT,2433].CNTBasis.dbo.Ship AS Ship1 ON Project_1.ShipID = Ship1.ShipID 
		INNER JOIN [NGSQLCNT,2433].CNTBasis.dbo.ProjectEx AS ProjectEx_1 ON Project_1.ProjectID = ProjectEx_1.ProjectID
GO

/****** Object:  View [dbo].[SAPIDVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
CREATE VIEW [dbo].[SAPIDVoyagesView]
AS
SELECT   Project_1.ProjectID AS Id, Ship1.Code AS VesselCode, Ship1.Owner AS ShipOwnerId, Project_1.Code AS VoyageNumber, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.StartDateTime, 5, 0, '/'), 8, 0, '/'), 11, 0, ' '), 14, 0, ':') + ':00' AS DATETIME) AS StartDateTime, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.EndDateTime, 5, 0, '/'), 8, 0, '/'), 11, 0, ' '), 14, 0, ':') + ':00' AS DATETIME) AS EndDateTime, 
                ProjectEx_1.[TripType],
				~ Project_1.Disabled AS IsActive
FROM    [NGSQLBS,2433].BLKBasis.dbo.Project AS Project_1 
        INNER JOIN [NGSQLBS,2433].BLKBasis.dbo.Ship AS Ship1 ON Project_1.ShipID = Ship1.ShipID
		INNER JOIN [NGSQLBS,2433].BLKBasis.dbo.ProjectEx AS ProjectEx_1 ON Project_1.ProjectID = ProjectEx_1.ProjectID
GO


