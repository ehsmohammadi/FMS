USE [MiniStock]
GO

DECLARE	@return_value int,
		@TransactionId int,
		@Code decimal(20, 2),
		@Message nvarchar(max)

EXEC	@return_value = [dbo].[TransactionOperation]
		@Action = N'insert',
		@TransactionAction = 1,
		@CompanyId = 1,
		@WarehouseId = 1,
		@TimeBucketId = 1,
		@StoreTypesId = 1,
		@RegistrationDate =N'2014-06-09',
		@ReferenceType = N'1',
		@ReferenceNo = N'1',
		@User_Creator_Id = 1,
		@TransactionId = @TransactionId OUTPUT,
		@Code = @Code OUTPUT,
		@Message = @Message OUTPUT

SELECT	@TransactionId as N'@TransactionId',
		@Code as N'@Code',
		@Message as N'@Message'

SELECT	'Return Value' = @return_value

GO
