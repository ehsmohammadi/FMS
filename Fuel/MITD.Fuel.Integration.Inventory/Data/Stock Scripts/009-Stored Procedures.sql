--USE MiniStock
--GO 
ALTER DATABASE StorageSpace SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE StorageSpaceRevert SET RECURSIVE_TRIGGERS OFF
GO
----------------------------------------------------------------------
IF OBJECT_ID (N'[Inventory].[ErrorHandling]',N'P') IS NOT NULL
   DROP PROCEDURE [Inventory].[ErrorHandling];
GO
CREATE PROCEDURE [Inventory].[ErrorHandling] 
--WITH ENCRYPTION
AS
BEGIN
	IF ERROR_NUMBER() IS NULL
		RETURN;
		
	DECLARE 
		@ErrorMessage    NVARCHAR(4000),
		@ErrorNumber     INT,
		@ErrorSeverity   INT,
		@ErrorState      INT,
		@ErrorLine       INT,
		@ErrorProcedure  NVARCHAR(200),
	    @ErrorText NVARCHAR(MAX)=NULL;
	    
	SELECT 
			@ErrorMessage= ERROR_MESSAGE(),
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');
		
	
	IF LEFT(@ErrorMessage,1)=N'@'
	  BEGIN
		SET @ErrorText =SUBSTRING(@ErrorMessage,2,LEN(@ErrorMessage))
		RAISERROR(@ErrorText,16,1,500)
	  END
	ELSE
	 BEGIN
		SET @ErrorText=(SELECT em.TextMessage  
					FROM [Inventory].[ErrorMessages] em
					WHERE (PATINDEX('%'+''''+UPPER(em.ErrorMessage)+''''+'%',UPPER(@ErrorMessage))<>0 OR 
					   PATINDEX('%'+'"'+UPPER(em.ErrorMessage)+'"'+'%',UPPER(@ErrorMessage))<>0)AND
						   PATINDEX('%'+UPPER(em.[Action])+'%',UPPER(@ErrorMessage))<>0)
 
		IF @ErrorText IS NULL OR @ErrorText=''
		  BEGIN
			SELECT @ErrorMessage =N'Error %d, Level %d, State %d, Procedure %s, Line %d, ' + 
									'Message: '+ ERROR_MESSAGE();
			RAISERROR 
			(
			@ErrorMessage, 
			@ErrorSeverity, 
			1,               
			@ErrorNumber,    -- parameter: original error number.
			@ErrorSeverity,  -- parameter: original error severity.
			@ErrorState,     -- parameter: original error state.
			@ErrorProcedure, -- parameter: original error procedure name.
			@ErrorLine       -- parameter: original error line number.
			);
		  END
		ELSE
			RAISERROR(@ErrorText,16,1,'500')
	 END
END 
GO
GRANT EXECUTE ON [Inventory].[ErrorHandling] TO [public] AS [dbo]
GO

-------- IsValidTransactionCode -------
	IF OBJECT_ID ( '[Inventory].IsValidTransactionCode', 'P' ) IS NOT NULL 
		DROP PROCEDURE [Inventory].IsValidTransactionCode;
	GO
	CREATE PROC [Inventory].IsValidTransactionCode
	(
		@Action tinyint,
		@Code decimal(20,2),
		@WarehouseId BIGINT,
		@RegistrationDate DATETIME,
		@TimeBucketId INT
	)
	AS
	BEGIN
		    IF @RegistrationDate IS NULL 
				BEGIN
					 RAISERROR(N'@Invalid Date In IsValidTransactionCode',16,1,'500')
				END
		SET NOCOUNT ON;
		SELECT [Inventory].IsValidRHCode(@Action,@Code,@WarehouseId,@RegistrationDate,@TimeBucketId)
	END
GO
GRANT EXECUTE ON [Inventory].IsValidTransactionCode TO [public] AS [dbo]
GO

----------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].[ChangeWarehouseStatus]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Inventory].[ChangeWarehouseStatus];
GO
CREATE PROCEDURE [Inventory].[ChangeWarehouseStatus]
(
	@IsActive BIT,
	--@CompanyId BIGINT,
	@WarehouseId BIGINT,
	@ChangeDateTime DATETIME,
	@UserCreatorId INT
)	
--WITH ENCRYPTION
AS
BEGIN
SET NOCOUNT ON;
DECLARE @TRANSACTIONCOUNT INT;
    SET @TRANSACTIONCOUNT = @@TRANCOUNT;
BEGIN TRY
	   if @TRANSACTIONCOUNT = 0
            BEGIN TRANSACTION
        --else
        --    SAVE TRANSACTION CurrentTransaction;


	   --DECLARE @WarehouseId BIGINT,
	   --        @CompanyId BIGINT
	   -- SET @CompanyId = (
	   --        SELECT TOP(1)c.Id
	   --        FROM   Companies c 
	   --        WHERE c.Name = @Company
	   -- )
	   --SET @WarehouseId = (
	   --        SELECT TOP(1)w.Id
	   --        FROM   Warehouse w
	   --        WHERE  w.Name = @Warehouse
	   --               AND w.CompanyId = @CompanyId
	   --) 
	   
	   DECLARE @ROB DECIMAL(20,3)
	   SET @ROB=(SELECT SUM(ISNULL(ti.QuantityAmount,0) * CASE WHEN t.[Action]=1 THEN (1) WHEN t.[Action]=2 THEN (-1) END)
							   FROM [Inventory].Transactions t
							   INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId = t.Id
							   INNER JOIN [Inventory].Warehouse a ON a.Id = t.WarehouseId
							   WHERE --a.CompanyId=@CompanyId
							         --AND 
							         t.WarehouseId=@WarehouseId 
									 AND t.RegistrationDate <= @ChangeDateTime

							         --AND ti.GoodId=CASE WHEN @GoodId <> NULL THEN @GoodId ELSE ti.GoodId END
							         --AND (@TransactionId IS NULL OR t.Id< @TransactionId )
							         --AND (@RowVersion IS NULL OR ti.RowVersion< @TransactionId )
							         --AND ti.QuantityUnitId=CASE WHEN @QuantityUnitId <> NULL THEN @QuantityUnitId ELSE ti.QuantityUnitId END
				)
	   --DECLARE @CHKNegativ TINYINT
	   --SET @CHKNegativ=[Inventory].CheckNegetiveWarehouse(@WarehouseId,0,0,0)
	   --IF @CHKNegativ=0
	   IF @ROB<>0
	   BEGIN
	   	   DECLARE @MSG NVARCHAR(256)
	   	   SET @MSG=N'@'+N' To '+CASE WHEN @IsActive=0 THEN N' deactivate ' WHEN @IsActive=1 THEN N' activate ' END+ N' the inventory should be empty '
		   RAISERROR(@MSG,16,1,'500')
	   END
	   ELSE
	   BEGIN
	   		UPDATE [Inventory].Warehouse
			SET
				[IsActive] = @IsActive,
				UserCreatorId = @UserCreatorId,
				CreateDate = getdate()
			WHERE Id=@WarehouseId
			Select CAST('The inventory status changed' as nvarchar(100))
	   END
	   IF @TRANSACTIONCOUNT = 0
			COMMIT TRANSACTION 					
	END try
	BEGIN CATCH
	     IF (XACT_STATE()) = -1 
			 ROLLBACK TRANSACTION
		 IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
			 ROLLBACK TRANSACTION
		 IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
             ROLLBACK TRANSACTION CurrentTransaction
		EXEC [Inventory].ErrorHandling
	END catch
END
--[User_ListGetAll]
GO
GRANT EXECUTE ON [Inventory].[ChangeWarehouseStatus] TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].ChangeCoefficientAndUpdatePriceByUnitConvertId','P') is Not Null
	drop procedure [Inventory].ChangeCoefficientAndUpdatePriceByUnitConvertId;
Go
Create Procedure [Inventory].ChangeCoefficientAndUpdatePriceByUnitConvertId
(
	@UnitConvertId INT,
	@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
BEGIN
set nocount ON;
DECLARE @TRANSACTIONCOUNT INT;
	SET @TRANSACTIONCOUNT = @@TRANCOUNT;
BEGIN TRY
	IF @TRANSACTIONCOUNT = 0
		BEGIN TRANSACTION
	--ELSE
	--	SAVE TRANSACTION CurrentTransaction;
					
DECLARE @EffectiveDateStart DATETIME  ,
	    @EffectiveDateEnd DATETIME,
	    @OriginalUnitId BIGINT,
	    @SubsidiaryUnitId BIGINT
	    

SELECT TOP(1) @OriginalUnitId=uc.UnitId,
			  @SubsidiaryUnitId=uc.SubUnitId,
              --@Coefficient=uc.Coefficient,
			  @EffectiveDateStart=uc.EffectiveDateStart,
			  @EffectiveDateEnd=uc.EffectiveDateEnd
FROM   [Inventory].UnitConverts uc
WHERE uc.Id = @UnitConvertId;


DECLARE @TransactionItemPriceId int =Null,
		@TransactionId INT,
		@TransactionItemId INT = NULL,
		@PriceUnitId BIGINT = NULL,
		@Fee DECIMAL(20,3) = 0,
		@RegistrationDate DATETIME = getdate(),
		@MainCurrencyUnitId BIGINT =NULL,
		@FeeInMainCurrency DECIMAL(20,3) =0,
		@PrimaryCoefficient TINYINT=2,
		@Coefficient DECIMAL(18,3) =0;
--*******************************
DISABLE TRIGGER TransactionItemPrices.UpdateTransactionItemPrices ON [Inventory].TransactionItemPrices;	

IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'TransactionItemPrices')
		DEALLOCATE TransactionItemPrices
		DECLARE TransactionItemPrices CURSOR FOR
		SELECT  tip.Id , tip.TransactionItemId, tip.TransactionId, tip.PriceUnitId, tip.Fee, tip.MainCurrencyUnitId, t.RegistrationDate
		FROM [Inventory].TransactionItemPrices tip
		INNER JOIN [Inventory].Transactions t ON t.Id = tip.TransactionId
		LEFT JOIN [Inventory].Transactions t2 ON t.id=t2.PricingReferenceId
		WHERE ((t.RegistrationDate >=@EffectiveDateStart 
		      AND t.RegistrationDate<=@EffectiveDateEnd
		      AND t.PricingReferenceId IS NULL)
		      OR 
		      (t2.RegistrationDate >=@EffectiveDateStart 
		      AND t2.RegistrationDate<=@EffectiveDateEnd
		      AND t2.PricingReferenceId IS NOT NULL))
		      AND (tip.PriceUnitId=@OriginalUnitId
		          OR
		          tip.MainCurrencyUnitId=@OriginalUnitId
		          )
		      AND (tip.PriceUnitId=@SubsidiaryUnitId
		          OR
		          tip.MainCurrencyUnitId=@SubsidiaryUnitId
		          )
		OPEN TransactionItemPrices
			FETCH NEXT FROM TransactionItemPrices INTO  @TransactionItemPriceId , @TransactionItemId, @TransactionId ,@PriceUnitId , @Fee, @MainCurrencyUnitId, @RegistrationDate
			WHILE @@Fetch_Status = 0  
			BEGIN
				SET @Coefficient=1
				SET @PrimaryCoefficient=1
							
			            IF @PriceUnitId<>@MainCurrencyUnitId
						BEGIN
							SELECT @Coefficient=fn.Coefficient ,@PrimaryCoefficient=fn.PrimaryCoefficient
								FROM [Inventory].[PrimaryCoefficient](@PriceUnitId,@MainCurrencyUnitId,@RegistrationDate) fn	
						END		                      
						ELSE 
						BEGIN
							SET @Coefficient=1
							SET @PrimaryCoefficient=1
						END
		
						IF @Coefficient<>0
						BEGIN
							IF @PrimaryCoefficient=1  
							BEGIN
						   			SET @Coefficient=(1  *  @Coefficient)
							END
							ELSE IF @PrimaryCoefficient=0 
							BEGIN
						   			SET @Coefficient=(1  /  @Coefficient)
							END    
						END
						ELSE
						BEGIN
							RAISERROR(N'@Base currency has no relation with selected currency ', 16, 1, '500')
						END
			SET @FeeInMainCurrency=@Fee * @Coefficient			
						                                                               
				BEGIN
						UPDATE [Inventory].TransactionItemPrices
						SET
							FeeInMainCurrency=@FeeInMainCurrency
						WHERE Id=@TransactionItemPriceId
				END			
		    FETCH TransactionItemPrices INTO   @TransactionItemPriceId , @TransactionItemId, @TransactionId ,@PriceUnitId , @Fee, @MainCurrencyUnitId, @RegistrationDate
			END		;
--*******************************
ENABLE TRIGGER TransactionItemPrices.UpdateTransactionItemPrices ON [Inventory].TransactionItemPrices;	
SET @Message=N'OperationSuccessful'
		IF @TRANSACTIONCOUNT = 0
			COMMIT TRANSACTION 					
	END try
	BEGIN CATCH
			IF (XACT_STATE()) = -1 
				ROLLBACK TRANSACTION
			IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
				ROLLBACK TRANSACTION
			IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
				ROLLBACK TRANSACTION CurrentTransaction
		    SET @Message=ERROR_MESSAGE();
		EXEC [Inventory].ErrorHandling
	END CATCH
END
GO
GRANT EXECUTE ON [Inventory].ChangeCoefficientAndUpdatePriceByUnitConvertId TO [public] AS [dbo]
GO

----------------------------------------------------------------------
if OBJECT_ID('[Inventory].[UnitConvertsOperation]','P') is Not Null
	DROP PROCEDURE [Inventory].[UnitConvertsOperation];
Go
CREATE PROCEDURE [Inventory].[UnitConvertsOperation]
(
	 @Action nvarchar(10),
	 @Id INT=NULL,
	 @UnitId BIGINT=NULL,
	 @SubUnitId BIGINT=NULL,
	 @Coefficient DECIMAL(18,3)=NULL,
	 @Fiscal_Year_ID INT=NULL,
	 @EffectiveDateStart DATETIME  =NULL,
	 @EffectiveDateEnd DATETIME =  NULL,
	 @UserCreatorId INT = NULL,
	 @Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
	BEGIN
		SET NOCOUNT ON;
		BEGIN TRY
		 SET XACT_ABORT ON;
		 BEGIN TRANSACTION 
		 IF @EffectiveDateStart IS NULL 
		    SET @EffectiveDateStart=GETDATE()
			if lower(@Action)='insert'
				BEGIN
					INSERT INTO [Inventory].UnitConverts
					(
						-- Id -- this column value is auto-generated
						UnitId,
						SubUnitId,
						Coefficient,
						EffectiveDateStart,
						EffectiveDateEnd,
						UserCreatorId,
						CreateDate
					)
					VALUES
					(
						@UnitId ,
						@SubUnitId ,
						@Coefficient ,
						@EffectiveDateStart ,
						@EffectiveDateEnd,
						@UserCreatorId ,
						getdate() 
					)
					SET @Id=@@identity
					Select CAST('The unit relation has created successfully' as nvarchar(100))
				END;
			--if LOWER(@Action)='update'
			--	BEGIN
			--		UPDATE [Inventory].UnitConverts
			--		SET
			--			-- Id = ? -- this column value is auto-generated
			--			UnitId = @UnitId,
			--			SubUnitId = @SubUnitId,
			--			Coefficient = @Coefficient,
			--			EffectiveDateStart = @EffectiveDateStart,
			--			EffectiveDateEnd=@EffectiveDateEnd,
			--			UserCreatorId = @UserCreatorId,
			--			CreateDate = getdate()
			--		WHERE Id=@Id
			--		Select CAST('رابطه واحد اندازه گيري با موفقیت ویرایش شد' as nvarchar(100))
			--	END			
			--if LOWER(@Action)='delete'
			--	BEGIN
			--		DELETE FROM [Inventory].UnitConverts
			--		WHERE Id=@Id
			--		Select CAST('رابطه واحد اندازه گيري با موفقیت حذف شد' as nvarchar(100))
			--	END
	        EXEC [Inventory].ChangeCoefficientAndUpdatePriceByUnitConvertId @UnitConvertId=@Id,@Message=@Message
			
		COMMIT TRANSACTION 	
			SET @Message=N'OperationSuccessful'				
		END try
		BEGIN CATCH
			IF (XACT_STATE()) = -1 
			BEGIN
				ROLLBACK TRANSACTION ; 
			END
			EXEC [Inventory].[ErrorHandling]
		END catch
	END
GO
GRANT EXECUTE ON [Inventory].[UnitConvertsOperation] TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].TransactionOperation','P') is Not Null
	drop procedure [Inventory].TransactionOperation;
Go
Create Procedure [Inventory].TransactionOperation
(
@Action nvarchar(10),
@Id int = Null,
@TransactionAction tinyint , -- رسيد 1 - حواله 2 -   فاکتور فروش 3
@Description nvarchar(max)=NULL,
@CompanyId BIGINT,--شرکت
@WarehouseId BIGINT,--انبار
@TimeBucketId INT,
@StoreTypesId INT ,--نوع رسيد و حواله(خريد؛برگشت از فروش؛ انتقال و ... )
@PricingReferenceId INT=NULL,
@AdjustmentForTransactionId INT=NULL,
@Status tinyint=null ,--وضعيت رسيد و حواله و ...
@RegistrationDate DATETIME=NULL,--تاريخ ثبت
@ReferenceType NVARCHAR(100)=NULL,--نوع مرجع
@ReferenceNo NVARCHAR(100)=NULL,--شماره مرجع
@UserCreatorId INT,
@TransactionId INT OUT,
@Code decimal(20,2) OUT,--شماره رسید یا حواله یا درخواست خرید,درخواست کالا 
@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
	BEGIN
		set nocount ON;
		DECLARE @TRANSACTIONCOUNT INT;
			SET @TRANSACTIONCOUNT = @@TRANCOUNT;
		BEGIN TRY
			IF @TRANSACTIONCOUNT = 0
				BEGIN TRANSACTION
			--ELSE
			--	SAVE TRANSACTION CurrentTransaction;
			
			   	IF @RegistrationDate IS NULL 
					BEGIN
							RAISERROR(N'@Invalid date in TransactionOperation',16,1,'500')
					END
	--DECLARE @WarehouseId BIGINT,
	--        @CompanyId BIGINT
	--		SET @CompanyId = (
	--				SELECT TOP(1)c.Id
	--				FROM   Companies c 
	--				WHERE c.Name = @Company
	--		)
	--		SET @WarehouseId = (
	--				SELECT TOP(1)w.Id
	--				FROM   Warehouse w
	--				WHERE  w.Name = @Warehouse
	--						AND w.CompanyId = @CompanyId
	--		) 
	IF @TransactionAction<1 OR @TransactionAction>3
		RAISERROR(N'@Transaction type should be receipt , issue or factor',16,1,'500')
	DECLARE @tbId INT
	SET @tbId =(SELECT TOP(1)tb.Id
	             FROM [Inventory].TimeBucket tb WHERE tb.[IsActive]=1 AND @RegistrationDate  BETWEEN tb.StartDate AND tb.EndDate)
	IF @TimeBucketId =0 OR @TimeBucketId IS NULL
	SET @TimeBucketId=@tbId
	IF @TimeBucketId<>@tbId
		 RAISERROR(N'@The transaction date mismatch with current active time bucket',16,1,'500')    
	IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Warehouse w WHERE w.[IsActive]=1 AND w.Id=@WarehouseId)
	     RAISERROR(N'@The inventory is not defined or active',16,1,'500')    
	IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Companies c WHERE c.[IsActive]=1 AND c.Id=@CompanyId)
	     RAISERROR(N'@The company is not defined or active',16,1,'500')
	IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].TimeBucket tb WHERE tb.[IsActive]=1 AND tb.Id=@TimeBucketId)
	     RAISERROR(N'@The time bucket is not defined or active',16,1,'500')
			if lower(@Action)='insert'
				BEGIN
					IF @Code IS NULL OR @Code =0.000
					   SET @Code=[Inventory].GetTransactionNewCode(@TransactionAction,@WarehouseId,@RegistrationDate,@TimeBucketId)
					INSERT INTO [Inventory].Transactions
					(
						-- Id -- this column value is auto-generated
						[Action],
						Code,
						[Description],
						--CrossId,
						WarehouseId,
						StoreTypesId,
						TimeBucketId,
						PricingReferenceId,
						[Status],
						RegistrationDate,
						SenderReciver,
						HardCopyNo,
						ReferenceType,
						ReferenceNo,
						ReferenceDate,
						UserCreatorId,
						CreateDate,
						AdjustmentForTransactionId
					)
					VALUES
					(
						@TransactionAction,
						@Code,
						@Description,
						--NULL,	/*{ CrossId }*/
						@WarehouseId,
						@StoreTypesId,
						@TimeBucketId,
						@PricingReferenceId,
						1,/*{ [Status] }*/
						@RegistrationDate,
						NULL,/*{ SenderReciver }*/
						NULL,/*{ HardCopyNo }*/
						@ReferenceType,
						@ReferenceNo,
						getdate(),/*{ ReferenceDate }*/
						@UserCreatorId,
						getdate(),/*{ CreateDate }*/
						@AdjustmentForTransactionId
					)
					--Select CAST('ثبت با موفقیت انجام شد' as nvarchar(100))
					SET @TransactionId=@@identity
				END;
			--if LOWER(@Action)='update'
			--BEGIN
			--		UPDATE [Inventory].Transaction
			--		SET
			--			-- Id = ? -- this column value is auto-generated
			--			--[Action] = @TransactionAction,
			--			--Code = @Code,
			--			[Description] = @Description,
			--			WarehouseId = @WarehouseId,
			--			StoreTypesId = @StoreTypesId,
			--          TimeBucketId=@TimeBucketId,
			--          PricingReferenceId=@PricingReferenceId,
			--			RegistrationDate = @RegistrationDate,
			--			ReferenceType = @ReferenceType,
			--			ReferenceNo = @ReferenceNo,
			--			ReferenceDate = getdate(),
			--			UserCreatorId = @UserCreatorId
			--		WHERE Id=@Id
			--END			
			--if LOWER(@Action)='delete'
			--BEGIN
			--	DELETE FROM [Inventory].Transaction
			--	WHERE Id=@Id
			--	Select CAST(N'@اطلاعات با موفقیت حذف شد' as nvarchar(100))
			--END
			 
			SET @Message=N'OperationSuccessful'
		IF @TRANSACTIONCOUNT = 0
		COMMIT TRANSACTION 					
		END try
		BEGIN CATCH
				IF (XACT_STATE()) = -1 
					ROLLBACK TRANSACTION
				IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
					ROLLBACK TRANSACTION
				IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
					ROLLBACK TRANSACTION CurrentTransaction
				SET @Message=ERROR_MESSAGE();
				SET @TransactionId=NULL
				SET @Code=NULL
			EXEC [Inventory].ErrorHandling
		END CATCH
	END
