/****** Object:  Table [dbo].[SensorData]    Script Date: 21/10/2018 8:58:32 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SensorData](
	[deviceID] [nvarchar](50) NULL,
	[Temperature] [float] NULL,
	[Weight] [float] NULL,
	[Longitude] [numeric](20, 10) NULL,
	[Lattitude] [numeric](20, 10) NULL,
	[EventEnqueuedUtcTime] [datetime] NULL
) ON [PRIMARY]
GO


