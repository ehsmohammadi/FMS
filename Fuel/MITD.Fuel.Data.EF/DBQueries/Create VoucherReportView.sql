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

CREATE VIEW [Fuel].[VoucherReportView]
AS
SELECT        Fuel.Vouchers.Id AS VoucherId, Fuel.JournalEntries.Id AS JournalEntryId, BasicInfo.CompanyView.Name AS VoucherCompany, Fuel.Vouchers.Description, Fuel.Vouchers.FinancialVoucherDate, Fuel.Vouchers.LocalVoucherDate, 
                         Fuel.Vouchers.LocalVoucherNo, Fuel.Vouchers.ReferenceNo, Fuel.Vouchers.VoucherRef, Fuel.Vouchers.ReferenceTypeId, 
                         Fuel.Vouchers.VoucherDetailTypeId, Fuel.Vouchers.VoucherTypeId, Fuel.JournalEntries.AccountNo, 
                         Fuel.JournalEntries.Description AS JournalEntryDescription, Fuel.JournalEntries.VoucherRef AS JournalEntryVoucherRef, Fuel.JournalEntries.ForeignAmount, 
                         Fuel.JournalEntries.IrrAmount, Fuel.JournalEntries.Typ AS JournalEntryType, BasicInfo.CurrencyView.Abbreviation AS JournalEntryCurrency, 
                         Fuel.Segments.Name AS SegmentName, Fuel.Segments.Code AS SegmentCode, Fuel.Segments.SegmentTypeName, Fuel.Segments.SegmentTypeCode, BasicInfo.UserView.Name AS UserName
FROM            Fuel.Vouchers INNER JOIN
                         BasicInfo.UserView ON Fuel.Vouchers.UserId = BasicInfo.UserView.Id RIGHT OUTER JOIN
                         Fuel.JournalEntries ON Fuel.Vouchers.Id = Fuel.JournalEntries.VoucherId LEFT OUTER JOIN
                         Fuel.Segments ON Fuel.JournalEntries.Id = Fuel.Segments.JournalEntryId LEFT OUTER JOIN
                         BasicInfo.CompanyView ON Fuel.Vouchers.CompanyId = BasicInfo.CompanyView.Id LEFT OUTER JOIN
                         BasicInfo.CurrencyView ON Fuel.JournalEntries.CurrencyId = BasicInfo.CurrencyView.Id


GO

GRANT SELECT ON [Fuel].[VoucherReportView] TO [public]
GO