USE [master]
GO
CREATE LOGIN [FMS] WITH PASSWORD=N'****', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE [offhire]
GO
CREATE USER [FMS] FOR LOGIN [FMS]
GO
USE [offhire]
GO
EXEC sp_addrolemember N'db_datareader', N'FMS'
GO

use [offhire]
GO
GRANT SELECT ON [dbo].[VW_OFFHIRE_CH] TO [FMS]
GO
use [offhire]
GO
GRANT DELETE ON [dbo].[OffhireVoucher] TO [FMS]
GO
use [offhire]
GO
GRANT INSERT ON [dbo].[OffhireVoucher] TO [FMS]
GO
use [offhire]
GO
GRANT SELECT ON [dbo].[OffhireVoucher] TO [FMS]
GO
use [offhire]
GO
GRANT UPDATE ON [dbo].[OffhireVoucher] TO [FMS]
GO
