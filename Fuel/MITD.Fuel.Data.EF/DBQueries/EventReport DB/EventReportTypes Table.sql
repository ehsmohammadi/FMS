USE [EventReport]
GO

/****** Object:  Table [dbo].[EventReportTypes]    Script Date: 2/10/2015 10:04:28 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EventReportTypes](
	[Type] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_EventReportTypes] PRIMARY KEY CLUSTERED 
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER AUTHORIZATION ON [dbo].[EventReportTypes] TO  SCHEMA OWNER 
GO

GRANT SELECT ON [dbo].[EventReportTypes] TO [public] AS [dbo]
GO



INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (1 , 'Noon Report')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (2 , 'End Of Voyage')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (3 , 'Arrival Report')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (4 , 'Departure Report')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (5 , 'End Of Year')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (6 , 'End Of Month')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (7 , 'Charter-In End')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (8 , 'Charter-Out Start')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (9 , 'Dry Dock')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (10 , 'Begin Of Off-Hire')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (11 , 'Lay-Up')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (12 , 'End Of Off-Hire')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (13 , 'Begin Of Passage')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (14 , 'End Of Passage')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (15 , 'Bunkering')
INSERT INTO [dbo].[EventReportTypes]([Type],NAME) VALUES (16 , 'Debunkering')
