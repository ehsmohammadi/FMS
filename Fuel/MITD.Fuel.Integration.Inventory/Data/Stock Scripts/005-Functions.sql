--USE MiniStock 
--GO 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-------------------------------------------------
IF OBJECT_ID ( '[Inventory].[CurrentTime]', 'FN' ) IS NOT NULL 
DROP FUNCTION [Inventory].[CurrentTime];
GO
CREATE FUNCTION [Inventory].[CurrentTime]()
RETURNS VARCHAR(10)
--WITH ENCRYPTION
AS
 BEGIN
 	RETURN (SELECT (cast(DATEPART(hour, GETDATE())AS NVARCHAR(3))+':'+cast(DATEPART(minute, GETDATE())AS NVARCHAR(3))+':'+cast(DATEPART(SECOND, GETDATE())AS NVARCHAR(3)))AS CTime)
 END
GO
GRANT EXECUTE ON [Inventory].[CurrentTime] TO [public]
GO
-----------------------------برای تراز بندی با کاراکتر دلخواه می باشدleft pad
IF OBJECT_ID ( '[Inventory].[LPAD]', 'FN' ) IS NOT NULL 
DROP FUNCTION [Inventory].[LPAD];
GO
CREATE FUNCTION [Inventory].[LPAD]
		(
		@SourceString VARCHAR(MAX),
        @FinalLength  INT,
        @PadChar      VARCHAR(1)
		)
RETURNS VARCHAR(MAX)
--WITH ENCRYPTION
AS
 BEGIN
 	IF @FinalLength - Len(@SourceString)<0
 		RETURN (select(LEFT(@SourceString,@FinalLength)))
    RETURN (select(Replicate(@PadChar,@FinalLength - Len(@SourceString)) + @SourceString))
 END
--SELECT [Galaxy].LPAD('012345678',10,'&')AS name 
GO
GRANT EXECUTE ON [Inventory].[LPAD] TO [public]
GO
-----------------------------برای تراز بندی با کاراکتر دلخواه می باشدright pad
IF OBJECT_ID ( '[Inventory].[RPAD]', 'FN' ) IS NOT NULL 
DROP FUNCTION [Inventory].[RPAD];
GO
CREATE FUNCTION [Inventory].[RPAD]
		(
		@SourceString VARCHAR(MAX),
        @FinalLength  INT,
        @PadChar      VARCHAR(1)
		)
RETURNS VARCHAR(MAX)
--WITH ENCRYPTION
AS
 BEGIN
 	IF @FinalLength - Len(@SourceString)<0
 		RETURN (RIGHT(@SourceString,@FinalLength))
 	RETURN (@SourceString + Replicate(@PadChar,@FinalLength - Len(@SourceString)))
 END
