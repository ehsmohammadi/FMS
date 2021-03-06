--USE MiniStock
--GO
------------------------------------------------------------------------------------
IF OBJECT_ID('[Inventory].[UpdateTransaction]', 'TR') IS NOT NULL
    DROP TRIGGER [Inventory].[UpdateTransaction];
GO
CREATE TRIGGER [Inventory].[UpdateTransaction]
ON [Inventory].[Transactions]
   AFTER INSERT, UPDATE
AS
BEGIN TRY
	DECLARE @Id        INT,
			@Action tinyint ,
			@Code decimal(20,2) ,
			@Description nvarchar(max),
			--@CrossId int ,
			@PricingReferenceId int ,
			@WarehouseId BIGINT ,
			@TimeBucketId INT,
			@StoreTypesId INT ,
			@Status tinyint ,
			@RegistrationDate DATETIME ,
			@SenderReciver int ,
			@HardCopyNo NVARCHAR(10) ,
			@ReferenceType NVARCHAR(100) ,
			@ReferenceNo NVARCHAR(100) ,
			@ReferenceDate DATETIME ,
			@UserCreatorId int,

			@CHKNegativ TINYINT,
			
			@IdOld        INT,
			@ActionOld tinyint ,
			@CodeOld decimal(20,2) ,
			@DescriptionOld nvarchar(max),
			@PricingReferenceIdOld int ,
			@WarehouseIdOld BIGINT ,
			@TimeBucketIdOld INT,
			@StoreTypesIdOld INT ,
			@StatusOld tinyint ,
			@RegistrationDateOld DATETIME ,
			@SenderReciverOld int ,
			@HardCopyNoOld NVARCHAR(10) ,
			@ReferenceTypeOld NVARCHAR(100) ,
			@ReferenceNoOld NVARCHAR(100) ,
			@ReferenceDateOld DATETIME ,
			@UserCreatorIdOld INT,
			
			@StartDateTimeBucket  DATETIME,
			@EndDateTimeBucket  DATETIME
			
	SELECT TOP(1) @Id         = Id,@Action  = Action,@Code  = Code,@Description  = Description,@PricingReferenceId  = PricingReferenceId,
					@WarehouseId  = WarehouseId,@TimeBucketId =TimeBucketId,@StoreTypesId  = StoreTypesId,@Status  = Status,@RegistrationDate  = RegistrationDate,
					@SenderReciver  = SenderReciver,@HardCopyNo  = HardCopyNo,@ReferenceType  = ReferenceType,
					@ReferenceNo  = ReferenceNo,@ReferenceDate  = ReferenceDate,@UserCreatorId  = UserCreatorId	FROM  INSERTED 
					
	SELECT TOP(1) @IdOld         = Id,@ActionOld  = Action,@CodeOld  = Code,
					@DescriptionOld  = Description,@PricingReferenceIdOld  = PricingReferenceId,@WarehouseIdOld  = WarehouseId,
					@TimeBucketIdOld =TimeBucketId, @StoreTypesIdOld  = StoreTypesId,@StatusOld  = Status,
					@RegistrationDateOld  = RegistrationDate,@SenderReciverOld  = SenderReciver,
					@HardCopyNoOld  = HardCopyNo,@ReferenceTypeOld  = ReferenceType,@ReferenceNoOld  = ReferenceNo,
					@ReferenceDateOld  = ReferenceDate,@UserCreatorIdOld  = UserCreatorId	FROM  DELETED
       
        IF (SELECT w.[IsActive] FROM [Inventory].Warehouse w WHERE w.Id=@WarehouseId)=0 AND (@Status = @StatusOld)
	 		 RAISERROR(N'@The inventory working on is not active while registering Transaction.', 16, 1, '500')
	 	
	 	SELECT @StartDateTimeBucket=tb.StartDate,
	           @EndDateTimeBucket=tb.EndDate
	 	FROM [Inventory].TimeBucket tb WHERE tb.Id=@TimeBucketId
	 	IF @RegistrationDate<@StartDateTimeBucket OR @RegistrationDate>@EndDateTimeBucket
	 	   RAISERROR(N'@The Registration date mismatch with current active time bucket ',16,1,'500')
	 		 
		SET @CHKNegativ=[Inventory].CheckNegetiveWarehouse(@WarehouseId,0,0,@TimeBucketId)
		IF @CHKNegativ=0
		BEGIN
			RAISERROR(N'Warehousemanfinist', 16, 1)
			RETURN
		END
		
		IF ISNULL((SELECT TOP(1)t.Status FROM [Inventory].Transactions t WHERE t.Id=@Id), 0)=4
		   AND @StatusOld<>3
		BEGIN
			DECLARE @ErrorMessage NVARCHAR(400)

			SET @ErrorMessage = N'@The transaction is vouchered and is locked while updating Transaction with Id = ' + CAST(@Id as nvarchar(50))
			RAISERROR(@ErrorMessage,16,1,'500')
		END
