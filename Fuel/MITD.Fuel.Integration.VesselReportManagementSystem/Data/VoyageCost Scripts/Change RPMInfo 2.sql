/*
   Monday, July 28, 20142:50:04 PM
   User: 
   Server: .\sqlExpress
   Database: VoyageCost930425
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
ALTER TABLE dbo.RPMInfo
	DROP CONSTRAINT FK_RPMInfo_Ships1
GO
ALTER TABLE dbo.Ships SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RPMInfo
	DROP CONSTRAINT FK_RPMInfo_Drafts1
GO
ALTER TABLE dbo.Drafts SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RPMInfo
	DROP CONSTRAINT DF_RPMInfo_DateIn
GO
ALTER TABLE dbo.RPMInfo
	DROP CONSTRAINT DF_RPMInfo_IsSM
GO
ALTER TABLE dbo.RPMInfo
	DROP CONSTRAINT DF_RPMInfo_Time
GO
ALTER TABLE dbo.RPMInfo
	DROP CONSTRAINT DF_RPMInfo_FuelReportType
GO
CREATE TABLE dbo.Tmp_RPMInfo
	(
	ID int NOT NULL IDENTITY (1, 1),
	DraftID int NULL,
	ShipID int NULL,
	ConsNo nvarchar(255) NULL,
	ShipName nvarchar(255) NULL,
	VoyageNo nvarchar(255) NULL,
	Year int NULL,
	Month int NULL,
	Day int NULL,
	PortName nvarchar(255) NULL,
	PortTime float(53) NULL,
	AtSeaLatitudeDegree float(53) NULL,
	AtSeaLatitudeMinute float(53) NULL,
	AtSeaLongitudeDegree float(53) NULL,
	AtSeaLongitudeMinute float(53) NULL,
	ObsDist float(53) NULL,
	EngDist float(53) NULL,
	SteamTime float(53) NOT NULL,
	AvObsSpeed decimal(5, 2) NOT NULL,
	AvEngSpeed float(53) NULL,
	RPM int NOT NULL,
	Slip float(53) NULL,
	WindDir float(53) NULL,
	WindForce float(53) NULL,
	SeaDir float(53) NULL,
	SeaForce float(53) NULL,
	ROBHO float(53) NULL,
	ROBDO float(53) NULL,
	ROBMGO float(53) NULL,
	ROBFW float(53) NULL,
	ConsInPortHO float(53) NULL,
	ConsInPortDO float(53) NULL,
	ConsInPortMGO float(53) NULL,
	ConsInPortFW float(53) NULL,
	ConsAtSeaHO float(53) NOT NULL,
	ConsAtSeaDO float(53) NULL,
	ConsAtSeaMGO float(53) NULL,
	ConsAtSeaFW float(53) NULL,
	ReceivedHO float(53) NULL,
	ReceivedDO float(53) NULL,
	ReceivedMGO float(53) NULL,
	ReceivedFW float(53) NULL,
	ETAPort nvarchar(255) NULL,
	ETADate nvarchar(255) NULL,
	Date  AS (CONVERT([datetime],(((CONVERT([varchar](4),[Year],(0))+'/')+CONVERT([varchar](2),[month],(0)))+'/')+CONVERT([varchar](2),[day],(0)),(0))),
	DateIn datetime NOT NULL,
	DailyFuelCons  AS (round(((24)*[consatseaho])/case [steamtime] when (0) then (1) else [steamtime] end,(2))) PERSISTED ,
	Speed  AS ([AVObsSpeed]),
	IsSM bit NOT NULL,
	InPortOrAtSea nvarchar(50) NULL,
	ImportDate char(10) NULL,
	TransferHo numeric(18, 3) NULL,
	TransferDo numeric(18, 3) NULL,
	TransferFW numeric(18, 3) NULL,
	TransferMGOLS numeric(18, 3) NULL,
	CorrectionHo numeric(18, 3) NULL,
	CorrectionDo numeric(18, 3) NULL,
	CorrectionFW numeric(18, 3) NULL,
	CorrectionMGOLS numeric(18, 3) NULL,
	CorrectionTypeHo bit NULL,
	CorrectionTypeDo bit NULL,
	CorrectionTypeFW bit NULL,
	CorrectionTypeMGOLS bit NULL,
	Time time(7) NOT NULL,
	FuelReportType tinyint NOT NULL,
	State tinyint NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Tmp_RPMInfo SET (LOCK_ESCALATION = TABLE)
GO
GRANT INSERT ON dbo.Tmp_RPMInfo TO fuel  AS dbo
GO
GRANT SELECT ON dbo.Tmp_RPMInfo TO fuel  AS dbo
GO
GRANT UPDATE ON dbo.Tmp_RPMInfo TO fuel  AS dbo
GO
ALTER TABLE dbo.Tmp_RPMInfo ADD CONSTRAINT
	DF_RPMInfo_DateIn DEFAULT (getdate()) FOR DateIn
GO
ALTER TABLE dbo.Tmp_RPMInfo ADD CONSTRAINT
	DF_RPMInfo_IsSM DEFAULT ((0)) FOR IsSM
GO
ALTER TABLE dbo.Tmp_RPMInfo ADD CONSTRAINT
	DF_RPMInfo_Time DEFAULT (CONVERT([time],'12:00:00',(0))) FOR Time
GO
ALTER TABLE dbo.Tmp_RPMInfo ADD CONSTRAINT
	DF_RPMInfo_FuelReportType DEFAULT ((1)) FOR FuelReportType
GO
SET IDENTITY_INSERT dbo.Tmp_RPMInfo ON
GO
IF EXISTS(SELECT * FROM dbo.RPMInfo)
	 EXEC('INSERT INTO dbo.Tmp_RPMInfo (ID, DraftID, ShipID, ConsNo, ShipName, VoyageNo, Year, Month, Day, PortName, PortTime, AtSeaLatitudeDegree, AtSeaLatitudeMinute, AtSeaLongitudeDegree, AtSeaLongitudeMinute, ObsDist, EngDist, SteamTime, AvObsSpeed, AvEngSpeed, RPM, Slip, WindDir, WindForce, SeaDir, SeaForce, ROBHO, ROBDO, ROBMGO, ROBFW, ConsInPortHO, ConsInPortDO, ConsInPortMGO, ConsInPortFW, ConsAtSeaHO, ConsAtSeaDO, ConsAtSeaMGO, ConsAtSeaFW, ReceivedHO, ReceivedDO, ReceivedMGO, ReceivedFW, ETAPort, ETADate, DateIn, IsSM, InPortOrAtSea, ImportDate, TransferHo, TransferDo, TransferFW, TransferMGOLS, CorrectionHo, CorrectionDo, CorrectionFW, CorrectionMGOLS, CorrectionTypeHo, CorrectionTypeDo, CorrectionTypeFW, CorrectionTypeMGOLS, Time, FuelReportType, State)
		SELECT ID, DraftID, ShipID, ConsNo, ShipName, VoyageNo, Year, Month, Day, PortName, PortTime, AtSeaLatitudeDegree, AtSeaLatitudeMinute, AtSeaLongitudeDegree, AtSeaLongitudeMinute, ObsDist, EngDist, SteamTime, AvObsSpeed, AvEngSpeed, RPM, Slip, WindDir, WindForce, SeaDir, SeaForce, ROBHO, ROBDO, ROBMGO, ROBFW, ConsInPortHO, ConsInPortDO, ConsInPortMGO, ConsInPortFW, ConsAtSeaHO, ConsAtSeaDO, ConsAtSeaMGO, ConsAtSeaFW, ReceivedHO, ReceivedDO, ReceivedMGO, ReceivedFW, ETAPort, ETADate, DateIn, IsSM, InPortOrAtSea, ImportDate, TransferHo, TransferDo, TransferFW, TransferMGOLS, CorrectionHo, CorrectionDo, CorrectionFW, CorrectionMGOLS, CorrectionTypeHo, CorrectionTypeDo, CorrectionTypeFW, CorrectionTypeMGOLS, Time, FuelReportType, State FROM dbo.RPMInfo WITH (HOLDLOCK TABLOCKX)')
GO
SET IDENTITY_INSERT dbo.Tmp_RPMInfo OFF
GO
DROP TABLE dbo.RPMInfo
GO
EXECUTE sp_rename N'dbo.Tmp_RPMInfo', N'RPMInfo', 'OBJECT' 
GO
ALTER TABLE dbo.RPMInfo ADD CONSTRAINT
	PK_Sheet1$ PRIMARY KEY CLUSTERED 
	(
	ID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.RPMInfo ADD CONSTRAINT
	FK_RPMInfo_Drafts1 FOREIGN KEY
	(
	DraftID
	) REFERENCES dbo.Drafts
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.RPMInfo ADD CONSTRAINT
	FK_RPMInfo_Ships1 FOREIGN KEY
	(
	ShipID
	) REFERENCES dbo.Ships
	(
	ID
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
COMMIT