--SELECT [Inventory].RPAD('1234567890',10,' ')AS name 
GO
GRANT EXECUTE ON [Inventory].[RPAD] TO [public]
GO
----------------------------- مقدار فعلی موجودی
IF OBJECT_ID ( '[Inventory].GetMojodi', 'FN' ) IS NOT NULL 
DROP FUNCTION [Inventory].GetMojodi;
GO
CREATE FUNCTION [Inventory].GetMojodi
(
	@GoodId BIGINT=NULL,
	@WarehouseId BIGINT,
	@TimeBucketId INT = NULL,
	@RegistrationDate DATETIME=NULL,
	@Action TINYINT =NULL,
	@TransactionId INT=NULL,
	@RowVersion SMALLINT=NULL,
	@QuantityUnitId BIGINT = NULL,
	@QuantityAmount DECIMAL(20,3)=NULL
	
)
RETURNS DECIMAL(20,3)
--WITH ENCRYPTION
AS 
BEGIN
	DECLARE @ActionType AS NVARCHAR(5)='12   '
	declare @retValue DECIMAL(20,3)
	SET @retValue =(SELECT SUM(t.s) AS CurrentMojodi
					FROM   (
							   SELECT 
							   SUM(CASE WHEN PATINDEX('%'+cast(t.Action AS NVARCHAR(5))+'%', @ActionType)=0 
								   THEN  0 ELSE
							        ti.QuantityAmount *(CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END) END) AS s
							   FROM [Inventory].TransactionItems ti
									  INNER  JOIN [Inventory].Transactions t
										   ON t.Id = ti.TransactionId
							   WHERE  ti.GoodId=@GoodId
							          AND ti.QuantityUnitId=@QuantityUnitId
									  AND t.WarehouseId=@WarehouseId
									  AND (@TimeBucketId IS NULL OR t.TimeBucketId=@TimeBucketId)
									  AND ((@TransactionId=0 OR @TransactionId IS NULL OR @RowVersion=0 OR @RowVersion IS NULL OR (CONVERT(NVARCHAR(20),@RegistrationDate,(120))) IS NULL) OR 
									      (
									          CONVERT(NVARCHAR(20), t.RegistrationDate, (120))
									          + [Inventory].RPAD(
									                        CAST(
									                            CASE 
									                                 WHEN t.Action 
									                                      = 1 THEN 
									                                      2
									                                 WHEN t.Action 
									                                      = 2 THEN 
									                                      1
									                                 ELSE t.Action
									                            END AS NVARCHAR(3) ), ' ', 3 ) 
									          + [Inventory].RPAD(CAST(t.ID AS NVARCHAR(15)), ' ', 15) 
												+ [Inventory].RPAD(CAST(ti.RowVersion AS NVARCHAR(10)), ' ', 10)
																						  ) <=(
																								  CONVERT(NVARCHAR(20), @RegistrationDate, (120)) 
																								  + [Inventory].RPAD(CAST(@Action AS NVARCHAR(3)), ' ', 3)
																								  + [Inventory].RPAD(CAST(@TransactionId AS NVARCHAR(15)), ' ', 15) 
																								  + [Inventory].RPAD(CAST(@RowVersion AS NVARCHAR(10)), ' ', 10)
																							  ))
																			   GROUP BY
																					  t.Action
																		   ) AS t
				  )
	return ISNULL(@retValue,0)
END;
GO
GRANT EXECUTE ON [Inventory].GetMojodi TO [public]
GO

----------------------------- مقدار فعلی موجودی  By A.Hatefi
IF OBJECT_ID ( '[Inventory].GetInventoryQuantity', 'FN' ) IS NOT NULL 
DROP FUNCTION [Inventory].GetInventoryQuantity;
GO
CREATE FUNCTION [Inventory].GetInventoryQuantity
(
	@GoodId BIGINT,
	@WarehouseId BIGINT,
	@RequestDateTime DATETIME=NULL	
)
RETURNS DECIMAL(20,3)
--WITH ENCRYPTION
AS 
BEGIN
	
	declare @retValue DECIMAL(20,3)

	SELECT @retValue = SUM(ti.QuantityAmount * CASE t.[Action] WHEN 1 THEN 1 ELSE -1 END) FROM Inventory.TransactionItems ti INNER JOIN Inventory.Transactions t
				ON t.Id = ti.TransactionId WHERE ti.GoodId = @GoodId AND t.WarehouseId = @WarehouseId AND (@RequestDateTime IS NULL OR t.RegistrationDate <= @RequestDateTime);

	return ISNULL(@retValue,0)
END;
GO
GRANT EXECUTE ON [Inventory].GetInventoryQuantity TO [public]
GO
------------------------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].CheckNegetiveWarehouseValue', 'FN' ) IS NOT NULL  
  DROP FUNCTION [Inventory].CheckNegetiveWarehouseValue;