END TRY
BEGIN CATCH

		 IF ERROR_MESSAGE() = 'Warehousemanfinist'
		    RAISERROR(
		        N'@The operation will result to lack in inventory quantity [UpdateTransaction]',
		        16,
		        1,
		        '500'
		    )
		   
	EXEC [Inventory].[ErrorHandling]
END CATCH
GO
------------------------------------------------------------------------------------
IF OBJECT_ID('[Inventory].[DeleteTransaction]', 'TR') IS NOT NULL
    DROP TRIGGER [Inventory].[DeleteTransaction];
GO
CREATE TRIGGER [Inventory].[DeleteTransaction]
ON [Inventory].[Transactions]
   AFTER DELETE
AS
BEGIN TRY
	DECLARE @Id        INT,
			@Action tinyint ,
			@Code decimal(20,2) ,
			@Description nvarchar(max),
			@PricingReferenceId int ,
			@WarehouseId BIGINT ,
			@TimeBucketId INT,
			@StoreTypesId INT ,
			@Status tinyint ,
			@RegistrationDate DATETIME ,
			@SenderReciver int ,
			@HardCopyNo NVARCHAR(10) ,
			@ReferenceType NVARCHAR(100) ,
			@ReferenceNo NVARCHAR(100) ,
			@ReferenceDate DATETIME ,
			@UserCreatorId int,
			@CHKNegativ TINYINT
	SELECT TOP(1) @Id         = Id,@Action  = Action,@Code  = Code,@Description  = Description,@PricingReferenceId  = PricingReferenceId,
					@WarehouseId  = WarehouseId,@TimeBucketId =TimeBucketId,@StoreTypesId  = StoreTypesId,@Status  = Status,@RegistrationDate  = RegistrationDate,
					@SenderReciver  = SenderReciver,@HardCopyNo  = HardCopyNo,@ReferenceType  = ReferenceType,
					@ReferenceNo  = ReferenceNo,@ReferenceDate  = ReferenceDate,@UserCreatorId  = UserCreatorId	FROM    DELETED
	   
	   IF (SELECT w.[IsActive] FROM [Inventory].Warehouse w WHERE w.Id=@WarehouseId)=0
	 		 RAISERROR(N'@The inventory working on is not active while removing Transaction.', 16, 1, '500')
	
		SET @CHKNegativ=[Inventory].CheckNegetiveWarehouse(@WarehouseId,0,0,@TimeBucketId)
		IF @CHKNegativ=0
		BEGIN
			RAISERROR(N'Warehousemanfinist', 16, 1)
			RETURN
		END
		
		IF ISNULL((SELECT TOP(1)t.Status FROM [Inventory].Transactions t WHERE t.Id=@Id), 0)=4
		BEGIN
			DECLARE @ErrorMessage NVARCHAR(400)

			SET @ErrorMessage = N'@The transaction is vouchered and is locked while removing Transaction with Id = ' + CAST(@Id as nvarchar(50))
			RAISERROR(@ErrorMessage,16,1,'500')
		END
END TRY
BEGIN CATCH
		 IF ERROR_MESSAGE() = 'Warehousemanfinist'
		    RAISERROR(
		        N'@The operation will result to lack in inventory quantity [DeleteTransaction]',
		        16,
		        1,
		        '500'
		    )
	EXEC [Inventory].[ErrorHandling]
END CATCH
GO
------------------------------------------------------------------------------------
IF OBJECT_ID('[Inventory].[UpdateTransactionItems]', 'TR') IS NOT NULL
    DROP TRIGGER [Inventory].[UpdateTransactionItems];
GO
CREATE TRIGGER [Inventory].[UpdateTransactionItems]
ON [Inventory].[TransactionItems]
   AFTER INSERT, UPDATE
