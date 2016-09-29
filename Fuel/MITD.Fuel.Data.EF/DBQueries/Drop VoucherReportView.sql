--USE [StorageSpace]
--GO

/****** Object:  View [dbo].[ViewVoucherReport]    Script Date: 8/20/2014 9:56:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[Fuel].[VoucherReportView]'))
	DROP VIEW [Fuel].[VoucherReportView]
GO