GO
	CREATE FUNCTION [Inventory].CheckNegetiveWarehouseValue
	(
		@WarehouseId BIGINT,
		@GoodId BIGINT,
		@TimeBucketId INT = NULL,
		@RowVersion SMALLINT= NULL,
		@Action tinyint= NULL,
		@TransactionId decimal(20,2)= NULL,
        @QuantityAmount DECIMAL(13, 3)= NULL,
		@QuantityUnitId BIGINT = NULL,
		@CallFromProcedure BIT =0
	)
	RETURNS  TINYINT
	--WITH ENCRYPTION
	AS 
	BEGIN
		declare @retValue TINYINT
		DECLARE @RegistrationDate DATETIME
		DECLARE @CurrentMojodi DECIMAL(20,3)
		DECLARE @Current_Value DECIMAL(20, 3)
		
			
			SET @RegistrationDate=(SELECT t.RegistrationDate FROM [Inventory].Transactions t
							  WHERE t.Action=@Action AND t.Id=@TransactionId AND t.WarehouseId=@WarehouseId 
							  			  --AND t.TimeBucketId=@TimeBucketId                          A.H  Order
							  )
			SET @CurrentMojodi=[Inventory].GetMojodi(@GoodId, @WarehouseId,NULL,@RegistrationDate,@Action,@TransactionId,@RowVersion,@QuantityUnitId,@QuantityAmount)
			
			set @Current_Value=(SELECT ti.QuantityAmount
			                    FROM   [Inventory].TransactionItems ti
			                           INNER JOIN [Inventory].Transactions t
			                                ON  t.Id = ti.TransactionId
			                    WHERE  t.Action = @Action
			                           AND ti.TransactionId = @TransactionId
			                           AND t.WarehouseId = @WarehouseId
			                           AND ti.GoodId = @GoodId
			                           --AND t.TimeBucketId=@TimeBucketId                          A.H  Order
			                           AND ti.RowVersion = @RowVersion)			
			
			IF @CallFromProcedure=1 AND   @Action=2 
				SET @CurrentMojodi=@CurrentMojodi+ISNULL(@Current_Value,0)-@QuantityAmount	
			
			IF @CallFromProcedure=1 AND @Action=1 
				SET @CurrentMojodi=@CurrentMojodi-ISNULL(@Current_Value,0)+@QuantityAmount	
			
			IF @CurrentMojodi<0
			 BEGIN
					SET @retValue=0
			 END
			ELSE 
				SET @retValue=1	--منفی نشده است 
		RETURN @retValue	
	END
GO
GRANT EXECUTE ON [Inventory].CheckNegetiveWarehouseValue TO [public]
GO
------------------------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].CheckNegetiveWarehouse', 'FN' ) IS NOT NULL  
  DROP FUNCTION [Inventory].CheckNegetiveWarehouse;
GO
CREATE FUNCTION [Inventory].CheckNegetiveWarehouse
(
	@WarehouseId BIGINT,
	@GoodId BIGINT,
	@TimeBucketId INT,
	@QuantityAmount DECIMAL(20,3)=0 
)
	RETURNS TINYINT
	--WITH ENCRYPTION
	AS 
	BEGIN
		DECLARE @retValue TINYINT
			
			DECLARE @value NVARCHAR(MAX)=''
			SET @value=[Inventory].negWarehouse(@WarehouseId,@GoodId,@TimeBucketId) 
			IF not @value=''
			BEGIN
				SET @retValue=0
			END	
			ELSE 
				SET @retValue=1	--منفي نشده است 
		RETURN @retValue	
	END
GO--print [Inventory].CheckNegetiveWarehouse(2,1,1,0)
------------------------------------------------------
GRANT EXECUTE ON [Inventory].CheckNegetiveWarehouse TO [public]
GO

