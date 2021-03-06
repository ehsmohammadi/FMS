--USE [MiniStock]

DELETE FROM [Inventory].OperationReference;
DELETE FROM [Inventory].TransactionItemPrices;
DELETE FROM [Inventory].TransactionItems;
DELETE FROM [Inventory].Transactions;

DELETE FROM [Inventory].[Goods];
DELETE FROM [Inventory].[StoreTypes];

DELETE FROM [Inventory].[UnitConverts];
DELETE FROM [Inventory].[Units];

DELETE FROM [Inventory].[TimeBucket];
DELETE FROM [Inventory].[FinancialYear];

DELETE FROM [Inventory].[Warehouse];
DELETE FROM [Inventory].[Companies];
DELETE FROM [Inventory].[Users];

GO

DECLARE @InventoryUserId INT;
SET @InventoryUserId = 100000;


------------------------------------------------------------------------------------------

IF(NOT EXISTS ( SELECT * FROM [Inventory].[Users] WHERE [Id] = @InventoryUserId))
	INSERT INTO [Inventory].[Users] ([Id], [Code], [Name], [User_Name], [Password], [IsActive], [Email_Address], [IPAddress], [Login], [SessionId], [UserCreatorId], [CreateDate]) VALUES (@InventoryUserId, @InventoryUserId, N'InventoryAdmin', N'InventoryAdmin', N'InventoryAdmin', 1, NULL, NULL, 0, NULL, @InventoryUserId, GETDATE())


INSERT INTO [Inventory].[Users] ([Id], [Code], [Name], [User_Name], [Password], [IsActive], [Email_Address], [IPAddress], [Login], [SessionId], [UserCreatorId], [CreateDate])
	SELECT [Id], [Id] AS Code, [LastName] + ', ' + [FirstName] AS Name, [UserName] AS [User_Name], '********' AS Password, [Active] AS [IsActive], [Email] AS [Email_Address], NULL AS [IPAddress], 0 AS [Login], '0X0000001' AS [SessionId], [Id] AS [UserCreatorId], GETDATE() AS [CreateDate]
	FROM dbo.Users
	WHERE [UserName] NOT IN (SELECT [User_Name] FROM [Inventory].[Users])

------------------------------------------------------------------------------------------

INSERT INTO [Inventory].[Companies] ([Id], [Code], [Name], [IsActive], [UserCreatorId], [CreateDate]) VALUES (1, N'0000116', N'IRISL', 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Companies] ([Id], [Code], [Name], [IsActive], [UserCreatorId], [CreateDate]) VALUES (2, N'0000088', N'SAPID', 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Companies] ([Id], [Code], [Name], [IsActive], [UserCreatorId], [CreateDate]) VALUES (3, N'0000086', N'HAFIZ', 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Companies] ([Id], [Code], [Name], [IsActive], [UserCreatorId], [CreateDate]) VALUES (4, N'0000003', N'IMSENGCO', 1, @InventoryUserId, GETDATE())

------------------------------------------------------------------------------------------
/*
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (1,  N'0123', N'ABBA / IRISL', 1, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (2,  N'0123', N'ABBA / SAPID', 2, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (3,  N'0093', N'AGIAN / IRISL',1, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (4,  N'0093', N'AGEAN / SAPID',2, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (5,  N'0110', N'APPOLO / IRISL',1, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (6,  N'0110', N'APPOLO / SAPID',2, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (7,  N'0092', N'AEROLITE / IRISL',1, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (8,  N'0092', N'AEROLITE / SAPID',2, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (9,  N'0125', N'MULBERRY / IRISL',1, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (10, N'0125', N'MULBERRY / SAPID',2, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (11, N'0170', N'BASHT / IRISL',1, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (12, N'0170', N'BASHT / HAFIZ',3, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (13, N'0165', N'SHABDIS / IRISL',1, 0, @InventoryUserId, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (14, N'0165', N'SHABDIS / HAFIZ',3, 0, @InventoryUserId, GETDATE());


INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) 
	SELECT vic.[Id], v.Code, vic.Description, [CompanyId], CASE WHEN [VesselStateCode] = 4 THEN 1 ELSE 0 END AS IsActive, @InventoryUserId, GETDATE() FROM [Fuel].[VesselInCompany] vic INNER JOIN [Fuel].[Vessel] v ON vic.VesselId = v.Id			
*/

------------------------------------------------------------------------------------------

SET IDENTITY_INSERT [Inventory].[FinancialYear] ON
INSERT INTO [Inventory].[FinancialYear] ([Id], [Name], [StartDate], [EndDate], [UserCreatorId], [CreateDate]) VALUES (1, N'93', N'2014-03-21 00:00:00', N'2015-03-20 00:00:00', @InventoryUserId, GETDATE())
SET IDENTITY_INSERT [Inventory].[FinancialYear] OFF

SET IDENTITY_INSERT [Inventory].[TimeBucket] ON
INSERT INTO [Inventory].[TimeBucket] ([Id], [Name], [StartDate], [EndDate], [FinancialYearId], [IsActive], [UserCreatorId], [CreateDate]) VALUES (1, N'93-1', N'2014-03-21 00:00:00', N'2015-03-20 00:00:00', 1, 1, @InventoryUserId, GETDATE())
SET IDENTITY_INSERT [Inventory].[TimeBucket] OFF

--------------------------------------------------------------------------------------------

