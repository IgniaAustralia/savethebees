/****** Object:  Table [dbo].[SensorData]    Script Date: 21/10/2018 10:01:24 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SensorData](
	[DeviceId] [nvarchar](50) NULL,
	[Internal Hive Temp (celcius)] [numeric](20, 10) NULL,
	[Internal Hive Pressure] [numeric](20, 10) NULL,
	[Internal Hive Hummidity] [numeric](20, 10) NULL,
	[External Temp] [numeric](20, 10) NULL,
	[External Pressure] [numeric](20, 10) NULL,
	[External Humidity] [numeric](20, 10) NULL,
	[Weight] [numeric](20, 10) NULL,
	[Internal Hive Loudness] [numeric](20, 10) NULL,
	[Vibration State] [numeric](20, 10) NULL,
	[Lat] [numeric](20, 10) NULL,
	[Long] [numeric](20, 10) NULL,
	[EventEnqueuedUtcTime] [datetime] NULL
) ON [PRIMARY]
GO


