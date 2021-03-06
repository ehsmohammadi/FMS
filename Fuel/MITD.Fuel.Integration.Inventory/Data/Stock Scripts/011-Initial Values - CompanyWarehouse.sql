DELETE FROM [Inventory].[Warehouse];

GO

DECLARE @InventoryUserId INT;
SET @InventoryUserId = 100000;

INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) 
	SELECT vic.[Id], v.Code, vic.Description, [CompanyId], CASE WHEN [VesselStateCode] = 4 THEN 1 ELSE 0 END AS IsActive, @InventoryUserId, GETDATE() FROM [Fuel].[VesselInCompany] vic INNER JOIN [Fuel].[Vessel] v ON vic.VesselId = v.Id			