AS
BEGIN TRY
	DECLARE @TransactionId        INT,
			@Id int =Null,
			@GoodId BIGINT = null,
			@QuantityUnitId BIGINT,
			@QuantityAmount DECIMAL(20,3) = 0,
			@Description nvarchar(max) =NULL,
			@RowVersion SMALLINT,
			
			@WarehouseId BIGINT,
			@Action TINYINT,
			@CHKNegativ TINYINT,
			@CHKOverFlow TINYINT,
			@TotalQuantityAmount DECIMAL(20,3),
			@PricedQuantityAmount DECIMAL(20,3),
			
			@TransactionIdOld        INT,
			@IdOld int =Null,
			@GoodIdOld BIGINT = null,
			@QuantityUnitIdOld BIGINT,
			@QuantityAmountOld DECIMAL(20,3) = 0,
			@DescriptionOld nvarchar(max) =NULL,
			@RowVersionOld SMALLINT,
			@TimeBucketId INT
			
	SELECT TOP(1) @TransactionId=TransactionId, @Id=Id,@GoodId=GoodId,@QuantityUnitId=QuantityUnitId,
	              @QuantityAmount=QuantityAmount,@Description=Description,@RowVersion=[RowVersion]	FROM  INSERTED 
	SELECT TOP(1) @TransactionIdOld=TransactionId, @IdOld=Id,@GoodIdOld=GoodId,@QuantityUnitIdOld=QuantityUnitId,
	              @QuantityAmountOld=QuantityAmount,@DescriptionOld=Description,@RowVersionOld=RowVersion	FROM  DELETED

	--<A.H>
	IF(ISNULL(@QuantityUnitIdOld, -1) = ISNULL(@QuantityUnitId, -1) AND
		ISNULL(@QuantityAmountOld, -1) = ISNULL(@QuantityAmount, -1))
		RETURN

	    SELECT TOP(1) @WarehouseId=t.WarehouseId,@Action=t.[Action] FROM [Inventory].Transactions t WHERE t.Id=@TransactionId
	    SELECT @TimeBucketId=TimeBucketId FROM [Inventory].Transactions t WHERE t.Id=@TransactionId
	 	IF (SELECT w.[IsActive] FROM [Inventory].Warehouse w WHERE w.Id=@WarehouseId)=0
	 		 RAISERROR(N'@The inventory working on is not active while inserting Items.', 16, 1, '500')
		SET @CHKNegativ=[Inventory].CheckNegetiveWarehouseValue(@WarehouseId,@GoodId ,@TimeBucketId ,@RowVersion,@Action,@TransactionId,@QuantityAmount,@QuantityUnitId,0)
		
		IF (SELECT TOP(1)u.IsCurrency FROM [Inventory].Units u WHERE u.Id=@QuantityUnitId)=1
		BEGIN
			RAISERROR(N'QuantityUnitNotUseForAmount', 16, 1)
			RETURN
		END
		IF @CHKNegativ=0
		BEGIN
			RAISERROR(N'Warehousemanfinist1', 16, 1)
			RETURN
		END
		SET @CHKNegativ=[Inventory].CheckNegetiveWarehouse(@WarehouseId,@GoodId,@TimeBucketId ,@QuantityAmount)
		IF @CHKNegativ=0
		BEGIN
			RAISERROR(N'Warehousemanfinist2', 16, 1)
			RETURN
		END
		SET @CHKOverFlow=[Inventory].CheckOverFlowQuantityPricing(@Action,@Id,@TransactionId,@QuantityUnitId)
		IF @CHKOverFlow=0
		BEGIN
			RAISERROR(N'OverFlowPriceing', 16, 1)
			RETURN
		END
		SET @TotalQuantityAmount=ISNULL((SELECT SUM(ti.QuantityAmount) FROM [Inventory].TransactionItems ti WHERE ti.TransactionId=@TransactionId),0)
		SET @PricedQuantityAmount=ISNULL((SELECT SUM(tip.QuantityAmount) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionId=@TransactionId),0)
		IF @TotalQuantityAmount=@PricedQuantityAmount
			UPDATE Transactions SET [Status] = 3 WHERE Id=@TransactionId AND [Status] <> 4
		ELSE IF @TotalQuantityAmount<>@PricedQuantityAmount
			UPDATE Transactions SET [Status] = 2 WHERE Id=@TransactionId
			
		IF ISNULL((SELECT TOP(1)t.Status FROM [Inventory].Transactions t WHERE t.Id=@TransactionId), 0)=4
		BEGIN
			DECLARE @ErrorMessage NVARCHAR(400)

			SET @ErrorMessage = N'@The transaction is vouchered and is locked while inserting Items for Transaction with Id = ' + CAST(@TransactionId as nvarchar(50))
			RAISERROR(@ErrorMessage,16,1,'500')
		END
END TRY
BEGIN CATCH

		 IF ERROR_MESSAGE() = 'Warehousemanfinist1'
		    RAISERROR(
		        N'@The operation will result to lack in inventory quantity [UpdateTransactionItems]1',
		        16,
		        1,
		        '500'
		    )

		IF ERROR_MESSAGE() = 'Warehousemanfinist2'
		    RAISERROR(
		        N'@The operation will result to lack in inventory quantity [UpdateTransactionItems]2',
		        16,
		        1,
		        '500'
		    )

		 IF ERROR_MESSAGE() = 'QuantityUnitNotUseForAmount'
		    RAISERROR(
		        N'@The unit for quantity should be measurable unit',
		        16,
		        1,
		        '500'
		    )
		 DECLARE @MSG NVARCHAR(256)
		 IF ERROR_MESSAGE() = 'OverFlowPriceing'
		 BEGIN
		 	SET @MSG= CASE WHEN @Action=1 THEN
         				N'@The pricing quantity is more than received quantity - 1'
         				WHEN @Action=2 THEN
         				N'@The pricing quantity is more than issued quantity - 1'
         		      END
         	RAISERROR(
		       @MSG,
		        16,
		        1,
		        '500'
		    )
		 END  
	EXEC [Inventory].[ErrorHandling]
END CATCH
GO
------------------------------------------------------------------------------------
IF OBJECT_ID('[Inventory].[DeleteTransactionItems]', 'TR') IS NOT NULL
    DROP TRIGGER [Inventory].[DeleteTransactionItems];
