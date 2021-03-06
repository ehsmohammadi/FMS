--USE StorageSpace
--GO
------------------------------------------------------------------------------------
IF OBJECT_ID('[Fuel].[UpdateVouchers]', 'TR') IS NOT NULL
    DROP TRIGGER [Fuel].[UpdateVouchers];
GO

CREATE TRIGGER [Fuel].[UpdateVouchers]
ON [Fuel].[Vouchers]
   AFTER INSERT, UPDATE
AS
DISABLE  TRIGGER Transactions.UpdateTransaction ON [Inventory].[Transactions];
BEGIN TRY
	DECLARE @Id                      int ,
			@VoucherTypeId TINYINT,--1Reciept 2-Issue  3-Invoice
			@ReferenceNumber NVARCHAR(512),
			@Status TINYINT,
			@IdOld int ,
			@VoucherTypeIdOld TINYINT,
			@ReferenceNumberOld NVARCHAR(512),
			@Code DECIMAL(18,2),
			@Action TINYINT,
			@InventoryOperationTextIndex INT,
			@WarehouseId BIGINT
			
	SELECT TOP(1) @Id  =Id , @VoucherTypeId  =VoucherTypeId , @ReferenceNumber  =ReferenceNo FROM  INSERTED 
	SELECT TOP(1) @IdOld  =Id , @VoucherTypeIdOld  =VoucherTypeId , @ReferenceNumberOld  = ReferenceNo FROM  DELETED

	IF(PATINDEX('%Offhire%', @ReferenceNumber) <> 0)
	--Offhire Vouchers must be bypassed.
		RETURN;

	SET @InventoryOperationTextIndex = PATINDEX('%|Issue%', @ReferenceNumber)
	IF (@InventoryOperationTextIndex = 0) 
		SET @InventoryOperationTextIndex = PATINDEX('%|Receipt%', @ReferenceNumber)
	IF (@InventoryOperationTextIndex = 0) 
		SET @InventoryOperationTextIndex = PATINDEX('%|Factor%', @ReferenceNumber)
		
	IF (@InventoryOperationTextIndex = 0)  -- The TextIndex using '|' delimiter not found.
	BEGIN
		SET @InventoryOperationTextIndex = PATINDEX('%Issue%', @ReferenceNumber)
		IF (@InventoryOperationTextIndex = 0) 
			SET @InventoryOperationTextIndex = PATINDEX('%Receipt%', @ReferenceNumber)
		IF (@InventoryOperationTextIndex = 0) 
			SET @InventoryOperationTextIndex = PATINDEX('%Factor%', @ReferenceNumber)			
	END
	ELSE
		SET @InventoryOperationTextIndex = @InventoryOperationTextIndex + 1 --The index has been found using '|' separator and it should be eliminated.
	
	IF (@InventoryOperationTextIndex = 0)  	
		RAISERROR(N'@Invalid Voucher Reference No. detected inorder to find relevant Inventory Transaction.', 16, 1, '500')	 
	
	
	SET @ReferenceNumber = 	SUBSTRING(@ReferenceNumber, @InventoryOperationTextIndex,  LEN(@ReferenceNumber))
	
	DECLARE @FirstSlashDelimiterPosition INT,
		@SecondSlashDelimiterPosition INT,
		@ThirdSlashDelimiterPosition INT

	SET @FirstSlashDelimiterPosition = PATINDEX('%/%', @ReferenceNumber);
	SET @SecondSlashDelimiterPosition = PATINDEX('%/%', SUBSTRING(@ReferenceNumber, @FirstSlashDelimiterPosition + 1, LEN(@ReferenceNumber)));
	SET @ThirdSlashDelimiterPosition = PATINDEX('%/%', SUBSTRING(@ReferenceNumber, @FirstSlashDelimiterPosition + @SecondSlashDelimiterPosition + 1, LEN(@ReferenceNumber)));
	
	IF(@SecondSlashDelimiterPosition = 0) SET @SecondSlashDelimiterPosition = 1
	IF(@ThirdSlashDelimiterPosition = 0) SET @ThirdSlashDelimiterPosition = 1

	SET @WarehouseId = CAST( SUBSTRING(@ReferenceNumber, @FirstSlashDelimiterPosition + 1, @SecondSlashDelimiterPosition - 1) as BIGINT)
	SET @Code = CAST( SUBSTRING(@ReferenceNumber, @FirstSlashDelimiterPosition + @SecondSlashDelimiterPosition + 1, @ThirdSlashDelimiterPosition - 1) as DEcimal)

	--SET @Code = cast( SUBSTRING(@ReferenceNumber, PATINDEX('%/%', @ReferenceNumber) + 1, LEN(@ReferenceNumber)) as DEcimal)
	SET @Action = CASE  SUBSTRING(@ReferenceNumber, 1,  PATINDEX('%/%', @ReferenceNumber) - 1) 
		WHEN 'Issue' THEN 2 
		WHEN 'Receipt' THEN 1 
		ELSE 3 END 
	
	SELECT TOP(1) @Status=t.[Status] FROM [Inventory].Transactions t WHERE t.WarehouseId = @WarehouseId AND t.Code= @Code AND t.[Action]= @Action
	    
	PRINT 'Insert : Status = ' + CAST( @Status AS NVARCHAR(10))

	--اگر در وضعيت قيمت گذاري شده باشد بايد سند بخورد
	IF (@Status=3 OR @Status=4)
	BEGIN
		UPDATE [Inventory].Transactions SET [Status] = 4  WHERE Code= @Code AND [Action]= @Action AND [Status]=@Status
	END
	ELSE
	BEGIN
		RAISERROR(N'@The selected reference is not fully priced and could not be vouchered ', 16, 1, '500')	 
	END