GO	
GRANT EXECUTE ON [Inventory].TransactionOperation TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].TransactionItemsOperation','P') is Not Null
	drop procedure [Inventory].TransactionItemsOperation;
Go
Create Procedure [Inventory].TransactionItemsOperation
(
@Action nvarchar(10),
@TransactionId  int,
@UserCreatorId INT,
@TransactionItems TypeTransactionItems READONLY,-- Id , GoodId, QuantityUnitId, QuantityAmount, [Description]
@TransactionItemsId NVARCHAR(256) OUT,
@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
	BEGIN
	set nocount ON;
	DECLARE @TRANSACTIONCOUNT INT;
		SET @TRANSACTIONCOUNT = @@TRANCOUNT;
	BEGIN TRY
	IF @TRANSACTIONCOUNT = 0
		BEGIN TRANSACTION
	--ELSE
	--	SAVE TRANSACTION CurrentTransaction;
		SET @TransactionItemsId=N''
	    DECLARE @Id int =Null,
				@GoodId BIGINT = null,
				@QuantityUnitId BIGINT,
				@QuantityAmount DECIMAL(20,3) = 0,
				@Description nvarchar(max) =NULL,
				@RowVersion SMALLINT = 0,
				@WarehouseId BIGINT,
				@CompanyId BIGINT,
				@TimeBucketId INT,
				@MSG NVARCHAR(max)
				
		SELECT TOP(1) @WarehouseId=isnull(t.WarehouseId,0),@CompanyId=ISNULL(w.CompanyId,0),@TimeBucketId=ISNULL(t.TimeBucketId,0)
	    FROM [Inventory].Transactions t
	    INNER JOIN [Inventory].Warehouse w ON w.Id=t.WarehouseId
	    WHERE t.Id=@TransactionId

		IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Warehouse w WHERE w.[IsActive]=1 AND w.Id=@WarehouseId)
			   	BEGIN
			   		SET @MSG=N'@ The inventory ' + (SELECT TOP(1) w.Code+N' '+w.Name FROM [Inventory].Warehouse w WHERE w.Id=@WarehouseId) +N' is not defined or active'
			   		RAISERROR(@MSG,16,1,'500')    
			   	END
		IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Companies c WHERE c.[IsActive]=1 AND c.Id=@CompanyId)
		BEGIN
			SET @MSG=N'@ The company ' + (SELECT TOP(1) c.Code+N' '+c.Name FROM [Inventory].Companies c WHERE c.Id=@CompanyId) +N' is not defined or active'
			RAISERROR(@MSG,16,1,'500')    
		END
		IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].TimeBucket tb WHERE tb.[IsActive]=1 AND tb.Id=@TimeBucketId)
		BEGIN
			SET @MSG=N'@ The time bucket ' + (SELECT TOP(1)tb.Name FROM [Inventory].TimeBucket tb WHERE tb.Id=@TimeBucketId) +N' is not defined or active'
			RAISERROR(@MSG,16,1,'500')    
		END	 
	    IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'TransactionItems')
		DEALLOCATE TransactionItems
		DECLARE TransactionItems CURSOR FOR
		SELECT  t.Id , t.GoodId, t.QuantityUnitId, t.QuantityAmount, t.[Description]
		FROM @TransactionItems t
		OPEN TransactionItems
			FETCH NEXT FROM TransactionItems INTO  @Id , @GoodId, @QuantityUnitId, @QuantityAmount, @Description
			WHILE @@Fetch_Status = 0  
			BEGIN
				
			   	IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Goods g WHERE g.[IsActive]=1 AND g.Id=@GoodId)
			   	BEGIN
			   		SET @MSG=N'@ کالاي ' + (SELECT TOP(1) g.Code+N' '+g.Name FROM [Inventory].Goods g WHERE g.Id=@GoodId) +N' معتبر (فعال) نيست'
			   		RAISERROR(@MSG,16,1,'500')    
			   	END
				IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Units u WHERE u.[IsActive]=1 AND u.IsCurrency=0 AND u.Id=@QuantityUnitId)
			   	BEGIN
			   		SET @MSG=N'@ واحد ' + (SELECT TOP(1) u.Abbreviation+N' '+u.Name FROM [Inventory].Units u WHERE u.Id=@QuantityUnitId) +N' معتبر (فعال) نيست'
			   		RAISERROR(@MSG,16,1,'500')    
			   	END
	     
			 --  	declare @CHKNegativ TINYINT
				--SET @CHKNegativ=[Inventory].CheckNegetiveWarehouseValue(@WarehouseId,@GoodId ,@RowVersion,@Action,@TransactionId,@QuantityAmount,@QuantityUnitId,1)
				--IF @CHKNegativ=0
				--BEGIN
				--DECLARE @m NVARCHAR(MAX)
				--SET @m=N'@انبار '+(SELECT top(1) w.Name
				--				FROM Warehouse w WHERE w.Id=@WarehouseId)+N' منفی پذیر نیست و نمیتواند منفی شود '
				--RAISERROR(@m,16,1,'500')
				--END		

			if lower(@Action)='insert'
				BEGIN
					SET @RowVersion=ISNULL((SELECT MAX(ti.[RowVersion]) FROM [Inventory].TransactionItems ti WHERE ti.TransactionId=@TransactionId),0)+1		
					INSERT INTO [Inventory].TransactionItems
					(
						-- Id -- this column value is auto-generated
						RowVersion,
						TransactionId,
						GoodId,
						QuantityUnitId,
						QuantityAmount,
						[Description],
						UserCreatorId,
						CreateDate
					)
					VALUES
					(
						@RowVersion ,
						@TransactionId ,
						@GoodId ,
						@QuantityUnitId ,
						@QuantityAmount ,
						@Description,
						@UserCreatorId ,
						getdate()
					)
					--Select CAST('ثبت با موفقیت انجام شد' as nvarchar(100))
					SET @TransactionItemsId+=CAST(@@identity AS NVARCHAR(15))+N';'
				END;
				--if LOWER(@Action)='update'
				--BEGIN
				--		UPDATE [Inventory].TransactionItems
				--		SET
				--			-- Id = ? -- this column value is auto-generated
				--			GoodId = @GoodId,
				--			QuantityUnitId = @QuantityUnitId,
				--			QuantityAmount = @QuantityAmount,
				--			[Description] = @Description,
				--			UserCreatorId = @UserCreatorId
				--		WHERE Id=@Id
				--END			
				--if LOWER(@Action)='delete'
				--BEGIN
				--	DELETE FROM [Inventory].TransactionItems
				--	WHERE Id=@Id
				--	Select CAST(N'@اطلاعات با موفقیت حذف شد' as nvarchar(100))
				--END
		    FETCH TransactionItems INTO  @Id , @GoodId, @QuantityUnitId, @QuantityAmount, @Description
			END
		
		SET @Message=N'OperationSuccessful'
			IF @TRANSACTIONCOUNT = 0
				COMMIT TRANSACTION 					
		END try
		BEGIN CATCH
				IF (XACT_STATE()) = -1 
					ROLLBACK TRANSACTION
				IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
					ROLLBACK TRANSACTION
				IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
					ROLLBACK TRANSACTION CurrentTransaction
			    SET @Message=ERROR_MESSAGE();
				SET @TransactionItemsId=NULL
			EXEC ErrorHandling
		END CATCH
	END
GO	
GRANT EXECUTE ON [Inventory].TransactionItemsOperation TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].TransactionItemPricesOperation','P') is Not Null
	drop procedure [Inventory].TransactionItemPricesOperation;
Go
Create Procedure [Inventory].TransactionItemPricesOperation
(
@Action nvarchar(10),
@UserCreatorId INT,
@TransactionItemPrices TypeTransactionItemPrices READONLY,-- Id ,TransactionItemId, QuantityUnitId ,QuantityAmount ,PriceUnitId ,Fee  ,[Description]
@TransactionItemPriceIds NVARCHAR(MAX) OUT,
@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
	BEGIN
	set nocount ON;
	DECLARE @TRANSACTIONCOUNT INT;
		SET @TRANSACTIONCOUNT = @@TRANCOUNT;
	BEGIN TRY
		IF @TRANSACTIONCOUNT = 0
			BEGIN TRANSACTION
		--ELSE
		--	SAVE TRANSACTION CurrentTransaction;
		SET @TransactionItemPriceIds=N''
	    DECLARE @Id int =Null,
				@TransactionId INT,
				@TransactionItemId INT = NULL,
				@PriceUnitId BIGINT = NULL,
				@Fee DECIMAL(20,3) = 0,
				@RegistrationDate DATETIME = getdate(),
				@QuantityUnitId BIGINT,
				@QuantityAmount DECIMAL(20,3) = 0,
				@Description nvarchar(max) =NULL,
				@RowVersion SMALLINT = 0,
				@WarehouseId BIGINT,
				@QuantityAmountTotal DECIMAL(20,3) = 0,
				@QuantityAmountSumSoFar DECIMAL(20,3) = 0,
				@ActionType TINYINT=0,
				@StoreTypeCode SMALLINT = 0,
				@MainCurrencyUnitId BIGINT =NULL,
				@FeeInMainCurrency DECIMAL(20,3) =0,
				@PrimaryCoefficient TINYINT=2,
				@Coefficient DECIMAL(18,3) =0
						                                    					
	    IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'TransactionItemPrices')
		DEALLOCATE TransactionItemPrices
		DECLARE TransactionItemPrices CURSOR FOR
		SELECT  t.Id , t.TransactionItemId, t.QuantityUnitId, t.QuantityAmount, t.PriceUnitId, t.Fee, t.RegistrationDate, t.[Description]
		FROM @TransactionItemPrices t
		OPEN TransactionItemPrices
			FETCH NEXT FROM TransactionItemPrices INTO  @Id , @TransactionItemId ,@QuantityUnitId ,@QuantityAmount ,@PriceUnitId , @Fee, @RegistrationDate, @Description
			WHILE @@Fetch_Status = 0  
			BEGIN
			   	 SELECT TOP(1) @WarehouseId = ISNULL(t.WarehouseId, 0),
			   	        @ActionType = ISNULL(t.Action, 0),
			   	        @StoreTypeCode= ISNULL(st.Code, 0),
			   	        @TransactionId=t.Id
			   	 FROM   [Inventory].Transactions t
			   	        INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
			   	        INNER JOIN [Inventory].TransactionItems ti
			   	             ON  t.Id = ti.TransactionId
			   	 WHERE  ti.Id = @TransactionItemId
			   	 
			   	 IF NOT(@ActionType=1 OR(@ActionType=3 AND @StoreTypeCode=9))--FuelReport Incremental Correction
			   	 BEGIN
				   SET @Message='Failed' 
				   RAISERROR(N'@The reciepts only could be priced manually ',16,1,'500')			   	 	
			   	 END
		   
				 SET @QuantityAmountTotal=ISNULL((SELECT sum(ti.QuantityAmount)
				                                    FROM [Inventory].TransactionItems ti WHERE ti.Id=@TransactionItemId
				                                                               AND ti.QuantityUnitId=@QuantityUnitId
				                                                               AND ti.TransactionId=@TransactionId),0)
                 SET @MainCurrencyUnitId=ISNULL((SELECT TOP(1) Id FROM [Inventory].Units u WHERE u.IsBaseCurrency=1),0)
                 if @MainCurrencyUnitId=0 
                 	RAISERROR(N'@The base currency should be defined before pricing ',16,1,'500')		
                 
                 IF @PriceUnitId<>@MainCurrencyUnitId
						BEGIN
							SELECT @Coefficient=fn.Coefficient ,@PrimaryCoefficient=fn.PrimaryCoefficient
								FROM [Inventory].[PrimaryCoefficient](@PriceUnitId,@MainCurrencyUnitId,@RegistrationDate) fn	
						END		                      
						ELSE 
						BEGIN
							SET @Coefficient=1
							SET @PrimaryCoefficient=1
						END
		
						IF @Coefficient<>0
						BEGIN
							IF @PrimaryCoefficient=1  
							BEGIN
						   			SET @Coefficient=(1  *  @Coefficient)
							END
							ELSE IF @PrimaryCoefficient=0 
							BEGIN
						   			SET @Coefficient=(1  /  @Coefficient)
							END    
						END
						ELSE
						BEGIN
							RAISERROR(N'@Base currency has no relation with selected currency ', 16, 1, '500')
						END
			SET @FeeInMainCurrency=@Fee * @Coefficient			
						                                                               
			SET @QuantityAmountSumSoFar=0				                                                               
			if lower(@Action)='insert'
				BEGIN
					SET @QuantityAmountSumSoFar=ISNULL((SELECT sum(tip.QuantityAmount)
					                                    FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@TransactionItemId
																						 AND tip.QuantityUnitId=@QuantityUnitId
																						 AND tip.TransactionId=@TransactionId),0)
					IF (@QuantityAmountTotal-@QuantityAmountSumSoFar)<@QuantityAmount
					     RAISERROR(N'@More than not priced quantity is mentioned for pricing ',16,1,'500')																						 
					SET @RowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@TransactionItemId),0)+1	
					INSERT INTO [Inventory].TransactionItemPrices
					(
						-- Id -- this column value is auto-generated
						RowVersion,
						TransactionId,
						TransactionItemId,
						QuantityUnitId,
						QuantityAmount,
						PriceUnitId,
						Fee,
						MainCurrencyUnitId,
						FeeInMainCurrency,
						RegistrationDate,
						[Description],
						UserCreatorId,
						CreateDate
					)
					VALUES
					(
						@RowVersion ,
						@TransactionId ,
						@TransactionItemId ,
						@QuantityUnitId,
						@QuantityAmount ,
						@PriceUnitId ,
						@Fee ,
						@MainCurrencyUnitId,
						@FeeInMainCurrency,
						@RegistrationDate,
						@Description ,
						@UserCreatorId ,
						getdate()
					)	
					--Select CAST('ثبت با موفقیت انجام شد' as nvarchar(100))
					SET @TransactionItemPriceIds+=CAST(@@identity AS NVARCHAR(15))+N';'
				END;
				--if LOWER(@Action)='update'
				--BEGIN
				--		SET @QuantityAmountSumSoFar=ISNULL((SELECT sum(tip.QuantityAmount)
				--	                                    FROM [Inventory].TransactionItemPrices tip WHERE tip.Id<>@Id
				--	                                                                     AND tip.TransactionItemId=@TransactionItemId
				--																		 AND tip.QuantityUnitId=@QuantityUnitId
				--																		 AND tip.TransactionId=@TransactionId),0)
				--      IF (@QuantityAmountTotal-@QuantityAmountSumSoFar)<=@QuantityAmount
				--	     RAISERROR(N'@بيشتر از مقدار قيمت گذاري نشده را براي قيمت گذاري ارسال کرده ايد ',16,1,'500')			
				--		UPDATE [Inventory].TransactionItemPrices
				--		SET
				--			-- Id = ? -- this column value is auto-generated
				--			TransactionId=@TransactionId,
				--			TransactionItemId=@TransactionItemId,
				--			QuantityUnitId=@QuantityUnitId,
				--			QuantityAmount=@QuantityAmount,
				--			PriceUnitId=@PriceUnitId,
				--			Fee=@Fee,
				--			MainCurrencyUnitId=@MainCurrencyUnitId,
				--			FeeInMainCurrency=@FeeInMainCurrency,
				--          RegistrationDate=@RegistrationDate,
				--			[Description]=@Description,
				--		WHERE Id=@Id
				--END			
				--if LOWER(@Action)='delete'
				--BEGIN
				--	DELETE FROM [Inventory].TransactionItemPrices
				--	WHERE Id=@Id
				--	Select CAST(N'@اطلاعات با موفقیت حذف شد' as nvarchar(100))
				--END
		    FETCH TransactionItemPrices INTO  @Id , @TransactionItemId ,@QuantityUnitId ,@QuantityAmount ,@PriceUnitId , @Fee, @RegistrationDate, @Description
			END
		
		SET @Message=N'OperationSuccessful'
			IF @TRANSACTIONCOUNT = 0
				COMMIT TRANSACTION 					
		END try
		BEGIN CATCH
				IF (XACT_STATE()) = -1 
					ROLLBACK TRANSACTION
				IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
					ROLLBACK TRANSACTION
				IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
					ROLLBACK TRANSACTION CurrentTransaction
		        SET @Message=ERROR_MESSAGE();
				SET @TransactionItemPriceIds=NULL
			EXEC [Inventory].ErrorHandling
		END CATCH
	END
GO	
GRANT EXECUTE ON [Inventory].TransactionItemPricesOperation TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].PriceGivenIssuedItems','P') is Not Null
	drop procedure [Inventory].PriceGivenIssuedItems;
Go
Create Procedure [Inventory].PriceGivenIssuedItems
(
@UserCreatorId INT,
@IssueItemIds Ids READONLY,-- Id 
@TransactionItemPriceIds NVARCHAR(MAX) OUT,
@Message NVARCHAR(MAX) OUT,
@NotPricedTransactionId INT OUT --First Receipt Number Without Pricing 
)
--WITH ENCRYPTION
AS 
BEGIN
set nocount ON;
DECLARE @TRANSACTIONCOUNT INT;
	SET @TRANSACTIONCOUNT = @@TRANCOUNT;
