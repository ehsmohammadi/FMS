/****** Object:  Table [dbo].[FuelReportLog]    Script Date: 7/28/2014 2:35:39 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[FuelReportLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RecordId] [bigint] NOT NULL,
	[FailureMessage] [nvarchar](1024) NOT NULL,
	[StackTrace] [nvarchar](max) NOT NULL,
	[Date] [datetime] NOT NULL,
 CONSTRAINT [PK_FuelReportLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[FuelReportLog] ADD  CONSTRAINT [DF_FuelReportLog_Date]  DEFAULT (getdate()) FOR [Date]
GO


