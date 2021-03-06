USE [VoyageCost]
GO
/****** Object:  Table [dbo].[EventReport]    Script Date: 2/4/2015 6:16:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EventReport](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DraftID] [int] NULL,
	[ShipCode] [nvarchar](4) NULL,
	[ConsNo] [nvarchar](255) NULL,
	[ShipName] [nvarchar](255) NULL,
	[VoyageNo] [nvarchar](255) NULL,
	[Year] [int] NULL,
	[Month] [int] NULL,
	[Day] [int] NULL,
	[PortName] [nvarchar](255) NULL,
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
	[ETAPort] [nvarchar](255) NULL,
	[ETADate] [nvarchar](255) NULL,
	[Date]  AS (CONVERT([datetime],(((CONVERT([varchar](4),[Year],(0))+'/')+CONVERT([varchar](2),[month],(0)))+'/')+CONVERT([varchar](2),[day],(0)),(0))),
	[DateIn] [datetime] NULL,
	[DailyFuelCons]  AS (round(((24)*[consatseaho])/case [steamtime] when (0) then (1) else [steamtime] end,(2))) PERSISTED,
	[Speed]  AS ([AVObsSpeed]),
	[IsSM] [bit] NULL,
	[InPortOrAtSea] [nvarchar](50) NULL,
	[ImportDate] [char](10) NULL,
	[TransferHo] [decimal](7, 3) NULL,
	[TransferDo] [decimal](7, 3) NULL,
	[TransferFW] [decimal](7, 3) NULL,
	[TransferMGOLS] [decimal](7, 3) NULL,
	[CorrectionHo] [decimal](7, 3) NULL,
	[CorrectionDo] [decimal](7, 3) NULL,
	[CorrectionFW] [decimal](7, 3) NULL,
	[CorrectionMGOLS] [decimal](7, 3) NULL,
	[CorrectionTypeHo] [char](1) NULL,
	[CorrectionTypeDo] [char](1) NULL,
	[CorrectionTypeFW] [char](1) NULL,
	[CorrectionTypeMGOLS] [char](1) NULL,
	[Time] [time](7) NULL,
	[FuelReportType] [tinyint] NULL,
	[State] [tinyint] NULL,
 CONSTRAINT [PK_Sheet11$] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER AUTHORIZATION ON [dbo].[EventReport] TO  SCHEMA OWNER 
GO
GRANT INSERT ON [dbo].[EventReport] TO [eventreportimportuser] AS [dbo]
GO
GRANT SELECT ON [dbo].[EventReport] TO [eventreportimportuser] AS [dbo]
GO
GRANT SELECT ON [dbo].[EventReport] TO [FMSVoyageCost] AS [dbo]
GO
GRANT UPDATE ON [dbo].[EventReport] TO [FMSVoyageCost] AS [dbo]
GO
/****** Object:  Table [dbo].[FuelReportLog]    Script Date: 2/4/2015 6:16:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FuelReportLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RecordId] [bigint] NOT NULL,
	[FailureMessage] [nvarchar](max) NOT NULL,
	[StackTrace] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_FuelReportLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
ALTER AUTHORIZATION ON [dbo].[FuelReportLog] TO  SCHEMA OWNER 
GO
GRANT DELETE ON [dbo].[FuelReportLog] TO [FMSVoyageCost] AS [dbo]
GO
GRANT INSERT ON [dbo].[FuelReportLog] TO [FMSVoyageCost] AS [dbo]
GO
GRANT SELECT ON [dbo].[FuelReportLog] TO [FMSVoyageCost] AS [dbo]
GO
GRANT UPDATE ON [dbo].[FuelReportLog] TO [FMSVoyageCost] AS [dbo]
GO
ALTER TABLE [dbo].[EventReport] ADD  CONSTRAINT [DF_EventReport_SteamTime]  DEFAULT ((0)) FOR [SteamTime]
GO
ALTER TABLE [dbo].[EventReport] ADD  CONSTRAINT [DF_EventReport_AvObsSpeed]  DEFAULT ((0)) FOR [AvObsSpeed]
GO
ALTER TABLE [dbo].[EventReport] ADD  CONSTRAINT [DF_EventReport_RPM]  DEFAULT ((0)) FOR [RPM]
GO
ALTER TABLE [dbo].[EventReport] ADD  CONSTRAINT [DF_EventReport_DateIn]  DEFAULT (getdate()) FOR [DateIn]
GO
ALTER TABLE [dbo].[EventReport] ADD  CONSTRAINT [DF_EventReport_IsSM]  DEFAULT ((0)) FOR [IsSM]
GO
ALTER TABLE [dbo].[EventReport] ADD  CONSTRAINT [DF_EventReport_Time]  DEFAULT (CONVERT([time],'12:00:00',(0))) FOR [Time]
GO
ALTER TABLE [dbo].[EventReport] ADD  CONSTRAINT [DF_EventReport_FuelReportType]  DEFAULT ((1)) FOR [FuelReportType]
GO
ALTER TABLE [dbo].[FuelReportLog] ADD  CONSTRAINT [DF_FuelReportLog_Date]  DEFAULT (getdate()) FOR [Date]
GO