BEGIN TRY
	IF @TRANSACTIONCOUNT = 0
		BEGIN TRANSACTION
	--ELSE
	--	SAVE TRANSACTION CurrentTransaction;
	DECLARE @IssueItemId INT,
	        @Description NVARCHAR(MAX),
		    @QuantityAmount DECIMAL(20,3)	
		SET @TransactionItemPriceIds=N''
	
	IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'IssueItemPrices')
		DEALLOCATE IssueItemPrices
	DECLARE IssueItemPrices CURSOR FOR
	SELECT  t.Id,t.Description
	FROM @IssueItemIds t
	OPEN IssueItemPrices
		FETCH NEXT FROM IssueItemPrices INTO  @IssueItemId, @Description
		WHILE @@Fetch_Status = 0  
		BEGIN
			DECLARE @WarehouseId BIGINT,
					@GoodId BIGINT ,
					@QuantityUnitId BIGINT ,
					@Code DECIMAL(20,2),
					@TimeBucketId INT,
					@RegistrationDate DATETIME,
					@IssueId INT=NULL,
					@IssueItemRowVersion SMALLINT=NULL,
		
					@IssueItemPriceId INT=NULL,
					@IssueItemPriceRowVersion SMALLINT=0,
					@CurrentQuantityAmount DECIMAL(20,3),
					@ActionType SMALLINT=NULL	
				
				SELECT TOP(1) @WarehouseId = ISNULL(t.WarehouseId, 0),
			   			@ActionType = ISNULL(t.Action, 0),
			   			@Code=t.Code,
			   			@GoodId=ti.GoodId,
			   			@QuantityUnitId=ti.QuantityUnitId,
			   			@QuantityAmount=ti.QuantityAmount - ISNULL((SELECT SUM(ISNULL(tip.QuantityAmount, 0)) FROM [Inventory].TransactionItemPrices tip
			   													WHERE tip.TransactionItemId = @IssueItemId), 0),  -- This is added by A.Hatefi to price remained quantity of Issued Item
			   			@TimeBucketId=t.TimeBucketId,
			   			@RegistrationDate=t.RegistrationDate,
			   			@IssueItemRowVersion=ti.RowVersion,
			   			@IssueId=t.Id
			   		FROM   [Inventory].Transactions t
			   			INNER JOIN [Inventory].TransactionItems ti
			   					ON  t.Id = ti.TransactionId
			   		WHERE  ti.Id = @IssueItemId
		
		
		IF @ActionType<>2
		BEGIN
				SET @Message='Failed' 
				RAISERROR(N'@The transaction type of automatic pricing is not of type issue ',16,1,'500')	
		END
		
		--*********Extended for implementing sequential pricing
		
		
		DECLARE @NotPricedIssueBeforeThis BIGINT
		
		SET @NotPricedIssueBeforeThis= ISNULL((
						SELECT TOP(1)t.Id
						FROM [Inventory].Transactions t
						INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId=t.Id
						INNER JOIN [Inventory].Warehouse a ON a.Id = t.WarehouseId AND a.[IsActive]=1
						WHERE a.Id=@WarehouseId
								AND ti.GoodId=@GoodId
								AND ti.QuantityUnitId=@QuantityUnitId
								--AND t.TimeBucketId=@TimeBucketId                          A.H  Order
								AND t.[Action]=2				--Finding not priced issues before current transaction.
								AND t.StoreTypesId NOT IN (28)  --Issued Total Received in Trust is excluded  -- By A.H
								AND t.Id<>@IssueId
								
								AND t.[Status]<>3
								AND t.[Status]<>4 
								
								AND t.RegistrationDate <= @RegistrationDate	--Changed from String to Field comparison by A.Hatefi on 1395-03-17
								AND t.Code < @Code							--Changed from String to Field comparison by A.Hatefi on 1395-03-17
				
						)  ,0)
			IF @NotPricedIssueBeforeThis<>0
			BEGIN
				RAISERROR(N'@There are some not fully priced issues before current issue ',16,1,'500')
			END       
			--*********Extended for implementing sequential pricing
		 
		IF OBJECT_ID('tempdb..##TempIssuePricing') IS NOT NULL
					EXEC('DROP TABLE ##TempIssuePricing')
			
		SELECT TOP(1) @IssueItemPriceId=ISNULL(MAX(tip.Id),0)+1  FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId
		IF @IssueItemPriceId=1 SET @IssueItemPriceRowVersion=1
		ELSE  SET @IssueItemPriceRowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	

		SELECT t.Id AS TransactionId,
				t.Code AS TransactionCode,	--Added by Hatefi on 1394-10-16
				t.RegistrationDate AS TransactionRegistrationDate,ti.Id AS TransactionItemId, ti.RowVersion AS TransactionItemRowVersion, 
				ti.QuantityAmount AS TransactionItemQuantityAmount,tip.Id AS TransactionItemPriceId, tip.RowVersion AS TransactionItemPriceRowVersion, 
				tip.RegistrationDate AS TransactionItemPriceRegistrationDate, tip.QuantityAmount AS TransactionItemPriceQuantityAmount, 
				tip.PriceUnitId AS TransactionItemPricePriceUnitId, tip.Fee AS TransactionItemPriceFee, tip.MainCurrencyUnitId AS TransactionItemPriceMainCurrencyUnitId, 
				tip.FeeInMainCurrency AS TransactionItemPriceExchangeRate, tip.QuantityAmountUseFIFO AS TransactionItemPriceQuantityAmountUseFIFO
		INTO ##TempIssuePricing
		FROM [Inventory].Transactions t
			INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId=t.Id
			INNER JOIN [Inventory].TransactionItemPrices tip ON tip.TransactionItemId=ti.Id
			INNER JOIN [Inventory].Warehouse a ON a.Id = t.WarehouseId AND a.[IsActive]=1
		WHERE a.Id=@WarehouseId
				AND ti.GoodId=@GoodId
				AND ti.QuantityUnitId=@QuantityUnitId
				--AND t.TimeBucketId=@TimeBucketId                          A.H  Order
				AND t.[Action]=1				--Finding Receipts
				AND t.StoreTypesId NOT IN (2)	--Receipts in Trust is excluded  -- By A.H
				AND tip.QuantityAmountUseFIFO<tip.QuantityAmount
				AND tip.Fee>0
				AND tip.FeeInMainCurrency>0
				
				AND (t.RegistrationDate <= @RegistrationDate	--Added by Hatefi on 1394-10-16
						OR tip.RegistrationDate  <= @RegistrationDate)  --Added by Hatefi on 1395-03-19
				AND t.Code < @Code							--Added by Hatefi on 1394-10-16
				
				/*AND ( CONVERT(NVARCHAR(20),t.RegistrationDate,(120))			--Commented by Hatefi on 1394-10-16
					--+ [Inventory].LPAD(CAST(t.Action AS nvarCHAR(3)) ,' ',3)
					+ [Inventory].LPAD(CAST(t.Id AS NVARCHAR(15)),' ',15)
					+ [Inventory].LPAD(CAST(ti.Id AS NVARCHAR(15)),' ',15)
					+ [Inventory].LPAD(CAST(ti.RowVersion AS NVARCHAR(10)),' ',10) 
					+ CONVERT(NVARCHAR(20),tip.RegistrationDate,(120))
					+ [Inventory].LPAD(CAST(tip.Id AS NVARCHAR(15)),' ',15)
					+ [Inventory].LPAD(CAST(tip.RowVersion AS NVARCHAR(15)),' ',15))<=
         
					( CONVERT(NVARCHAR(20),@RegistrationDate,(120))
					--+ [Inventory].LPAD(CAST(t.Action AS NVARCHAR(3)),' ',3)
					+ [Inventory].LPAD(CAST(@IssueId AS NVARCHAR(15)),' ',15)
					+ [Inventory].LPAD(CAST(@IssueItemId AS NVARCHAR(15)),' ',15)
					+ [Inventory].LPAD(CAST(@IssueItemRowVersion AS NVARCHAR(10)),' ',10) 
					+ CONVERT(NVARCHAR(20),GETDATE(),(120))
					+ [Inventory].LPAD(CAST(@IssueItemPriceId AS NVARCHAR(15)),' ',15)
					+ [Inventory].LPAD(CAST(@IssueItemPriceRowVersion AS NVARCHAR(15)),' ',15)
					)*/
			ORDER BY 
				--t.RegistrationDate,  --Commneted by A.Hatefi on 1395-03-10  and replaced by below order by 
				tip.RegistrationDate,  --
				t.Code,
				t.Id ,
				ti.Id ,
				ti.RowVersion ,
				--tip.RegistrationDate,  --Commneted by A.Hatefi on 1395-03-10  and replaced by below order by 
				tip.Id ,
				tip.RowVersion 
		--SELECT * FROM ##TempIssuePricing
		DECLARE @RegistrationDateFirstFree DATETIME,
				@TransactionIdFirstFree INT=NULL,
				@TransactionCodeFirstFree DECIMAL(20,2)=NULL,	--Added by A.Hatefi on 1394-10-16

				@TransactionItemIdFirstFree INT=NULL,
				@TransactionItemRowVersionFirstFree SMALLINT=NULL,
				@TransactionItemPriceRegistrationDateFirstFree DATETIME,
				@TransactionItemPriceIdFirstFree INT=NULL,
				@TransactionItemPriceRowVersionFirstFree SMALLINT=0 ,
				@TransactionId INT, 
				@TransactionRegistrationDate DATETIME,
				@TransactionItemId INT,
				@TransactionItemRowVersion SMALLINT, 
				@TransactionItemQuantityAmount DECIMAL(20,3),
				@TransactionItemPriceId INT,
				@TransactionItemPriceRowVersion SMALLINT, 
				@TransactionItemPriceRegistrationDate DATETIME,
				@TransactionItemPriceQuantityAmount DECIMAL(20,3),
				@TransactionItemPricePriceUnitId BIGINT,
				@TransactionItemPriceFee DECIMAL(20,3),
				@TransactionItemPriceMainCurrencyUnitId BIGINT, 
				@TransactionItemPriceExchangeRate DECIMAL(20,3),
				@TransactionItemPriceQuantityAmountUseFIFO DECIMAL(20,3);

		SELECT TOP(1) @RegistrationDateFirstFree=tip.TransactionRegistrationDate, @TransactionIdFirstFree=TransactionId, 
						@TransactionCodeFirstFree = tip.TransactionCode,	--Added by Hatefi on 1394-10-16
						@TransactionItemIdFirstFree=TransactionItemId, @TransactionItemRowVersionFirstFree=TransactionItemRowVersion, 
						@TransactionItemPriceRegistrationDateFirstFree=TransactionItemPriceRegistrationDate, 
						@TransactionItemPriceIdFirstFree=TransactionItemPriceId, 
						@TransactionItemPriceRowVersionFirstFree=TransactionItemPriceRowVersion
		FROM ##TempIssuePricing tip				 
		SET @NotPricedTransactionId= ISNULL((
						SELECT TOP(1)t.Id
						FROM [Inventory].Transactions t
						INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId=t.Id
						INNER JOIN [Inventory].TransactionItemPrices tip ON tip.TransactionItemId=ti.Id
						INNER JOIN [Inventory].Warehouse a ON a.Id = t.WarehouseId AND a.[IsActive]=1
						WHERE a.Id=@WarehouseId
								AND ti.GoodId=@GoodId
								AND ti.QuantityUnitId=@QuantityUnitId
								--AND t.TimeBucketId=@TimeBucketId                          A.H  Order
								AND t.[Action]=1
								AND t.StoreTypesId NOT IN (2)	--Receipts in Trust is excluded  -- By A.H
								
								--AND tip.QuantityAmountUseFIFO<tip.QuantityAmount		--Commented by Hatefi on 1394-10-16
								--AND (tip.Fee=0 OR tip.FeeInMainCurrency=0)			--Commented by Hatefi on 1394-10-16
								AND t.[Status]<>3										--Added by Hatefi on 1394-10-16
								AND t.[Status]<>4 										--Added by Hatefi on 1394-10-16

								AND t.RegistrationDate <= @RegistrationDateFirstFree		--Added by Hatefi on 1394-10-16
								AND t.Code < @TransactionCodeFirstFree					--Added by Hatefi on 1394-10-16

								/*AND ( CONVERT(NVARCHAR(20),t.RegistrationDate,(120))	--Commented by Hatefi on 1394-10-16
								+ [Inventory].LPAD(CAST(t.Id AS NVARCHAR(15)),' ',15)
								+ [Inventory].LPAD(CAST(ti.Id AS NVARCHAR(15)),' ',15)
								+ [Inventory].LPAD(CAST(ti.RowVersion AS NVARCHAR(10)),' ',10) 
								+ CONVERT(NVARCHAR(20),tip.RegistrationDate,(120))
								+ [Inventory].LPAD(CAST(tip.Id AS NVARCHAR(15)),' ',15)
								+ [Inventory].LPAD(CAST(tip.RowVersion AS NVARCHAR(15)),' ',15))<=
         
								( CONVERT(NVARCHAR(20),@RegistrationDateFirstFree,(120))
								+ [Inventory].LPAD(CAST(@TransactionIdFirstFree AS NVARCHAR(15)),' ',15)
								+ [Inventory].LPAD(CAST(@TransactionItemIdFirstFree AS NVARCHAR(15)),' ',15)
								+ [Inventory].LPAD(CAST(@TransactionItemRowVersionFirstFree AS NVARCHAR(10)),' ',10) 
								+ CONVERT(NVARCHAR(20),@TransactionItemPriceRegistrationDateFirstFree,(120))
								+ [Inventory].LPAD(CAST(@TransactionItemPriceIdFirstFree AS NVARCHAR(15)),' ',15)
								+ [Inventory].LPAD(CAST(@TransactionItemPriceRowVersionFirstFree AS NVARCHAR(15)),' ',15)
								)*/
						)  ,0)
			IF @NotPricedTransactionId<>0
			BEGIN
					IF OBJECT_ID('tempdb..##TempIssuePricing') IS NOT NULL
						EXEC('DROP TABLE ##TempIssuePricing')
				RAISERROR(N'@There are some not fully priced receipts for pricing current issue ',16,1,'500')
			
				RETURN
			END       
			ELSE
			BEGIN
			
				IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'AssignIssuePrice')
					DEALLOCATE AssignIssuePrice
				DECLARE AssignIssuePrice CURSOR FOR
				SELECT  t.TransactionId, 
						t.TransactionRegistrationDate,
						t.TransactionItemId,
						t.TransactionItemRowVersion, 
						t.TransactionItemQuantityAmount,
						t.TransactionItemPriceId,
						t.TransactionItemPriceRowVersion, 
						t.TransactionItemPriceRegistrationDate,
						t.TransactionItemPriceQuantityAmount, 
						t.TransactionItemPricePriceUnitId,
						t.TransactionItemPriceFee,
						t.TransactionItemPriceMainCurrencyUnitId, 
						t.TransactionItemPriceExchangeRate,
						t.TransactionItemPriceQuantityAmountUseFIFO
				FROM ##TempIssuePricing t
				ORDER BY 
					t.TransactionItemPriceRegistrationDate,	--Added by A.Hatefi on 1395-03-17
					t.TransactionCode   --Added by Hatefi on 1394-10-17
					

				OPEN AssignIssuePrice
					FETCH NEXT FROM AssignIssuePrice INTO   @TransactionId, 
															@TransactionRegistrationDate,
															@TransactionItemId,
															@TransactionItemRowVersion, 
															@TransactionItemQuantityAmount,
															@TransactionItemPriceId,
															@TransactionItemPriceRowVersion, 
															@TransactionItemPriceRegistrationDate,
															@TransactionItemPriceQuantityAmount, 
															@TransactionItemPricePriceUnitId,
															@TransactionItemPriceFee,
															@TransactionItemPriceMainCurrencyUnitId, 
															@TransactionItemPriceExchangeRate,
															@TransactionItemPriceQuantityAmountUseFIFO
				WHILE @@Fetch_Status = 0  
				BEGIN
					SET @CurrentQuantityAmount= @TransactionItemPriceQuantityAmount-@TransactionItemPriceQuantityAmountUseFIFO;
					IF @CurrentQuantityAmount>(@QuantityAmount)
					BEGIN
						SET @CurrentQuantityAmount=@QuantityAmount
					END
					DECLARE @RowVersion SMALLINT
					SET @RowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	
					INSERT INTO [Inventory].TransactionItemPrices
					(
						-- Id -- this column value is auto-generated
						RowVersion,
						TransactionId,
						TransactionItemId,
						[Description],
						QuantityUnitId,
						QuantityAmount,
						PriceUnitId,
						Fee,
						MainCurrencyUnitId,
						FeeInMainCurrency,
						RegistrationDate,
						--QuantityAmountUseFIFO,
						TransactionReferenceId,
						UserCreatorId,
						CreateDate
					)
					VALUES
					(
						@RowVersion ,
						@IssueId ,
						@IssueItemId ,
						@Description,
						@QuantityUnitId ,
						@CurrentQuantityAmount,
						@TransactionItemPricePriceUnitId,
						@TransactionItemPriceFee,
						@TransactionItemPriceMainCurrencyUnitId,
						@TransactionItemPriceExchangeRate,
						@RegistrationDate,
						/*{ QuantityAmountUseFIFO }*/
						@TransactionItemPriceId,
						@UserCreatorId ,
						getdate()
					)
					
					SET @TransactionItemPriceIds+=cast(@@identity AS NVARCHAR(15))+N';'
						
					UPDATE [Inventory].TransactionItemPrices
					SET
						QuantityAmountUseFIFO += @CurrentQuantityAmount,
						IssueReferenceIds = isnull (IssueReferenceIds,N'')+cast(@@identity AS NVARCHAR(15))+N';'
					WHERE Id=@TransactionItemPriceId
									
					SET @QuantityAmount-=@CurrentQuantityAmount
					IF @QuantityAmount=0
						BREAK;
									
         			FETCH AssignIssuePrice INTO @TransactionId, 
												@TransactionRegistrationDate,
												@TransactionItemId,
												@TransactionItemRowVersion, 
												@TransactionItemQuantityAmount,
												@TransactionItemPriceId,
												@TransactionItemPriceRowVersion, 
												@TransactionItemPriceRegistrationDate,
												@TransactionItemPriceQuantityAmount, 
												@TransactionItemPricePriceUnitId,
												@TransactionItemPriceFee,
												@TransactionItemPriceMainCurrencyUnitId, 
												@TransactionItemPriceExchangeRate,
												@TransactionItemPriceQuantityAmountUseFIFO
				END
			END
			IF @QuantityAmount<>0
			BEGIN
				SET @Message = N'@There is not enough priced receipts for pricing current quantity >>> ' + CAST(@GoodId AS NVARCHAR(20)) + '  >  ' + CAST(@QuantityAmount AS NVARCHAR(20))
				RAISERROR(@Message,16,1,'500')
			END
						
			FETCH IssueItemPrices INTO  @IssueItemId , @Description
		END
			SET @Message=N'OperationSuccessful'
		IF @TRANSACTIONCOUNT = 0
				COMMIT TRANSACTION 					
		END try
		BEGIN CATCH
				IF (XACT_STATE()) = -1 
					ROLLBACK TRANSACTION
				IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
					ROLLBACK TRANSACTION
				IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
					ROLLBACK TRANSACTION CurrentTransaction
				SET @Message=ERROR_MESSAGE();
				SET @NotPricedTransactionId=NULL
				SET @TransactionItemPriceIds=NULL			    
			EXEC [Inventory].ErrorHandling
		END CATCH
END

GO	
GRANT EXECUTE ON [Inventory].PriceGivenIssuedItems TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].PriceAllSuspendedIssuedItems','P') is Not Null
	drop procedure [Inventory].PriceAllSuspendedIssuedItems;
