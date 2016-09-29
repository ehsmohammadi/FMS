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
ALTER TABLE Inventory.Units ADD
	Code nvarchar(256) NOT NULL CONSTRAINT DF_Units_Code DEFAULT N'_'
GO
ALTER TABLE Inventory.Units SET (LOCK_ESCALATION = TABLE)
GO

UPDATE Inventory.Units 
SET Code = Abbreviation 

UPDATE Inventory.Units 
SET Abbreviation = N'IRR'
WHERE Abbreviation = N'011'

UPDATE Inventory.Units 
SET Abbreviation = N'USD'
WHERE Abbreviation = N'066'

UPDATE Inventory.Units 
SET Abbreviation = N'EUR'
WHERE Abbreviation = N'098'

COMMIT