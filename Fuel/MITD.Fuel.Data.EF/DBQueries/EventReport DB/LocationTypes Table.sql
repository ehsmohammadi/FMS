USE [EventReport]
GO

/****** Object:  Table [dbo].[LocationTypes]    Script Date: 2/10/2015 10:16:33 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LocationTypes](
	[Type] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_LocationTypes] PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER AUTHORIZATION ON [dbo].[LocationTypes] TO  SCHEMA OWNER 
GO

GRANT SELECT ON [dbo].[LocationTypes] TO [public] AS [dbo]
GO


INSERT INTO [dbo].[LocationTypes]([Type],NAME) VALUES (1 , 'At Sea')
INSERT INTO [dbo].[LocationTypes]([Type],NAME) VALUES (2 , 'In Port')
INSERT INTO [dbo].[LocationTypes]([Type],NAME) VALUES (3 , 'At Anchorage')