Go
Create Procedure [Inventory].PriceAllSuspendedIssuedItems
(
@CompanyId BIGINT=0,
@WarehouseId BIGINT=0,
@FromDate DATETIME = NULL,
@ToDate DATETIME = NULL,
@UserCreatorId INT,
@TransactionItemPriceIds NVARCHAR(MAX) OUT,
@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
BEGIN
set nocount ON;
DECLARE @TRANSACTIONCOUNT INT;
	SET @TRANSACTIONCOUNT = @@TRANCOUNT;
BEGIN TRY
	IF @TRANSACTIONCOUNT = 0
		BEGIN TRANSACTION
	--ELSE
	--	SAVE TRANSACTION CurrentTransaction;
	
			
		SET @FromDate=ISNULL(@FromDate, '1800-01-01 00:00:00')
		SET @ToDate=ISNULL(@ToDate, '9999-01-01 00:00:00')
		SET @CompanyId=ISNULL(@CompanyId, 0)
		SET @WarehouseId=ISNULL(@WarehouseId, 0)
		
	DECLARE @IssueItemId INT,
			@GoodId BIGINT ,
			@QuantityUnitId BIGINT ,
			@QuantityAmount DECIMAL(20,3),
			@TimeBucketId INT,
			@RegistrationDate DATETIME,
			@IssueId INT=NULL,
			@IssueItemRowVersion SMALLINT=NULL,
			@ActionType SMALLINT=NULL,
			@NotPricedTransactionId INT,
			@Code DECIMAL(20,3),
			@WarehouseTempId BIGINT
			
			
		SET @TransactionItemPriceIds=N''
	IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'CursorIssueItemPricesAutomatically')
	DEALLOCATE CursorIssueItemPricesAutomatically
	DECLARE CursorIssueItemPricesAutomatically CURSOR FOR
	SELECT  
		ISNULL(t.Action, 0) AS ActionType,
		ti.GoodId AS GoodId,
		ti.QuantityUnitId AS QuantityUnitId,
		ti.QuantityAmount AS QuantityAmount,
		t.TimeBucketId AS TimeBucketId,
		t.RegistrationDate AS RegistrationDate,
		ti.RowVersion AS IssueItemRowVersion,
		ti.Id AS IssueItemId,
		t.Id AS IssueId,
		t.WarehouseId,
		t.Code
	FROM   [Inventory].Transactions t
	    INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId
		INNER JOIN [Inventory].TransactionItems ti
			   	ON  t.Id = ti.TransactionId
	WHERE  @CompanyId=CASE WHEN @CompanyId=0 THEN @CompanyId ELSE w.CompanyId END  
	       AND @WarehouseId=CASE WHEN @WarehouseId=0 THEN @WarehouseId ELSE t.WarehouseId END  
	       AND @FromDate<=t.RegistrationDate   
		   AND @ToDate>=t.RegistrationDate
	       AND t.[Action]=2
	       AND (t.[Status]=1 OR  t.[Status]=2)
	ORDER BY t.RegistrationDate,
			 t.Action ,
			 t.Id ,
			 ti.Id ,
			 ti.RowVersion 
	OPEN CursorIssueItemPricesAutomatically
		FETCH NEXT FROM CursorIssueItemPricesAutomatically INTO  @ActionType,
																 @GoodId,
																 @QuantityUnitId,
																 @QuantityAmount,
															     @TimeBucketId,
															     @RegistrationDate,
															     @IssueItemRowVersion,
															     @IssueItemId,
															     @IssueId,
															     @WarehouseTempId,
															     @Code
		WHILE @@Fetch_Status = 0  
		BEGIN
			DECLARE @IssueItemPriceId INT=NULL,
					@IssueItemPriceRowVersion SMALLINT=0,
					@CurrentQuantityAmount DECIMAL(20,3)
		--IF @ActionType<>2
		--BEGIN
		--	   SET @Message='Failed' 
		--	   RAISERROR(N'@فقط عمليات حواله بصورت سيستمي قابل قيمت گذاري مي باشد ',16,1,'500')	
		--END
		 
		 
		 SET @QuantityAmount=@QuantityAmount- ISNULL((SELECT SUM(ISNULL(tip.QuantityAmount,0)) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)
		 IF @QuantityAmount>0
		 BEGIN
		 	
			--*********Extended for implementing sequential pricing (Begin) *********************************************************
			DECLARE @NotPricedIssueBeforeThis BIGINT
		
			SET @NotPricedIssueBeforeThis= ISNULL((
							SELECT TOP(1)t.Id
							FROM [Inventory].Transactions t
							INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId=t.Id 
							INNER JOIN [Inventory].Warehouse a ON a.Id = t.WarehouseId AND a.[IsActive]=1
							WHERE a.Id=@WarehouseTempId
									AND ti.GoodId=@GoodId
									AND ti.QuantityUnitId=@QuantityUnitId
									--AND t.TimeBucketId=@TimeBucketId                          A.H  Order
									AND t.[Action]=2
									AND t.StoreTypesId NOT IN (28)  --Issued total Received in Trust is excluded  -- By A.H
									AND t.Id<>@IssueId
								
									AND t.[Status]<>3
									AND t.[Status]<>4 
								
									
									  AND t.RegistrationDate <= @RegistrationDate	--Changed from String to Field comparison by A.Hatefi on 1395-03-17
									  AND t.Code < @Code							--Changed from String to Field comparison by A.Hatefi on 1395-03-17
				
							)  ,0)
						
				--*********Extended for implementing sequential pricing (End) *********************************************************
			
			IF @NotPricedIssueBeforeThis=0
			BEGIN
		
			IF OBJECT_ID('tempdb..##TempIssuePriced') IS NOT NULL
					   EXEC('DROP TABLE ##TempIssuePriced')
			
					SELECT TOP(1) @IssueItemPriceId=ISNULL(MAX(tip.Id),0)+1  FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId
					IF @IssueItemPriceId=1 SET @IssueItemPriceRowVersion=1
					ELSE  SET @IssueItemPriceRowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	

			SELECT t.Id AS TransactionId, 
				t.Code AS TransactionCode,	--Added by Hatefi on 1394-10-17
				t.RegistrationDate AS TransactionRegistrationDate,ti.Id AS TransactionItemId, ti.RowVersion AS TransactionItemRowVersion, 
				   ti.QuantityAmount AS TransactionItemQuantityAmount,tip.Id AS TransactionItemPriceId, tip.RowVersion AS TransactionItemPriceRowVersion, 
				   tip.RegistrationDate AS TransactionItemPriceRegistrationDate, tip.QuantityAmount AS TransactionItemPriceQuantityAmount, 
				   tip.PriceUnitId AS TransactionItemPricePriceUnitId, tip.Fee AS TransactionItemPriceFee, tip.MainCurrencyUnitId AS TransactionItemPriceMainCurrencyUnitId, 
				   tip.FeeInMainCurrency AS TransactionItemPriceExchangeRate, tip.QuantityAmountUseFIFO AS TransactionItemPriceQuantityAmountUseFIFO
			INTO ##TempIssuePriced
			FROM [Inventory].Transactions t
			INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId=t.Id
			INNER JOIN [Inventory].TransactionItemPrices tip ON tip.TransactionItemId=ti.Id
			INNER JOIN [Inventory].Warehouse w ON w.Id = t.WarehouseId AND w.[IsActive]=1
			WHERE @CompanyId=CASE WHEN @CompanyId=0 THEN @CompanyId ELSE w.CompanyId END  
			   AND @WarehouseId=CASE WHEN @WarehouseId=0 THEN @WarehouseId ELSE t.WarehouseId END  
			   --AND @FromDate<=t.RegistrationDate   A.H Commented
		       AND @ToDate>=t.RegistrationDate
				  AND ti.GoodId=@GoodId
				  AND ti.QuantityUnitId=@QuantityUnitId
				  --AND t.TimeBucketId=@TimeBucketId                          A.H  Order
				  AND t.[Action]=1   --case when @ActionType=1 then 2 else 1 end
				  AND t.StoreTypesId NOT IN (2)  --Receipts in Trust is excluded  -- By A.H
				  AND tip.QuantityAmountUseFIFO<tip.QuantityAmount
				  AND tip.Fee>0
				  AND tip.FeeInMainCurrency>0

				  AND (t.RegistrationDate <= @RegistrationDate	--Added by Hatefi on 1394-10-17
						OR tip.RegistrationDate  <= @RegistrationDate)  --Added by Hatefi on 1395-03-19
				  AND t.Code < @Code							--Added by Hatefi on 1394-10-17
				
				  /*AND ( CONVERT(NVARCHAR(20),t.RegistrationDate,(120))
					 --+ [Inventory].LPAD(CAST(t.Action AS nvarCHAR(3)) ,' ',3)
					 + [Inventory].LPAD(CAST(t.Id AS NVARCHAR(15)),' ',15)
					 + [Inventory].LPAD(CAST(ti.Id AS NVARCHAR(15)),' ',15)
					 + [Inventory].LPAD(CAST(ti.RowVersion AS NVARCHAR(10)),' ',10) 
					 + CONVERT(NVARCHAR(20),tip.RegistrationDate,(120))
					 + [Inventory].LPAD(CAST(tip.Id AS NVARCHAR(15)),' ',15)
					 + [Inventory].LPAD(CAST(tip.RowVersion AS NVARCHAR(15)),' ',15))<=
         
					 ( CONVERT(NVARCHAR(20),@RegistrationDate,(120))
					 --+ [Inventory].LPAD(CAST(t.Action AS NVARCHAR(3)),' ',3)
					 + [Inventory].LPAD(CAST(@IssueId AS NVARCHAR(15)),' ',15)
					 + [Inventory].LPAD(CAST(@IssueItemId AS NVARCHAR(15)),' ',15)
					 + [Inventory].LPAD(CAST(@IssueItemRowVersion AS NVARCHAR(10)),' ',10) 
					 + CONVERT(NVARCHAR(20),GETDATE(),(120))
					 + [Inventory].LPAD(CAST(@IssueItemPriceId AS NVARCHAR(15)),' ',15)
					 + [Inventory].LPAD(CAST(@IssueItemPriceRowVersion AS NVARCHAR(15)),' ',15)
					 )*/
				ORDER BY 
					t.RegistrationDate
					,t.Code
					,tip.RegistrationDate
					/*t.RegistrationDate,
					t.Code,
					t.Id ,
					ti.Id ,
					ti.RowVersion ,
					tip.RegistrationDate,
					tip.Id ,
					tip.RowVersion */
       
					DECLARE @RegistrationDateFirstFree DATETIME,
							@TransactionIdFirstFree INT=NULL,
							@TransactionCodeFirstFree DECIMAL(20,2)=NULL,	--Added by Hatefi on 1394-10-17

							@TransactionItemIdFirstFree INT=NULL,
							@TransactionItemRowVersionFirstFree SMALLINT=NULL,
							@TransactionItemPriceRegistrationDateFirstFree DATETIME,
							@TransactionItemPriceIdFirstFree INT=NULL,
							@TransactionItemPriceRowVersionFirstFree SMALLINT=0 ,
							@TransactionId INT, 
							@TransactionRegistrationDate DATETIME,
							@TransactionItemId INT,
							@TransactionItemRowVersion SMALLINT, 
							@TransactionItemQuantityAmount DECIMAL(20,3),
							@TransactionItemPriceId INT,
							@TransactionItemPriceRowVersion SMALLINT, 
							@TransactionItemPriceRegistrationDate DATETIME,
							@TransactionItemPriceQuantityAmount DECIMAL(20,3),
							@TransactionItemPricePriceUnitId BIGINT,
							@TransactionItemPriceFee DECIMAL(20,3),
							@TransactionItemPriceMainCurrencyUnitId BIGINT, 
							@TransactionItemPriceExchangeRate DECIMAL(20,3),
							@TransactionItemPriceQuantityAmountUseFIFO DECIMAL(20,3);

					SELECT TOP(1) @RegistrationDateFirstFree=tip.TransactionRegistrationDate, @TransactionIdFirstFree=TransactionId, 
								@TransactionCodeFirstFree = tip.TransactionCode,	--Added by Hatefi on 1394-10-17
								  @TransactionItemIdFirstFree=TransactionItemId, @TransactionItemRowVersionFirstFree=TransactionItemRowVersion, 
								  @TransactionItemPriceRegistrationDateFirstFree=TransactionItemPriceRegistrationDate, 
								  @TransactionItemPriceIdFirstFree=TransactionItemPriceId, 
								  @TransactionItemPriceRowVersionFirstFree=TransactionItemPriceRowVersion
					FROM ##TempIssuePriced tip				 
					SET @NotPricedTransactionId= ISNULL((
								 SELECT TOP(1)t.Id
								  FROM [Inventory].Transactions t
									INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId=t.Id
									INNER JOIN [Inventory].TransactionItemPrices tip ON tip.TransactionItemId=ti.Id
									INNER JOIN [Inventory].Warehouse w ON w.Id = t.WarehouseId AND w.[IsActive]=1
									WHERE  @CompanyId=CASE WHEN @CompanyId=0 THEN @CompanyId ELSE w.CompanyId END  
									   AND @WarehouseId=CASE WHEN @WarehouseId=0 THEN @WarehouseId ELSE t.WarehouseId END  
									   AND @FromDate<=t.RegistrationDate   
									   AND @ToDate>=t.RegistrationDate
										  AND ti.GoodId=@GoodId
										  AND ti.QuantityUnitId=@QuantityUnitId
										  --AND t.TimeBucketId=@TimeBucketId                          A.H  Order
										  AND t.[Action]=1
										  AND t.StoreTypesId NOT IN (2)  --Receipts in Trust is excluded  -- By A.H
										--AND tip.QuantityAmountUseFIFO<tip.QuantityAmount		--Commented by Hatefi on 1394-10-17
										--AND (tip.Fee=0 OR tip.FeeInMainCurrency=0)			--Commented by Hatefi on 1394-10-17
										AND t.[Status]<>3										--Added by Hatefi on 1394-10-17
										AND t.[Status]<>4 										--Added by Hatefi on 1394-10-17

										AND t.RegistrationDate <= @RegistrationDateFirstFree	--Added by Hatefi on 1394-10-17
										AND t.Code < @TransactionCodeFirstFree					--Added by Hatefi on 1394-10-17

										 /* AND ( CONVERT(NVARCHAR(20),t.RegistrationDate,(120))	--Commented by Hatefi on 1394-10-17
										 + [Inventory].LPAD(CAST(t.Id AS NVARCHAR(15)),' ',15)
										 + [Inventory].LPAD(CAST(ti.Id AS NVARCHAR(15)),' ',15)
										 + [Inventory].LPAD(CAST(ti.RowVersion AS NVARCHAR(10)),' ',10) 
										 + CONVERT(NVARCHAR(20),tip.RegistrationDate,(120))
										 + [Inventory].LPAD(CAST(tip.Id AS NVARCHAR(15)),' ',15)
										 + [Inventory].LPAD(CAST(tip.RowVersion AS NVARCHAR(15)),' ',15))<=
         
										 ( CONVERT(NVARCHAR(20),@RegistrationDateFirstFree,(120))
										 + [Inventory].LPAD(CAST(@TransactionIdFirstFree AS NVARCHAR(15)),' ',15)
										 + [Inventory].LPAD(CAST(@TransactionItemIdFirstFree AS NVARCHAR(15)),' ',15)
										 + [Inventory].LPAD(CAST(@TransactionItemRowVersionFirstFree AS NVARCHAR(10)),' ',10) 
										 + CONVERT(NVARCHAR(20),@TransactionItemPriceRegistrationDateFirstFree,(120))
										 + [Inventory].LPAD(CAST(@TransactionItemPriceIdFirstFree AS NVARCHAR(15)),' ',15)
										 + [Inventory].LPAD(CAST(@TransactionItemPriceRowVersionFirstFree AS NVARCHAR(15)),' ',15)
										 )*/
									)  ,0)
					--IF @NotPricedTransactionId<>0
					--BEGIN
					--	RAISERROR(N'@رسيدي که لازم است در قيمت گذاري از آن استفاده شود؛ هنوز قيمت نخورده است',16,1,'500')
					--	IF OBJECT_ID('tempdb..##TempIssuePriced') IS NOT NULL
					--		 EXEC('DROP TABLE ##TempIssuePriced')
					--	RETURN
					--END       
					--ELSE
					IF @NotPricedTransactionId=0
					BEGIN
			
						IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'AssignIssuePrice')
								DEALLOCATE AssignIssuePrice
							DECLARE AssignIssuePrice CURSOR FOR
								SELECT  t.TransactionId, 
										t.TransactionRegistrationDate,
										t.TransactionItemId,
										t.TransactionItemRowVersion, 
										t.TransactionItemQuantityAmount,
										t.TransactionItemPriceId,
										t.TransactionItemPriceRowVersion, 
										t.TransactionItemPriceRegistrationDate,
										t.TransactionItemPriceQuantityAmount, 
										t.TransactionItemPricePriceUnitId,
										t.TransactionItemPriceFee,
										t.TransactionItemPriceMainCurrencyUnitId, 
										t.TransactionItemPriceExchangeRate,
										t.TransactionItemPriceQuantityAmountUseFIFO
								FROM ##TempIssuePriced t
								ORDER BY 
									t.TransactionItemPriceRegistrationDate,	--Added by A.Hatefi on 1395-03-17
									t.TransactionCode   --Added by Hatefi on 1394-10-17

							OPEN AssignIssuePrice
								FETCH NEXT FROM AssignIssuePrice INTO   @TransactionId, 
																		@TransactionRegistrationDate,
																		@TransactionItemId,
																		@TransactionItemRowVersion, 
																		@TransactionItemQuantityAmount,
																		@TransactionItemPriceId,
																		@TransactionItemPriceRowVersion, 
																		@TransactionItemPriceRegistrationDate,
																		@TransactionItemPriceQuantityAmount, 
																		@TransactionItemPricePriceUnitId,
																		@TransactionItemPriceFee,
																		@TransactionItemPriceMainCurrencyUnitId, 
																		@TransactionItemPriceExchangeRate,
																		@TransactionItemPriceQuantityAmountUseFIFO
								WHILE @@Fetch_Status = 0  
								BEGIN
									SET @CurrentQuantityAmount= @TransactionItemPriceQuantityAmount-@TransactionItemPriceQuantityAmountUseFIFO;
									IF @CurrentQuantityAmount>(@QuantityAmount)
									BEGIN
										SET @CurrentQuantityAmount=@QuantityAmount
									END
									DECLARE @RowVersion SMALLINT
									SET @RowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	
									INSERT INTO [Inventory].TransactionItemPrices
									(
										-- Id -- this column value is auto-generated
										RowVersion,
										TransactionId,
										TransactionItemId,
										--[Description],
										QuantityUnitId,
										QuantityAmount,
										PriceUnitId,
										Fee,
										MainCurrencyUnitId,
										FeeInMainCurrency,
										RegistrationDate,
										--QuantityAmountUseFIFO,
										TransactionReferenceId,
										UserCreatorId,
										CreateDate
									)
									VALUES
									(
										@RowVersion ,
										@IssueId ,
										@IssueItemId ,
										/*{ [Description] }*/
										@QuantityUnitId ,
										@CurrentQuantityAmount,
										@TransactionItemPricePriceUnitId,
										@TransactionItemPriceFee,
										@TransactionItemPriceMainCurrencyUnitId,
										@TransactionItemPriceExchangeRate,
										@RegistrationDate,
										/*{ QuantityAmountUseFIFO }*/
										@TransactionItemPriceId,
										@UserCreatorId ,
										getdate()
									)
									 SET @TransactionItemPriceIds+=cast(@@identity AS NVARCHAR(15))+N';'
						
									UPDATE [Inventory].TransactionItemPrices
									SET
										QuantityAmountUseFIFO += @CurrentQuantityAmount,
										IssueReferenceIds = isnull (IssueReferenceIds,N'')+cast(@@identity AS NVARCHAR(15))+N';'
									WHERE Id=@TransactionItemPriceId
									
									SET @QuantityAmount-=@CurrentQuantityAmount
									IF @QuantityAmount=0
									   BREAK;
         							FETCH AssignIssuePrice INTO @TransactionId, 
																@TransactionRegistrationDate,
																@TransactionItemId,
																@TransactionItemRowVersion, 
																@TransactionItemQuantityAmount,
																@TransactionItemPriceId,
																@TransactionItemPriceRowVersion, 
																@TransactionItemPriceRegistrationDate,
																@TransactionItemPriceQuantityAmount, 
																@TransactionItemPricePriceUnitId,
																@TransactionItemPriceFee,
																@TransactionItemPriceMainCurrencyUnitId, 
																@TransactionItemPriceExchangeRate,
																@TransactionItemPriceQuantityAmountUseFIFO
								END
						END
			
					IF @QuantityAmount<>0
					BEGIN
						SET @Message = N'@There is not enough priced receipts for pricing current quantity > ' + CAST(@GoodId AS NVARCHAR(20)) + '  >  ' + CAST(@QuantityAmount AS NVARCHAR(20))
						RAISERROR(@Message,16,1,'500')
							END
						END	--ELSE		
							--RAISERROR(N'@There are some not fully priced issues before current issue ',16,1,'500')
		END
			FETCH CursorIssueItemPricesAutomatically INTO  @ActionType,
															@GoodId,
															@QuantityUnitId,
															@QuantityAmount,
															@TimeBucketId,
															@RegistrationDate,
															@IssueItemRowVersion,
															@IssueItemId,
															@IssueId,
															@WarehouseTempId,
															@Code
		
		END
		
		SET @Message=N'OperationSuccessful'
		IF @TRANSACTIONCOUNT = 0
			COMMIT TRANSACTION 					
		END try
		BEGIN CATCH
				IF (XACT_STATE()) = -1 
					ROLLBACK TRANSACTION
				IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
					ROLLBACK TRANSACTION
				IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
					ROLLBACK TRANSACTION CurrentTransaction
			    SET @Message=ERROR_MESSAGE();
				SET @TransactionItemPriceIds=NULL
			EXEC [Inventory].ErrorHandling
		END CATCH
	END
GO	
GRANT EXECUTE ON [Inventory].PriceAllSuspendedIssuedItems TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].PriceAllSuspendedTransactionItemsUsingReference','P') is Not Null
	drop procedure [Inventory].PriceAllSuspendedTransactionItemsUsingReference;
