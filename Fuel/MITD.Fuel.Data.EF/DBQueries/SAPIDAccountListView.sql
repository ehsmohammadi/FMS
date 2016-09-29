/****** Object:  View [dbo].[SAPIDAccountListView]    Script Date: 9/17/2014 10:54:35 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SAPIDAccountListView]
AS
SELECT        AccountListID, ParentID, AccountCode, Nature, LevelCode, Name, NameL, Disabled
FROM            [NGSQLBS,2433].BLKAcc93.dbo.AccountList

GO

GRANT SELECT ON [dbo].[SAPIDAccountListView] TO [public];

GO