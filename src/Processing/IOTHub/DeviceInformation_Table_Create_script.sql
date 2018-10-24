/****** Object:  Table [dbo].[DeviceInformation]    Script Date: 21/10/2018 8:57:55 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[DeviceInformation](
	[deviceID] [nvarchar](50) NULL,
	[DeviceName] [nvarchar](100) NULL,
	[ClusterName] [nvarchar](100) NULL,
	[BeeKeeperID] [nvarchar](100) NULL,
	[BeeKeeperName] [nvarchar](500) NULL,
	[BeeKeeperAddress] [nvarchar](1000) NULL
) ON [PRIMARY]
GO