Go
Create Procedure [Inventory].PriceAllSuspendedTransactionItemsUsingReference
(
@CompanyId BIGINT = NULL,
@WarehouseId BIGINT = NULL,
@FromDate DATETIME = NULL,
@ToDate DATETIME = NULL,
@UserCreatorId INT,
@Action TINYINT = NULL,
@TransactionItemPriceIds NVARCHAR(MAX) OUT,
@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
BEGIN
set nocount ON;
DECLARE @TRANSACTIONCOUNT INT;
	SET @TRANSACTIONCOUNT = @@TRANCOUNT;
BEGIN TRY
	IF @TRANSACTIONCOUNT = 0
		BEGIN TRANSACTION
	--ELSE
	--	SAVE TRANSACTION CurrentTransaction;
	
	SET @FromDate=ISNULL(@FromDate, '1800-01-01 00:00:00')
	SET @ToDate=ISNULL(@ToDate, '9999-01-01 00:00:00')
	SET @CompanyId=ISNULL(@CompanyId, 0)
	SET @WarehouseId=ISNULL(@WarehouseId, 0)
	SET @Action=ISNULL(@Action, 0)
		
	DECLARE @IssueItemId INT,
			@GoodId BIGINT ,
			@QuantityUnitId BIGINT ,
			@QuantityAmount DECIMAL(20,3),
			@TimeBucketId INT,
			@RegistrationDate DATETIME,
			@IssueId INT=NULL,
			@PricingReferenceId INT,
			@IssueItemRowVersion SMALLINT=NULL,
			@ActionType SMALLINT=NULL,
			
			@NotPricedTransactionId INT,
			@StoreTypesId INT=NULL,
		    @StoreTypesCode SMALLINT,
			
			
			 @IssueItemPriceId INT=NULL,
			 @IssueItemPriceRowVersion SMALLINT=0,
			 @CurrentQuantityAmount DECIMAL(20,3)
			
		SET @TransactionItemPriceIds=N''
	IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'CursorIssueItemPricesBackPayAutomatically')
	DEALLOCATE CursorIssueItemPricesBackPayAutomatically
	DECLARE CursorIssueItemPricesBackPayAutomatically CURSOR FOR
	SELECT  
		ISNULL(t.Action, 0) AS ActionType,
		ti.GoodId AS GoodId,
		ti.QuantityUnitId AS QuantityUnitId,
		ti.QuantityAmount AS QuantityAmount,
		t.TimeBucketId AS TimeBucketId,
		t.PricingReferenceId ,
		t.RegistrationDate AS RegistrationDate,
		ti.RowVersion AS IssueItemRowVersion,
		ti.Id AS IssueItemId,
		t.Id AS IssueId,
		st.Id AS StoreTypesId
	FROM   [Inventory].Transactions t
	    INNER JOIN Inventory.Warehouse w ON w.Id = t.WarehouseId
		INNER JOIN [Inventory].TransactionItems ti
			   	ON  t.Id = ti.TransactionId
	    INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
	WHERE  @CompanyId=CASE WHEN @CompanyId=0 THEN @CompanyId ELSE w.CompanyId END  
	       AND @WarehouseId=CASE WHEN @WarehouseId=0 THEN @WarehouseId ELSE t.WarehouseId END  
	       AND @FromDate<=t.RegistrationDate   
		   AND @ToDate>=t.RegistrationDate
	       AND @Action=CASE WHEN @Action=0 THEN 0 ELSE t.[Action] END
	       AND (t.[Status]=1 OR  t.[Status]=2)
	       AND ISNULL(t.PricingReferenceId,0)<>0  
	ORDER BY t.RegistrationDate,
			 t.Action ,
			 t.Id ,
			 ti.Id ,
			 ti.RowVersion 
			 
	OPEN CursorIssueItemPricesBackPayAutomatically
		FETCH NEXT FROM CursorIssueItemPricesBackPayAutomatically INTO  @ActionType,
																 @GoodId,
																 @QuantityUnitId,
																 @QuantityAmount,
															     @TimeBucketId,
															     @PricingReferenceId,
															     @RegistrationDate,
															     @IssueItemRowVersion,
															     @IssueItemId,
															     @IssueId,
															     @StoreTypesId
		WHILE @@Fetch_Status = 0  
		BEGIN
			SET @IssueItemPriceId =NULL
			SET		@IssueItemPriceRowVersion =0
			SET		@CurrentQuantityAmount =0

		IF ISNULL(@PricingReferenceId,0)=0
		BEGIN
			   SET @Message='No Pricing ReferenceId' 
			    RAISERROR(N'@Only rejected receipts/issue or factors could have reference for pricing ',16,1,'500')	
		END
		
		IF ISNULL(@PricingReferenceId,0)<>ISNULL((SELECT TOP(1)tip.TransactionReferenceId
		                                            FROM [Inventory].TransactionItemPrices tip 
		                                          WHERE tip.TransactionReferenceId<>@PricingReferenceId 
														AND tip.TransactionItemId=@IssueItemId),@PricingReferenceId)
		BEGIN
			   SET @Message='Inconsistent Pricing ReferenceId' 
			   RAISERROR(N'@Rejected receipts/issue or factors could have one reference for pricing  ',16,1,'500')	
		END 
		 
		 SET @QuantityAmount=@QuantityAmount- ISNULL((SELECT SUM(ISNULL(tip.QuantityAmount,0)) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)
		
		 IF @QuantityAmount>0
		 BEGIN
			IF OBJECT_ID('tempdb..##TempIssuePriced') IS NOT NULL
					   EXEC('DROP TABLE ##TempIssuePriced')
			
					SELECT TOP(1) @IssueItemPriceId=ISNULL(MAX(tip.Id),0)+1  FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId
					IF @IssueItemPriceId=1 SET @IssueItemPriceRowVersion=1
					ELSE  SET @IssueItemPriceRowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	

			SELECT t.Id AS TransactionId, t.RegistrationDate AS TransactionRegistrationDate,ti.Id AS TransactionItemId, ti.RowVersion AS TransactionItemRowVersion, 
				   ti.QuantityAmount AS TransactionItemQuantityAmount,tip.Id AS TransactionItemPriceId, tip.RowVersion AS TransactionItemPriceRowVersion, 
				   tip.RegistrationDate AS TransactionItemPriceRegistrationDate, tip.QuantityAmount AS TransactionItemPriceQuantityAmount, 
				   tip.PriceUnitId AS TransactionItemPricePriceUnitId, tip.Fee AS TransactionItemPriceFee, tip.MainCurrencyUnitId AS TransactionItemPriceMainCurrencyUnitId, 
				   tip.FeeInMainCurrency AS TransactionItemPriceExchangeRate, tip.QuantityAmountUseFIFO AS TransactionItemPriceQuantityAmountUseFIFO
			INTO ##TempIssuePriced
			FROM [Inventory].Transactions t
			INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId=t.Id
			INNER JOIN [Inventory].TransactionItemPrices tip ON tip.TransactionItemId=ti.Id
			INNER JOIN [Inventory].Warehouse w ON w.Id = t.WarehouseId AND w.[IsActive]=1
			WHERE  @CompanyId=CASE WHEN @CompanyId=0 THEN @CompanyId ELSE w.CompanyId END  
			   AND @WarehouseId=CASE WHEN @WarehouseId=0 THEN @WarehouseId ELSE t.WarehouseId END  
			   --AND @FromDate<=t.RegistrationDate   
		       AND @ToDate>=t.RegistrationDate
				  AND ti.GoodId=@GoodId
				  AND ti.QuantityUnitId=@QuantityUnitId
				  --AND t.TimeBucketId=@TimeBucketId                          A.H  Order
				  --AND t.[Action]=case when @ActionType=1 then 2  when @ActionType=2 then  1  when @ActionType=3 then 2 end
				  --AND tip.QuantityAmountUseFIFO<tip.QuantityAmount    --  با جناب هاتفي هماهنگ کرده و نظر ايشان اعمال شد
				  AND tip.Fee>0
				  AND tip.FeeInMainCurrency>0
				  AND t.Id=@PricingReferenceId
				ORDER BY 
				       t.RegistrationDate ASC,
					   t.Action ASC,
					   t.Id ASC,
					   ti.Id ASC,
					   ti.RowVersion ASC,
					   tip.RegistrationDate ASC,
					   tip.Id ASC,
					   tip.RowVersion ASC --بصورت FIFO قيمت گذاري برگشتي انجام مي شود	
				
				DECLARE     @TransactionId INT, 
							@TransactionRegistrationDate DATETIME,
							@TransactionItemId INT,
							@TransactionItemRowVersion SMALLINT, 
							@TransactionItemQuantityAmount DECIMAL(20,3),
							@TransactionItemPriceId INT,
							@TransactionItemPriceRowVersion SMALLINT, 
							@TransactionItemPriceRegistrationDate DATETIME,
							@TransactionItemPriceQuantityAmount DECIMAL(20,3),
							@TransactionItemPricePriceUnitId BIGINT,
							@TransactionItemPriceFee DECIMAL(20,3),
							@TransactionItemPriceMainCurrencyUnitId BIGINT, 
							@TransactionItemPriceExchangeRate DECIMAL(20,3),
							@TransactionItemPriceQuantityAmountUseFIFO DECIMAL(20,3);	
					--**************************
					--قسمتي که براي بررسي رسيدهاي قبلي قيمت گذاري شده باشد براي اين سناريو حذف کردم
					--**************************
					DECLARE @RowVersion SMALLINT
					IF @ActionType=3
					BEGIN
						IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'AssignIssuePrice')
								DEALLOCATE AssignIssuePrice
							DECLARE AssignIssuePrice CURSOR FOR
								SELECT  t.TransactionId, 
										t.TransactionRegistrationDate,
										t.TransactionItemId,
										t.TransactionItemRowVersion, 
										t.TransactionItemQuantityAmount,
										t.TransactionItemPriceId,
										t.TransactionItemPriceRowVersion, 
										t.TransactionItemPriceRegistrationDate,
										t.TransactionItemPriceQuantityAmount, 
										t.TransactionItemPricePriceUnitId,
										t.TransactionItemPriceFee,
										t.TransactionItemPriceMainCurrencyUnitId, 
										t.TransactionItemPriceExchangeRate,
										t.TransactionItemPriceQuantityAmountUseFIFO
								FROM ##TempIssuePriced t
								ORDER BY 
								t.TransactionRegistrationDate ASC,
								t.TransactionId ASC,
								t.TransactionItemId ASC,
								t.TransactionItemRowVersion ASC,
								t.TransactionItemPriceRegistrationDate ASC,
								t.TransactionItemPriceId ASC,
								t.TransactionItemPriceRowVersion ASC --بصورت FIFO قيمت گذاري برگشتي انجام مي شود
							OPEN AssignIssuePrice
								FETCH NEXT FROM AssignIssuePrice INTO   @TransactionId, 
																		@TransactionRegistrationDate,
																		@TransactionItemId,
																		@TransactionItemRowVersion, 
																		@TransactionItemQuantityAmount,
																		@TransactionItemPriceId,
																		@TransactionItemPriceRowVersion, 
																		@TransactionItemPriceRegistrationDate,
																		@TransactionItemPriceQuantityAmount, 
																		@TransactionItemPricePriceUnitId,
																		@TransactionItemPriceFee,
																		@TransactionItemPriceMainCurrencyUnitId, 
																		@TransactionItemPriceExchangeRate,
																		@TransactionItemPriceQuantityAmountUseFIFO
								WHILE @@Fetch_Status = 0  
								BEGIN
									--به گفته جناب هاتفي(خانم رضاييان)در تاريخ 1393-04-03
									--تمام مقادير Correction+
									-- با في يک آرتيکل از مرجع انتخاب شده قيمت گذاري گردد و کاري با ميزان و موجودي آن مرجع نداريم
									
									--1393-12-07.1 : Pricing by reference does not repect the possible providing amount of origin for pricing.
									/*
									SET @StoreTypesCode=(SELECT TOP(1) Code FROM [Inventory].StoreTypes st WHERE st.Id=@StoreTypesId)
									IF (@StoreTypesCode=7 AND @ActionType=1)
									OR (@StoreTypesCode=13 AND @ActionType=2)
									BEGIN
										SET @CurrentQuantityAmount=@QuantityAmount
									END
									ELSE
									BEGIN
										-- روند عادي قيمت گذاري از روي مرجع و سطر به سطر بر اساس تعداد و قيمت مرجع
										SET @CurrentQuantityAmount= @TransactionItemPriceQuantityAmount-@TransactionItemPriceQuantityAmountUseFIFO;
										IF @CurrentQuantityAmount>(@QuantityAmount)
										BEGIN
											SET @CurrentQuantityAmount=@QuantityAmount
										END	
									END
									--SET @CurrentQuantityAmount= @TransactionItemPriceQuantityAmount-@TransactionItemPriceQuantityAmountUseFIFO;
									--IF @CurrentQuantityAmount>(@QuantityAmount)
									--BEGIN
									--	SET @CurrentQuantityAmount=@QuantityAmount
									--END
									*/
									
									--1393-12-07.1 : , hence the amount of pricing is the same as current processing amount.
									SET @CurrentQuantityAmount=@QuantityAmount
									
									SET @RowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	
									INSERT INTO [Inventory].TransactionItemPrices
									(
										-- Id -- this column value is auto-generated
										RowVersion,
										TransactionId,
										TransactionItemId,
										--[Description],
										QuantityUnitId,
										QuantityAmount,
										PriceUnitId,
										Fee,
										MainCurrencyUnitId,
										FeeInMainCurrency,
										RegistrationDate,
										--QuantityAmountUseFIFO,
										TransactionReferenceId,
										UserCreatorId,
										CreateDate
									)
									VALUES
									(
										@RowVersion ,
										@IssueId ,
										@IssueItemId ,
										/*{ [Description] }*/
										@QuantityUnitId ,
										@CurrentQuantityAmount,
										@TransactionItemPricePriceUnitId,
										@TransactionItemPriceFee,
										@TransactionItemPriceMainCurrencyUnitId,
										@TransactionItemPriceExchangeRate,
										@RegistrationDate,
										/*{ QuantityAmountUseFIFO }*/
										@TransactionItemPriceId,
										@UserCreatorId ,
										getdate()
									)
									 SET @TransactionItemPriceIds+=cast(@@identity AS NVARCHAR(15))+N';'
						
									UPDATE [Inventory].TransactionItemPrices
									SET
										--1393-12-07.1 : This is commented by Hatefi because there is no need to update the provided amount of origin for pricing other transactions.
										--QuantityAmountUseFIFO += CASE WHEN (@StoreTypesCode=7 OR @StoreTypesCode=13) THEN 0 ELSE @CurrentQuantityAmount END,
										IssueReferenceIds = isnull (IssueReferenceIds,N'')+cast(@@identity AS NVARCHAR(15))+N';'
									WHERE Id=@TransactionItemPriceId
									
									SET @QuantityAmount-=@CurrentQuantityAmount
									IF @QuantityAmount=0
									   BREAK;
         							FETCH AssignIssuePrice INTO @TransactionId, 
																@TransactionRegistrationDate,
																@TransactionItemId,
																@TransactionItemRowVersion, 
																@TransactionItemQuantityAmount,
																@TransactionItemPriceId,
																@TransactionItemPriceRowVersion, 
																@TransactionItemPriceRegistrationDate,
																@TransactionItemPriceQuantityAmount, 
																@TransactionItemPricePriceUnitId,
																@TransactionItemPriceFee,
																@TransactionItemPriceMainCurrencyUnitId, 
																@TransactionItemPriceExchangeRate,
																@TransactionItemPriceQuantityAmountUseFIFO
								END
					END
					ELSE
					BEGIN
						IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'AssignIssuePrice')
								DEALLOCATE AssignIssuePrice
							DECLARE AssignIssuePrice CURSOR FOR
								SELECT  t.TransactionId, 
										t.TransactionRegistrationDate,
										t.TransactionItemId,
										t.TransactionItemRowVersion, 
										t.TransactionItemQuantityAmount,
										t.TransactionItemPriceId,
										t.TransactionItemPriceRowVersion, 
										t.TransactionItemPriceRegistrationDate,
										t.TransactionItemPriceQuantityAmount, 
										t.TransactionItemPricePriceUnitId,
										t.TransactionItemPriceFee,
										t.TransactionItemPriceMainCurrencyUnitId, 
										t.TransactionItemPriceExchangeRate,
										t.TransactionItemPriceQuantityAmountUseFIFO
								FROM ##TempIssuePriced t
								ORDER BY 
								    t.TransactionRegistrationDate DESC,
									t.TransactionId DESC,
									t.TransactionItemId DESC,
									t.TransactionItemRowVersion DESC,
									t.TransactionItemPriceRegistrationDate DESC,
									t.TransactionItemPriceId DESC,
									t.TransactionItemPriceRowVersion DESC --بصورت LIFO قيمت گذاري برگشتي انجام مي شود
							OPEN AssignIssuePrice
								FETCH NEXT FROM AssignIssuePrice INTO   @TransactionId, 
																		@TransactionRegistrationDate,
																		@TransactionItemId,
																		@TransactionItemRowVersion, 
																		@TransactionItemQuantityAmount,
																		@TransactionItemPriceId,
																		@TransactionItemPriceRowVersion, 
																		@TransactionItemPriceRegistrationDate,
																		@TransactionItemPriceQuantityAmount, 
																		@TransactionItemPricePriceUnitId,
																		@TransactionItemPriceFee,
																		@TransactionItemPriceMainCurrencyUnitId, 
																		@TransactionItemPriceExchangeRate,
																		@TransactionItemPriceQuantityAmountUseFIFO
								WHILE @@Fetch_Status = 0  
								BEGIN
									--به گفته جناب هاتفي(خانم رضاييان)در تاريخ 1393-04-03
									--تمام مقادير Correction+
									-- با في يک آرتيکل از مرجع انتخاب شده قيمت گذاري گردد و کاري با ميزان و موجودي آن مرجع نداريم

									--1393-12-07.1 : Pricing by reference does not repect the possible providing amount of origin for pricing.
									/*
									SET @StoreTypesCode=(SELECT TOP(1) Code FROM [Inventory].StoreTypes st WHERE st.Id=@StoreTypesId)
									IF (@StoreTypesCode=7 AND @ActionType=1)
										OR (@StoreTypesCode=13 AND @ActionType=2)
									BEGIN
										SET @CurrentQuantityAmount=@QuantityAmount
									END
									ELSE
									BEGIN
										-- روند عادي قيمت گذاري از روي مرجع و سطر به سطر بر اساس تعداد و قيمت مرجع
										SET @CurrentQuantityAmount= @TransactionItemPriceQuantityAmount-@TransactionItemPriceQuantityAmountUseFIFO;
										IF @CurrentQuantityAmount>(@QuantityAmount)
										BEGIN
											SET @CurrentQuantityAmount=@QuantityAmount
										END	
									END
									--SET @CurrentQuantityAmount= @TransactionItemPriceQuantityAmount-@TransactionItemPriceQuantityAmountUseFIFO;
									--IF @CurrentQuantityAmount>(@QuantityAmount)
									--BEGIN
									--	SET @CurrentQuantityAmount=@QuantityAmount
									--END
									*/
									--1393-12-07.1 : , hence the amount of pricing is the same as current processing amount.
									SET @CurrentQuantityAmount=@QuantityAmount
									
									SET @RowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	
									INSERT INTO [Inventory].TransactionItemPrices
									(
										-- Id -- this column value is auto-generated
										RowVersion,
										TransactionId,
										TransactionItemId,
										--[Description],
										QuantityUnitId,
										QuantityAmount,
										PriceUnitId,
										Fee,
										MainCurrencyUnitId,
										FeeInMainCurrency,
										RegistrationDate,
										--QuantityAmountUseFIFO,
										TransactionReferenceId,
										UserCreatorId,
										CreateDate
									)
									VALUES
									(
										@RowVersion ,
										@IssueId ,
										@IssueItemId ,
										/*{ [Description] }*/
										@QuantityUnitId ,
										@CurrentQuantityAmount,
										@TransactionItemPricePriceUnitId,
										@TransactionItemPriceFee,
										@TransactionItemPriceMainCurrencyUnitId,
										@TransactionItemPriceExchangeRate,
										@RegistrationDate,
										/*{ QuantityAmountUseFIFO }*/
										@TransactionItemPriceId,
										@UserCreatorId ,
										getdate()
									)
									 SET @TransactionItemPriceIds+=cast(@@identity AS NVARCHAR(15))+N';'
													
									UPDATE [Inventory].TransactionItemPrices
									SET
										--1393-12-07.1 : This is commented by Hatefi because there is no need to update the provided amount of origin for pricing other transactions.
										--QuantityAmountUseFIFO += CASE WHEN (@StoreTypesCode=7 OR @StoreTypesCode=13) THEN 0 ELSE @CurrentQuantityAmount END,
										IssueReferenceIds = isnull (IssueReferenceIds,N'')+cast(@@identity AS NVARCHAR(15))+N';'
									WHERE Id=@TransactionItemPriceId
									
									SET @QuantityAmount-=@CurrentQuantityAmount
									IF @QuantityAmount=0
									   BREAK;
         							FETCH AssignIssuePrice INTO @TransactionId, 
																@TransactionRegistrationDate,
																@TransactionItemId,
																@TransactionItemRowVersion, 
																@TransactionItemQuantityAmount,
																@TransactionItemPriceId,
																@TransactionItemPriceRowVersion, 
																@TransactionItemPriceRegistrationDate,
																@TransactionItemPriceQuantityAmount, 
																@TransactionItemPricePriceUnitId,
																@TransactionItemPriceFee,
																@TransactionItemPriceMainCurrencyUnitId, 
																@TransactionItemPriceExchangeRate,
																@TransactionItemPriceQuantityAmountUseFIFO
								END
					END
				 END--else @QuantityAmount<>0
				    --RAISERROR(N'@موجودي رسيدهاي با قيمت براي قيمت گزاري حواله کافي نيست',16,1,'500')
				FETCH CursorIssueItemPricesBackPayAutomatically INTO  @ActionType,
																 @GoodId,
																 @QuantityUnitId,
																 @QuantityAmount,
															     @TimeBucketId,
															     @PricingReferenceId,
															     @RegistrationDate,
															     @IssueItemRowVersion,
															     @IssueItemId,
															     @IssueId,
															     @StoreTypesId
		
		END 
		SET @Message=N'OperationSuccessful'
		IF @TRANSACTIONCOUNT = 0
				COMMIT TRANSACTION 					
		END try
		BEGIN CATCH
				IF (XACT_STATE()) = -1 
					ROLLBACK TRANSACTION
				IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
					ROLLBACK TRANSACTION
				IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
					ROLLBACK TRANSACTION CurrentTransaction
				 SET @Message=ERROR_MESSAGE();
				SET @TransactionItemPriceIds=NULL
			EXEC [Inventory].ErrorHandling
		END CATCH
	END
GO	
GRANT EXECUTE ON [Inventory].PriceAllSuspendedTransactionItemsUsingReference TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].PriceSuspendedTransactionUsingReference','P') is Not Null
	drop procedure [Inventory].PriceSuspendedTransactionUsingReference;
