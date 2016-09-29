/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
---------------------------------------------------------
BEGIN TRANSACTION
GO
ALTER TABLE Inventory.Transactions ADD
	AdjustmentForTransactionId int NULL
GO
ALTER TABLE Inventory.Transactions ADD CONSTRAINT
	FK_Transactions_AdjustmentFor_Transactions FOREIGN KEY
	(
	AdjustmentForTransactionId
	) REFERENCES Inventory.Transactions
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE Inventory.Transactions SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
---------------------------------------------------------
BEGIN TRANSACTION
GO
ALTER TABLE Inventory.StoreTypes ADD
	IsAdjustment bit NOT NULL CONSTRAINT DF_StoreTypes_IsAdjustment DEFAULT 0
GO
ALTER TABLE Inventory.StoreTypes SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
---------------------------------------------------------
BEGIN TRY

	BEGIN TRANSACTION

	UPDATE [Inventory].[OperationReference] 
	SET ReferenceType = REPLACE(ReferenceType, '_PRICING', '')

	COMMIT TRANSACTION

END TRY
BEGIN CATCH

	ROLLBACK TRANSACTION

END CATCH