GO
CREATE TRIGGER [Inventory].[DeleteTransactionItems]
ON [Inventory].[TransactionItems]
   AFTER DELETE
AS
BEGIN TRY
	DECLARE @TransactionId        INT,
			@Id int =Null,
			@GoodId BIGINT = null,
			@QuantityUnitId BIGINT,
			@QuantityAmount DECIMAL(20,3) = 0,
			@Description nvarchar(max) =NULL,
			@RowVersion SMALLINT,
			@WarehouseId BIGINT,
			@Action TINYINT,
			@CHKNegativ TINYINT,
			@CHKOverFlow TINYINT,
			@TimeBucketId INT,
			@TotalQuantityAmount DECIMAL(20,3),
			@PricedQuantityAmount DECIMAL(20,3)

			
	SELECT TOP(1) @TransactionId=TransactionId, @Id=Id,@GoodId=GoodId,@QuantityUnitId=QuantityUnitId,
	              @QuantityAmount=QuantityAmount,@Description=Description,@RowVersion=[RowVersion]	FROM   DELETED
	   
	  SELECT TOP(1) @WarehouseId=t.WarehouseId,@Action=t.[Action] FROM [Inventory].Transactions t WHERE t.Id=@TransactionId
	   SELECT @TimeBucketId=TimeBucketId FROM [Inventory].Transactions t WHERE t.Id=@TransactionId
	  IF (SELECT w.[IsActive] FROM [Inventory].Warehouse w WHERE w.Id=@WarehouseId)=0
	 		 RAISERROR(N'@The inventory working on is not active while removing Items.', 16, 1, '500')
	 		 
	  SET @CHKNegativ=[Inventory].CheckNegetiveWarehouseValue(@WarehouseId,@GoodId ,@TimeBucketId ,@RowVersion,@Action,@TransactionId,@QuantityAmount,@QuantityUnitId,0)
		IF @CHKNegativ=0
		BEGIN
			RAISERROR(N'Warehousemanfinist', 16, 1)
			RETURN
		END
		SET @CHKNegativ=[Inventory].CheckNegetiveWarehouse(@WarehouseId,@GoodId,@TimeBucketId ,@QuantityAmount)
		IF @CHKNegativ=0
		BEGIN
			RAISERROR(N'Warehousemanfinist', 16, 1)
			RETURN
		END
		SET @CHKOverFlow=[Inventory].CheckOverFlowQuantityPricing(@Action,@Id,@TransactionId,@QuantityUnitId)
		IF @CHKOverFlow=0
		BEGIN
			RAISERROR(N'OverFlowPriceing', 16, 1)
			RETURN
		END
		SET @TotalQuantityAmount=ISNULL((SELECT SUM(ti.QuantityAmount) FROM [Inventory].TransactionItems ti WHERE ti.TransactionId=@TransactionId),0)
		SET @PricedQuantityAmount=ISNULL((SELECT SUM(tip.QuantityAmount) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionId=@TransactionId),0)
		IF @TotalQuantityAmount=@PricedQuantityAmount
			UPDATE Transactions SET [Status] = 3 WHERE Id=@TransactionId
		ELSE IF @TotalQuantityAmount<>@PricedQuantityAmount
			UPDATE Transactions SET [Status] = 2 WHERE Id=@TransactionId
			
		IF ISNULL((SELECT TOP(1)t.Status FROM [Inventory].Transactions t WHERE t.Id=@TransactionId), 0)=4
		BEGIN
			DECLARE @ErrorMessage NVARCHAR(400)

			SET @ErrorMessage = N'@The transaction is vouchered and is locked while removing Items for Transaction with Id = ' + CAST(@TransactionId as nvarchar(50))
			RAISERROR(@ErrorMessage,16,1,'500')
		END
END TRY
BEGIN CATCH
		 IF ERROR_MESSAGE() = 'Warehousemanfinist'
		    RAISERROR(
		        N'@The operation will result to lack in inventory quantity [DeleteTransactionItems]',
		        16,
		        1,
		        '500'
		    )
		 DECLARE @MSG NVARCHAR(256)
		 IF ERROR_MESSAGE() = 'OverFlowPriceing'
		 BEGIN
		 	SET @MSG= CASE WHEN @Action=1 THEN
         		     	N'@The pricing quantity is more than received quantity - 2'
         				WHEN @Action=2 THEN
         				N'@The pricing quantity is more than issued quantity - 2'
         				WHEN @Action=3 THEN
         				N'@The pricing factor quantity is more than referenced issued quantity'
         		      END
         	RAISERROR(
		       @MSG,
		        16,
		        1,
		        '500'
		    )
		 END
	EXEC [Inventory].[ErrorHandling]
END CATCH
GO
------------------------------------------------------------------------------------
IF OBJECT_ID('[Inventory].[UpdateTransactionItemPrices]', 'TR') IS NOT NULL
    DROP TRIGGER [Inventory].[UpdateTransactionItemPrices];
GO
CREATE TRIGGER [Inventory].[UpdateTransactionItemPrices]
ON [Inventory].[TransactionItemPrices]
   AFTER INSERT, UPDATE
