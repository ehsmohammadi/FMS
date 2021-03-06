--USE [StorageSpace]
--GO

/****** Object:  View [dbo].[HAFEZVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[HAFEZVoyagesView]'))
DROP VIEW [dbo].[HAFEZVoyagesView]
GO

/****** Object:  View [dbo].[HAFEZVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
CREATE VIEW [dbo].[HAFEZVoyagesView]
AS
SELECT   Project_1.ProjectID AS Id, Ship1.Code AS VesselCode, Ship1.Owner AS ShipOwnerId, Project_1.Code AS VoyageNumber, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.StartDateTime, 5, 0, '/'), 8, 0, '/'), 11, 0, ' '), 14, 0, ':') + ':00' AS DATETIME) AS StartDateTime, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.EndDateTime, 5, 0, '/'), 8, 0, '/'), 11, 0, ' '), 14, 0, ':') + ':00' AS DATETIME) AS EndDateTime, 
				ProjectEx_1.[TripType],
                ~ Project_1.Disabled AS IsActive
FROM    [NGSQLHDA,2433].HDABasis.dbo.Project AS Project_1 
		INNER JOIN [NGSQLHDA,2433].HDABasis.dbo.Ship AS Ship1 ON Project_1.ShipID = Ship1.ShipID 
		INNER JOIN [NGSQLHDA,2433].HDABasis.dbo.ProjectEx AS ProjectEx_1 ON Project_1.ProjectID = ProjectEx_1.ProjectID
GO
