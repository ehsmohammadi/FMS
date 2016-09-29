USE [master]
GO

/****** Object:  LinkedServer [NGSQLBS,2433]    Script Date: 27/4/2014 5:55:33 PM ******/
EXEC master.dbo.sp_addlinkedserver @server = N'NGSQLBS,2433', @srvproduct=N'SQL Server'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'NGSQLBS,2433',@useself=N'False',@locallogin=NULL,@rmtuser=N'blkituser',@rmtpassword='it1234'

GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLBS,2433', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO



/****************************************/
USE [master]
GO

/****** Object:  LinkedServer [NGSQLCNT,2433]    Script Date: 27/4/2014 5:55:46 PM ******/
EXEC master.dbo.sp_addlinkedserver @server = N'NGSQLCNT,2433', @srvproduct=N'SQL Server'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'NGSQLCNT,2433',@useself=N'False',@locallogin=NULL,@rmtuser=N'cntituser',@rmtpassword='it1234'

GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLCNT,2433', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