Go
Create Procedure [Inventory].PriceSuspendedTransactionUsingReference
(
@TransactionId INT,
@Description NVARCHAR(MAX)=NULL,
@EffectiveDateTime DATETIME = NULL,
@UserCreatorId INT,
@TransactionItemPriceIds NVARCHAR(MAX) OUT,
@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
BEGIN
set nocount ON;
DECLARE @TRANSACTIONCOUNT INT;
	SET @TRANSACTIONCOUNT = @@TRANCOUNT;
BEGIN TRY
	IF @TRANSACTIONCOUNT = 0
		BEGIN TRANSACTION
	--ELSE
	--	SAVE TRANSACTION CurrentTransaction;
	DECLARE @IssueItemId INT,
			@GoodId BIGINT ,
			@QuantityUnitId BIGINT ,
			@QuantityAmount DECIMAL(20,3),
			@TimeBucketId INT,
			@RegistrationDate DATETIME,
			@IssueId INT=NULL,
			@PricingReferenceId INT,
			@IssueItemRowVersion SMALLINT=NULL,
			@WarehouseId BIGINT,
			@ActionType SMALLINT=NULL,
			@StoreTypesId INT=NULL,
		    @StoreTypesCode SMALLINT
			
			
			
		SET @TransactionItemPriceIds=N''
	IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'CursorPriceSuspendedTransactionUsingReference')
	DEALLOCATE CursorPriceSuspendedTransactionUsingReference
	DECLARE CursorPriceSuspendedTransactionUsingReference CURSOR FOR
	SELECT  
		ISNULL(t.Action, 0) AS ActionType,
		ti.GoodId AS GoodId,
		ti.QuantityUnitId AS QuantityUnitId,
		ti.QuantityAmount AS QuantityAmount,
		t.TimeBucketId AS TimeBucketId,
		t.PricingReferenceId ,
		t.RegistrationDate AS RegistrationDate,  --This will be used for registreation date (effective date) of pricing line if no effective date is given at the time of calling the procedure.
		ti.RowVersion AS IssueItemRowVersion,
		ti.Id AS IssueItemId,
		t.Id AS IssueId,
		t.WarehouseId,
		t.StoreTypesId
	FROM   [Inventory].Transactions t
		INNER JOIN [Inventory].TransactionItems ti
			   	ON  t.Id = ti.TransactionId
	WHERE  t.Id=@TransactionId 
	       AND (t.[Status]=1 OR  t.[Status]=2)
	       AND ISNULL(t.PricingReferenceId,0)<>0  
	ORDER BY t.RegistrationDate,
			 t.Action ,
			 t.Id ,
			 ti.Id ,
			 ti.RowVersion 
			 
	OPEN CursorPriceSuspendedTransactionUsingReference
		FETCH NEXT FROM CursorPriceSuspendedTransactionUsingReference INTO  @ActionType,
																 @GoodId,
																 @QuantityUnitId,
																 @QuantityAmount,
															     @TimeBucketId,
															     @PricingReferenceId,
															     @RegistrationDate,
															     @IssueItemRowVersion,
															     @IssueItemId,
															     @IssueId,
															     @WarehouseId,
															     @StoreTypesId
		WHILE @@Fetch_Status = 0  
		BEGIN
			DECLARE @IssueItemPriceId INT=NULL,
					@IssueItemPriceRowVersion SMALLINT=0,
					@CurrentQuantityAmount DECIMAL(20,3)
		IF ISNULL(@PricingReferenceId,0)=0
		BEGIN
			   SET @Message='No Pricing ReferenceId' 
			   RAISERROR(N'@Only rejected receipts/issue or factors could have reference for pricing  ',16,1,'500')	
		END
		IF ISNULL(@PricingReferenceId,0)<>ISNULL((SELECT TOP(1)tip.TransactionReferenceId
		                                            FROM [Inventory].TransactionItemPrices tip 
		                                          WHERE tip.TransactionReferenceId<>@PricingReferenceId 
														AND tip.TransactionItemId=@IssueItemId),@PricingReferenceId)
		BEGIN
			   SET @Message='Inconsistent Pricing ReferenceId' 
			   RAISERROR(N'@Rejected receipts/issue or factors could have one reference for pricing  ',16,1,'500')	
		END 
		 
		 SET @QuantityAmount=@QuantityAmount- ISNULL((SELECT SUM(ISNULL(tip.QuantityAmount,0)) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)
		 IF @QuantityAmount>0 --If any quantity is remained yet without any pricing.
		 BEGIN
		 	IF OBJECT_ID('tempdb..##TempIssuePriced') IS NOT NULL
						   EXEC('DROP TABLE ##TempIssuePriced')
					SELECT TOP(1) @IssueItemPriceId=ISNULL(MAX(tip.Id),0)+1  FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId
					IF @IssueItemPriceId=1 SET @IssueItemPriceRowVersion=1
					ELSE  SET @IssueItemPriceRowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	

				SELECT t.Id AS TransactionId,t.[Action], t.RegistrationDate AS TransactionRegistrationDate,ti.Id AS TransactionItemId, ti.RowVersion AS TransactionItemRowVersion, 
					   ti.QuantityAmount AS TransactionItemQuantityAmount,tip.Id AS TransactionItemPriceId, tip.RowVersion AS TransactionItemPriceRowVersion, 
					   tip.RegistrationDate AS TransactionItemPriceRegistrationDate, tip.QuantityAmount AS TransactionItemPriceQuantityAmount, 
					   tip.PriceUnitId AS TransactionItemPricePriceUnitId, tip.Fee AS TransactionItemPriceFee, tip.MainCurrencyUnitId AS TransactionItemPriceMainCurrencyUnitId, 
					   tip.FeeInMainCurrency AS TransactionItemPriceExchangeRate, tip.QuantityAmountUseFIFO AS TransactionItemPriceQuantityAmountUseFIFO
				INTO ##TempIssuePriced
				FROM [Inventory].Transactions t
				INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId=t.Id
				INNER JOIN [Inventory].TransactionItemPrices tip ON tip.TransactionItemId=ti.Id
				INNER JOIN [Inventory].Warehouse a ON a.Id = t.WarehouseId AND a.[IsActive]=1
				WHERE a.Id=@WarehouseId
					  AND ti.GoodId=@GoodId
					  AND ti.QuantityUnitId=@QuantityUnitId
					  
					  --AND t.TimeBucketId=@TimeBucketId  --Commented by Hatefi

					  --AND t.[Action]=case when @ActionType=1 then 2 else 1 end
					  --AND tip.QuantityAmountUseFIFO<tip.QuantityAmount    --  با جناب هاتفي هماهنگ کرده و نظر ايشان اعمال شد
					  AND tip.Fee>0
					  AND tip.FeeInMainCurrency>0
					  AND t.Id=@PricingReferenceId
					ORDER BY
					   t.RegistrationDate ASC,
					   t.Action ASC,
					   t.Id ASC,
					   ti.Id ASC,
					   ti.RowVersion ASC,
					   tip.RegistrationDate ASC,
					   tip.Id ASC,
					   tip.RowVersion ASC --بصورت FIFO قيمت گذاري برگشتي انجام مي شود	
					   
				   
				DECLARE     @TransactionRegistrationDate DATETIME,
							@TransactionItemId INT,
							@Action SMALLINT,
							@TransactionItemRowVersion SMALLINT, 
							@TransactionItemQuantityAmount DECIMAL(20,3),
							@TransactionItemPriceId INT,
							@TransactionItemPriceRowVersion SMALLINT, 
							@TransactionItemPriceRegistrationDate DATETIME,
							@TransactionItemPriceQuantityAmount DECIMAL(20,3),
							@TransactionItemPricePriceUnitId BIGINT,
							@TransactionItemPriceFee DECIMAL(20,3),
							@TransactionItemPriceMainCurrencyUnitId BIGINT, 
							@TransactionItemPriceExchangeRate DECIMAL(20,3),
							@TransactionItemPriceQuantityAmountUseFIFO DECIMAL(20,3);	
					--**************************
					--قسمتي که براي بررسي رسيدهاي قبلي قيمت گذاري شده باشد براي اين سناريو حذف کردم
					--**************************

					SET @StoreTypesCode=(SELECT TOP(1) Code FROM [Inventory].StoreTypes st WHERE st.Id=@StoreTypesId)

					DECLARE @RowVersion SMALLINT
					IF @ActionType=3
					BEGIN
						IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'AssignIssuePrice')
								DEALLOCATE AssignIssuePrice
							DECLARE AssignIssuePrice CURSOR FOR
								SELECT  t.TransactionId, 
								        t.Action,
										t.TransactionRegistrationDate,
										t.TransactionItemId,
										t.TransactionItemRowVersion, 
										t.TransactionItemQuantityAmount,
										t.TransactionItemPriceId,
										t.TransactionItemPriceRowVersion, 
										t.TransactionItemPriceRegistrationDate,
										t.TransactionItemPriceQuantityAmount, 
										t.TransactionItemPricePriceUnitId,
										t.TransactionItemPriceFee,
										t.TransactionItemPriceMainCurrencyUnitId, 
										t.TransactionItemPriceExchangeRate,
										t.TransactionItemPriceQuantityAmountUseFIFO
								FROM ##TempIssuePriced t
								ORDER BY
									   t.TransactionRegistrationDate ASC,
									   t.TransactionId ASC,
									   t.TransactionItemId ASC,
									   t.TransactionItemRowVersion ASC,
									   t.TransactionItemPriceRegistrationDate ASC,
									   t.TransactionItemPriceId ASC,
									   t.TransactionItemPriceRowVersion ASC --بصورت FIFO قيمت گذاري برگشتي انجام مي شود
							OPEN AssignIssuePrice
								FETCH NEXT FROM AssignIssuePrice INTO   @TransactionId, 
																		@Action,
																		@TransactionRegistrationDate,
																		@TransactionItemId,
																		@TransactionItemRowVersion, 
																		@TransactionItemQuantityAmount,
																		@TransactionItemPriceId,
																		@TransactionItemPriceRowVersion, 
																		@TransactionItemPriceRegistrationDate,
																		@TransactionItemPriceQuantityAmount, 
																		@TransactionItemPricePriceUnitId,
																		@TransactionItemPriceFee,
																		@TransactionItemPriceMainCurrencyUnitId, 
																		@TransactionItemPriceExchangeRate,
																		@TransactionItemPriceQuantityAmountUseFIFO
								WHILE @@Fetch_Status = 0  
								BEGIN
									--به گفته جناب هاتفي(خانم رضاييان)در تاريخ 1393-04-03
									--تمام مقادير Correction+
									-- با في يک آرتيکل از مرجع انتخاب شده قيمت گذاري گردد و کاري با ميزان و موجودي آن مرجع نداريم

									--1393-12-07.1 : Pricing by reference does not repect the possible providing amount of origin for pricing.
									/*
									SET @StoreTypesCode=(SELECT TOP(1) Code FROM [Inventory].StoreTypes st WHERE st.Id=@StoreTypesId)
									IF (@StoreTypesCode=7 AND @ActionType=1)
										OR (@StoreTypesCode=13 AND @ActionType=2)
									BEGIN
										SET @CurrentQuantityAmount=@QuantityAmount
									END
									ELSE
									BEGIN
										-- روند عادي قيمت گذاري از روي مرجع و سطر به سطر بر اساس تعداد و قيمت مرجع
										SET @CurrentQuantityAmount= @TransactionItemPriceQuantityAmount-@TransactionItemPriceQuantityAmountUseFIFO;
										IF @CurrentQuantityAmount>(@QuantityAmount)
										BEGIN
											SET @CurrentQuantityAmount=@QuantityAmount
										END	
									END
									*/
									
									--1393-12-07.1 : , hence the amount of pricing is the same as current processing amount.
									SET @CurrentQuantityAmount=@QuantityAmount
									
									SET @RowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	
									INSERT INTO [Inventory].TransactionItemPrices
									(
										-- Id -- this column value is auto-generated
										RowVersion,
										TransactionId,
										TransactionItemId,
										[Description],
										QuantityUnitId,
										QuantityAmount,
										PriceUnitId,
										Fee,
										MainCurrencyUnitId,
										FeeInMainCurrency,
										RegistrationDate,
										--QuantityAmountUseFIFO,
										TransactionReferenceId,
										UserCreatorId,
										CreateDate
									)
									VALUES
									(
										@RowVersion ,
										@IssueId ,
										@IssueItemId ,
										@Description,
										@QuantityUnitId ,
										@CurrentQuantityAmount,
										@TransactionItemPricePriceUnitId,
										@TransactionItemPriceFee,
										@TransactionItemPriceMainCurrencyUnitId,
										@TransactionItemPriceExchangeRate,
										ISNULL(@EffectiveDateTime, @RegistrationDate),
										/*{ QuantityAmountUseFIFO }*/
										@TransactionItemPriceId,
										@UserCreatorId ,
										getdate()
									)
									 SET @TransactionItemPriceIds+=cast(@@identity AS NVARCHAR(15))+N';'
									 
									UPDATE [Inventory].TransactionItemPrices
									SET
										--1393-12-07.1 : This is commented by Hatefi because there is no need to update the provided amount of origin for pricing other transactions.
										--QuantityAmountUseFIFO += CASE WHEN (@StoreTypesCode=7 OR @StoreTypesCode=13) THEN 0 ELSE @CurrentQuantityAmount END,
										IssueReferenceIds = isnull (IssueReferenceIds,N'')+cast(@@identity AS NVARCHAR(15))+N';'
									WHERE Id=@TransactionItemPriceId
									
									SET @QuantityAmount-=@CurrentQuantityAmount
									IF @QuantityAmount=0
									   BREAK;
         							FETCH AssignIssuePrice INTO @TransactionId, 
																@Action, 
																@TransactionRegistrationDate,
																@TransactionItemId,
																@TransactionItemRowVersion, 
																@TransactionItemQuantityAmount,
																@TransactionItemPriceId,
																@TransactionItemPriceRowVersion, 
																@TransactionItemPriceRegistrationDate,
																@TransactionItemPriceQuantityAmount, 
																@TransactionItemPricePriceUnitId,
																@TransactionItemPriceFee,
																@TransactionItemPriceMainCurrencyUnitId, 
																@TransactionItemPriceExchangeRate,
																@TransactionItemPriceQuantityAmountUseFIFO
								END
								
						END
					ELSE
					BEGIN
						IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'AssignIssuePrice')
								DEALLOCATE AssignIssuePrice
							DECLARE AssignIssuePrice CURSOR FOR
								SELECT  t.TransactionId, 
								        t.Action,
										t.TransactionRegistrationDate,
										t.TransactionItemId,
										t.TransactionItemRowVersion, 
										t.TransactionItemQuantityAmount,
										t.TransactionItemPriceId,
										t.TransactionItemPriceRowVersion, 
										t.TransactionItemPriceRegistrationDate,
										t.TransactionItemPriceQuantityAmount, 
										t.TransactionItemPricePriceUnitId,
										t.TransactionItemPriceFee,
										t.TransactionItemPriceMainCurrencyUnitId, 
										t.TransactionItemPriceExchangeRate,
										t.TransactionItemPriceQuantityAmountUseFIFO
								FROM ##TempIssuePriced t
								ORDER BY
									   t.TransactionRegistrationDate DESC,
									   t.TransactionId DESC,
									   t.TransactionItemId DESC,
									   t.TransactionItemRowVersion DESC,
									   t.TransactionItemPriceRegistrationDate DESC,
									   t.TransactionItemPriceId DESC,
									   t.TransactionItemPriceRowVersion DESC --بصورت LIFO قيمت گذاري برگشتي انجام مي شود
							OPEN AssignIssuePrice
								FETCH NEXT FROM AssignIssuePrice INTO   @TransactionId, 
																		@Action,
																		@TransactionRegistrationDate,
																		@TransactionItemId,
																		@TransactionItemRowVersion, 
																		@TransactionItemQuantityAmount,
																		@TransactionItemPriceId,
																		@TransactionItemPriceRowVersion, 
																		@TransactionItemPriceRegistrationDate,
																		@TransactionItemPriceQuantityAmount, 
																		@TransactionItemPricePriceUnitId,
																		@TransactionItemPriceFee,
																		@TransactionItemPriceMainCurrencyUnitId, 
																		@TransactionItemPriceExchangeRate,
																		@TransactionItemPriceQuantityAmountUseFIFO
								WHILE @@Fetch_Status = 0  
								BEGIN
									--به گفته جناب هاتفي(خانم رضاييان)در تاريخ 1393-04-03
									--تمام مقادير Correction+
									-- با في يک آرتيکل از مرجع انتخاب شده قيمت گذاري گردد و کاري با ميزان و موجودي آن مرجع نداريم
									
									--1393-12-07.1 : Pricing by reference does not repect the possible providing amount of origin for pricing.
									/*
									SET @StoreTypesCode=(SELECT TOP(1) Code FROM [Inventory].StoreTypes st WHERE st.Id=@StoreTypesId)
									IF (@StoreTypesCode=7 AND @ActionType=1)
									OR (@StoreTypesCode=13 AND @ActionType=2)
									BEGIN
										SET @CurrentQuantityAmount=@QuantityAmount
									END
									ELSE
									BEGIN
										-- روند عادي قيمت گذاري از روي مرجع و سطر به سطر بر اساس تعداد و قيمت مرجع
										SET @CurrentQuantityAmount= @TransactionItemPriceQuantityAmount-@TransactionItemPriceQuantityAmountUseFIFO;
										IF @CurrentQuantityAmount>(@QuantityAmount)
										BEGIN
											SET @CurrentQuantityAmount=@QuantityAmount
										END	
									END
									*/
									
									--1393-12-07.1 : , hence the amount of pricing is the same as current processing amount.
									SET @CurrentQuantityAmount=@QuantityAmount

									SET @RowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@IssueItemId),0)+1	
									
									INSERT INTO [Inventory].TransactionItemPrices
									(
										-- Id -- this column value is auto-generated
										RowVersion,
										TransactionId,
										TransactionItemId,
										[Description],
										QuantityUnitId,
										QuantityAmount,
										PriceUnitId,
										Fee,
										MainCurrencyUnitId,
										FeeInMainCurrency,
										RegistrationDate,
										--QuantityAmountUseFIFO,
										TransactionReferenceId,
										UserCreatorId,
										CreateDate
									)
									VALUES
									(
										@RowVersion ,
										@IssueId ,
										@IssueItemId ,
										@Description,
										@QuantityUnitId ,
										@CurrentQuantityAmount,
										@TransactionItemPricePriceUnitId,
										@TransactionItemPriceFee,
										@TransactionItemPriceMainCurrencyUnitId,
										@TransactionItemPriceExchangeRate,
										ISNULL(@EffectiveDateTime, @RegistrationDate),
										/*{ QuantityAmountUseFIFO }*/
										@TransactionItemPriceId,
										@UserCreatorId ,
										getdate()
									)
									SET @TransactionItemPriceIds+=cast(@@identity AS NVARCHAR(15))+N';'

									UPDATE [Inventory].TransactionItemPrices
									SET
										--1393-12-07.1 : This is commented by Hatefi because there is no need to update the provided amount of origin for pricing other transactions.
										--QuantityAmountUseFIFO += CASE WHEN (@StoreTypesCode=7 OR @StoreTypesCode=13) THEN 0 ELSE @CurrentQuantityAmount END,
										--QuantityAmountUseFIFO += CASE WHEN @StoreTypesCode IN (27) THEN @CurrentQuantityAmount ELSE 0 END, --این خط کد، جهت کم کردن میزان قیمت برداشته شده در زمان ثبت قیمت "حواله تعدیل موجودی"، اضافه شده بود که به دلیل قیمت گذاری با مرجع، حذف می گردد 
										IssueReferenceIds = isnull (IssueReferenceIds,N'')+cast(@@identity AS NVARCHAR(15))+N';'
									WHERE Id=@TransactionItemPriceId
									
									SET @QuantityAmount-=@CurrentQuantityAmount
									IF @QuantityAmount=0
									   BREAK;
         							FETCH AssignIssuePrice INTO @TransactionId, 
																@Action, 
																@TransactionRegistrationDate,
																@TransactionItemId,
																@TransactionItemRowVersion, 
																@TransactionItemQuantityAmount,
																@TransactionItemPriceId,
																@TransactionItemPriceRowVersion, 
																@TransactionItemPriceRegistrationDate,
																@TransactionItemPriceQuantityAmount, 
																@TransactionItemPricePriceUnitId,
																@TransactionItemPriceFee,
																@TransactionItemPriceMainCurrencyUnitId, 
																@TransactionItemPriceExchangeRate,
																@TransactionItemPriceQuantityAmountUseFIFO
								END
								
						END
				FETCH CursorPriceSuspendedTransactionUsingReference INTO  @ActionType,
																 @GoodId,
																 @QuantityUnitId,
																 @QuantityAmount,
															     @TimeBucketId,
															     @PricingReferenceId,
															     @RegistrationDate,
															     @IssueItemRowVersion,
															     @IssueItemId,
															     @IssueId,
															     @WarehouseId,
															     @StoreTypesId
		 END
			 --IF @QuantityAmount<>0
				--	RAISERROR(N'@موجودي رسيدهاي با قيمت براي قيمت گزاري حواله کافي نيست',16,1,'500')
		END 
		SET @Message=N'OperationSuccessful'
		IF @TRANSACTIONCOUNT = 0
				COMMIT TRANSACTION 					
		END try
		BEGIN CATCH
				IF (XACT_STATE()) = -1 
					ROLLBACK TRANSACTION
				IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
					ROLLBACK TRANSACTION
				IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
					ROLLBACK TRANSACTION CurrentTransaction
				 SET @Message=ERROR_MESSAGE();
				SET @TransactionItemPriceIds=NULL
			EXEC [Inventory].ErrorHandling
		END CATCH
	END
GO	
GRANT EXECUTE ON [Inventory].PriceSuspendedTransactionUsingReference TO [public] AS [dbo]
GO

----------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].[TransactionsGetAll]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Inventory].[TransactionsGetAll];
GO
CREATE PROCEDURE [Inventory].[TransactionsGetAll]

--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	SELECT t.Id, t.[Action], t.Code, t.[Description], t.WarehouseId, --t.CrossId,
	       t.StoreTypesId, t.TimeBucketId, t.[Status], t.RegistrationDate,
	       t.SenderReciver, t.HardCopyNo, t.ReferenceType, t.ReferenceNo,
	       t.ReferenceDate, t.UserCreatorId, t.CreateDate,
	       st.Code AS StoreTypeCode, st.InputName AS StoreTypeInputName, st.OutputName AS StoreTypeOutputName,
	       w.Code AS WarehouseCode, w.Name AS WarehouseName, w.CompanyId, w.[IsActive] AS WarehouseStatus,
	       c.Code AS CompanyCode, c.Name AS CompanyName, c.[IsActive] AS CompanyStatus
	FROM [Inventory].Transactions t
	INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
	INNER JOIN [Inventory].Warehouse w ON w.Id = t.WarehouseId
	INNER JOIN [Inventory].Companies c ON c.Id = w.CompanyId
END
GO
GRANT EXECUTE ON [Inventory].[TransactionsGetAll] TO [public] AS [dbo]
GO

----------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].[TransactionItemsGetAll]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Inventory].[TransactionItemsGetAll];
GO
CREATE PROCEDURE [Inventory].[TransactionItemsGetAll]

--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ti.Id AS tiId, ti.RowVersion AS tiRowVersion, ti.GoodId AS tiGoodId, ti.QuantityUnitId AS tiQuantityUnitId,
	       ti.QuantityAmount AS tiQuantityAmount, ti.[Description] AS tiDescription, 
	       ti.UserCreatorId AS tiUserCreatorId, ti.CreateDate AS tiCreateDate,
	       ti.TransactionId AS tId, t.[Action] AS tAction, t.Code AS tCode, t.[Description] AS tDescription, t.PricingReferenceId AS tPricingReferenceId,--t.CrossId AS tCrossId, 
	       t.WarehouseId AS tWarehouseId,t.StoreTypesId AS tStoreTypesId, t.TimeBucketId AS tTimeBucketId, t.[Status] AS tStatus,
	       t.RegistrationDate AS tRegistrationDate,t.SenderReciver AS tSenderReciver, t.HardCopyNo AS tHardCopyNo, 
	       t.ReferenceType AS tReferenceType, t.ReferenceNo AS tReferenceNo,t.ReferenceDate AS tReferenceDate, 
	       t.UserCreatorId AS tUserCreatorId, t.CreateDate AS tCreateDate,
	       st.Code AS StoreTypeCode, st.InputName AS StoreTypeInputName, st.OutputName AS StoreTypeOutputName,
	       w.Code AS WarehouseCode, w.Name AS WarehouseName, w.CompanyId, w.[IsActive] AS WarehouseStatus,
	       c.Code AS CompanyCode, c.Name AS CompanyName, c.[IsActive] AS CompanyStatus,
	       g.Code AS GoodCode, g.Name AS GoodName, g.[IsActive] AS GoodStatus,
	       u.Abbreviation AS UnitAbbreviation, u.Name AS UnitName, u.IsCurrency AS UnitIsCurrency, u.IsBaseCurrency AS UnitIsBaseCurrency, u.[IsActive] AS UnitStatus
	FROM [Inventory].TransactionItems ti 
	INNER JOIN [Inventory].Transactions t ON t.Id = ti.TransactionId
	INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
	INNER JOIN [Inventory].Warehouse w ON w.Id = t.WarehouseId
	INNER JOIN [Inventory].Companies c ON c.Id = w.CompanyId
	INNER JOIN [Inventory].Goods g ON g.Id = ti.GoodId
	INNER JOIN [Inventory].Units u ON u.Id = ti.QuantityUnitId
END
GO
GRANT EXECUTE ON [Inventory].[TransactionItemsGetAll] TO [public] AS [dbo]
GO

----------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].[TransactionItemPricesGetAll]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Inventory].[TransactionItemPricesGetAll];
GO
CREATE PROCEDURE [Inventory].[TransactionItemPricesGetAll]

--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	SELECT tip.Id AS tipId, tip.RowVersion AS tipRowVersion, tip.[Description] AS tipDescription, tip.QuantityUnitId AS tipQuantityUnitId,
	       tip.QuantityAmount AS tipQuantityAmount, tip.PriceUnitId AS tipPriceUnitId, tip.Fee AS tipFee, tip.MainCurrencyUnitId AS tipMainCurrencyUnitId,
	       tip.FeeInMainCurrency AS tipFeeInMainCurrency, tip.RegistrationDate AS tipRegistrationDate,
	       tip.QuantityAmountUseFIFO AS tipQuantityAmountUseFIFO, tip.TransactionReferenceId AS tipTransactionReferenceId,
	       tip.IssueReferenceIds AS tipIssueReferenceIds, tip.UserCreatorId AS tipUserCreatorId, tip.CreateDate AS tipCreateDate ,
	       
		   ti.Id AS tiId, ti.RowVersion AS tiRowVersion, ti.GoodId AS tiGoodId, ti.QuantityUnitId AS tiQuantityUnitId,
	       ti.QuantityAmount AS tiQuantityAmount, ti.[Description] AS tiDescription, 
	       ti.UserCreatorId AS tiUserCreatorId, ti.CreateDate AS tiCreateDate,
	       ti.TransactionId AS tId, t.[Action] AS tAction, t.Code AS tCode, t.[Description] AS tDescription,t.PricingReferenceId AS tPricingReferenceId, --t.CrossId AS tCrossId, 
	       t.WarehouseId AS tWarehouseId,t.StoreTypesId AS tStoreTypesId, t.TimeBucketId AS tTimeBucketId, t.[Status] AS tStatus,
	       t.RegistrationDate AS tRegistrationDate,t.SenderReciver AS tSenderReciver, t.HardCopyNo AS tHardCopyNo, 
	       t.ReferenceType AS tReferenceType, t.ReferenceNo AS tReferenceNo,t.ReferenceDate AS tReferenceDate, 
	       t.UserCreatorId AS tUserCreatorId, t.CreateDate AS tCreateDate,
	       
	       st.Code AS StoreTypeCode, st.InputName AS StoreTypeInputName, st.OutputName AS StoreTypeOutputName,
	       w.Code AS WarehouseCode, w.Name AS WarehouseName, w.CompanyId, w.[IsActive] AS WarehouseStatus,
	       c.Code AS CompanyCode, c.Name AS CompanyName, c.[IsActive] AS CompanyStatus,
	       g.Code AS GoodCode, g.Name AS GoodName, g.[IsActive] AS GoodStatus,
	       u.Abbreviation AS QuantityUnitAbbreviation, u.Name AS QuantityUnitName, u.[IsActive] AS QuantityUnitStatus,
	       u2.Abbreviation AS PriceUnitAbbreviation, u2.Name AS PriceUnitName, u2.[IsActive] AS PriceUnitStatus,
	       u3.Abbreviation AS MainCurrencyAbbreviation, u3.Name AS MainCurrencyName, u3.[IsActive] AS BaseUnitStatus
	FROM [Inventory].TransactionItemPrices tip
	INNER JOIN [Inventory].TransactionItems ti ON ti.Id = tip.TransactionItemId
	INNER JOIN [Inventory].Transactions t ON t.Id = ti.TransactionId
	INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
	INNER JOIN [Inventory].Warehouse w ON w.Id = t.WarehouseId
	INNER JOIN [Inventory].Companies c ON c.Id = w.CompanyId
	INNER JOIN [Inventory].Goods g ON g.Id = ti.GoodId
	LEFT JOIN [Inventory].Units u ON u.Id = tip.QuantityUnitId
	LEFT JOIN [Inventory].Units u2 ON u2.Id = tip.PriceUnitId AND u2.IsCurrency=1
	LEFT JOIN [Inventory].Units u3 ON u3.Id = tip.MainCurrencyUnitId AND u3.IsCurrency=1
END
GO
GRANT EXECUTE ON [Inventory].[TransactionItemPricesGetAll] TO [public] AS [dbo]
GO