IF OBJECT_ID ( '[Inventory].negWarehouse', 'FN' ) IS NOT NULL 
DROP FUNCTION [Inventory].negWarehouse;
GO
CREATE FUNCTION [Inventory].negWarehouse
(
	@WarehouseId BIGINT,
	@GoodId BIGINT,
	@TimeBucketId INT
)
RETURNS NVARCHAR(MAX)
--WITH ENCRYPTION
AS 
BEGIN	
	IF @WarehouseId IS NULL 
		SET @WarehouseId=0
		
	IF @GoodId IS NULL 
		SET @GoodId=0
	IF @TimeBucketId IS NULL 
		SET @TimeBucketId=0	
		
	DECLARE @value VARCHAR(MAX),
	        @tmpWarehouseId BIGINT,
	        @tmpGoodId BIGINT,
	        @c DECIMAL(20,3)
	SET @value=''
	SET @tmpGoodId=0
	SET @tmpWarehouseId=0
	SET @TimeBucketId=0
	SET @c=0
	
	SELECT @c=CASE WHEN @tmpGoodId<>t.GoodId OR @tmpWarehouseId<>t.WarehouseId THEN t.sumValue ELSE @c+t.sumValue END,
		   @tmpGoodId=t.GoodId,@tmpWarehouseId=t.WarehouseId,
		   @value=CASE WHEN @c<0 THEN (@value+CAST(t.Action AS NVARCHAR(1))+';'+CAST(t.Id AS NVARCHAR(20))+'@'+
						 cast(t.WarehouseId AS NVARCHAR(13))+'#'+CONVERT(NVARCHAR(20),t.RegistrationDate,(120))+'$'+Cast(t.GoodId AS NVARCHAR(10))+CHAR(13)+char(10)) ELSE @value END
	FROM
	(SELECT  t.Action,t.Id,t.WarehouseId,t.RegistrationDate,ti.GoodId,
			 ti.QuantityAmount * (CASE WHEN t.[Action]=1 THEN 1 WHEN t.[Action]=2 THEN -1 END) sumValue      
			 ,ROW_NUMBER() OVER(PARTITION BY ti.GoodId
				  			    ORDER BY t.WarehouseId,ti.GoodId, t.RegistrationDate, t.Action,
								         t.Id )RNo
	 FROM [Inventory].Transactions t
		   INNER JOIN [Inventory].TransactionItems ti
				ON  t.Id = ti.TransactionId
	 WHERE @WarehouseId=CASE WHEN @WarehouseId=0 THEN 0 ELSE t.WarehouseId END AND
		   @GoodId=CASE WHEN @GoodId=0 THEN 0 ELSE ti.GoodId END AND
		   (t.Action=1 OR t.Action=2)		
	)t      
	ORDER BY WarehouseId,GoodId,RegistrationDate,Action,Id	
	RETURN @Value 
END;
GO
GRANT EXECUTE ON [Inventory].negWarehouse TO [public]
GO
------------------------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].CheckOverFlowQuantityPricing', 'FN' ) IS NOT NULL  
  DROP FUNCTION [Inventory].CheckOverFlowQuantityPricing;
GO
CREATE FUNCTION [Inventory].CheckOverFlowQuantityPricing
(
	@Action TINYINT,
	@TransactionItemId INT,
	@TransactionId INT,
	@QuantityUnitId  BIGINT 
)
	RETURNS TINYINT
	--WITH ENCRYPTION
	AS 
	BEGIN
		DECLARE @retValue TINYINT,
		        @value DECIMAL(20,3)=0,
		        @QuantityAmountTotal DECIMAL(20,3) = 0,
				@QuantityAmountSumSoFar DECIMAL(20,3) = 0 
		
		SET @QuantityAmountTotal=ISNULL((SELECT sum(ti.QuantityAmount)
				                                    FROM [Inventory].TransactionItems ti WHERE ti.Id=@TransactionItemId
																			   AND ti.QuantityUnitId=@QuantityUnitId
				                                                               AND ti.TransactionId=@TransactionId),0)	
        --IF @Action=2
			SET @QuantityAmountSumSoFar=ISNULL((SELECT sum(tip.QuantityAmount)
					                                    FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionItemId=@TransactionItemId
																						 AND tip.QuantityUnitId=@QuantityUnitId
																						 AND tip.TransactionId=@TransactionId),0)	
		--ELSE IF @Action=1
		--	SET @QuantityAmountSumSoFar=ISNULL((SELECT sum(tip.QuantityAmount)
		--			                                    FROM [Inventory].TransactionItemPrices tip WHERE tip.TransactionReferenceId=@TransactionItemId
		--																				 AND tip.QuantityUnitId=@QuantityUnitId
		--																				 --AND tip.TransactionId=@TransactionId
		--																				 ),0)
																						 	 
			IF @QuantityAmountTotal<@QuantityAmountSumSoFar
			BEGIN
				SET @retValue=0--سرريز شده است 
			END	
			ELSE 
				SET @retValue=1	--سرريز نشده است 
		RETURN @retValue	
	END
