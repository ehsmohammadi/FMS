USE [MiniStock]
GO

DECLARE	@return_value int,
		@TransactionItemsId nvarchar(256),
		@Message nvarchar(max)
declare @p4 dbo.TypeTransactionItems
insert into @p4 values(NULL,1,1,10,N'تست 1')

EXEC	@return_value = [dbo].[TransactionItemsOperation]
		@Action = N'insert',
		@TransactionId = 1,
		@User_Creator_Id = 1,
		@TransactionItems=@p4,
		@TransactionItemsId = @TransactionItemsId OUTPUT,
		@Message = @Message OUTPUT

SELECT	@TransactionItemsId as N'@TransactionItemsId',
		@Message as N'@Message'

SELECT	'Return Value' = @return_value

GO