----------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].[Cardex]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Inventory].[Cardex];
GO
CREATE PROCEDURE [Inventory].[Cardex]
(
	@WarehouseId BIGINT,
	@GoodId BIGINT,
	@StartDate DATETIME,
	@EndDate DATETIME
)
--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @StartDateTimeBucket DATETIME,
	        @EndDatePeriod DATETIME
	SET @StartDateTimeBucket =(SELECT TOP(1)tb.StartDate FROM [Inventory].TimeBucket tb WHERE (@StartDate BETWEEN tb.StartDate AND tb.EndDate))
	SET @EndDatePeriod = DATEADD(millisecond,-1,@StartDate)
	
	SET @EndDate=DATEADD(millisecond,1,@EndDate)
	
	DECLARE @Cardex TABLE
	(
	   RegistrationDate DATETIME,
	   Description NVARCHAR(MAX),
	   Code DECIMAL(20,2),
	   Action TINYINT,
	   SignAction SMALLINT,
	   QuantityUnitId BIGINT,
	   QuantityUnitAbbreviation NVARCHAR(512),
	   QuantityUnitName NVARCHAR(256),
	   PriceUnitId BIGINT,
	   PriceUnitAbbreviation NVARCHAR(512), 
	   PriceUnitName NVARCHAR(256),
	   MainCurrencyUnitId BIGINT,
	   MainCurrencyAbbreviation NVARCHAR(512), 
	   MainCurrencyName NVARCHAR(256),
	   FeeInMainCurrency DECIMAL(20,3),
	   QuantityAmount DECIMAL(20,3),
	   QuantityAmountPriced DECIMAL(20,3),
	   TotalPrice DECIMAL(20,3),
	   VoyageNumber NVARCHAR(256)
	)
	
	INSERT INTO @Cardex
	(
		RegistrationDate,
		Description,
		Code,
		Action,
		SignAction,
		QuantityUnitId,
		QuantityUnitAbbreviation,
		QuantityUnitName,
		PriceUnitId,
		PriceUnitAbbreviation,
		PriceUnitName,
		MainCurrencyUnitId,
		MainCurrencyAbbreviation,
		MainCurrencyName,
		FeeInMainCurrency,
		QuantityAmount,
		QuantityAmountPriced,
		TotalPrice,
		VoyageNumber 
	)
	SELECT NULL,/*{ @RegistrationDate }*/
		   NULL,/*{ @Description }*/
		   NULL,/*{ @Code }*/
		   NULL,/*{ @Action }*/
		   NULL,/*{ @SignAction }*/
		   tip.QuantityUnitId,
		   u.Abbreviation AS QuantityUnitAbbreviation, 
		   u.Name AS QuantityUnitName, 
		   tip.PriceUnitId, 
		   u2.Abbreviation AS PriceUnitAbbreviation, 
		   u2.Name AS PriceUnitName,
		   tip.MainCurrencyUnitId, 
		   u3.Abbreviation AS MainCurrencyAbbreviation, 
		   u3.Name AS MainCurrencyName,
		   NULL AS FeeInMainCurrency,
	       SUM(ISNULL(ti.QuantityAmount*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END),0)) AS QuantityAmount,
		   SUM(ISNULL(tip.QuantityAmount*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END),0)) AS QuantityAmountPriced,
	       SUM(ISNULL(tip.FeeInMainCurrency,0)*ISNULL(tip.QuantityAmount,0)*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END)) AS TotalPrice,
	       NULL
	FROM [Inventory].TransactionItemPrices tip
	INNER JOIN [Inventory].TransactionItems ti ON ti.Id = tip.TransactionItemId
	INNER JOIN [Inventory].Transactions t ON t.Id = ti.TransactionId
	INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
	LEFT JOIN [Inventory].Units u ON u.Id = tip.QuantityUnitId
	LEFT JOIN [Inventory].Units u2 ON u2.Id = tip.PriceUnitId AND u2.IsCurrency=1
	LEFT JOIN [Inventory].Units u3 ON u3.Id = tip.MainCurrencyUnitId AND u3.IsCurrency=1
	WHERE t.WarehouseId=@WarehouseId
			AND (t.RegistrationDate BETWEEN @StartDateTimeBucket AND @EndDatePeriod)
			AND ti.GoodId=@GoodId
	GROUP BY 
	         tip.QuantityUnitId,u.Abbreviation , u.Name , 
		     tip.PriceUnitId, u2.Abbreviation, u2.Name,
		     tip.MainCurrencyUnitId, u3.Abbreviation, u3.Name;
	
	UPDATE @Cardex
	SET
		RegistrationDate = @EndDatePeriod,
		[Description] = N'اول دوره',
		Code = 0,
		[Action] = CASE WHEN QuantityAmount >= 0 THEN 1 ELSE 2 END,
		SignAction =  CASE WHEN QuantityAmount >= 0 THEN 1 WHEN [Action] = 2 THEN -1 END,
		FeeInMainCurrency=TotalPrice/QuantityAmount
	;	 
	
	set @StartDate = ISNULL(@StartDate, '2000-01-01')
	
	INSERT INTO @Cardex
	(
		RegistrationDate,
		Description,
		Code,
		Action,
		SignAction,
		QuantityUnitId,
		QuantityUnitAbbreviation,
		QuantityUnitName,
		PriceUnitId,
		PriceUnitAbbreviation,
		PriceUnitName,
		MainCurrencyUnitId,
		MainCurrencyAbbreviation,
		MainCurrencyName,
		FeeInMainCurrency,
		QuantityAmount,
		QuantityAmountPriced,
		TotalPrice,
		VoyageNumber
	)-- t.[Description],
	SELECT t.RegistrationDate,st.InputName,t.Code,t.[Action],CASE WHEN t.[Action] = 1 THEN 1 WHEN t.[Action] = 2 THEN -1 END AS SignAction ,
		   tip.QuantityUnitId,u.Abbreviation AS QuantityUnitAbbreviation, u.Name AS QuantityUnitName, 
		   tip.PriceUnitId, u2.Abbreviation AS PriceUnitAbbreviation, u2.Name AS PriceUnitName,
		   tip.MainCurrencyUnitId, u3.Abbreviation AS MainCurrencyAbbreviation, u3.Name AS MainCurrencyName,
		   tip.FeeInMainCurrency AS FeeInMainCurrency,
		   SUM(ISNULL(ti.QuantityAmount,0)) AS QuantityAmount,
	       SUM(ISNULL(tip.QuantityAmount,0)) AS QuantityAmountPriced,
	       SUM(ISNULL(tip.FeeInMainCurrency,0)*ISNULL(tip.QuantityAmount,0)) AS TotalPrice,
			(SELECT TOP 1 voy.VoyageNumber FROM  [Fuel].[Voyage] voy
				INNER JOIN [Inventory].Warehouse w ON w.Id = @WarehouseId
				INNER JOIN [Fuel].[Vessel] v ON v.Code = w.Code 
				INNER JOIN [Fuel].[VesselInCompany] vic ON vic.CompanyId = w.CompanyId AND vic.VesselId = v.Id
			WHERE voy.VesselInCompanyId = vic.Id AND voy.IsActive = 1 AND t.RegistrationDate BETWEEN voy.StartDate AND ISNULL(voy.EndDate, GETDATE())
			ORDER BY voy.EndDate DESC) AS VoyageNumber
	FROM [Inventory].TransactionItemPrices tip
	INNER JOIN [Inventory].TransactionItems ti ON ti.Id = tip.TransactionItemId
	INNER JOIN [Inventory].Transactions t ON t.Id = ti.TransactionId
	INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
	LEFT JOIN [Inventory].Units u ON u.Id = tip.QuantityUnitId
	LEFT JOIN [Inventory].Units u2 ON u2.Id = tip.PriceUnitId AND u2.IsCurrency=1
	LEFT JOIN [Inventory].Units u3 ON u3.Id = tip.MainCurrencyUnitId AND u3.IsCurrency=1
	WHERE t.WarehouseId=@WarehouseId
			AND (t.RegistrationDate BETWEEN @StartDate AND @EndDate)
			AND ti.GoodId=@GoodId
	GROUP BY tip.Id	,t.RegistrationDate, st.InputName,t.Code,t.[Action],CASE WHEN t.[Action] = 1 THEN 1 WHEN t.[Action] = 2 THEN -1 END,
	         tip.QuantityUnitId,u.Abbreviation , u.Name , 
		     tip.PriceUnitId, u2.Abbreviation, u2.Name,
		     tip.MainCurrencyUnitId, u3.Abbreviation, u3.Name,
		     tip.FeeInMainCurrency
		

	--IF(NOT EXISTS(SELECT * FROM @Cardex WHERE [Action] = 1))
	--	INSERT INTO @Cardex (RegistrationDate, [Description], Code, [Action], SignAction,
	--       QuantityUnitId, QuantityUnitAbbreviation, QuantityUnitName, PriceUnitId,
	--       PriceUnitAbbreviation, PriceUnitName, MainCurrencyUnitId, MainCurrencyAbbreviation,
	--       MainCurrencyName, QuantityAmount, QuantityAmountPriced, FeeInMainCurrency, TotalPrice)
	--	   VALUES ((SELECT TOP 1 RegistrationDate FROM @Cardex) , '', NULL, 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);

	--IF(NOT EXISTS(SELECT * FROM @Cardex WHERE [Action] = 2))
	--	INSERT INTO @Cardex (RegistrationDate, [Description], Code, [Action], SignAction,
	--       QuantityUnitId, QuantityUnitAbbreviation, QuantityUnitName, PriceUnitId,
	--       PriceUnitAbbreviation, PriceUnitName, MainCurrencyUnitId, MainCurrencyAbbreviation,
	--       MainCurrencyName, QuantityAmount, QuantityAmountPriced, FeeInMainCurrency, TotalPrice)
	--	   VALUES ((SELECT TOP 1 RegistrationDate FROM @Cardex), '', NULL, 2, -1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL);
		        
	SELECT c.RegistrationDate, c.[Description], c.Code, c.[Action], c.SignAction,
	       c.QuantityUnitId, c.QuantityUnitAbbreviation, c.QuantityUnitName, c.PriceUnitId,
	       c.PriceUnitAbbreviation, c.PriceUnitName, c.MainCurrencyUnitId, c.MainCurrencyAbbreviation,
	       c.MainCurrencyName, c.QuantityAmount, 
		   --Changed to Sum aggregation by HATEFI
		   SUM(c.QuantityAmountPriced) AS QuantityAmountPriced, c.FeeInMainCurrency, SUM(c.TotalPrice) AS TotalPrice,
		   c.VoyageNumber --Added by Hatefi 95-01-31
	FROM @Cardex c
	GROUP BY c.RegistrationDate, c.[Description], c.Code, c.[Action], c.SignAction,
	       c.QuantityUnitId, c.QuantityUnitAbbreviation, c.QuantityUnitName, c.PriceUnitId,
	       c.PriceUnitAbbreviation, c.PriceUnitName, c.MainCurrencyUnitId, c.MainCurrencyAbbreviation,
	       c.MainCurrencyName, c.QuantityAmount, c.FeeInMainCurrency,
	       c.VoyageNumber
	ORDER BY  c.RegistrationDate,
			  c.Action,
			  c.Code
			  
	DELETE FROM @Cardex
END
GO
GRANT EXECUTE ON [Inventory].[Cardex] TO [public] AS [dbo]
GO

----------------------------------------------------------------------
--Added by Hatefi 95-01-31
IF OBJECT_ID ( '[Inventory].[InventoryCardex]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Inventory].[InventoryCardex];
GO
CREATE PROCEDURE [Inventory].[InventoryCardex]
(
	@WarehouseId BIGINT,
	@GoodId BIGINT,
	@StartDate DATETIME,
	@EndDate DATETIME
)
--WITH ENCRYPTION
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @StartDateTimeBucket DATETIME,
	        @EndDatePeriod DATETIME
	SET @StartDateTimeBucket =(SELECT TOP(1)tb.StartDate FROM [Inventory].TimeBucket tb WHERE (@StartDate BETWEEN tb.StartDate AND tb.EndDate))
	SET @EndDatePeriod = DATEADD(millisecond,-1,@StartDate)
	
	SET @EndDate=DATEADD(millisecond,1,@EndDate)
	
	DECLARE @Cardex TABLE
	(
	   RegistrationDate DATETIME,
	   Description NVARCHAR(MAX),
	   Code DECIMAL(20,2),
	   Action TINYINT,
	   SignAction SMALLINT,
	   QuantityUnitId BIGINT,
	   QuantityUnitAbbreviation NVARCHAR(512),
	   QuantityUnitName NVARCHAR(256),
	   --PriceUnitId BIGINT,
	   --PriceUnitAbbreviation NVARCHAR(512), 
	   --PriceUnitName NVARCHAR(256),
	   --MainCurrencyUnitId BIGINT,
	   --MainCurrencyAbbreviation NVARCHAR(512), 
	   --MainCurrencyName NVARCHAR(256),
	   --FeeInMainCurrency DECIMAL(20,3),
	   QuantityAmount DECIMAL(20,3)/*,
	   QuantityAmountPriced DECIMAL(20,3),
	   TotalPrice DECIMAL(20,3)*/,
	   VoyageNumber NVARCHAR(256),
	   PricingStatus NVARCHAR(256)
	)
	
	INSERT INTO @Cardex
	(
		RegistrationDate,
		Description,
		Code,
		Action,
		SignAction,
		QuantityUnitId,
		QuantityUnitAbbreviation,
		QuantityUnitName,
		--PriceUnitId,
		--PriceUnitAbbreviation,
		--PriceUnitName,
		--MainCurrencyUnitId,
		--MainCurrencyAbbreviation,
		--MainCurrencyName,
		--FeeInMainCurrency,
		QuantityAmount/*,
		QuantityAmountPriced,
		TotalPrice*/,
		VoyageNumber,
		PricingStatus
	)
	SELECT NULL,/*{ @RegistrationDate }*/
		   NULL,/*{ @Description }*/
		   NULL,/*{ @Code }*/
		   NULL,/*{ @Action }*/
		   NULL,/*{ @SignAction }*/
		   ti.QuantityUnitId,
		   u.Abbreviation AS QuantityUnitAbbreviation, 
		   u.Name AS QuantityUnitName, 
		   --tip.PriceUnitId, 
		   --u2.Abbreviation AS PriceUnitAbbreviation, 
		   --u2.Name AS PriceUnitName,
		   --tip.MainCurrencyUnitId, 
		   --u3.Abbreviation AS MainCurrencyAbbreviation, 
		   --u3.Name AS MainCurrencyName,
		   --NULL AS FeeInMainCurrency,
	       SUM(ISNULL(ti.QuantityAmount*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END),0)) AS QuantityAmount/*,
		   SUM(ISNULL(tip.QuantityAmount*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END),0)) AS QuantityAmountPriced,
	       SUM(ISNULL(tip.FeeInMainCurrency,0)*ISNULL(tip.QuantityAmount,0)*(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END)) AS TotalPrice*/,
		   NULL,
		   NULL
	FROM /*[Inventory].TransactionItemPrices tip
	INNER JOIN*/ [Inventory].TransactionItems ti --ON ti.Id = tip.TransactionItemId
	INNER JOIN [Inventory].Transactions t ON t.Id = ti.TransactionId
	INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
	LEFT JOIN [Inventory].Units u ON u.Id = ti.QuantityUnitId
	--LEFT JOIN [Inventory].Units u2 ON u2.Id = tip.PriceUnitId AND u2.IsCurrency=1
	--LEFT JOIN [Inventory].Units u3 ON u3.Id = tip.MainCurrencyUnitId AND u3.IsCurrency=1
	WHERE t.WarehouseId=@WarehouseId
			AND (t.RegistrationDate BETWEEN @StartDateTimeBucket AND @EndDatePeriod)
			AND ti.GoodId=@GoodId
	GROUP BY 
	         ti.QuantityUnitId,u.Abbreviation , u.Name /*, 
		     tip.PriceUnitId, u2.Abbreviation, u2.Name,
		     tip.MainCurrencyUnitId, u3.Abbreviation, u3.Name*/;
	
	UPDATE @Cardex
	SET
		RegistrationDate = @EndDatePeriod,
		[Description] = N'اول دوره',
		Code = 0,
		[Action] = CASE WHEN QuantityAmount >= 0 THEN 1 ELSE 2 END,
		SignAction =  CASE WHEN QuantityAmount >= 0 THEN 1 WHEN [Action] = 2 THEN -1 END/*,
		FeeInMainCurrency=TotalPrice/QuantityAmount*/
	;	 
	
	set @StartDate = ISNULL(@StartDate, '2000-01-01')
	
	INSERT INTO @Cardex
	(
		RegistrationDate,
		Description,
		Code,
		Action,
		SignAction,
		QuantityUnitId,
		QuantityUnitAbbreviation,
		QuantityUnitName,
		--PriceUnitId,
		--PriceUnitAbbreviation,
		--PriceUnitName,
		--MainCurrencyUnitId,
		--MainCurrencyAbbreviation,
		--MainCurrencyName,
		--FeeInMainCurrency,
		QuantityAmount/*,
		QuantityAmountPriced,
		TotalPrice*/,
		VoyageNumber,
		PricingStatus
	)
	SELECT t.RegistrationDate,st.InputName,t.Code,t.[Action],CASE WHEN t.[Action] = 1 THEN 1 WHEN t.[Action] = 2 THEN -1 END AS SignAction,
		   ti.QuantityUnitId,u.Abbreviation AS QuantityUnitAbbreviation, u.Name AS QuantityUnitName, 
		   --tip.PriceUnitId, u2.Abbreviation AS PriceUnitAbbreviation, u2.Name AS PriceUnitName,
		   --tip.MainCurrencyUnitId, u3.Abbreviation AS MainCurrencyAbbreviation, u3.Name AS MainCurrencyName,
		   --tip.FeeInMainCurrency AS FeeInMainCurrency,
		   SUM(ISNULL(ti.QuantityAmount,0)) AS QuantityAmount/*,
	       SUM(ISNULL(tip.QuantityAmount,0)) AS QuantityAmountPriced,
	       SUM(ISNULL(tip.FeeInMainCurrency,0)*ISNULL(tip.QuantityAmount,0)) AS TotalPrice*/,
			(SELECT TOP 1 voy.VoyageNumber FROM  [Fuel].[Voyage] voy
				INNER JOIN [Inventory].Warehouse w ON w.Id = @WarehouseId
				INNER JOIN [Fuel].[Vessel] v ON v.Code = w.Code 
				INNER JOIN [Fuel].[VesselInCompany] vic ON vic.CompanyId = w.CompanyId AND vic.VesselId = v.Id
			WHERE voy.VesselInCompanyId = vic.Id AND voy.IsActive = 1 AND t.RegistrationDate BETWEEN voy.StartDate AND ISNULL(voy.EndDate, GETDATE())
			ORDER BY voy.StartDate ASC) AS VoyageNumber,
		   CASE WHEN t.[Status] IN (3 , 4) THEN 'Full Priced' ELSE 'Not Priced' END AS PricingStatus
	FROM /*[Inventory].TransactionItemPrices tip
	INNER JOIN */[Inventory].TransactionItems ti --ON ti.Id = tip.TransactionItemId
	INNER JOIN [Inventory].Transactions t ON t.Id = ti.TransactionId
	INNER JOIN [Inventory].StoreTypes st ON st.Id = t.StoreTypesId
	--INNER JOIN [Inventory].Warehouse w ON w.Id = t.WarehouseId
	--INNER JOIN [Inventory].Companies c ON c.Id = w.CompanyId
	--INNER JOIN [Inventory].Goods g ON g.Id = ti.GoodId
	LEFT JOIN [Inventory].Units u ON u.Id = ti.QuantityUnitId
	--LEFT JOIN [Inventory].Units u2 ON u2.Id = tip.PriceUnitId AND u2.IsCurrency=1
	--LEFT JOIN [Inventory].Units u3 ON u3.Id = tip.MainCurrencyUnitId AND u3.IsCurrency=1
	WHERE t.WarehouseId=@WarehouseId
			AND (t.RegistrationDate BETWEEN @StartDate AND @EndDate)
			AND ti.GoodId=@GoodId
	GROUP BY /*tip.Id,*/ t.RegistrationDate, st.InputName,t.Code,t.[Action],CASE WHEN t.[Action] = 1 THEN 1 WHEN t.[Action] = 2 THEN -1 END,
	         ti.QuantityUnitId,u.Abbreviation , u.Name /*, 
		     tip.PriceUnitId, u2.Abbreviation, u2.Name,
		     tip.MainCurrencyUnitId, u3.Abbreviation, u3.Name,
		     tip.FeeInMainCurrency*/,
		     t.[Status]
		 
	SELECT c.RegistrationDate, c.[Description], c.Code, c.[Action], c.SignAction,
	       c.QuantityUnitId, c.QuantityUnitAbbreviation, c.QuantityUnitName, /*c.PriceUnitId,
	       c.PriceUnitAbbreviation, c.PriceUnitName, c.MainCurrencyUnitId, c.MainCurrencyAbbreviation,
	       c.MainCurrencyName, */c.QuantityAmount/*, 
		   --Changed to Sum aggregation by HATEFI
		   SUM(c.QuantityAmountPriced) AS QuantityAmountPriced, c.FeeInMainCurrency, SUM(c.TotalPrice) AS TotalPrice*/,
		   c.VoyageNumber,
		   c.PricingStatus
	FROM @Cardex c
	GROUP BY c.RegistrationDate, c.[Description], c.Code, c.[Action], c.SignAction,
	       c.QuantityUnitId, c.QuantityUnitAbbreviation, c.QuantityUnitName, /*c.PriceUnitId,
	       c.PriceUnitAbbreviation, c.PriceUnitName, c.MainCurrencyUnitId, c.MainCurrencyAbbreviation,
	       c.MainCurrencyName,*/ c.QuantityAmount/*, c.FeeInMainCurrency*/,
	       c.VoyageNumber,
		   c.PricingStatus
	ORDER BY  c.RegistrationDate,
			  c.Action,
			  c.Code
			  
	DELETE FROM @Cardex
END
GO
GRANT EXECUTE ON [Inventory].[InventoryCardex] TO [public] AS [dbo]
GO

----------------------------------------------------------------------------------
if OBJECT_ID('[Inventory].ActivateWarehouseIncludingRecieptsOperation','P') is Not Null
	drop procedure [Inventory].ActivateWarehouseIncludingRecieptsOperation;