AS
BEGIN TRY
	DECLARE @Id                      int ,
			@RowVersion              smallint ,
			@TransactionId               int ,
			@TransactionItemId           int ,
			@Description             nvarchar(max),
			@QuantityUnitId           BIGINT ,
			@QuantityAmount          DECIMAL(20,3) ,
			@PriceUnitId             INT ,
			@Fee                     DECIMAL(20,3) ,
			@RegistrationDate         DATETIME,
			@QuantityAmountUseFIFO   DECIMAL(20,3) ,
			@IdOld                      int ,
			@RowVersionOld           	smallint ,
			@TransactionIdOld            	int ,
			@TransactionItemIdOld        	int ,
			@DescriptionOld          	nvarchar(max),
			@QuantityUnitIdOld           BIGINT ,
			@QuantityAmountOld       	DECIMAL(20,3) ,
			@PriceUnitIdOld          	BIGINT ,
			@FeeOld                  	DECIMAL(20,3) ,
			@RegistrationDateOld         DATETIME,
			@QuantityAmountUseFIFOOld	DECIMAL(20,3) ,
			@TotalQuantityAmount DECIMAL(20,3),
			@PricedQuantityAmount DECIMAL(20,3),
			
			@WarehouseId BIGINT,
			@Action TINYINT,
			@CHKOverFlow TINYINT,
			@GoodId      BIGINT,
			@Code DECIMAL(20,2)			--Added by Hatefi 1394-10-16
			
	SELECT TOP(1) @Id  =Id , @RowVersion  =RowVersion , @TransactionId  =TransactionId , @TransactionItemId  =TransactionItemId,
					@Description  =DESCRIPTION ,@QuantityUnitId=QuantityUnitId, @QuantityAmount  = QuantityAmount , @PriceUnitId  =PriceUnitId ,
					@Fee  =Fee ,@RegistrationDate=RegistrationDate, @QuantityAmountUseFIFO  =QuantityAmountUseFIFO	FROM  INSERTED 
	SELECT TOP(1) @IdOld=Id,@RowVersionOld	=RowVersion,@TransactionIdOld	=TransactionId,@TransactionItemIdOld	=TransactionItemId,
					@DescriptionOld	=Description,@QuantityUnitIdOld=QuantityUnitId,@QuantityAmountOld	=QuantityAmount,@PriceUnitIdOld	=PriceUnitId,
					@FeeOld	=Fee,@RegistrationDateOld=RegistrationDate,@QuantityAmountUseFIFOOld	=QuantityAmountUseFIFO	FROM  DELETED



	--<A.H>
	IF(ISNULL(@QuantityUnitIdOld, -1) = ISNULL(@QuantityUnitId, -1) AND
		ISNULL(@QuantityAmountOld, -1) = ISNULL(@QuantityAmount, -1) AND
		ISNULL(@PriceUnitIdOld, -1) = ISNULL(@PriceUnitId, -1) AND
		ISNULL(@FeeOld, -1) = ISNULL(@Fee, -1) )
		RETURN




	    SELECT TOP(1) @WarehouseId=t.WarehouseId,@Action=t.[Action] ,
			@Code = Code			--Added by Hatefi 1394-10-16
		FROM [Inventory].Transactions t WHERE t.Id=@TransactionId

	    SELECT TOP(1) @GoodId=ti.GoodId FROM [Inventory].TransactionItems ti WHERE ti.Id=@TransactionItemId
	 	IF (SELECT w.[IsActive] FROM [Inventory].Warehouse w WHERE w.Id=@WarehouseId)=0
	 		 RAISERROR(N'@The inventory working on is not active while updating Prices.', 16, 1, '500')
		SET @CHKOverFlow=[Inventory].CheckOverFlowQuantityPricing(@Action,@TransactionItemId,@TransactionId,@QuantityUnitId)
		IF NOT EXISTS (SELECT TOP(1)u.IsBaseCurrency
		      FROM [Inventory].Units u WHERE u.IsBaseCurrency = 1)
		BEGIN
			RAISERROR(N'NeedBaseCurrency', 16, 1)
			RETURN
		END
		IF (SELECT TOP(1)u.IsCurrency FROM [Inventory].Units u WHERE u.Id=@QuantityUnitId)=1
		BEGIN
			RAISERROR(N'QuantityUnitNotUseForAmount', 16, 1)
			RETURN
		END
		IF (SELECT TOP(1)u.IsCurrency FROM [Inventory].Units u WHERE u.Id=@PriceUnitId)=0
		BEGIN
			RAISERROR(N'@PriceUnitNotUseForMony', 16, 1)
			RETURN
		END
		IF @Action=1 AND  @QuantityAmountUseFIFO>0 AND (@QuantityAmount<@QuantityAmountUseFIFO OR @Fee<>@FeeOld)
		BEGIN
			RAISERROR(N'ThisRecordUseForIssueAndNotChange', 16, 1)
			RETURN
		END
		IF @CHKOverFlow=0
		BEGIN
			RAISERROR(N'OverFlowPriceing', 16, 1)
			RETURN
		END
		IF @Action=1 AND @QuantityAmountUseFIFO>@QuantityAmount
		BEGIN
			RAISERROR(N'ThisRecordUseForFIFOSystem', 16, 1)
			RETURN
		END	
		IF @Action=1 
		   AND @QuantityAmountUseFIFO>@QuantityAmount 
		   AND EXISTS (SELECT TOP(1) t.Id FROM [Inventory].Transactions t WHERE t.PricingReferenceId=@TransactionId)
		BEGIN
			RAISERROR(N'ValidAnyRecordUsingThisReference',16,1)
			RETURN
		END
		--IF @QuantityAmount<>@QuantityAmountOld OR @Fee<>@FeeOld		--Commented by Hatefi 1394-10-16
		--BEGIN
		--	IF @Action=2 AND (EXISTS (SELECT TOP(1) tip.Id
		--							 FROM [Inventory].TransactionItems ti
		--							 INNER JOIN [Inventory].TransactionItemPrices tip
		--								ON tip.TransactionItemId = ti.Id
		--							 INNER JOIN [Inventory].Transactions t 
		--								ON t.Id = ti.TransactionId
		--							 WHERE t.Action=2 
		--								   --AND tip.Id>=@Id		--Commented by Hatefi 1394-10-16
		--								   AND t.RegistrationDate >= @RegistrationDate	--Added by Hatefi 1394-10-16
		--								   AND t.Code > @Code		--Added by Hatefi 1394-10-16
		--								   AND tip.RowVersion>@RowVersion
		--								   AND ti.GoodId=@GoodId
		--								   AND t.WarehouseId=@WarehouseId
		--							)
		--							OR
		--							EXISTS(SELECT TOP(1) t.Id 
		--								   FROM [Inventory].Transactions t WHERE t.PricingReferenceId=@TransactionId)
		--						)
		--	BEGIN
		--		RAISERROR(N'ValidAnyRecordAfterThisRecordAndUseFIFO', 16, 1)
		--		RETURN
		--	END
		--END
		
		SET @TotalQuantityAmount=ISNULL((SELECT SUM(ti.QuantityAmount) FROM [Inventory].TransactionItems ti WHERE ti.TransactionId=@TransactionId),0)
		SET @PricedQuantityAmount=ISNULL((SELECT SUM(tip.QuantityAmount) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionId=@TransactionId),0)
		IF @TotalQuantityAmount=@PricedQuantityAmount
			UPDATE [Inventory].Transactions SET [Status] = 3 WHERE Id=@TransactionId AND [Status] <> 4
		ELSE IF @TotalQuantityAmount<>@PricedQuantityAmount
			UPDATE [Inventory].Transactions SET [Status] = 2 WHERE Id=@TransactionId
			
		IF ISNULL((SELECT TOP(1)t.Status FROM [Inventory].Transactions t WHERE t.Id=@TransactionId), 0)=4
		BEGIN
			DECLARE @ErrorMessage NVARCHAR(400)

			SET @ErrorMessage = N'@The transaction is vouchered and is locked while updating Prices for Transaction with Id = ' + CAST(@TransactionId as nvarchar(50))
			RAISERROR(@ErrorMessage,16,1,'500')
		END
