/****** Object:  View [BasicInfo].[AccountListView]    Script Date: 9/17/2014 10:53:10 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [BasicInfo].[AccountListView]
AS
SELECT DISTINCT AccountCode AS Code, Name
FROM	(SELECT AccountCode, Name
			FROM  dbo.HAFIZAccountListView
		UNION
		SELECT  AccountCode, Name
			FROM  dbo.SAPIDAccountListView) AS tb
WHERE        (AccountCode <> '')

GO

GRANT SELECT ON [BasicInfo].[AccountListView] TO [public];

GO