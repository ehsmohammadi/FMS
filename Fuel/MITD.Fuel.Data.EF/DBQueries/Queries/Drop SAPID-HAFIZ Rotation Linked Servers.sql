USE [master]
GO

/****** Object:  LinkedServer [NGSQLBS,2433]    Script Date: 27/4/2014 5:56:34 PM ******/
EXEC master.dbo.sp_dropserver @server=N'NGSQLBS,2433', @droplogins='droplogins'
GO


/*************************************/

USE [master]
GO

/****** Object:  LinkedServer [NGSQLCNT,2433]    Script Date: 27/4/2014 5:56:42 PM ******/
EXEC master.dbo.sp_dropserver @server=N'NGSQLCNT,2433', @droplogins='droplogins'
GO


