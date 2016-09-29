using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FluentMigrator;

namespace MITD.Fuel.Data.EF.Migrations
{
    [Migration(36)]
    public class Migration_V36 : Migration
    {
        public override void Up()
        {
            createEventReportLinkedServer();

            createVesselEventReportTable();

            createUpdateVesselEventReportsStoredProcedure();

            createUpdateVesselEventReportsJob();

            modifyVesselEventReportsView();

            modifyGetFuelOriginalDataStoredProcedure();

            modifyGetVesselReportDataStoredProcedure();

            modifyGetVesselReportShipNameDataStoredProcedure();

            modifyGetVesselReportVoyageDataStoredProcedure();

            modifyGetVesselsRunningValuesReportStoredProcedure();

            modifyInventoryCompanyTableToAddBasisId();

            modifyBasicInfoCompanyViewToAddBasisId();
        }

        private void createEventReportLinkedServer()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(
                @"
USE [master]
--GO

--set implicit_transactions off

/****** Object:  LinkedServer [EVENTREPORT]    Script Date: 1/28/2016 2:51:42 PM ******/
EXEC master.dbo.sp_addlinkedserver @server = N'EVENTREPORT', @srvproduct=N'10.0.30.28', @provider=N'SQLNCLI', @datasrc=N'10.0.30.28', @catalog=N'EventReport'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'EVENTREPORT',@useself=N'False',@locallogin=NULL,@rmtuser=N'FMS',@rmtpassword='AmDhnoRx#'

--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'collation compatible', @optvalue=N'false'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'data access', @optvalue=N'true'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'dist', @optvalue=N'false'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'pub', @optvalue=N'false'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'rpc', @optvalue=N'false'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'rpc out', @optvalue=N'false'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'sub', @optvalue=N'false'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'connect timeout', @optvalue=N'0'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'collation name', @optvalue=null
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'lazy schema validation', @optvalue=N'false'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'query timeout', @optvalue=N'0'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'use remote collation', @optvalue=N'true'
--GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'remote proc transaction promotion', @optvalue=N'true'
--GO

--set implicit_transactions on
", conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        private void createUpdateVesselEventReportsJob()
        {
            Execute.Sql(@"
USE [msdb]
GO

/****** Object:  Job [StorageSpace - Update EventReports]    Script Date: 2/1/2016 4:02:10 PM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 2/1/2016 4:02:10 PM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'StorageSpace - Update EventReports', 
		@enabled=1, 
		@notify_level_eventlog=2, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'FMS\administrator', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Update Vessel.EventReports]    Script Date: 2/1/2016 4:02:10 PM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Update Vessel.EventReports', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'EXEC Vessel.UpdateVesselEventReports', 
		@database_name=N'StorageSpace', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'Update Vessel.EventReports Schedule', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=10, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20160201, 
		@active_end_date=99991231, 
		@active_start_time=60000, 
		@active_end_time=205959, 
		@schedule_uid=N'6edbae8c-41ef-4d0c-bba7-0f8d33b7012f'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO
");
            var connection = new SqlConnection(this.ConnectionString);

            this.Execute.Sql(string.Format("USE {0};\nGo", connection.Database));
        }

        private void modifyInventoryCompanyTableToAddBasisId()
        {
            Execute.Sql(@"
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
ALTER TABLE Inventory.Companies ADD
	BasisId bigint NOT NULL CONSTRAINT DF_Companies_BasisId DEFAULT 0
GO
ALTER TABLE Inventory.Companies SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
");
        }

        private void modifyBasicInfoCompanyViewToAddBasisId()
        {
            Execute.Sql(@"
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                ALTER VIEW [BasicInfo].[CompanyView]
                AS
	                SELECT c.Id, c.Code, c.Name, c.IsMemberOfHolding, c.BasisId
	                FROM [Inventory].Companies c
                GO
            ");
        }

        private void modifyGetVesselsRunningValuesReportStoredProcedure()
        {
            Execute.Sql(@"
ALTER PROCEDURE [Report].[GetVesselsRunningValuesReport]
(
	@CompanyId BIGINT,
	@FromDate DATETIME, 
	@ToDate DATETIME
)
AS
BEGIN

SET @FromDate = CONVERT(NVARCHAR(50), @FromDate , 111)

SET @ToDate = CONVERT(NVARCHAR(50), @ToDate , 111) + ' 23:59:59.000'

SELECT 
VesselCodes.VesselCode, 
vic.Name, 
vic.VesselStateCode,
AveragesAtSea.Average_ObsDist,
AveragesAtSea.Average_EngDist,
AveragesAtSea.Average_RPM,
ConsumptionInPort.Average_ConsInPortHO,
ConsumptionInPort.Total_ConsInPortHO,
ConsumptionInPort.Average_ConsInPortDO,
ConsumptionInPort.Total_ConsInPortDO,
ConsumptionAtSea.Average_ConsAtSeaHO,
ConsumptionAtSea.Total_ConsAtSeaHO,
ConsumptionAtSea.Average_ConsAtSeaDO,
ConsumptionAtSea.Total_ConsAtSeaDO

FROM 
(SELECT  RIGHT('0000' + [ShipCode], 4) AS VesselCode
	FROM Vessel.[EventReport] 
	GROUP BY RIGHT('0000' + [ShipCode], 4)
) VesselCodes 
LEFT JOIN 
(SELECT 
      RIGHT('0000' + [ShipCode], 4) AS VesselCode
      ,AVG([ObsDist]) AS Average_ObsDist
      ,AVG([EngDist]) AS Average_EngDist
      ,AVG([RPM]) AS Average_RPM
	FROM Vessel.[EventReport] verv
	WHERE ([InPortOrAtSea] = 1) AND (verv.[Date] BETWEEN @FromDate AND @ToDate)
	GROUP BY RIGHT('0000' + [ShipCode], 4)
) AveragesAtSea
ON VesselCodes.VesselCode = AveragesAtSea.VesselCode
LEFT JOIN 
(SELECT  
      RIGHT('0000' + [ShipCode], 4) AS VesselCode
      ,AVG([ConsInPortHO]) AS Average_ConsInPortHO
      ,SUM([ConsInPortHO]) AS Total_ConsInPortHO
      ,AVG([ConsInPortDO]) AS Average_ConsInPortDO
      ,SUM([ConsInPortDO]) AS Total_ConsInPortDO
	FROM Vessel.[EventReport] verv
	WHERE ([InPortOrAtSea] IN (2, 3)) AND (verv.[Date] BETWEEN @FromDate AND @ToDate)
	GROUP BY RIGHT('0000' + [ShipCode], 4)
) ConsumptionInPort
ON VesselCodes.VesselCode = ConsumptionInPort.VesselCode
LEFT JOIN
(SELECT  
      RIGHT('0000' + [ShipCode], 4) AS VesselCode      
      ,AVG([ConsAtSeaHO]) AS Average_ConsAtSeaHO
      ,SUM([ConsAtSeaHO]) AS Total_ConsAtSeaHO
      ,AVG([ConsAtSeaDO]) AS Average_ConsAtSeaDO
      ,SUM([ConsAtSeaDO]) AS Total_ConsAtSeaDO
	FROM Vessel.[EventReport] verv
	WHERE ([InPortOrAtSea] = 1) AND [FuelReportType] IN (1, 10, 12) AND (verv.[Date] BETWEEN @FromDate AND @ToDate)
	GROUP BY RIGHT('0000' + [ShipCode], 4)
) ConsumptionAtSea    
ON VesselCodes.VesselCode = ConsumptionAtSea.VesselCode
  INNER JOIN Fuel.Vessel v ON VesselCodes.VesselCode COLLATE SQL_Latin1_General_CP1_CI_AS = v.Code COLLATE SQL_Latin1_General_CP1_CI_AS   
  INNER JOIN Fuel.VesselInCompany vic ON vic.VesselId = v.Id 
WHERE vic.CompanyId = @CompanyId 
AND vic.Id IN (SELECT VesselInCompanyId FROM Fuel.FuelReport WHERE EventDate BETWEEN @FromDate AND @ToDate)

END

GO


");
        }

        private void modifyGetVesselReportVoyageDataStoredProcedure()
        {
            Execute.Sql(@"
ALTER PROCEDURE [Fuel].[GetVesselReportVoyageData]
(
	@ShipCode NVARCHAR(50) = NULL
)
AS
BEGIN
 
 	DECLARE @ResultTable AS TABLE (VoyageNo NVARCHAR(50), ShipCode NVARCHAR(200))

	INSERT INTO @ResultTable VALUES (' - All - ', NULL)

	INSERT INTO @ResultTable
		SELECT DISTINCT UPPER(REPLACE(REPLACE(er.VoyageNo, ' ', ''), '-', '')) AS VoyageNo, RIGHT('0000' + UPPER(er.ShipCode), 4) AS ShipCode 
		FROM Vessel.[EventReport] er
		WHERE (@ShipCode IS NULL OR RIGHT('0000' + UPPER(er.ShipCode), 4) = @ShipCode)


	SELECT * FROM @ResultTable
	ORDER BY VoyageNo

END

GO
");

        }

        private void modifyGetVesselReportShipNameDataStoredProcedure()
        {
            Execute.Sql(@"
ALTER PROCEDURE [Fuel].[GetVesselReportShipNameData]
AS
BEGIN
 
	DECLARE @ResultTable AS TABLE (ShipCode NVARCHAR(50), ShipName NVARCHAR(200))

	INSERT INTO @ResultTable VALUES (NULL, ' - All - ');

	WITH ShipsData AS (
		SELECT distinct RIGHT('0000' + UPPER(ier.ShipCode), 4) AS ShipCode,  UPPER(ier.ShipName) AS ShipName ,
			ROW_NUMBER() OVER(PARTITION BY RIGHT('0000' + UPPER(ier.ShipCode), 4) ORDER BY ier.Date DESC) AS rk
		FROM Vessel.[EventReport] ier)

	INSERT INTO @ResultTable
	SELECT s.ShipCode, s.ShipName
	  FROM ShipsData s
	 WHERE s.rk = 1
 
	SELECT * FROM @ResultTable ORDER BY ShipName
END

GO

");
        }

        private void modifyGetVesselReportDataStoredProcedure()
        {
            Execute.Sql(@"
ALTER PROCEDURE [Fuel].[GetVesselReportData] 
(
	@ShipCode NVARCHAR(10) = NULL,
	@VoyageNo NVARCHAR(50) = NULL,
	@FromDate DATETIME = NULL,
	@ToDate DATETIME = NULL,
	@PortTime FLOAT = NULL,
	@PortTimeMOL FLOAT = NULL,
	@LocationType NVARCHAR(10) = NULL
)
AS
BEGIN

	IF(@FromDate IS NULL) 
		SET @FromDate = '1753-01-01 00:00:00.000'

	IF(@ToDate IS NULL)
		SET @ToDate = '9999-12-31 23:59:59.997'


	DECLARE @FromYear  int,@FromMonth int, @FromDay int
	DECLARE @ToYear  int,@ToMonth int, @ToDay int
	
	--SET @FromYear = DATEPART(YEAR, @FromDate)
	--SET @FromMonth = DATEPART(MONTH, @FromDate)
	--SET @FromDay = DATEPART(DAY, @FromDate)

	--SET @ToYear = DATEPART(YEAR, @ToDate)
	--SET @ToMonth = DATEPART(MONTH, @ToDate)
	--SET @ToDay = DATEPART(DAY, @ToDate)

	DECLARE @MinPortTime FLOAT, @MaxPortTime FLOAT 

	SET @MinPortTime = @PortTime - ABS(ISNULL(@PortTimeMOL, 0))
	SET @MaxPortTime = @PortTime + ABS(ISNULL(@PortTimeMOL, 0))

	SET @MinPortTime = ISNULL(@MinPortTime, -100000)
	SET @MaxPortTime = ISNULL(@MaxPortTime, 100000)



	--DECLARE @FuelReportTypeNamesTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (1 , 'Noon Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (2 , 'End Of Voyage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (3 , 'Arrival Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (4 , 'Departure Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (5 , 'End Of Year')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (6 , 'End Of Month')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (7 , 'Charter-In End')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (8 , 'Charter-Out Start')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (9 , 'Dry Dock')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (10 , 'Begin Of Off-Hire')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (11 , 'Lay-Up')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (12 , 'End Of Off-Hire')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (13 , 'Begin Of Passage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (14 , 'End Of Passage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (15 , 'Bunkering')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (16 , 'Debunkering')
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()

	--DECLARE @LocationTypeTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (1 , 'At Sea')
	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (2 , 'In Port')
	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (3 , 'At Anchorage')

	SELECT er.*, 
		CASE ISNULL(er.CorrectionHo, 0) WHEN 0 THEN '' ELSE CorrectionTypeHo + ' ' + CAST(er.CorrectionHo AS NVARCHAR(50)) END AS CorrectionHoValue , 
		CASE ISNULL(er.CorrectionDo, 0) WHEN 0 THEN '' ELSE CorrectionTypeDo + ' ' + CAST(er.CorrectionDo AS NVARCHAR(50)) END AS CorrectionDoValue ,  
		CASE ISNULL(er.CorrectionFW, 0) WHEN 0 THEN '' ELSE CorrectionTypeFW + ' ' + CAST(er.CorrectionFW AS NVARCHAR(50)) END AS CorrectionFWValue , 
		CASE ISNULL(er.CorrectionMGOLS, 0) WHEN 0 THEN '' ELSE CorrectionTypeMGOLS + ' ' + CAST(er.CorrectionMGOLS AS NVARCHAR(50)) END AS CorrectionMGOLSValue 
	FROM Vessel.[EventReport] er
	WHERE (@ShipCode IS NULL OR RIGHT('0000' + UPPER(er.ShipCode), 4) = @ShipCode) AND 
		(@VoyageNo IS NULL OR UPPER(REPLACE(REPLACE(er.VoyageNo, ' ', ''), '-', '')) = UPPER(REPLACE(REPLACE(@VoyageNo, ' ', ''), '-', ''))) AND
		(DATEFROMPARTS(er.[Year], er.[Month], er.[Day]) BETWEEN @FromDate AND @ToDate) AND
		(ISNULL(er.PortTime, 0) BETWEEN @MinPortTime AND @MaxPortTime) AND 
		(@LocationType IS NULL OR @LocationType LIKE '%' + er.InPortOrAtSea + '%')
		

	ORDER BY [ShipCode],[Year],[Month],[Day],[Time]

END
GO
");
        }

        private void modifyGetFuelOriginalDataStoredProcedure()
        {
            Execute.Sql(@"
ALTER PROCEDURE [Fuel].[GetFuelOriginalData]
(
	@Code NVARCHAR(50)
)
AS
BEGIN

	--DECLARE @FuelReportTypeNamesTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (1 , 'Noon Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (2 , 'End Of Voyage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (3 , 'Arrival Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (4 , 'Departure Report')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (5 , 'End Of Year')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (6 , 'End Of Month')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (7 , 'Charter-In End')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (8 , 'Charter-Out Start')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (9 , 'Dry Dock')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (10 , 'Begin Of Off-Hire')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (11 , 'Lay-Up')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (12 , 'End Of Off-Hire')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (13 , 'Begin Of Passage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (14 , 'End Of Passage')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (15 , 'Bunkering')
	--INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES (16 , 'Debunkering')
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()
	----INSERT INTO @FuelReportTypeNamesTable([Type],NAME) VALUES ()

	--DECLARE @LocationTypeTable AS TABLE ([Type] INT, [Name] NVARCHAR(100))

	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (1 , 'At Sea')
	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (2 , 'In Port')
	--INSERT INTO @LocationTypeTable([Type],NAME) VALUES (3 , 'At Anchorage')

	DECLARE @IdToSearch BIGINT;
	SET @IdToSearch = CAST(@Code AS BIGINT);

	SELECT er.*, 
		CASE ISNULL(er.CorrectionHo, 0) WHEN 0 THEN '' ELSE CorrectionTypeHo + ' ' + CAST(er.CorrectionHo AS NVARCHAR(50)) END AS CorrectionHoValue , 
		CASE ISNULL(er.CorrectionDo, 0) WHEN 0 THEN '' ELSE CorrectionTypeDo + ' ' + CAST(er.CorrectionDo AS NVARCHAR(50)) END AS CorrectionDoValue ,  
		CASE ISNULL(er.CorrectionFW, 0) WHEN 0 THEN '' ELSE CorrectionTypeFW + ' ' + CAST(er.CorrectionFW AS NVARCHAR(50)) END AS CorrectionFWValue , 
		CASE ISNULL(er.CorrectionMGOLS, 0) WHEN 0 THEN '' ELSE CorrectionTypeMGOLS + ' ' + CAST(er.CorrectionMGOLS AS NVARCHAR(50)) END AS CorrectionMGOLSValue 
	FROM Vessel.[EventReport] er
	WHERE er.[ID]  = @IdToSearch

END
GO
");
        }

        private void modifyVesselEventReportsView()
        {
            Execute.Sql(@"

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [Report].[VesselEventReportsView]
AS
SELECT    *
FROM         Vessel.EventReport

GO

");
        }

        private void createUpdateVesselEventReportsStoredProcedure()
        {
Execute.Sql(@"
CREATE PROCEDURE Vessel.UpdateVesselEventReports
AS
BEGIN
MERGE Vessel.[EventReport] AS target
		USING (SELECT [ID]
      ,[DraftID]
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
      ,[Date]
      ,[DateIn]
      ,[DailyFuelCons]
      ,[Speed]
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
      ,[State]
      ,[ReportTypeName]
      ,[LocationTypeName]
		       FROM
		       (SELECT er.ID, er.DraftID, RIGHT('0000' + er.[ShipCode], 4) AS ShipCode, er.ConsNo, er.ShipName, er.VoyageNo, er.Year, er.Month, er.Day, er.PortName, er.PortTime, er.AtSeaLatitudeDegree, er.AtSeaLatitudeMinute, 
						er.AtSeaLongitudeDegree, er.AtSeaLongitudeMinute, er.ObsDist, er.EngDist, er.SteamTime, er.AvObsSpeed, er.AvEngSpeed, er.RPM, er.Slip, er.WindDir, er.WindForce, 
						er.SeaDir, er.SeaForce, er.ROBHO, er.ROBDO, er.ROBMGO, er.ROBFW, er.ConsInPortHO, er.ConsInPortDO, er.ConsInPortMGO, er.ConsInPortFW, er.ConsAtSeaHO, 
						er.ConsAtSeaDO, er.ConsAtSeaMGO, er.ConsAtSeaFW, er.ReceivedHO, er.ReceivedDO, er.ReceivedMGO, er.ReceivedFW, er.ETAPort, er.ETADate, er.Date, er.DateIn, 
						er.DailyFuelCons, er.Speed, er.IsSM, er.InPortOrAtSea, er.ImportDate, er.TransferHo, er.TransferDo, er.TransferFW, er.TransferMGOLS, er.CorrectionHo, er.CorrectionDo, 
						er.CorrectionFW, er.CorrectionMGOLS, er.CorrectionTypeHo, er.CorrectionTypeDo, er.CorrectionTypeFW, er.CorrectionTypeMGOLS, er.Time, er.FuelReportType, er.State, 
						ert.Name AS ReportTypeName, lt.Name AS LocationTypeName
				FROM    [EVENTREPORT].EventReport.dbo.EventReport AS er LEFT OUTER JOIN
						[EVENTREPORT].EventReport.dbo.EventReportTypes AS ert ON er.FuelReportType = ert.Type LEFT OUTER JOIN
						[EVENTREPORT].EventReport.dbo.LocationTypes AS lt ON er.InPortOrAtSea = lt.Type) verv WHERE verv.ID IN (SELECT Code FROM Fuel.FuelReport fr)) AS source ([ID]
      ,[DraftID]
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
      ,[Date]
      ,[DateIn]
      ,[DailyFuelCons]
      ,[Speed]
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
      ,[State]
      ,[ReportTypeName]
      ,[LocationTypeName])
		ON (target.Id = source.Id)
		WHEN MATCHED THEN 
			UPDATE SET 
	   [ID]                     =source.[ID]
      ,[DraftID]				=source.[DraftID]
      ,[ShipCode]				=source.[ShipCode]
      ,[ConsNo]					=source.[ConsNo]
      ,[ShipName]				=source.[ShipName]
      ,[VoyageNo]				=source.[VoyageNo]
      ,[Year]					=source.[Year]
      ,[Month]					=source.[Month]
      ,[Day]					=source.[Day]
      ,[PortName]				=source.[PortName]
      ,[PortTime]				=source.[PortTime]
      ,[AtSeaLatitudeDegree]	=source.[AtSeaLatitudeDegree]
      ,[AtSeaLatitudeMinute]	=source.[AtSeaLatitudeMinute]
      ,[AtSeaLongitudeDegree]	=source.[AtSeaLongitudeDegree]
      ,[AtSeaLongitudeMinute]	=source.[AtSeaLongitudeMinute]
      ,[ObsDist]				=source.[ObsDist]
      ,[EngDist]				=source.[EngDist]
      ,[SteamTime]				=source.[SteamTime]
      ,[AvObsSpeed]				=source.[AvObsSpeed]
      ,[AvEngSpeed]				=source.[AvEngSpeed]
      ,[RPM]					=source.[RPM]
      ,[Slip]					=source.[Slip]
      ,[WindDir]				=source.[WindDir]
      ,[WindForce]				=source.[WindForce]
      ,[SeaDir]					=source.[SeaDir]
      ,[SeaForce]				=source.[SeaForce]
      ,[ROBHO]					=source.[ROBHO]
      ,[ROBDO]					=source.[ROBDO]
      ,[ROBMGO]					=source.[ROBMGO]
      ,[ROBFW]					=source.[ROBFW]
      ,[ConsInPortHO]			=source.[ConsInPortHO]
      ,[ConsInPortDO]			=source.[ConsInPortDO]
      ,[ConsInPortMGO]			=source.[ConsInPortMGO]
      ,[ConsInPortFW]			=source.[ConsInPortFW]
      ,[ConsAtSeaHO]			=source.[ConsAtSeaHO]
      ,[ConsAtSeaDO]			=source.[ConsAtSeaDO]
      ,[ConsAtSeaMGO]			=source.[ConsAtSeaMGO]
      ,[ConsAtSeaFW]			=source.[ConsAtSeaFW]
      ,[ReceivedHO]				=source.[ReceivedHO]
      ,[ReceivedDO]				=source.[ReceivedDO]
      ,[ReceivedMGO]			=source.[ReceivedMGO]
      ,[ReceivedFW]				=source.[ReceivedFW]
      ,[ETAPort]				=source.[ETAPort]
      ,[ETADate]				=source.[ETADate]
      ,[Date]					=source.[Date]
      ,[DateIn]					=source.[DateIn]
      ,[DailyFuelCons]			=source.[DailyFuelCons]
      ,[Speed]					=source.[Speed]
      ,[IsSM]					=source.[IsSM]
      ,[InPortOrAtSea]			=source.[InPortOrAtSea]
      ,[ImportDate]				=source.[ImportDate]
      ,[TransferHo]				=source.[TransferHo]
      ,[TransferDo]				=source.[TransferDo]
      ,[TransferFW]				=source.[TransferFW]
      ,[TransferMGOLS]			=source.[TransferMGOLS]
      ,[CorrectionHo]			=source.[CorrectionHo]
      ,[CorrectionDo]			=source.[CorrectionDo]
      ,[CorrectionFW]			=source.[CorrectionFW]
      ,[CorrectionMGOLS]		=source.[CorrectionMGOLS]
      ,[CorrectionTypeHo]		=source.[CorrectionTypeHo]
      ,[CorrectionTypeDo]		=source.[CorrectionTypeDo]
      ,[CorrectionTypeFW]		=source.[CorrectionTypeFW]
      ,[CorrectionTypeMGOLS]	=source.[CorrectionTypeMGOLS]
      ,[Time]					=source.[Time]
      ,[FuelReportType]			=source.[FuelReportType]
      ,[State]					=source.[State]
      ,[ReportTypeName]			=source.[ReportTypeName]
      ,[LocationTypeName]		=source.[LocationTypeName]
		WHEN NOT MATCHED THEN
		INSERT
		([ID]
           ,[DraftID]
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
           ,[Date]
           ,[DateIn]
           ,[DailyFuelCons]
           ,[Speed]
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
           ,[State]
           ,[ReportTypeName]
           ,[LocationTypeName])
     VALUES
     (source.[ID]
	  ,source.[DraftID]
	  ,source.[ShipCode]
	  ,source.[ConsNo]
	  ,source.[ShipName]
	  ,source.[VoyageNo]
	  ,source.[Year]
	  ,source.[Month]
	  ,source.[Day]
	  ,source.[PortName]
	  ,source.[PortTime]
	  ,source.[AtSeaLatitudeDegree]
	  ,source.[AtSeaLatitudeMinute]
	  ,source.[AtSeaLongitudeDegree]
	  ,source.[AtSeaLongitudeMinute]
	  ,source.[ObsDist]
	  ,source.[EngDist]
	  ,source.[SteamTime]
	  ,source.[AvObsSpeed]
	  ,source.[AvEngSpeed]
	  ,source.[RPM]
	  ,source.[Slip]
	  ,source.[WindDir]
	  ,source.[WindForce]
	  ,source.[SeaDir]
	  ,source.[SeaForce]
	  ,source.[ROBHO]
	  ,source.[ROBDO]
	  ,source.[ROBMGO]
	  ,source.[ROBFW]
	  ,source.[ConsInPortHO]
	  ,source.[ConsInPortDO]
	  ,source.[ConsInPortMGO]
	  ,source.[ConsInPortFW]
	  ,source.[ConsAtSeaHO]
	  ,source.[ConsAtSeaDO]
	  ,source.[ConsAtSeaMGO]
	  ,source.[ConsAtSeaFW]
	  ,source.[ReceivedHO]
	  ,source.[ReceivedDO]
	  ,source.[ReceivedMGO]
	  ,source.[ReceivedFW]
	  ,source.[ETAPort]
	  ,source.[ETADate]
	  ,source.[Date]
	  ,source.[DateIn]
	  ,source.[DailyFuelCons]
	  ,source.[Speed]
	  ,source.[IsSM]
	  ,source.[InPortOrAtSea]
	  ,source.[ImportDate]
	  ,source.[TransferHo]
	  ,source.[TransferDo]
	  ,source.[TransferFW]
	  ,source.[TransferMGOLS]
	  ,source.[CorrectionHo]
	  ,source.[CorrectionDo]
	  ,source.[CorrectionFW]
	  ,source.[CorrectionMGOLS]
	  ,source.[CorrectionTypeHo]
	  ,source.[CorrectionTypeDo]
	  ,source.[CorrectionTypeFW]
	  ,source.[CorrectionTypeMGOLS]
	  ,source.[Time]
	  ,source.[FuelReportType]
	  ,source.[State]
	  ,source.[ReportTypeName]
	  ,source.[LocationTypeName]);
END
GO

GRANT EXECUTE ON Vessel.UpdateVesselEventReports TO [public]
GO
");
        }

        private void createVesselEventReportTable()
        {
            Execute.Sql(@"

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

IF OBJECT_ID('Vessel.[EventReport]', 'U') IS NOT NULL
  DROP TABLE Vessel.[EventReport]; 

CREATE TABLE Vessel.[EventReport](
	[ID] int NOT NULL,
	[DraftID] [int] NULL,
	[ShipCode] [nvarchar](4) COLLATE Arabic_CI_AS NULL,
	[ConsNo] [nvarchar](255) COLLATE Arabic_CI_AS NULL,
	[ShipName] [nvarchar](255) COLLATE Arabic_CI_AS NULL,
	[VoyageNo] [nvarchar](255) COLLATE Arabic_CI_AS NULL,
	[Year] [int] NULL,
	[Month] [int] NULL,
	[Day] [int] NULL,
	[PortName] [nvarchar](255) COLLATE Arabic_CI_AS NULL,
	[PortTime] [float] NULL,
	[AtSeaLatitudeDegree] [float] NULL,
	[AtSeaLatitudeMinute] [float] NULL,
	[AtSeaLongitudeDegree] [float] NULL,
	[AtSeaLongitudeMinute] [float] NULL,
	[ObsDist] [float] NULL,
	[EngDist] [float] NULL,
	[SteamTime] [float] NULL,
	[AvObsSpeed] [decimal](5, 2) NULL,
	[AvEngSpeed] [float] NULL,
	[RPM] [int] NULL,
	[Slip] [float] NULL,
	[WindDir] [float] NULL,
	[WindForce] [float] NULL,
	[SeaDir] [float] NULL,
	[SeaForce] [float] NULL,
	[ROBHO] [decimal](7, 3) NULL,
	[ROBDO] [decimal](7, 3) NULL,
	[ROBMGO] [decimal](7, 3) NULL,
	[ROBFW] [decimal](7, 3) NULL,
	[ConsInPortHO] [decimal](7, 3) NULL,
	[ConsInPortDO] [decimal](7, 3) NULL,
	[ConsInPortMGO] [decimal](7, 3) NULL,
	[ConsInPortFW] [decimal](7, 3) NULL,
	[ConsAtSeaHO] [decimal](7, 3) NULL,
	[ConsAtSeaDO] [decimal](7, 3) NULL,
	[ConsAtSeaMGO] [decimal](7, 3) NULL,
	[ConsAtSeaFW] [decimal](7, 3) NULL,
	[ReceivedHO] [decimal](7, 3) NULL,
	[ReceivedDO] [decimal](7, 3) NULL,
	[ReceivedMGO] [decimal](7, 3) NULL,
	[ReceivedFW] [decimal](7, 3) NULL,
	[ETAPort] [nvarchar](255) COLLATE Arabic_CI_AS NULL,
	[ETADate] [nvarchar](255) COLLATE Arabic_CI_AS NULL,
	[Date] [datetime2](3) NULL,
	[DateIn] [datetime2](3) NULL,
	[DailyFuelCons] [float] NULL,
	[Speed] [decimal](5, 2) NULL,
	[IsSM] [bit] NULL,
	[InPortOrAtSea] [nvarchar](50) COLLATE Arabic_CI_AS NULL,
	[ImportDate] [char](10) COLLATE Arabic_CI_AS NULL,
	[TransferHo] [decimal](7, 3) NULL,
	[TransferDo] [decimal](7, 3) NULL,
	[TransferFW] [decimal](7, 3) NULL,
	[TransferMGOLS] [decimal](7, 3) NULL,
	[CorrectionHo] [decimal](7, 3) NULL,
	[CorrectionDo] [decimal](7, 3) NULL,
	[CorrectionFW] [decimal](7, 3) NULL,
	[CorrectionMGOLS] [decimal](7, 3) NULL,
	[CorrectionTypeHo] [char](1) COLLATE Arabic_CI_AS NULL,
	[CorrectionTypeDo] [char](1) COLLATE Arabic_CI_AS NULL,
	[CorrectionTypeFW] [char](1) COLLATE Arabic_CI_AS NULL,
	[CorrectionTypeMGOLS] [char](1) COLLATE Arabic_CI_AS NULL,
	[Time] [sql_variant] NULL,
	[FuelReportType] [tinyint] NULL,
	[State] [tinyint] NULL,
	[ReportTypeName] [nvarchar](255) COLLATE Arabic_CI_AS NULL,
	[LocationTypeName] [nvarchar](255) COLLATE Arabic_CI_AS NULL,
 CONSTRAINT [PK_Vessel_EventReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
");
        }

        public override void Down()
        {

        }

    }
}

