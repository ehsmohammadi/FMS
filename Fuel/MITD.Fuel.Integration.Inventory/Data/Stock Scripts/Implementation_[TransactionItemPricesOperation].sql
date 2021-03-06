USE [MiniStock]
GO

DECLARE	@return_value int,
		@TransactionItemPriceIds nvarchar(max),
		@Message nvarchar(max)
declare @p4 dbo.TypeTransactionItemPrices
insert into @p4 values(NULL,1,1,5,2,3500,N'2014-06-09',N'تست 1')

EXEC	@return_value = [dbo].[TransactionItemPricesOperation]
		@Action = N'insert',
		@User_Creator_Id = 1,
		@TransactionItemPrices=@p4,
		@TransactionItemPriceIds = @TransactionItemPriceIds OUTPUT,
		@Message = @Message OUTPUT

SELECT	@TransactionItemPriceIds as N'@TransactionItemPriceIds',
		@Message as N'@Message'

SELECT	'Return Value' = @return_value

GO
