IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[BasicInfo].[DF_ActivityLocation_CountryName]') AND type = 'D')
BEGIN
ALTER TABLE [BasicInfo].[ActivityLocation] DROP CONSTRAINT [DF_ActivityLocation_CountryName]
END

GO
/****** Object:  Index [IX_ActivityLocation_Code]    Script Date: 7/3/2014 12:50:22 PM ******/
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[BasicInfo].[ActivityLocation]') AND name = N'IX_ActivityLocation_Code')
DROP INDEX [IX_ActivityLocation_Code] ON [BasicInfo].[ActivityLocation]
GO
/****** Object:  Table [BasicInfo].[ActivityLocation]    Script Date: 7/3/2014 12:50:22 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[BasicInfo].[ActivityLocation]') AND type in (N'U'))
DROP TABLE [BasicInfo].[ActivityLocation]
GO