END TRY
BEGIN CATCH
   --      DECLARE @MSG NVARCHAR(256)
		 --IF ERROR_MESSAGE() = 'ThisRecordUseForFIFOSystem' OR ERROR_MESSAGE() = 'ThisRecordUseForFIFOSystem'
		 --   RAISERROR(
		 --       N'@اين رکورد در قيمت گذاري حواله استفاده شده و قابل تغيير نمي باشد ',
		 --       16,
		 --       1,
		 --       '500'
		 --   )
	EXEC [Inventory].[ErrorHandling]
END CATCH;
ENABLE TRIGGER Transactions.UpdateTransaction ON [Inventory].[Transactions];
GO
------------------------------------------------------------------------------------
IF OBJECT_ID('[Fuel].[DeleteVouchers]', 'TR') IS NOT NULL
    DROP TRIGGER [Fuel].[DeleteVouchers];
GO
CREATE TRIGGER [Fuel].[DeleteVouchers]
ON [Fuel].[Vouchers]
   AFTER DELETE
AS
DISABLE TRIGGER Transactions.UpdateTransaction ON [Inventory].[Transactions];
BEGIN TRY
	DECLARE @Id                      int ,
			@VoucherTypeId TINYINT,--			1-Reciept 2-Issue  3-Invoice
			@ReferenceNumber NVARCHAR(512),
			@Status TINYINT,
			@Code DECIMAL(18,2),
			@Action TINYINT,
			@InventoryOperationTextIndex INT,
			@WarehouseId BIGINT
			
    SELECT TOP(1) @Id  =Id , @VoucherTypeId  =VoucherTypeId , @ReferenceNumber  =ReferenceNo FROM   DELETED
      
    IF(PATINDEX('%Offhire%', @ReferenceNumber) <> 0)
	--Offhire Vouchers must be bypassed.
		RETURN;

	SET @InventoryOperationTextIndex = PATINDEX('%|Issue%', @ReferenceNumber)
	IF (@InventoryOperationTextIndex = 0) 
		SET @InventoryOperationTextIndex = PATINDEX('%|Receipt%', @ReferenceNumber)
	IF (@InventoryOperationTextIndex = 0) 
		SET @InventoryOperationTextIndex = PATINDEX('%|Factor%', @ReferenceNumber)
		
	IF (@InventoryOperationTextIndex = 0)  -- The TextIndex using '|' delimiter not found.
	BEGIN
		SET @InventoryOperationTextIndex = PATINDEX('%Issue%', @ReferenceNumber)
		IF (@InventoryOperationTextIndex = 0) 
			SET @InventoryOperationTextIndex = PATINDEX('%Receipt%', @ReferenceNumber)
		IF (@InventoryOperationTextIndex = 0) 
			SET @InventoryOperationTextIndex = PATINDEX('%Factor%', @ReferenceNumber)			
	END
	ELSE
		SET @InventoryOperationTextIndex = @InventoryOperationTextIndex + 1 --The index has been found using '|' separator and it should be eliminated.
	
	IF (@InventoryOperationTextIndex = 0)  	
		RAISERROR(N'@Invalid Voucher Reference No. inorder to find relevant Inventory Transaction.', 16, 1, '500')	 
	
	
	SET @ReferenceNumber = SUBSTRING(@ReferenceNumber, @InventoryOperationTextIndex,  LEN(@ReferenceNumber))
	
	DECLARE @FirstSlashDelimiterPosition INT,
		@SecondSlashDelimiterPosition INT,
		@ThirdSlashDelimiterPosition INT

	SET @FirstSlashDelimiterPosition = PATINDEX('%/%', @ReferenceNumber);
	SET @SecondSlashDelimiterPosition = PATINDEX('%/%', SUBSTRING(@ReferenceNumber, @FirstSlashDelimiterPosition + 1, LEN(@ReferenceNumber)));
	SET @ThirdSlashDelimiterPosition = PATINDEX('%/%', SUBSTRING(@ReferenceNumber, @FirstSlashDelimiterPosition + @SecondSlashDelimiterPosition + 1, LEN(@ReferenceNumber)));
	
	IF(@SecondSlashDelimiterPosition = 0) SET @SecondSlashDelimiterPosition = 1
	IF(@ThirdSlashDelimiterPosition = 0) SET @ThirdSlashDelimiterPosition = 1

	SET @WarehouseId = CAST( SUBSTRING(@ReferenceNumber, @FirstSlashDelimiterPosition + 1, @SecondSlashDelimiterPosition - 1) as BIGINT)
	SET @Code = CAST( SUBSTRING(@ReferenceNumber, @FirstSlashDelimiterPosition + @SecondSlashDelimiterPosition + 1, @ThirdSlashDelimiterPosition - 1) as DEcimal)

	SET @Action = CASE  SUBSTRING(@ReferenceNumber, 1,  PATINDEX('%/%', @ReferenceNumber) - 1) 
		WHEN 'Issue' THEN 2 
		WHEN 'Receipt' THEN 1 
		ELSE 3 END 
		
		

	SELECT TOP(1) @Status=t.[Status] FROM [Inventory].Transactions t WHERE  t.Code= @Code AND t.[Action]= @Action
	
	PRINT 'Delete : Status = ' + CAST( @Status AS NVARCHAR(10))

		--اگر در وضعيت ثبت سند باشد بايد به قيمت گذاري شده تغيير داده شود
	 IF (@Status=4)
		BEGIN
				

			UPDATE [Inventory].Transactions SET [Status] = 3  WHERE Code=@Code AND [Action]= @Action AND [Status]=4

			
		END
END TRY
	
BEGIN CATCH
		--DECLARE @MSG NVARCHAR(256)
		--IF ERROR_MESSAGE() = 'ValidAnyRecordUsingThisReference'
		--RAISERROR(
		--    16,
		--    1,
		--    '500'
		--)
	EXEC [Inventory].[ErrorHandling]
END CATCH;
ENABLE TRIGGER Transactions.UpdateTransaction ON [Inventory].[Transactions];
GO
----------------------------------------------------------------------------
	RAISERROR(N'تریگرها با موفقیت ایجاد شدند.', 0, 1) WITH NOWAIT
GO

--SELECT PATINDEX('%/%', '123456789'), SUBSTRING('123/456/789', PATINDEX('%/%', '123/456/789') + 1,0)