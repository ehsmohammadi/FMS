--use master
--GO 

--DECLARE @MiniStockDataFilePath NVARCHAR(4000),
--	@MiniStockLogFilePath NVARCHAR(4000)

--SELECT @MiniStockDataFilePath = CAST(SERVERPROPERTY('instancedefaultdatapath') AS NVARCHAR(4000)) + 'MiniStock.MDF',
--	@MiniStockLogFilePath = CAST(SERVERPROPERTY('instancedefaultlogpath') AS NVARCHAR(4000)) + 'MiniStock_Log.ldf';

--IF NOT EXISTS (SELECT * FROM sys.sysdatabases s WHERE s.name='MiniStock')
--	EXECUTE ('CREATE DATABASE MiniStock ON PRIMARY
--		(NAME = ''MiniStock'', FILENAME =  ''' + @MiniStockDataFilePath + ''', SIZE = 6000KB , FILEGROWTH = 1024KB )
--		LOG ON
--		(NAME = ''MiniStock_log'', FILENAME = ''' + @MiniStockLogFilePath + ''', SIZE = 1024KB , FILEGROWTH = 10%)');
--GO

--USE MiniStock 
--GO
CREATE SCHEMA Inventory
GO

USE MASTER
GO
EXEC SP_CONFIGURE 'show advanced options',1
GO
RECONFIGURE
GO
EXEC sp_configure 'xp_cmdshell', 1
GO
RECONFIGURE
GO

ALTER DATABASE StorageSpace SET ALLOW_SNAPSHOT_ISOLATION ON;
GO
ALTER DATABASE StorageSpace SET READ_COMMITTED_SNAPSHOT ON;
GO