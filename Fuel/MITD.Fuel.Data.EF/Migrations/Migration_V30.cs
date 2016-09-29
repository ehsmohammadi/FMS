
using System.Data;
using System.Data.SqlClient;
using FluentMigrator;
using MITD.Fuel.Data.EF.Migrations;

namespace MITD.Data.EF.Migrations
{
    [Migration(30)]
   public class Migration_V30:Migration
    {
        public override void Down()
        {
            
        }

        public override void Up()
        {
            this.Create.Table("IRISLVoyages").InSchema(Migration_Initial.BASIC_INFO_SCHEMA)
              .WithColumn("Id").AsInt64().NotNullable().PrimaryKey()
              .WithColumn("VoyageNumber").AsString(200)
              .WithColumn("Description").AsString(200)
              .WithColumn("VesselInCompanyId").AsInt64().NotNullable().Indexed()
                    .ForeignKey("FK_IRISLVoyages_VesselInCompanyId_VesselInCompany_Id", Migration_Initial.FUEL_SCHEMA, "VesselInCompany", "Id")
              .WithColumn("CompanyId").AsInt64().NotNullable().Indexed()
              .WithColumn("StartDate").AsDateTime().NotNullable()
              .WithColumn("EndDate").AsDateTime().Nullable()
              .WithColumn("IsActive").AsBoolean().NotNullable();


            this.Execute.Sql(@"INSERT INTO BasicInfo.IRISLVoyages        
                SELECT Id, VoyageNumber, [Description], CompanyId,
                       VesselInCompanyId, StartDate, EndDate, IsActive 
                FROM Fuel.Voyage");

            this.Execute.Sql("DROP VIEW [BasicInfo].[VoyagesView]");

            createNewRotationVoyagesView();

            createUpdateVoyagesFromRotationDataStoredProcedure();

            this.Execute.Sql("exec [Fuel].[UpdateVoyagesFromRotationData]");

            createUpdateVoyagesFromRotationDataJob();
        }

        private void createUpdateVoyagesFromRotationDataJob()
        {
            this.Execute.Sql(@"
/****** Object:  Job [UpdateVoyagesFromRotationData Job]    Script Date: 8/30/2015 11:08:32 AM ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]    Script Date: 8/30/2015 11:08:32 AM ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'StorageSpace.UpdateVoyagesFromRotationData Job', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'No description available.', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [Execute [Fuel].[UpdateVoyagesFromRotationData] Step]    Script Date: 8/30/2015 11:08:32 AM ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'Execute StorageSpace.[Fuel].[UpdateVoyagesFromRotationData] Step', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'exec StorageSpace.[Fuel].[UpdateVoyagesFromRotationData]', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'StorageSpace.UpdateVoyagesFromRotationData Every 15 minutes', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=15, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20150830, 
		@active_end_date=99991231, 
		@active_start_time=40000, 
		@active_end_time=220000, 
		@schedule_uid=N'6ccb4a22-75f5-4b21-a83c-402e9b971853'
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
        }

        private void createUpdateVoyagesFromRotationDataStoredProcedure()
        {
            this.Execute.Sql(@"
CREATE PROCEDURE [Fuel].[UpdateVoyagesFromRotationData]  
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	MERGE Fuel.Voyage AS target
    USING (SELECT Id, VoyageNumber, [Description], CompanyId,
       VesselInCompanyId, StartDate, EndDate, IsActive FROM BasicInfo.RotationVoyagesView vv ) AS source (Id, VoyageNumber, [Description], CompanyId,
       VesselInCompanyId, StartDate, EndDate, IsActive)
    ON (target.Id = source.Id)
    WHEN MATCHED THEN 
        UPDATE SET VoyageNumber = source.VoyageNumber, [Description] = source.[Description], CompanyId = source.CompanyId,
       VesselInCompanyId = source.VesselInCompanyId, StartDate = source.StartDate, EndDate = source.EndDate, IsActive = source.IsActive     
	WHEN NOT MATCHED THEN
    INSERT (Id, VoyageNumber, [Description], CompanyId,
       VesselInCompanyId, StartDate, EndDate, IsActive)
    VALUES (source.Id, source.VoyageNumber, source.[Description], source.CompanyId,
       source.VesselInCompanyId, source.StartDate, source.EndDate, source.IsActive);
       
END

GO

GRANT EXECUTE ON [Fuel].[UpdateVoyagesFromRotationData] TO [public] AS [dbo]
GO

");
        }

        private void createNewRotationVoyagesView()
        {
            this.Execute.Sql(@"
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [BasicInfo].[RotationVoyagesView]
AS
SELECT   CAST(VoyagesQuery.Id AS BIGINT) AS Id, VoyagesQuery.VoyageNumber COLLATE Arabic_CI_AS AS VoyageNumber, 
                VoyagesQuery.VoyageNumber COLLATE Arabic_CI_AS AS Description, CAST(VoyagesQuery.CompanyId AS BIGINT) AS CompanyId, 
                Fuel.VesselInCompany.Id AS VesselInCompanyId, VoyagesQuery.StartDateTime AS StartDate, VoyagesQuery.EndDateTime AS EndDate, VoyagesQuery.IsActive
FROM      (SELECT   Id, VesselCode, 1 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, CASE WHEN [TripType] IN (5, 6, 7) 
                                 THEN 1 /* As ShipOwner*/ ELSE
                                     (SELECT   TOP 1 Id
                                      FROM      [BasicInfo].[CompanyView]
                                      WHERE   NAME = 'SAPID') /*As Operation Company*/ END AS CompanyId
                 FROM      dbo.SAPIDVoyagesView
                 WHERE   EndDateTime IS NULL OR
                                 EndDateTime > '2014-01-01 00:00:00.000'
                 UNION
                 SELECT   10000000 + Id AS Id, VesselCode, 1 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, CASE WHEN [TripType] IN (5, 6, 7) 
                                 THEN 1 /* As ShipOwner*/ ELSE
                                     (SELECT   TOP 1 Id
                                      FROM      [BasicInfo].[CompanyView]
                                      WHERE   NAME = 'HAFIZ') /*As Operation Company*/ END AS CompanyId
                 FROM      dbo.HAFIZVoyagesView
                 WHERE   EndDateTime IS NULL OR
                                 EndDateTime > '2014-01-01 00:00:00.000'
                 UNION
                 SELECT   20000000 + Id AS Id, VesselCode, 1 AS ShipOwnerId, VoyageNumber, StartDateTime, EndDateTime, IsActive, CASE WHEN [TripType] IN (5, 6, 7) 
                                 THEN 1 /* As ShipOwner*/ ELSE
                                     (SELECT   TOP 1 Id
                                      FROM      [BasicInfo].[CompanyView]
                                      WHERE   NAME = 'HAFEZ') END AS CompanyId
                 FROM      dbo.[HAFEZVoyagesView]
                 WHERE   EndDateTime IS NULL OR
                                 EndDateTime > '2014-01-01 00:00:00.000') AS VoyagesQuery INNER JOIN
                Fuel.Vessel ON VoyagesQuery.VesselCode = Fuel.Vessel.Code COLLATE SQL_Latin1_General_CP1256_CI_AS INNER JOIN
                Fuel.VesselInCompany ON Fuel.VesselInCompany.VesselId = Fuel.Vessel.Id AND Fuel.VesselInCompany.CompanyId = VoyagesQuery.CompanyId
UNION
SELECT   Id, VoyageNumber, Description, CompanyId, VesselInCompanyId, StartDate, EndDate, IsActive
FROM      BasicInfo.IRISLVoyages
");
        }
    }
}
