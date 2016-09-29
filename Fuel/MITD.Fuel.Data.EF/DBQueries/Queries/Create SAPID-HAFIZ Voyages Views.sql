USE [StorageSpace]
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'SAPIDVoyagesView', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'SAPIDVoyagesView'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'SAPIDVoyagesView', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'SAPIDVoyagesView'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPaneCount' , N'SCHEMA',N'dbo', N'VIEW',N'HAFIZVoyagesView', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPaneCount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HAFIZVoyagesView'

GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'HAFIZVoyagesView', NULL,NULL))
EXEC sys.sp_dropextendedproperty @name=N'MS_DiagramPane1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'HAFIZVoyagesView'

GO
/****** Object:  View [dbo].[SAPIDVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[SAPIDVoyagesView]'))
DROP VIEW [dbo].[SAPIDVoyagesView]
GO
/****** Object:  View [dbo].[HAFIZVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[HAFIZVoyagesView]'))
DROP VIEW [dbo].[HAFIZVoyagesView]
GO
/****** Object:  View [dbo].[HAFIZVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[HAFIZVoyagesView]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[HAFIZVoyagesView]
AS
SELECT   Project_1.ProjectID AS Id, Ship1.Code AS VesselCode, Ship1.Owner AS ShipOwnerId, Project_1.Code AS VoyageNumber, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.StartDateTime, 5, 0, ''/''), 8, 0, ''/''), 11, 0, '' ''), 14, 0, '':'') + '':00'' AS DATETIME) AS StartDateTime, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.EndDateTime, 5, 0, ''/''), 8, 0, ''/''), 11, 0, '' ''), 14, 0, '':'') + '':00'' AS DATETIME) AS EndDateTime, 
                ~ Project_1.Disabled AS IsActive
FROM      [NGSQLCNT,2433].CNTRotation.dbo.Project AS Project_1 INNER JOIN
                [NGSQLCNT,2433].CNTRotation.dbo.Ship AS Ship1 ON Project_1.ShipID = Ship1.ShipID
' 
GO
/****** Object:  View [dbo].[SAPIDVoyagesView]    Script Date: 14/5/2014 11:13:53 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[SAPIDVoyagesView]'))
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[SAPIDVoyagesView]
AS
SELECT   Project_1.ProjectID AS Id, Ship1.Code AS VesselCode, Ship1.Owner AS ShipOwnerId, Project_1.Code AS VoyageNumber, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.StartDateTime, 5, 0, ''/''), 8, 0, ''/''), 11, 0, '' ''), 14, 0, '':'') + '':00'' AS DATETIME) AS StartDateTime, 
                CAST(STUFF(STUFF(STUFF(STUFF(Project_1.EndDateTime, 5, 0, ''/''), 8, 0, ''/''), 11, 0, '' ''), 14, 0, '':'') + '':00'' AS DATETIME) AS EndDateTime, 
                ~ Project_1.Disabled AS IsActive
FROM      [NGSQLBS,2433].BLKRotation.dbo.Project AS Project_1 INNER JOIN
                [NGSQLBS,2433].BLKRotation.dbo.Ship AS Ship1 ON Project_1.ShipID = Ship1.ShipID
' 
GO
