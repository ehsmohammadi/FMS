/****** Object:  View [dbo].[HAFEZAccountListView]    Script Date: 9/17/2014 10:54:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[HAFEZAccountListView]'))
DROP VIEW [dbo].[HAFEZAccountListView]
GO

CREATE VIEW [dbo].[HAFEZAccountListView]
AS
SELECT        AccountListID, ParentID, AccountCode, Nature, LevelCode, Name, NameL, Disabled
FROM            [NGSQLHDA,2433].HDAAcc93.dbo.AccountList 

GO

GRANT SELECT ON [dbo].[HAFEZAccountListView] TO [public];

GO