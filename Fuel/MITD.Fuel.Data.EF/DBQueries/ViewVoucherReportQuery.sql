SELECT        BasicInfo.CompanyView.Name AS VoucherCompany, Fuel.Vouchers.Description, Fuel.Vouchers.FinancialVoucherDate, Fuel.Vouchers.LocalVoucherDate, 
                         Fuel.Vouchers.LocalVoucherNo, Fuel.Vouchers.ReferenceNo, Fuel.Vouchers.VoucherRef, Fuel.Vouchers.ReferenceTypeId, 
                         Fuel.Vouchers.VoucherDetailTypeId, Fuel.Vouchers.VoucherTypeId, Fuel.JournalEntries.AccountNo, 
                         Fuel.JournalEntries.Description AS JournalEntryDescription, Fuel.JournalEntries.VoucherRef AS JournalEntryVoucherRef, Fuel.JournalEntries.ForeignAmount, 
                         Fuel.JournalEntries.IrrAmount, Fuel.JournalEntries.Typ AS JournalEntryType, BasicInfo.CurrencyView.Abbreviation AS JournalEntryCurrency, 
                         Fuel.JournalEntries.AccountNo AS JournalEntryAccountCode, Fuel.Segments.Name, Fuel.Segments.Code, BasicInfo.UserView.Name AS UserName
FROM            Fuel.Vouchers INNER JOIN
                         BasicInfo.UserView ON Fuel.Vouchers.UserId = BasicInfo.UserView.Id RIGHT OUTER JOIN
                         Fuel.JournalEntries ON Fuel.Vouchers.Id = Fuel.JournalEntries.VoucherId LEFT OUTER JOIN
                         Fuel.Segments ON Fuel.JournalEntries.Id = Fuel.Segments.JournalEntryId LEFT OUTER JOIN
                         BasicInfo.CompanyView ON Fuel.Vouchers.CompanyId = BasicInfo.CompanyView.Id LEFT OUTER JOIN
                         BasicInfo.CurrencyView ON Fuel.JournalEntries.CurrencyId = BasicInfo.CurrencyView.Id