END TRY
BEGIN CATCH
         DECLARE @MSG NVARCHAR(256)
         IF ERROR_MESSAGE() = 'NeedBaseCurrency'
		    RAISERROR(
		        N'@The base currency should be defined for pricing ',
		        16,
		        1,
		        '500'
		    )
         IF ERROR_MESSAGE() = 'QuantityUnitNotUseForAmount'
		    RAISERROR(
		        N'@The unit for quantity should be measurable unit ',
		        16,
		        1,
		        '500'
		    )
		  IF ERROR_MESSAGE() = 'PriceUnitNotUseForMony'
		    RAISERROR(
		        N'@The unit for price should be in currency ',
		        16,
		        1,
		        '500'
		    )
		 IF ERROR_MESSAGE() = 'OverFlowPriceing'
		 BEGIN
		 	SET @MSG= CASE WHEN @Action=1 THEN
         				N'@The pricing quantity is more than received quantity - 3'
         				WHEN @Action=2 THEN
         				N'@The pricing quantity is more than issued quantity - 3'
         		      END
         	RAISERROR(
		       @MSG,
		        16,
		        1,
		        '500'
		    )
		 END
		 IF ERROR_MESSAGE() = 'ThisRecordUseForFIFOSystem' OR ERROR_MESSAGE() = 'ThisRecordUseForFIFOSystem'
		    RAISERROR(
		        N'@The transaction is used for pricing an issue and could not be changed ',
		        16,
		        1,
		        '500'
		    )
		 IF ERROR_MESSAGE() = 'ValidAnyRecordAfterThisRecordAndUseFIFO'
		    RAISERROR(
		        N'@This trnasaction is not the last record in FIFO pricing system and could not be changed',
		        16,
		        1,
		        '500'
		    )
		    IF ERROR_MESSAGE() = 'ValidAnyRecordUsingThisReference'
		    RAISERROR(
		        N'@This receipt is referenced by another transaction for pricing and could not be changed ',
		        16,
		        1,
		        '500'
		    )
		    
	EXEC [Inventory].[ErrorHandling]
