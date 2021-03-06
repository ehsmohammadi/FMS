DECLARE @InventoryUserId INT;
SET @InventoryUserId = 100000;

BEGIN TRANSACTION

BEGIN TRY

--------------------------------------------------------------------------------------------

INSERT INTO [Inventory].[Goods] ([Id], [Code], [Name], [IsActive], [MainUnitId], [UserCreatorId], [CreateDate]) VALUES (4, N'IFO', N'سوخت IFO', 1, 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Goods] ([Id], [Code], [Name], [IsActive], [MainUnitId], [UserCreatorId], [CreateDate]) VALUES (5, N'HOLS', N'سوخت HO LS', 1, 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Goods] ([Id], [Code], [Name], [IsActive], [MainUnitId], [UserCreatorId], [CreateDate]) VALUES (6, N'DOLS', N'سوخت DO LS', 1, 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Goods] ([Id], [Code], [Name], [IsActive], [MainUnitId], [UserCreatorId], [CreateDate]) VALUES (7, N'IFOLS', N'سوخت IFO LS', 1, 1, @InventoryUserId, GETDATE())

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