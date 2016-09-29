USE [master]
GO

/****** Object:  LinkedServer [10.9.80.15]    Script Date: 11/26/2014 8:32:17 AM ******/
EXEC master.dbo.sp_addlinkedserver @server = N'10.9.80.15', @srvproduct=N'SQL Server'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'10.9.80.15',@useself=N'False',@locallogin=NULL,@rmtuser=N'FMSVoyageCost',@rmtpassword='QRzHtfDI!'

GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'10.9.80.15', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO


