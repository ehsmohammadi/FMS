DECLARE @InventoryUserId INT;
SET @InventoryUserId = 100000;

BEGIN TRANSACTION

BEGIN TRY

--------------------------------------------------------------------------------------------

UPDATE [Inventory].[StoreTypes]
SET [InputName] = N'FuelReport Decremental Correction Inventory Adjustment',
	CreateDate = GETDATE()
WHERE Code = 13

--------------------------------------------------------------------------------------------

UPDATE [Inventory].[StoreTypes]
SET [InputName] = N'FuelReport Incremental Correction Inventory Adjustment',
	CreateDate = GETDATE()
WHERE Code = 7

--------------------------------------------------------------------------------------------

DECLARE @WarehouseActivationInitialReceiptStoreTypeId INT
SELECT @WarehouseActivationInitialReceiptStoreTypeId = Id FROM [Inventory].[StoreTypes] WHERE Code = 24;

DISABLE TRIGGER Transactions.UpdateTransaction ON Inventory.Transactions;
UPDATE Inventory.Transactions
SET StoreTypesId = 19
WHERE StoreTypesId = @WarehouseActivationInitialReceiptStoreTypeId;
ENABLE TRIGGER Transactions.UpdateTransaction ON Inventory.Transactions;

SET IDENTITY_INSERT [Inventory].[StoreTypes] ON

DELETE FROM [Inventory].[StoreTypes] WHERE Code = 24
INSERT INTO [Inventory].[StoreTypes] (Id, [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (24, 24, 2, N'FuelReport Decremental Correction for Issued Voyage', NULL, @InventoryUserId,  GETDATE())
INSERT INTO [Inventory].[StoreTypes] (Id, [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (25, 25, 1, N'FuelReport Incremental Correction for Issued Voyage', NULL, @InventoryUserId,  GETDATE())

--------------------------------------------------------------------------------------------



COMMIT TRANSACTION

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION

raiserror(N'خطا در اجرای پروسیجر.',0,1) with nowait

END CATCH
--------------------------------------------------------------------------------------------

GO

raiserror(N'پایان انجام اصلاحیه ردیفهای عملیات انبار.',0,1) with nowait
GO	