SET IDENTITY_INSERT [Inventory].[Units] ON
INSERT INTO [Inventory].[Units] ([Id], [Abbreviation], [Name], [IsCurrency], [IsBaseCurrency], [IsActive], [UserCreatorId], [CreateDate]) VALUES (1, N'TON', N'تن', 0, 0, 1,  @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Units] ([Id], [Abbreviation], [Name], [IsCurrency], [IsBaseCurrency], [IsActive], [UserCreatorId], [CreateDate]) VALUES (2, N'011', N'ريال', 1, 1, 1,   @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Units] ([Id], [Abbreviation], [Name], [IsCurrency], [IsBaseCurrency], [IsActive], [UserCreatorId], [CreateDate]) VALUES (3, N'066', N'دلار', 1, 0, 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Units] ([Id], [Abbreviation], [Name], [IsCurrency], [IsBaseCurrency], [IsActive], [UserCreatorId], [CreateDate]) VALUES (4, N'098', N'یورو', 1, 0, 1, @InventoryUserId, GETDATE())
SET IDENTITY_INSERT [Inventory].[Units] OFF


SET IDENTITY_INSERT [Inventory].[UnitConverts] ON
INSERT INTO [Inventory].[UnitConverts] ([Id], [UnitId], [SubUnitId], [Coefficient], [EffectiveDateStart], [EffectiveDateEnd], [RowVersion], [UserCreatorId], [CreateDate]) VALUES (1, 3, 2, CAST(26500.000 AS Decimal(18, 3)), N'2014-06-10 00:00:00', NULL, 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[UnitConverts] ([Id], [UnitId], [SubUnitId], [Coefficient], [EffectiveDateStart], [EffectiveDateEnd], [RowVersion], [UserCreatorId], [CreateDate]) VALUES (2, 4, 2, CAST(36863.000 AS Decimal(18, 3)), N'2014-08-21 00:00:00', NULL, 2, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[UnitConverts] ([Id], [UnitId], [SubUnitId], [Coefficient], [EffectiveDateStart], [EffectiveDateEnd], [RowVersion], [UserCreatorId], [CreateDate]) VALUES (3, 4, 3, CAST(1.391 AS Decimal(18, 3)), N'2014-08-21 00:00:00', NULL, 3, @InventoryUserId, GETDATE())
SET IDENTITY_INSERT [Inventory].[UnitConverts] OFF

--------------------------------------------------------------------------------------------

INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (1,  1, N'Charter In Start', NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue     Usage in current implementation
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (2,  1, N'FuelReport Receive Trust', NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (3,  1, N'FuelReport Receive InternalTransfer', NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (4,  1, N'FuelReport Receive TransferPurchase', NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (5,  1, N'FuelReport Receive Purchase', NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (6,  1, N'FuelReport Receive From Tank', NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (7,  1, N'FuelReport Incremental Correction', NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (8,  1, N'Charter Out End', NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
																												     
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (9,  2, N'Charter Out Start',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (10, 2, N'FuelReport Transfer InternalTransfer',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (11, 2, N'FuelReport Transfer TransferSale',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (12, 2, N'FuelReport Transfer Rejected',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (13, 2, N'FuelReport Decremental Correction',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (14, 2, N'EOV Consumption',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (15, 2, N'EOM Consumption',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (16, 2, N'EOY Consumption',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (17, 2, N'Charter In End',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], UserCreatorId, CreateDate) VALUES (18, 2, N'Scrap',NULL, @InventoryUserId, GETDATE())  --Type=1 : Receipt ,    Type=2 : Issue
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (19, 1, N'Vessel Activation', NULL, @InventoryUserId,  GETDATE())
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (20, 1, N'Charter Out Start Decremental Adjustment', NULL, @InventoryUserId,  GETDATE())
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (21, 1, N'Charter Out Start Incremental Adjustment', NULL, @InventoryUserId,  GETDATE())
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (22, 1, N'Charter In End Decremental Adjustment', NULL, @InventoryUserId,  GETDATE())
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (23, 1, N'Charter In End Incremental Adjustment', NULL, @InventoryUserId,  GETDATE())
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (24, 1, N'Warehouse Activation Initial Receipt', NULL, @InventoryUserId,  GETDATE())
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (25, 1, N'ReceiptAdjustment', NULL, @InventoryUserId,  GETDATE())
INSERT INTO [Inventory].[StoreTypes] ( [Code], [Type], [InputName], [OutputName], [UserCreatorId], [CreateDate]) VALUES (26, 1, N'IssueAdjustment', NULL, @InventoryUserId,  GETDATE())

--------------------------------------------------------------------------------------------

INSERT INTO [Inventory].[Goods] ([Id], [Code], [Name], [IsActive], [MainUnitId], [UserCreatorId], [CreateDate]) VALUES (1, N'HFO', N'سوخت سنگین', 1, 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Goods] ([Id], [Code], [Name], [IsActive], [MainUnitId], [UserCreatorId], [CreateDate]) VALUES (2, N'MDO', N'سوخت دیزل', 1, 1, @InventoryUserId, GETDATE())
INSERT INTO [Inventory].[Goods] ([Id], [Code], [Name], [IsActive], [MainUnitId], [UserCreatorId], [CreateDate]) VALUES (3, N'MGO', N'سوخت گازوئیل', 1, 1, @InventoryUserId, GETDATE())

--------------------------------------------------------------------------------------------

GO

raiserror(N'پایان ايجاد داده هاي پيش فرض سيستم.',0,1) with nowait
GO	