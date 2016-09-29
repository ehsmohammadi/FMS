/****** Object:  View [dbo].[HAFIZAccountListView]    Script Date: 9/17/2014 10:54:11 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[HAFIZAccountListView]
AS
SELECT        AccountListID, ParentID, AccountCode, Nature, LevelCode, Name, NameL, Disabled
FROM            [NGSQLCNT,2433].CNTAcc93.dbo.AccountList 

GO

GRANT SELECT ON [dbo].[HAFIZAccountListView] TO [public];

GO