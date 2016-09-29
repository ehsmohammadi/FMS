USE [master]
GO
CREATE LOGIN [FMSVoyageCost] WITH PASSWORD=N'QRzHtfDI!', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF
GO
USE VoyageCost
GO
CREATE USER [FMSVoyageCost] FOR LOGIN [FMSVoyageCost]
GO
USE VoyageCost
GO
EXEC sp_addrolemember N'db_datareader', N'FMSVoyageCost'
GO

use VoyageCost
GO
GRANT SELECT ON [dbo].[RPMInfo] TO [FMSVoyageCost]
GO
use VoyageCost
GO
GRANT UPDATE ON [dbo].[RPMInfo] TO [FMSVoyageCost]
GO
use VoyageCost
GO
GRANT DELETE ON [dbo].[FuelReportLog] TO [FMSVoyageCost]
GO
use VoyageCost
GO
GRANT INSERT ON [dbo].[FuelReportLog] TO [FMSVoyageCost]
GO
use VoyageCost
GO
GRANT SELECT ON [dbo].[FuelReportLog] TO [FMSVoyageCost]
GO
use VoyageCost
GO
GRANT UPDATE ON [dbo].[FuelReportLog] TO [FMSVoyageCost]
GO