END CATCH
GO
------------------------------------------------------------------------------------
IF OBJECT_ID('[Inventory].[DeleteTransactionItemPrices]', 'TR') IS NOT NULL
    DROP TRIGGER [Inventory].[DeleteTransactionItemPrices];
GO
CREATE TRIGGER [Inventory].[DeleteTransactionItemPrices]
ON [Inventory].[TransactionItemPrices]
   AFTER DELETE
AS
BEGIN TRY
	DECLARE @Id                      int ,
			@RowVersion              smallint ,
			@TransactionId               int ,
			@TransactionItemId           int ,
			@Description             nvarchar(max),
			@QuantityUnitId           BIGINT ,
			@QuantityAmount          DECIMAL(20,3) ,
			@PriceUnitId             BIGINT ,
			@Fee                     DECIMAL(20,3) ,
			@RegistrationDate         DATETIME,
			@QuantityAmountUseFIFO   DECIMAL(20,3) ,
			
			
			@WarehouseId BIGINT,
			@Action TINYINT,
			@CHKOverFlow TINYINT,
			@GoodId      INT,
			@TotalQuantityAmount DECIMAL(20,3),
			@PricedQuantityAmount DECIMAL(20,3),
			@Code DECIMAL(20,2)			--Added by Hatefi 1394-10-16

			
    	SELECT TOP(1) @Id  =Id ,@RowVersion  =RowVersion , @TransactionId  =TransactionId , @TransactionItemId  =TransactionItemId,
					@Description  =DESCRIPTION ,@QuantityUnitId=QuantityUnitId, @QuantityAmount  = QuantityAmount , @PriceUnitId  =PriceUnitId ,
					@Fee  =Fee ,@RegistrationDate=RegistrationDate, @QuantityAmountUseFIFO  =QuantityAmountUseFIFO FROM   DELETED
	   
	  SELECT TOP(1) @WarehouseId=t.WarehouseId,@Action=t.[Action], 
			@Code = Code			--Added by Hatefi 1394-10-16
	  FROM [Inventory].Transactions t WHERE t.Id=@TransactionId
	  SELECT TOP(1) @GoodId=ti.GoodId
			  FROM [Inventory].TransactionItems ti WHERE ti.Id=@TransactionItemId
			  
	 	IF (SELECT w.[IsActive] FROM [Inventory].Warehouse w WHERE w.Id=@WarehouseId)=0
	 		 RAISERROR(N'@The inventory working on is not active while removing Prices.', 16, 1, '500')
		SET @CHKOverFlow=[Inventory].CheckOverFlowQuantityPricing(@Action,@TransactionItemId,@TransactionId,@QuantityUnitId)
		IF @CHKOverFlow=0
		BEGIN
			RAISERROR(N'OverFlowPriceing', 16, 1)
			RETURN
		END
		IF @Action=1 AND @QuantityAmountUseFIFO>0
		BEGIN
			RAISERROR(N'ThisRecordUseForFIFOSystem', 16, 1)
			RETURN
		END
		IF @Action=1 
		   AND @QuantityAmountUseFIFO>@QuantityAmount 
		   AND EXISTS (SELECT TOP(1) t.Id FROM [Inventory].Transactions t WHERE t.PricingReferenceId=@TransactionId)
		BEGIN
			RAISERROR(N'ValidAnyRecordUsingThisReference',16,1)
			RETURN
		END
		--IF @Action=2 AND (EXISTS (SELECT TOP(1) tip.Id		--Commented by Hatefi 1394-10-16
		--                         FROM [Inventory].TransactionItems ti
		--						 INNER JOIN [Inventory].TransactionItemPrices tip
		--							ON tip.TransactionItemId = ti.Id
		--						 INNER JOIN [Inventory].Transactions t 
		--							ON t.Id = ti.TransactionId
		--                         WHERE t.Action=2 
		--                               --AND tip.Id>=@Id		--Commented by Hatefi 1394-10-16
		--							   AND t.RegistrationDate >= @RegistrationDate	--Added by Hatefi 1394-10-16
		--							   AND t.Code > @Code		--Added by Hatefi 1394-10-16
		--							   AND tip.RowVersion>@RowVersion
		--						       AND ti.GoodId=@GoodId
		--						       AND t.WarehouseId=@WarehouseId
		--						)
		--						OR
		--						--1393-12-10 : Commented by Hatefi and replaced by more specific implementation
		--						--EXISTS(SELECT TOP(1) t.Id 
		--						--       FROM [Inventory].Transactions t WHERE t.PricingReferenceId=@TransactionId)
		--						EXISTS(SELECT * FROM [Inventory].Transactions t INNER JOIN Inventory.TransactionItems ti ON ti.TransactionId = t.Id
		--								INNER JOIN Inventory.TransactionItemPrices tip ON tip.TransactionItemId = ti.Id 
		--								WHERE t.PricingReferenceId=@TransactionId AND t.WarehouseId = @WarehouseId AND ti.GoodId = @GoodId)
		--					)
		--BEGIN

		--	print 'TransactionId  : ' + CAST ( @TransactionId	  as NVARCHAR(40))
		--	print 'TransactionItemId  : ' + CAST ( @TransactionItemId           as NVARCHAR(40))
		--	print 'GoodId  : ' + CAST ( @GoodId		  as NVARCHAR(40))
		--	print 'TransactionItemPriceId  : ' + CAST ( @Id           as NVARCHAR(40))
		--	print 'RowVersion  : ' + CAST ( @RowVersion	  as NVARCHAR(40))
		--	print 'WarehouseId  : ' + CAST ( @WarehouseId	  as NVARCHAR(40))

		--	RAISERROR(N'ValidAnyRecordAfterThisRecordAndUseFIFO', 16, 1)
		--	RETURN
		--END

		--1393-12-10 : Commented by Hatefi beacause of duplicated implementation.
		/*
		IF @Action=2 AND EXISTS (SELECT TOP(1) tip.Id
		                         FROM [Inventory].TransactionItems ti
								 INNER JOIN [Inventory].TransactionItemPrices tip
									ON tip.TransactionItemId = ti.Id
								 INNER JOIN [Inventory].Transactions t 
									ON t.Id = ti.TransactionId
		                         WHERE t.Action=2 
		                               AND tip.Id>=@Id
									   AND tip.RowVersion>@RowVersion
								       AND ti.GoodId=@GoodId
								       AND t.WarehouseId=@WarehouseId
		)
		BEGIN
			RAISERROR(N'ValidAnyRecordAfterThisRecordAndUseFIFO', 16, 1)
			RETURN
		END
		*/
		SET @TotalQuantityAmount=ISNULL((SELECT SUM(ti.QuantityAmount) FROM [Inventory].TransactionItems ti WHERE ti.TransactionId=@TransactionId),0)
		SET @PricedQuantityAmount=ISNULL((SELECT SUM(tip.QuantityAmount) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionId=@TransactionId),0)
		IF @TotalQuantityAmount=@PricedQuantityAmount
			UPDATE Transactions SET [Status] = 3 WHERE Id=@TransactionId AND [Status] <> 4
		ELSE IF @TotalQuantityAmount<>@PricedQuantityAmount
			UPDATE Transactions SET [Status] = 2 WHERE Id=@TransactionId
		
		UPDATE Inventory.TransactionItemPrices
		SET
			QuantityAmountUseFIFO -= @QuantityAmount,
			IssueReferenceIds = REPLACE(IssueReferenceIds, CAST(@Id AS NVARCHAR(20)) + ';', '')
		WHERE IssueReferenceIds LIKE '%' + CAST(@Id AS NVARCHAR(20)) + ';%' AND @Action = 2 
		
		IF ISNULL((SELECT TOP(1)t.Status FROM [Inventory].Transactions t WHERE t.Id=@TransactionId), 0)=4
		BEGIN
			DECLARE @ErrorMessage NVARCHAR(400)

			SET @ErrorMessage = N'@The transactions is vouchered and is locked while removing Prices for Transaction with Id = ' + CAST(@TransactionId as nvarchar(50))
			RAISERROR(@ErrorMessage,16,1,'500')
		END