Go
Create Procedure [Inventory].ActivateWarehouseIncludingRecieptsOperation
(
@Description nvarchar(max)=NULL,
@WarehouseId BIGINT,--انبار
@TimeBucketId INT,
@StoreTypesId INT, 
@RegistrationDate DATETIME=NULL,--تاريخ ثبت
@ReferenceType NVARCHAR(100)=NULL,--نوع مرجع
@ReferenceNo NVARCHAR(100)=NULL,--شماره مرجع
@TransactionItems TypeTransactionItemWithPrice READONLY,-- Id , GoodId, QuantityUnitId, QuantityAmount, PriceUnitId, Fee, [Description]
@UserCreatorId INT

)
--WITH ENCRYPTION
AS 
BEGIN
	set nocount ON;
	DECLARE @Code decimal(20,2) ,
			@Message NVARCHAR(MAX),
			@MSG NVARCHAR(MAX),
			@TransactionId  int,
			@tbId INT,
			@TransactionItemsId NVARCHAR(256),
			@MessageItem NVARCHAR(MAX),
			@TransactionItemPriceIds NVARCHAR(MAX),
			@MessageItemPrice NVARCHAR(MAX),
			@Id int ,
			@GoodId INT,
			@QuantityUnitId INT,
			@QuantityAmount DECIMAL(20,3),
			@PriceUnitId INT ,
			@Fee DECIMAL(20,3) = 0,
			@TransactionItemId INT ,
			@RowVersion SMALLINT = 0,
			@StoreTypeCode SMALLINT = 0,
			@MainCurrencyUnitId BIGINT ,
			@FeeInMainCurrency DECIMAL(20,3) =0,
			@PrimaryCoefficient TINYINT=2,
			@Coefficient DECIMAL(18,3) =0,
			@IsActive BIT,
			@CompanyId BIGINT
		
		
		
		SET @IsActive=1;
				
	DECLARE @TRANSACTIONCOUNT INT;
		SET @TRANSACTIONCOUNT = @@TRANCOUNT;
	BEGIN TRY
		IF @TRANSACTIONCOUNT = 0
			BEGIN TRANSACTION
		--ELSE
		--	SAVE TRANSACTION CurrentTransaction;
			
			IF @RegistrationDate IS NULL 
				BEGIN
						RAISERROR(N'@Invalid date in TransactionOperation',16,1,'500')
				END
				
				
				
				
				
				DECLARE @ROB DECIMAL(20,3)
					SET @ROB=(SELECT SUM(ISNULL(ti.QuantityAmount,0) * CASE WHEN t.[Action]=1 THEN (1) WHEN t.[Action]=2 THEN (-1) END)
											FROM [Inventory].Transactions t
											INNER JOIN [Inventory].TransactionItems ti ON ti.TransactionId = t.Id
											INNER JOIN [Inventory].Warehouse a ON a.Id = t.WarehouseId
											WHERE t.WarehouseId=@WarehouseId)
					IF @ROB<>0
					BEGIN
	   					SET @MSG=N'@'+N' To '+CASE WHEN @IsActive=0 THEN N' deactivate ' WHEN @IsActive=1 THEN N' activate ' END+ N' the inventory should be empty '
						RAISERROR(@MSG,16,1,'500')
					END
					ELSE
					BEGIN
	   					UPDATE [Inventory].Warehouse
						SET
							[IsActive] = @IsActive,
							UserCreatorId = @UserCreatorId,
							CreateDate = getdate()
						WHERE Id=@WarehouseId
						Select CAST('The inventory status changed' as nvarchar(100))
					END
				
				
			declare @WarehouseCode nvarchar(200)
				
			SELECT TOP(1) @CompanyId=	w.CompanyId,
				@WarehouseCode = w.Code
			FROM [Inventory].Warehouse w WHERE w.Id=@WarehouseId
				
					
			SET @tbId =(SELECT TOP(1)tb.Id
	             FROM [Inventory].TimeBucket tb WHERE tb.[IsActive]=1 AND @RegistrationDate  BETWEEN tb.StartDate AND tb.EndDate)
			IF @TimeBucketId =0 OR @TimeBucketId IS NULL
			SET @TimeBucketId=@tbId
			IF @TimeBucketId<>@tbId
				 RAISERROR(N'@The transaction date mismatch with current active time bucket',16,1,'500')    
			IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Warehouse w WHERE w.[IsActive]=1 AND w.Id=@WarehouseId)
				 RAISERROR(N'@The inventory is not defined or active',16,1,'500')    
			IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Companies c WHERE c.[IsActive]=1 AND c.Id=@CompanyId)
				 RAISERROR(N'@The company is not defined or active',16,1,'500')
			IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].TimeBucket tb WHERE tb.[IsActive]=1 AND tb.Id=@TimeBucketId)
				 RAISERROR(N'@The time bucket is not defined or active',16,1,'500')
						
							IF @Code IS NULL OR @Code =0.000
							   SET @Code=[Inventory].GetTransactionNewCode(1,@WarehouseId,@RegistrationDate,@TimeBucketId)
							INSERT INTO [Inventory].Transactions
							(
								-- Id -- this column value is auto-generated
								[Action],
								Code,
								[Description],
								WarehouseId,
								StoreTypesId,
								TimeBucketId,
								[Status],
								RegistrationDate,
								SenderReciver,
								HardCopyNo,
								ReferenceType,
								ReferenceNo,
								ReferenceDate,
								UserCreatorId,
								CreateDate
							)
							VALUES
							(
								1,
								@Code,
								@Description,
								@WarehouseId,
								@StoreTypesId,
								@TimeBucketId,
								1,/*{ [Status] }*/
								@RegistrationDate,
								NULL,
								NULL,
								@ReferenceType,
								@ReferenceNo,
								getdate(),
								@UserCreatorId,
								getdate()
							)
							Select CAST('ثبت با موفقیت انجام شد Transactions ' as nvarchar(100))
							SET @TransactionId=@@identity
						
					
				
						
						
						
							IF EXISTS(SELECT cursor_name FROM sys.syscursors WHERE cursor_name = 'TransactionItems')
							DEALLOCATE TransactionItems
							DECLARE TransactionItems CURSOR FOR
							SELECT  t.Id , t.GoodId, t.QuantityUnitId, t.QuantityAmount, t.PriceUnitId , t.Fee, t.[Description]
							FROM @TransactionItems t
							OPEN TransactionItems
								FETCH NEXT FROM TransactionItems INTO  @Id , @GoodId, @QuantityUnitId, @QuantityAmount, @PriceUnitId, @Fee, @Description
								WHILE @@Fetch_Status = 0  
								BEGIN
				
			   						IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Goods g WHERE g.[IsActive]=1 AND g.Id=@GoodId)
			   						BEGIN
			   							SET @MSG=N'@ کالاي ' + (SELECT TOP(1) g.Code+N' '+g.Name FROM [Inventory].Goods g WHERE g.Id=@GoodId) +N' معتبر (فعال) نيست'
			   							RAISERROR(@MSG,16,1,'500')    
			   						END
									IF NOT EXISTS (SELECT TOP(1) Id FROM [Inventory].Units u WHERE u.[IsActive]=1 AND u.IsCurrency=0 AND u.Id=@QuantityUnitId)
			   						BEGIN
			   							SET @MSG=N'@ واحد ' + (SELECT TOP(1) u.Abbreviation+N' '+u.Name FROM [Inventory].Units u WHERE u.Id=@QuantityUnitId) +N' معتبر (فعال) نيست'
			   							RAISERROR(@MSG,16,1,'500')    
			   						END
			   						
			   						
										SET @RowVersion=ISNULL((SELECT MAX(ti.[RowVersion]) FROM [Inventory].TransactionItems ti WHERE ti.TransactionId=@TransactionId),0)+1		
										INSERT INTO [Inventory].TransactionItems
										(
											-- Id -- this column value is auto-generated
											RowVersion,
											TransactionId,
											GoodId,
											QuantityUnitId,
											QuantityAmount,
											[Description],
											UserCreatorId,
											CreateDate
										)
										VALUES
										(
											@RowVersion ,
											@TransactionId ,
											@GoodId ,
											@QuantityUnitId ,
											@QuantityAmount ,
											@Description,
											@UserCreatorId ,
											getdate()
										)
										Select CAST('ثبت با موفقیت انجام شد TransactionItems ' as nvarchar(100))
										SET @TransactionItemId=@@identity
										
										
										
										
										
									SET @MainCurrencyUnitId=ISNULL((SELECT TOP(1) Id FROM [Inventory].Units u WHERE u.IsBaseCurrency=1),0)
									 if @MainCurrencyUnitId=0 
                 						RAISERROR(N'@The base currency should be defined before pricing ',16,1,'500')		
                 
									 IF @PriceUnitId<>@MainCurrencyUnitId
											BEGIN
												SELECT @Coefficient=fn.Coefficient ,@PrimaryCoefficient=fn.PrimaryCoefficient
													FROM [Inventory].[PrimaryCoefficient](@PriceUnitId,@MainCurrencyUnitId,@RegistrationDate) fn	
											END		                      
											ELSE 
											BEGIN
												SET @Coefficient=1
												SET @PrimaryCoefficient=1
											END
		
											IF @Coefficient<>0
											BEGIN
												IF @PrimaryCoefficient=1  
												BEGIN
						   								SET @Coefficient=(1  *  @Coefficient)
												END
												ELSE IF @PrimaryCoefficient=0 
												BEGIN
						   								SET @Coefficient=(1  /  @Coefficient)
												END    
											END
											ELSE
											BEGIN
												RAISERROR(N'@Base currency has no relation with selected currency ', 16, 1, '500')
											END
										SET @FeeInMainCurrency=@Fee * @Coefficient			
						                                                               
										SET @RowVersion=ISNULL((SELECT MAX(tip.[RowVersion]) FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@TransactionItemId),0)+1	
										INSERT INTO [Inventory].TransactionItemPrices
										(
											-- Id -- this column value is auto-generated
											RowVersion,
											TransactionId,
											TransactionItemId,
											QuantityUnitId,
											QuantityAmount,
											PriceUnitId,
											Fee,
											MainCurrencyUnitId,
											FeeInMainCurrency,
											RegistrationDate,
											[Description],
											UserCreatorId,
											CreateDate
										)
										VALUES
										(
											@RowVersion ,
											@TransactionId ,
											@TransactionItemId ,
											@QuantityUnitId,
											@QuantityAmount ,
											@PriceUnitId ,
											@Fee ,
											@MainCurrencyUnitId,
											@FeeInMainCurrency,
											@RegistrationDate,
											@Description ,
											@UserCreatorId ,
											getdate()
										)	
										Select CAST('ثبت با موفقیت انجام شد TransactionItemPrices ' as nvarchar(100))

										
								FETCH TransactionItems INTO @Id , @GoodId, @QuantityUnitId, @QuantityAmount, @PriceUnitId, @Fee, @Description
								END
			
			
			--DECLARE @VesselInCompanyId BIGINT;

			--SELECT @VesselInCompanyId = vic.Id FROM Fuel.VesselInCompany vic inner join Fuel.Vessel v ON vic.VesselId = v.Id AND v.Code = @WarehouseCode
			--Where vic.CompanyId = @CompanyId
								
			--UPDATE Fuel.VesselInCompany
			--SET VesselStateCode = 4   -- Owned State
			--WHERE Id=@VesselInCompanyId

			UPDATE vic
			SET VesselStateCode = 4   -- Owned State
			FROM Fuel.VesselInCompany vic INNER JOIN Fuel.Vessel v ON vic.VesselId = v.Id
			Where vic.CompanyId = @CompanyId AND v.Code = @WarehouseCode
					
			Select CAST('ثبت با موفقیت انجام شد VesselInCompany ' as nvarchar(100))
						
						
			SET @Message=N'OperationSuccessful'
	IF @TRANSACTIONCOUNT = 0
	COMMIT TRANSACTION 					
	END try
	BEGIN CATCH
			IF (XACT_STATE()) = -1 
				ROLLBACK TRANSACTION
			IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
				ROLLBACK TRANSACTION
			IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
				ROLLBACK TRANSACTION CurrentTransaction
			SET @Message=ERROR_MESSAGE();
			SET @TransactionId=NULL
			SET @Code=NULL
		EXEC [Inventory].ErrorHandling
	END CATCH
END
GO	
GRANT EXECUTE ON [Inventory].ActivateWarehouseIncludingRecieptsOperation TO [public] AS [dbo]
GO

------------------------------------------------------------	
if OBJECT_ID('[Inventory].[RemoveTransactionItemPrices]','P') is Not Null
	drop procedure [Inventory].[RemoveTransactionItemPrices];
Go
Create Procedure [Inventory].[RemoveTransactionItemPrices]
(
@TransactionItemId INT,
@UserId INT,
@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
BEGIN
SET NOCOUNT ON;
DECLARE @TRANSACTIONCOUNT INT;
	SET @TRANSACTIONCOUNT = @@TRANCOUNT;
BEGIN TRY
	IF @TRANSACTIONCOUNT = 0
		BEGIN TRANSACTION
	--ELSE
	--	SAVE TRANSACTION CurrentTransaction;

	DECLARE @PricingReferenceId INT,
		@ActionType SMALLINT=NULL
			

	SELECT
		@PricingReferenceId = t.PricingReferenceId,
		@ActionType = t.[Action]
	FROM [Inventory].TransactionItems ti INNER JOIN Inventory.Transactions t ON t.Id = ti.TransactionId
	 WHERE ti.Id = @TransactionItemId


	IF(@PricingReferenceId IS NOT NULL)
	BEGIN
		UPDATE ReferencedItemPrices SET 
			ReferencedItemPrices.IssueReferenceIds = 
				REPLACE(ReferencedItemPrices.IssueReferenceIds, CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';', '')
		FROM 
		Inventory.TransactionItemPrices ReferencedItemPrices INNER JOIN (SELECT tip.Id
		              FROM Inventory.TransactionItemPrices tip WHERE tip.TransactionItemId = @TransactionItemId) AS ItemPricesToBeRemoved
		           ON 
		           ReferencedItemPrices.IssueReferenceIds LIKE '%' + CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';%' 
		
		              
	END
	ELSE IF(@ActionType = 2)  --Issue with FIFO pricing
	BEGIN  --The transactionItem is priced in FIFO
		
		IF(EXISTS(SELECT * FROM Inventory.TransactionItemPrices ReferencedItemPrices INNER JOIN 
					(SELECT tip.Id, tip.QuantityAmount, tip.QuantityUnitId
						FROM Inventory.TransactionItemPrices tip WHERE tip.TransactionItemId = @TransactionItemId) AS ItemPricesToBeRemoved
							   ON ReferencedItemPrices.IssueReferenceIds LIKE '%' + CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';%'   
					WHERE ReferencedItemPrices.QuantityAmountUseFIFO < ItemPricesToBeRemoved.QuantityAmount))
		BEGIN
			RAISERROR(N'@Removing Item FIFO Prices is not allowed due to inconsistency with quantities picked from pricing references.', 16, 1, '500')
			RETURN
		END

		UPDATE ReferencedItemPrices SET 
			ReferencedItemPrices.IssueReferenceIds = 
				REPLACE(ReferencedItemPrices.IssueReferenceIds, CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';', ''),
			ReferencedItemPrices.QuantityAmountUseFIFO -= ItemPricesToBeRemoved.QuantityAmount
		FROM 
		Inventory.TransactionItemPrices ReferencedItemPrices INNER JOIN (SELECT tip.Id, tip.QuantityAmount, tip.QuantityUnitId
		              FROM Inventory.TransactionItemPrices tip WHERE tip.TransactionItemId = @TransactionItemId) AS ItemPricesToBeRemoved
		           ON 
		           ReferencedItemPrices.IssueReferenceIds LIKE '%' + CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';%' 
	END

	UPDATE ItemPricesPricedByRemovingItemPrices SET 
		ItemPricesPricedByRemovingItemPrices.TransactionReferenceId = NULL
	FROM 
		Inventory.TransactionItemPrices ItemPricesPricedByRemovingItemPrices INNER JOIN (SELECT tip.Id
		            FROM Inventory.TransactionItemPrices tip WHERE tip.TransactionItemId = @TransactionItemId) AS ItemPricesToBeRemoved
		        ON 
		        ItemPricesPricedByRemovingItemPrices.TransactionReferenceId = ItemPricesToBeRemoved.Id

	DELETE FROM Inventory.OperationReference
	WHERE OperationType = 3 AND
		OperationId IN (SELECT Id FROM Inventory.TransactionItemPrices WHERE TransactionItemId = @TransactionItemId);

	DELETE FROM Inventory.TransactionItemPrices 
	WHERE TransactionItemId = @TransactionItemId;
	
	
		 
	SET @Message=N'OperationSuccessful'
	IF @TRANSACTIONCOUNT = 0
			COMMIT TRANSACTION 					
	END try
	BEGIN CATCH
			IF (XACT_STATE()) = -1 
				ROLLBACK TRANSACTION
			IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
				ROLLBACK TRANSACTION
			IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
				ROLLBACK TRANSACTION CurrentTransaction
				SET @Message=ERROR_MESSAGE();
		EXEC [Inventory].ErrorHandling
	END CATCH
END
GO	
GRANT EXECUTE ON [Inventory].[RemoveTransactionItemPrices] TO [public] AS [dbo]
GO

------------------------------------------------------------	

if OBJECT_ID('[Inventory].[RemoveTransactionItemPrice]','P') is Not Null
	drop procedure [Inventory].[RemoveTransactionItemPrice];
Go
Create Procedure [Inventory].[RemoveTransactionItemPrice]
(
@TransactionItemId INT,
@TransactionItemPriceId INT,
@UserId INT,
@Message NVARCHAR(MAX) OUT
)
--WITH ENCRYPTION
AS 
BEGIN
SET NOCOUNT ON;
DECLARE @TRANSACTIONCOUNT INT;
	SET @TRANSACTIONCOUNT = @@TRANCOUNT;
BEGIN TRY
	IF @TRANSACTIONCOUNT = 0
		BEGIN TRANSACTION;
		
	--ELSE
	--	SAVE TRANSACTION CurrentTransaction;

	DECLARE @PricingReferenceId INT,
		@ActionType SMALLINT=NULL
			

	SELECT
		@PricingReferenceId = t.PricingReferenceId,
		@ActionType = t.[Action]
	FROM [Inventory].TransactionItems ti INNER JOIN Inventory.Transactions t ON t.Id = ti.TransactionId
	 WHERE ti.Id = @TransactionItemId


	IF(@PricingReferenceId IS NOT NULL)
	BEGIN
		UPDATE ReferencedItemPrices SET 
			ReferencedItemPrices.IssueReferenceIds = 
				REPLACE(ReferencedItemPrices.IssueReferenceIds, CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';', '')
		FROM Inventory.TransactionItemPrices ReferencedItemPrices INNER JOIN (SELECT tip.Id
		              FROM Inventory.TransactionItemPrices tip WHERE tip.TransactionItemId = @TransactionItemId AND tip.Id = @TransactionItemPriceId) AS ItemPricesToBeRemoved
		           ON 
		           ReferencedItemPrices.IssueReferenceIds LIKE '%' + CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';%' 
		
		              
	END
	ELSE IF(@ActionType = 2)  --Issue with FIFO pricing
	BEGIN  --The transactionItem is priced in FIFO
		
		IF(EXISTS(SELECT * FROM Inventory.TransactionItemPrices ReferencedItemPrices INNER JOIN 
					(SELECT tip.Id, tip.QuantityAmount, tip.QuantityUnitId
						FROM Inventory.TransactionItemPrices tip WHERE tip.TransactionItemId = @TransactionItemId AND tip.Id = @TransactionItemPriceId) AS ItemPricesToBeRemoved
							   ON ReferencedItemPrices.IssueReferenceIds LIKE '%' + CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';%'   
					WHERE ReferencedItemPrices.QuantityAmountUseFIFO < ItemPricesToBeRemoved.QuantityAmount))
		BEGIN
			RAISERROR(N'@Removing Item FIFO Prices is not allowed due to inconsistency with quantities picked from pricing references.', 16, 1, '500')
			RETURN
		END

		UPDATE ReferencedItemPrices SET 
			ReferencedItemPrices.IssueReferenceIds = 
				REPLACE(ReferencedItemPrices.IssueReferenceIds, CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';', ''),
			ReferencedItemPrices.QuantityAmountUseFIFO -= ItemPricesToBeRemoved.QuantityAmount
		FROM 
		Inventory.TransactionItemPrices ReferencedItemPrices INNER JOIN (SELECT tip.Id, tip.QuantityAmount, tip.QuantityUnitId
		              FROM Inventory.TransactionItemPrices tip WHERE tip.TransactionItemId = @TransactionItemId AND tip.Id = @TransactionItemPriceId) AS ItemPricesToBeRemoved
		           ON 
		           ReferencedItemPrices.IssueReferenceIds LIKE '%' + CAST(ItemPricesToBeRemoved.Id AS NVARCHAR(20)) + ';%' 
	END

	UPDATE ItemPricesPricedByRemovingItemPrices SET 
		ItemPricesPricedByRemovingItemPrices.TransactionReferenceId = NULL
	FROM 
		Inventory.TransactionItemPrices ItemPricesPricedByRemovingItemPrices INNER JOIN (SELECT tip.Id
		            FROM Inventory.TransactionItemPrices tip WHERE tip.TransactionItemId = @TransactionItemId AND tip.Id = @TransactionItemPriceId) AS ItemPricesToBeRemoved
		        ON 
		        ItemPricesPricedByRemovingItemPrices.TransactionReferenceId = ItemPricesToBeRemoved.Id

	DELETE FROM Inventory.OperationReference
	WHERE OperationType = 3 AND
		OperationId IN (SELECT Id FROM Inventory.TransactionItemPrices WHERE TransactionItemId = @TransactionItemId AND Id = @TransactionItemPriceId);

	DELETE FROM Inventory.TransactionItemPrices 
	WHERE TransactionItemId = @TransactionItemId AND Id = @TransactionItemPriceId;
	
	DECLARE @COUNT NVARCHAR(50)

	SET @COUNT = CAST( @@ROWCOUNT AS nvarchar(50))

	--RAISERROR(@COUNT, 16, 1, '500')
	--		RETURN
		 
	SET @Message=N'OperationSuccessful'
	IF @TRANSACTIONCOUNT = 0
			COMMIT TRANSACTION 					
	END try
	BEGIN CATCH
			IF (XACT_STATE()) = -1 
				ROLLBACK TRANSACTION
			IF (XACT_STATE()) = 1 AND @TRANSACTIONCOUNT=0
				ROLLBACK TRANSACTION
			IF  (XACT_STATE()) = 1 and @TRANSACTIONCOUNT > 0
				ROLLBACK TRANSACTION CurrentTransaction
				SET @Message=ERROR_MESSAGE();
		EXEC [Inventory].ErrorHandling
	END CATCH
END
GO	
GRANT EXECUTE ON [Inventory].[RemoveTransactionItemPrice] TO [public] AS [dbo]
GO

------------------------------------------------------------	

RAISERROR('پایان اجرای پروسيجرها.',0,1) WITH NOWAIT

 