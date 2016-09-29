/************************************************************
 * Code formatted by SoftTree SQL Assistant © v6.3.153
 * Time: 8/28/2014 10:24:25 AM
 ************************************************************/

DECLARE @OwningCompanyId BIGINT, @VesselCode NVARCHAR(50), @WarehouseId BIGINT,
		@ReferenceType NVARCHAR(100), @Description NVARCHAR(MAX)
		
DECLARE @NameInOwnerCompany NVARCHAR(100), @NameInSubsidiaryCompany NVARCHAR(100), @VesselName NVARCHAR(100), @SubsidiaryCompanyId BIGINT

SELECT @NameInOwnerCompany = N'GOLBON / IRISL', 
		@NameInSubsidiaryCompany = N'GOLBON / HAFIZ',
		@VesselName = N'GOLBON'

INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES ((SELECT MAX(Id) + 1 FROM [Inventory].[Warehouse]), N'0158', N'GOLBON / IRISL',1, 0, 100000, GETDATE());
INSERT INTO [Inventory].[Warehouse] ([Id], [Code], [Name], [CompanyId], [IsActive], [UserCreatorId], [CreateDate]) VALUES ((SELECT MAX(Id) + 1 FROM [Inventory].[Warehouse]), N'0158', N'GOLBON / HAFIZ',3, 0, 100000, GETDATE());


SET @OwningCompanyId = 1   -- IRISL
SET @SubsidiaryCompanyId = 3  -- HAFIZ
SET @VesselCode = N'0158'



INSERT INTO [Fuel].[Vessel]
           ([Code]
           ,[OwnerId])
     VALUES
           (@VesselCode
           ,@OwningCompanyId);
           
DECLARE @InsertedVesselId BIGINT

SET @InsertedVesselId = @@identity

INSERT INTO [Fuel].[VesselInCompany]  --Insert in Owning Company
           ([Name]
           ,[Description]
           ,[CompanyId]
           ,[VesselId]
           ,[VesselStateCode])
     VALUES
           (@VesselName
           ,@NameInOwnerCompany
           ,@OwningCompanyId
           ,@InsertedVesselId
           ,4)
           
INSERT INTO [Fuel].[VesselInCompany]  --Insert in Subsidiary Company
           ([Name]
           ,[Description]
           ,[CompanyId]
           ,[VesselId]
           ,[VesselStateCode])
     VALUES
           (@VesselName
           ,@NameInSubsidiaryCompany
           ,@SubsidiaryCompanyId
           ,@InsertedVesselId
           ,1)
           

DECLARE @TransactionItemsList dbo.TypeTransactionItemWithPrice
INSERT INTO @TransactionItemsList
(
	Id ,
	GoodId ,
	QuantityUnitId ,
	QuantityAmount ,
	PriceUnitId ,
	Fee ,
	[Description] 
)	
VALUES
  (
    NULL,
    1,  --HFO in Inventory
    1,
    1715.5,  --2083,--2753.80,
    2,
    17291323,
    'Starting Receipt > HFO'
  )
INSERT INTO @TransactionItemsList
(
	Id ,
	GoodId ,
	QuantityUnitId ,
	QuantityAmount ,
	PriceUnitId ,
	Fee ,
	[Description] 
)	
VALUES
  (
    NULL,
    2,  --MDO in Inventory
    1,
    67.7,--101.5,--97.5,
    2,
    33059279,
    'Starting Receipt > MDO'    
  )

DECLARE @Date DATETIME 
SET @Date='2014/12/11 00:00:00'

SELECT @WarehouseId = Id FROM Inventory.Warehouse WHERE Code = @VesselCode AND CompanyId = @OwningCompanyId

SET @ReferenceType = @VesselCode + ' Registration';
SET @Description = N'Inventory Initiation of ' + @VesselCode;


EXEC [Inventory].[ActivateWarehouseIncludingRecieptsOperation]
     @WarehouseId=@WarehouseId,
     @TimeBucketId=1,
     @StoreTypesId=19,
     @RegistrationDate=@Date,
     @ReferenceType=@ReferenceType,
     @ReferenceNo=N'0',
     @TransactionItems=@TransactionItemsList,
     @Description=@Description,
     @UserCreatorId=1


Select * from Inventory.Warehouse

Select * from Fuel.Vessel

Select * from Fuel.VesselInCompany