END TRY
BEGIN CATCH
		DECLARE @MSG NVARCHAR(256)
         
		 IF ERROR_MESSAGE() = 'OverFlowPriceing'
		 BEGIN
		 	SET @MSG= CASE WHEN @Action=1 THEN
         				N'@The pricing quantity is more than received quantity - 4'
         				WHEN @Action=2 THEN
         				N'@The pricing quantity is more than issued quantity - 4'
         		      END
         	RAISERROR(
		       @MSG,
		        16,
		        1,
		        '500'
		    )
		 END
		 IF ERROR_MESSAGE() = 'ThisRecordUseForFIFOSystem'
		    RAISERROR(
		      N'@The transaction is used for pricing and could not be deleted ',
		        16,
		        1,
		        '500'
		    )
		IF ERROR_MESSAGE() = 'ValidAnyRecordAfterThisRecordAndUseFIFO'
		    RAISERROR(
		        N'@This trnasaction is not the last record in FIFO pricing system and could not be deleted',
		        16,
		        1,
		        '500'
		    )
		    IF ERROR_MESSAGE() = 'ValidAnyRecordUsingThisReference'
		    RAISERROR(
		         N'@This receipt is referenced by another transaction for pricing and could not be changed ',
		        16,
		        1,
		        '500'
		    )
	EXEC [Inventory].[ErrorHandling]
END CATCH
GO
----------------------------------------------------------------------------

RAISERROR(N'تریگرها با موفقیت ایجاد شدند.', 0, 1) WITH NOWAIT
GO