GO
GRANT EXECUTE ON [Inventory].CheckOverFlowQuantityPricing TO [public]
GO
-----------------------------------------------------------------------------------------------
IF OBJECT_ID ('[Inventory].[PrimaryCoefficient]') IS NOT NULL 
	DROP FUNCTION [Inventory].PrimaryCoefficient;
GO
CREATE FUNCTION [Inventory].PrimaryCoefficient
(
	@OriginalUnitId BIGINT,
	@SubsidiaryUnitId BIGINT,
	@EffectiveDate DATETIME
)
RETURNS @tbl TABLE 
(
	Coefficient DECIMAL(18,0),
    PrimaryCoefficient TINYINT
)
--WITH ENCRYPTION
AS
BEGIN
	DECLARE @SubsidiaryUnit_Coefficient DECIMAL(18,0)
		SET @SubsidiaryUnit_Coefficient=0
	DECLARE @PrimaryCoefficient TINYINT
		SET @PrimaryCoefficient=2

		SET @SubsidiaryUnit_Coefficient = ISNULL((
									SELECT TOP(1)uc.Coefficient
									FROM   [Inventory].UnitConverts uc
									WHERE uc.UnitId = @OriginalUnitId
									      AND uc.SubUnitId = @SubsidiaryUnitId
										  AND uc.EffectiveDateStart <=@EffectiveDate
										  AND (uc.EffectiveDateEnd IS NULL OR uc.EffectiveDateEnd >=@EffectiveDate) 
									--ORDER BY uc.[RowVersion] DESC
							),0)
	   IF @SubsidiaryUnit_Coefficient=0
	   BEGIN
	   		SET @SubsidiaryUnit_Coefficient = ISNULL((
									SELECT TOP(1)uc.Coefficient
									FROM   [Inventory].UnitConverts uc
									WHERE uc.UnitId = @SubsidiaryUnitId
										  AND uc.SubUnitId = @OriginalUnitId
										  AND uc.EffectiveDateStart <=@EffectiveDate
										  AND (uc.EffectiveDateEnd IS NULL OR uc.EffectiveDateEnd >=@EffectiveDate) 
									--ORDER BY uc.[RowVersion] DESC
							),0)
			IF @SubsidiaryUnit_Coefficient<>0 SET @PrimaryCoefficient=0
	   END
	   ELSE  SET @PrimaryCoefficient=1
   INSERT INTO @tbl
    (
    	Coefficient,
    	PrimaryCoefficient
    )
    VALUES
    (
    	@SubsidiaryUnit_Coefficient,
    	@PrimaryCoefficient
    )
    RETURN 
END
GO
GRANT SELECT ON [Inventory].[PrimaryCoefficient] TO [public]
GO

--------------------توليد کد جديد رسيد يا حواله
IF OBJECT_ID('[Inventory].GenerateTransactionNewCode', 'FN') IS NOT NULL
    DROP FUNCTION [Inventory].GenerateTransactionNewCode;
