USE [master]
GO

/****** Object:  LinkedServer [NGSQLHDA,2433]    Script Date: 3/8/2015 2:39:18 PM ******/
EXEC master.dbo.sp_addlinkedserver @server = N'NGSQLHDA,2433', @srvproduct=N'10.8.14.13,2433', @provider=N'SQLNCLI', @datasrc=N'10.8.14.13,2433'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'NGSQLHDA,2433',@useself=N'False',@locallogin=NULL,@rmtuser=N'hdaituser',@rmtpassword='it1234'

GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'NGSQLHDA,2433', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


