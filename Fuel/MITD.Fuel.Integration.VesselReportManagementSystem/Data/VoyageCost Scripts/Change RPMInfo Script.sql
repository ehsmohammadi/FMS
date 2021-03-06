/*
   Monday, July 28, 20142:38:42 PM
   User: 
   Server: .\sqlExpress
   Database: VoyageCost
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RPMInfo ADD
	[Time] time(7) NOT NULL CONSTRAINT DF_RPMInfo_Time DEFAULT (CONVERT([time],'12:00:00',(0))),
	FuelReportType tinyint NOT NULL CONSTRAINT DF_RPMInfo_FuelReportType DEFAULT 1,
	State tinyint NULL
GO

ALTER TABLE dbo.RPMInfo SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