GO
CREATE FUNCTION [Inventory].GenerateTransactionNewCode
(
	--@isNormal BIT,					--Commented By A.Hatefi 1395-04-03
	--@Action               TINYINT,	--Commented By A.Hatefi before 1395-04-03
	@WarehouseId          INT,
	@RegistrationDate     DATETIME
)
RETURNS DECIMAL(20, 2)
AS
BEGIN
	DECLARE @retValue DECIMAL(20, 2),
			@Floor    DECIMAL(20, 2),
			@Ceiling  DECIMAL(20, 2),
			@Diffrence DECIMAL(20,2)
			

	--IF @isNormal=1  --Commented By A.Hatefi 1395-04-03
	--	SET @retValue = (
	--	        SELECT MAX(ISNULL(t.Code, 0)) + 1
	--	        FROM   Inventory.Transactions t
	--	        WHERE  --t.Action = @Action AND   --Commented By A.Hatefi before 1395-04-03
	--	                t.WarehouseId = @WarehouseId
	--	    )
	--ELSE
	--BEGIN  --Commented By A.Hatefi 1395-04-03
		SET @Floor = (
		        SELECT MAX(ISNULL(t.Code, 0))
		        FROM   Inventory.Transactions t
		        WHERE  --t.Action = @Action AND   --Commented By A.Hatefi before 1395-04-03
		                t.WarehouseId = @WarehouseId
		               AND t.RegistrationDate <= @RegistrationDate
		)
		SET @Ceiling = (
		        --SELECT MIN(ISNULL(t.Code, 0))    --Commented By A.Hatefi 1395-04-03
				SELECT MIN(t.Code)  
		        FROM   Inventory.Transactions t
		        WHERE  --t.Action = @Action AND
		                t.WarehouseId = @WarehouseId
		               AND t.RegistrationDate > @RegistrationDate
		)
		SET @Diffrence=(@Ceiling - @Floor)
		SET @retValue = CASE 
		                     WHEN @Ceiling IS NULL OR @Diffrence > 1 THEN CEILING(@Floor)
		                     WHEN @Diffrence > 0.1 THEN (@Floor + 0.1)
		                     WHEN @Diffrence < 0.1 AND @Diffrence > 0.01 THEN (@Floor + 0.01)
		                     
		                     ELSE (@Floor + 0.01)
		                END
	--END  --Commented By A.Hatefi 1395-04-03
			 
	IF @retValue IS NULL
	    SET @retValue = 1
	
	RETURN @retValue
END;
GO
--------------------توليد کد جديد رسيد يا حواله
IF OBJECT_ID('[Inventory].GetTransactionNewCode', 'FN') IS NOT NULL
    DROP FUNCTION [Inventory].GetTransactionNewCode;
GO
CREATE FUNCTION [Inventory].GetTransactionNewCode
(
	@Action TINYINT,
	@WarehouseId BIGINT,
	@RegistrationDate DATETIME,
	@TimeBucketId INT
)
RETURNS DECIMAL(20, 2)
AS
BEGIN
	--DECLARE @retValue DECIMAL(20, 2)
	
	--SET @retValue = (
	--				   SELECT CASE 
	--						  WHEN @RegistrationDate >= MAX(t.RegistrationDate)
	--							   OR MAX(t.RegistrationDate) IS NULL 
	--						  THEN 
	--							   [Inventory].GenerateTransactionNewCode(1, @WarehouseId, @RegistrationDate)
	--						  ELSE 
	--							   [Inventory].GenerateTransactionNewCode(0, @WarehouseId, @RegistrationDate)
	--						  END
	--					FROM Inventory.Transactions t 
	--					WHERE --t.Action = @Action AND   --Commented By A.Hatefi before 1395-04-03
	--						   t.WarehouseId = @WarehouseId
	--				)
	DECLARE @retValue DECIMAL(20, 2),
			@PreviousCode    DECIMAL(20, 2),
			@NextCode  DECIMAL(20, 2),
			@Difference DECIMAL(20,2)
				
	SELECT  @PreviousCode = MAX(ISNULL(t.Code, 0))
	FROM   Inventory.Transactions t
	WHERE  t.WarehouseId = @WarehouseId
		    AND t.RegistrationDate <= @RegistrationDate
		
	SELECT @NextCode = MIN(t.Code)
	FROM   Inventory.Transactions t
	WHERE t.WarehouseId = @WarehouseId
		    AND t.RegistrationDate > @RegistrationDate
	
	SET @Difference = (@NextCode - @PreviousCode)
	SET @retValue = CASE
		                    WHEN @NextCode IS NULL OR @Difference > 1 THEN FLOOR(@PreviousCode) + 1.00
							WHEN @Difference = 1 AND @PreviousCode <> FLOOR(@PreviousCode)  --The previous and next codes are in floating point format such as X.30 and Y.30 with exactly 1 value difference, so the generated code should be (Y.00) or (X.00 + 1)
								THEN FLOOR(@NextCode) 
		                    WHEN @Difference > 0.1 THEN (@PreviousCode + 0.1)
		                    WHEN @Difference < 0.1 AND @Difference > 0.01 THEN (@PreviousCode + 0.01)
		                    ELSE (@PreviousCode + 0.01)
		            END
	
	IF @retValue IS NULL
	    SET @retValue = 1
	
	RETURN @retValue
