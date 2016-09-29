USE [master]
GO

--set implicit_transactions off

/****** Object:  LinkedServer [EVENTREPORT]    Script Date: 1/28/2016 2:51:42 PM ******/
EXEC master.dbo.sp_addlinkedserver @server = N'EVENTREPORT', @srvproduct=N'10.0.30.28', @provider=N'SQLNCLI', @datasrc=N'10.0.30.28', @catalog=N'EventReport'
 /* For security reasons the linked server remote logins password is changed with ######## */
EXEC master.dbo.sp_addlinkedsrvlogin @rmtsrvname=N'EVENTREPORT',@useself=N'False',@locallogin=NULL,@rmtuser=N'FMS',@rmtpassword='AmDhnoRx#'

GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'collation compatible', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'data access', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'dist', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'pub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'rpc', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'rpc out', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'sub', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'connect timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'collation name', @optvalue=null
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'lazy schema validation', @optvalue=N'false'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'query timeout', @optvalue=N'0'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'use remote collation', @optvalue=N'true'
GO

EXEC master.dbo.sp_serveroption @server=N'EVENTREPORT', @optname=N'remote proc transaction promotion', @optvalue=N'true'
GO

--set implicit_transactions on