EXEC sp_Configure filestream_access_level, 2
GO
RECONFIGURE
GO
------------------------------------------------------------------------------
ALTER DATABASE StorageSpace
 ADD FILEGROUP FileStreamFuelAttachment CONTAINS FILESTREAM
GO

ALTER DATABASE StorageSpace
    ADD FILE
      (
       NAME = AttachmentContents,
       FILENAME = 'D:\FuelFiles'
      )
   TO FILEGROUP FileStreamFuelAttachment
GO

------------------------------------------------------------------------------
USE StorageSpace
GO

--GUID Required
CREATE TABLE Fuel.Attachments
(
	RowID INT IDENTITY PRIMARY KEY,
	AttachmentContent VARBINARY(MAX) FILESTREAM,
	AttachmentName  nvarchar(150) not null,
	AttachmentExt nvarchar(10),
	EntityId bigint not null,
	EntityType int not null,
	RowGUID UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() UNIQUE ROWGUIDCOL
)
GO
SP_HELPFileGroup 
GO