END;
GO
----------------------تولید کد جدید رسید یا حواله
--IF OBJECT_ID ( '[Inventory].GetTransactionNewCode', 'FN' ) IS NOT NULL 
--DROP FUNCTION [Inventory].GetTransactionNewCode;
--GO
--CREATE FUNCTION [Inventory].GetTransactionNewCode
--(
--	@Action TINYINT,
--	@WarehouseId BIGINT,
--	@RegistrationDate DATETIME,
--	@TimeBucketId INT
--)
--RETURNS decimal(20,2)
----WITH ENCRYPTION
--AS 
--BEGIN
--	declare @retValue decimal(20,2)
	
--	SET @retValue=(SELECT CASE WHEN @RegistrationDate>=MAX(t.RegistrationDate) OR MAX(t.RegistrationDate) IS NULL THEN 
--										[Inventory].GenerateTransactionNewCode(MAX(t.Code)) 
--									ELSE	
--										(SELECT [Inventory].GenerateTransactionNewCode(MAX(t.Code)) 
--										 FROM [Inventory].Transactions t WHERE t.RegistrationDate<=@RegistrationDate 
--										 AND t.WarehouseId=@WarehouseId 
--										 --AND t.TimeBucketId=@TimeBucketId                          A.H  Order
--										 ) 
--									END
--							 FROM Transactions t
--							 WHERE  
--							 --t.Action=@Action AND 
--							 t.WarehouseId=@WarehouseId 
--							 --AND t.TimeBucketId=@TimeBucketId                          A.H  Order
--	)
	
--	RETURN @retValue
--END;
--GO
GRANT EXECUTE ON [Inventory].GetTransactionNewCode TO [public]
GO
-------------------------------------------------------------------------
IF OBJECT_ID ( '[Inventory].IsValidRHCode', 'FN' ) IS NOT NULL 
DROP FUNCTION [Inventory].IsValidRHCode;
GO
CREATE FUNCTION [Inventory].IsValidRHCode
(
	@Action tinyint ,
	@Code decimal(20,2),
	@WarehouseId BIGINT,
	@RegistrationDate DATETIME,
	@TimeBucketId INT
)
RETURNS BIT
--WITH ENCRYPTION
AS 
BEGIN
	declare @retValue bit		
	DECLARE @c SMALLINT
			
	SET @c=(SELECT COUNT(t.Code) FROM Transactions t
			WHERE t.Action=@Action AND t.WarehouseId=@WarehouseId --AND t.TimeBucketId=@TimeBucketId                           A.H  Order
			AND(
			  (t.Code<=@Code AND t.RegistrationDate>@RegistrationDate AND t.Code<>@Code) 
			  OR (t.Code>=@Code AND t.RegistrationDate<@RegistrationDate AND t.Code<>@Code)))			
	SET @retValue=CASE WHEN @c=0 THEN 1 ELSE 0 END
	return @retValue
END;
GO
GRANT EXECUTE ON [Inventory].IsValidRHCode TO [public]
GO
------------------------------------------------------
raiserror(N'پایان ایجاد توابع.',0,1) with nowait
GO
