USE [offhire]
GO
/****** Object:  Table [dbo].[OffhireVoucher]    Script Date: 8/5/2014 8:26:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OffhireVoucher](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[OffhireId] [bigint] NOT NULL,
	[VoucherNumber] [nvarchar](50) NOT NULL,
	[VoucherDate] [datetime] NOT NULL,
	[PersianVoucherDate] [varchar](8) NOT NULL,
	[FiscalYear] [int] NOT NULL,
	[VoucherStatus] [nvarchar](200) NULL,
	[RegisterDate] [datetime] NOT NULL,
 CONSTRAINT [PK_OffhireVoucher] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
--SET ANSI_PADDING OFF
--GO
--/****** Object:  View [dbo].[VW_OFFHIRE_CH]    Script Date: 8/5/2014 8:26:51 AM ******/
--SET ANSI_NULLS ON
--GO
--SET QUOTED_IDENTIFIER ON
--GO
--CREATE VIEW [dbo].[VW_OFFHIRE_CH]
--AS
--SELECT   TOP (100) PERCENT dbo.Sections.ID AS SectionID, dbo.OffHires.ShipID_FK AS Ship, dbo.Sections.Name AS SectionName, dbo.OffHireTypes.ID AS TypeID, 
--                dbo.OffHireTypes.Name AS TypeName, dbo.OffHires.nvoyg AS VoyageNumber, dbo.OffHires.ChangeFromDate AS FromDate, dbo.OffHires.ChangeFromTime AS FromTime, 
--                dbo.OffHires.ChangeToDate AS ToDate, dbo.OffHires.ChangeToTime AS ToTime, dbo.Ships.Name AS ShipName, dbo.OffHires.Commitee1Approved AS comit1ok, 
--                dbo.OffHires.Commitee1ApproverID_FK, dbo.OffHires.Commitee2Approved AS comit2ok, dbo.OffHires.Commitee2ApproverID_FK, dbo.OffHires.RootCause, 
--                dbo.OffHires.nstartrobho AS StartRobHO, dbo.OffHires.nstartrobdo AS StartRobDO, dbo.OffHires.nendrobho AS EndRobHO, dbo.OffHires.nendrobdo AS EndRobDO, 
--                dbo.OffHires.ndorec AS DOReceived, dbo.OffHires.nhorec AS HOReceived, dbo.OffHires.ActionDate, dbo.OffHires.ActionTypeID_FK, dbo.OffHires.RealDay AS OffhireDay, 
--                dbo.OffHires.RealHours AS OffhireHours, dbo.OffHires.RealMinute AS OffhireMinute, dbo.OffHires.Owner, dbo.OffHires.RealTonHo, dbo.OffHires.RealTonDo, dbo.OffHires.ID, 
--                dbo.OffHires.PortID_FK AS PortID
--FROM      dbo.OffHires INNER JOIN
--                dbo.OffHireTypes ON dbo.OffHires.TypeID_FK = dbo.OffHireTypes.ID INNER JOIN
--                dbo.Sections ON dbo.OffHireTypes.SectionID_FK = dbo.Sections.ID INNER JOIN
--                dbo.Ships ON dbo.OffHires.ShipID_FK = dbo.Ships.ID
--ORDER BY SectionID, TypeID, FromDate, ToDate

--GO
ALTER TABLE [dbo].[OffhireVoucher] ADD  CONSTRAINT [DF_OffhireVoucher_RegisterDate]  DEFAULT (getdate()) FOR [RegisterDate]
GO
ALTER TABLE [dbo].[OffhireVoucher]  WITH CHECK ADD  CONSTRAINT [FK_OffhireVoucher_OffHires] FOREIGN KEY([OffhireId])
REFERENCES [dbo].[OffHires] ([ID])
GO
ALTER TABLE [dbo].[OffhireVoucher] CHECK CONSTRAINT [FK_OffhireVoucher_OffHires]
GO
