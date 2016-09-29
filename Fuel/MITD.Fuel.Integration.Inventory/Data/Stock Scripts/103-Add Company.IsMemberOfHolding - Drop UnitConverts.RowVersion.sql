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
BEGIN TRANSACTION
GO
ALTER TABLE Inventory.Companies ADD
	IsMemberOfHolding bit NOT NULL CONSTRAINT DF_Companies_IsMemberOfHolding DEFAULT 0
GO
ALTER TABLE Inventory.Companies SET (LOCK_ESCALATION = TABLE)
GO

UPDATE [Inventory].[Companies]
   SET [IsMemberOfHolding] = 1
 WHERE Id IN (1,2,3,4,5)

ALTER TABLE Inventory.UnitConverts
	DROP CONSTRAINT UQ_UnitConverts_RowVersion
GO
ALTER TABLE Inventory.UnitConverts
	DROP COLUMN RowVersion
GO
ALTER TABLE Inventory.UnitConverts SET (LOCK_ESCALATION = TABLE)
GO

ALTER TABLE Inventory.UnitConverts
	DROP CONSTRAINT UQ_UnitConverts_PrimarySecondary_FinancialYear
GO
ALTER TABLE Inventory.UnitConverts ADD CONSTRAINT
	UQ_UnitConverts_PrimarySecondary_FinancialYear UNIQUE NONCLUSTERED 
	(
	UnitId,
	SubUnitId,
	EffectiveDateStart
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE Inventory.UnitConverts SET (LOCK_ESCALATION = TABLE)
GO

COMMIT
