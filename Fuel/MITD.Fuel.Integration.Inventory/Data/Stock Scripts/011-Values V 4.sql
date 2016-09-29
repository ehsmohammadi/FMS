DECLARE @InventoryUserId INT;
SET @InventoryUserId = 100000;

BEGIN TRANSACTION

BEGIN TRY

--------------------------------------------------------------------------------------------

INSERT INTO [Inventory].[StoreTypes] ([Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (28, 2, N'Issue Total Received Trust', NULL, @InventoryUserId,  GETDATE())

--------------------------------------------------------------------------------------------


COMMIT TRANSACTION

END TRY
BEGIN CATCH

ROLLBACK TRANSACTION

raiserror(N'خطا در ذخیره سازی داده ها.',0,1) with nowait

END CATCH
--------------------------------------------------------------------------------------------

GO

raiserror(N'پایان انجام اصلاحیه ردیفهای عملیات انبار.',0,1) with nowait
GO	