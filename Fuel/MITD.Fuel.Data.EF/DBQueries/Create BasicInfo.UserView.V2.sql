
/****** Object: View [BasicInfo].[UserView] Script Date: 3/6/2014 5:24:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/****** Object:  View [BasicInfo].[UserView]    Script Date: 3/6/2014 10:59:17 AM ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[BasicInfo].[UserView]'))
DROP VIEW [BasicInfo].[UserView]
GO

CREATE VIEW [BasicInfo].[UserView] AS 
SELECT 
	u.Id AS Id,  --User Id in Company
	u.Id AS IdentityId, --Id in Users Table
	u.LastName + ', ' + u.FirstName AS Name, 
	u.CompanyId  --Assigned Company to User
FROM      dbo.Users u
	/*INNER JOIN Fuel.UserInCompany uc ON u.Id = uc.UserId
    INNER JOIN BasicInfo.CompanyView c ON uc.CompanyId = c.Id*/