---------------------------------------------
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
ALTER TABLE Inventory.Companies
	DROP CONSTRAINT FK_Companies_UserCreatorId
GO
ALTER TABLE Inventory.FinancialYear
	DROP CONSTRAINT FK_FinancialYear_UserCreatorId
GO
ALTER TABLE Inventory.TimeBucket
	DROP CONSTRAINT FK_TimeBucket_UserCreatorId
GO
ALTER TABLE Inventory.Warehouse
	DROP CONSTRAINT FK_Warehouse_UserCreatorId
GO
ALTER TABLE Inventory.Units
	DROP CONSTRAINT FK_Units_UserCreatorId
GO
ALTER TABLE Inventory.Goods
	DROP CONSTRAINT FK_Goods_UserCreatorId
GO
ALTER TABLE Inventory.UnitConverts
	DROP CONSTRAINT FK_UnitConverts_UserCreatorId
GO
ALTER TABLE Inventory.StoreTypes
	DROP CONSTRAINT FK_StoreTypes_UserCreatorId
GO
ALTER TABLE Inventory.Transactions
	DROP CONSTRAINT FK_Transaction_UserCreatorId
GO
ALTER TABLE Inventory.TransactionItems
	DROP CONSTRAINT FK_TransactionItems_UserCreatorId
GO
ALTER TABLE Inventory.TransactionItemPrices
	DROP CONSTRAINT FK_TransactionItemPrices_UserCreatorId
GO
ALTER TABLE Inventory.Users SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.Companies SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.FinancialYear SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.TransactionItemPrices SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.TransactionItems SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.Transactions SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.StoreTypes SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.UnitConverts SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.Goods SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.Units SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.TimeBucket SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

BEGIN TRANSACTION
GO
ALTER TABLE Inventory.Warehouse SET (LOCK_ESCALATION = TABLE)
GO
COMMIT



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
EXECUTE sp_rename N'Inventory.Users', N'InventoryUsers', 'OBJECT' 
GO
ALTER TABLE Inventory.InventoryUsers SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

-----------------------------------------------------

/****** Object:  View [BasicInfo].[UserView]    Script Date: 5/28/2015 12:16:40 PM ******/
DROP VIEW [Inventory].[Users]
GO

/****** Object:  View [BasicInfo].[UserView]    Script Date: 5/28/2015 12:16:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [Inventory].[Users] AS 

SELECT  CAST(100000 AS INT) AS  Id,  CAST(100000 AS INT) AS Code, 'InventoryAdmin' AS Name, 'InventoryAdmin' AS [User_Name], '****' AS Password, CAST(1 AS BIT) AS IsActive, '' AS Email_Address, NULL 
					AS IPAddress, NULL AS Login, NULL AS SessionId, NULL AS UserCreatorId, GETDATE() AS CreateDate
					
					UNION
					
	SELECT   CAST (Id AS INT) AS Id, CAST (Id AS INT) AS Code, FirstName + ' ' + LastName AS Name, UserName AS [User_Name], '****' AS Password, CAST(1 AS BIT) AS IsActive, Email AS Email_Address, NULL 
					AS IPAddress, NULL AS Login, NULL AS SessionId, NULL AS UserCreatorId, GETDATE() AS CreateDate
	FROM      dbo.Users AS u

GO

ALTER AUTHORIZATION ON [Inventory].[Users] TO  SCHEMA OWNER 
GO

GRANT SELECT ON [Inventory].[Users] TO [public] AS [dbo]
GO


