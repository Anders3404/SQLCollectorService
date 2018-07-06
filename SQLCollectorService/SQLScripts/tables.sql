USE [IDBMGMT]
GO
/****** Object:  Schema [collector]    Script Date: 05-07-2018 12:13:48 ******/
CREATE SCHEMA [collector]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_collectionDate]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[fn_collectionDate]
(@CollectID int)
Returns datetime
as
BEGIN
return (select
	CollectDate
from
	collector.collections with (NOLOCK)
where
	CollectID=@CollectID)
END

GO
/****** Object:  UserDefinedFunction [dbo].[fn_currentcollection]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE function [dbo].[fn_currentcollection]
(@servername sysname)
Returns bigint
as
BEGIN
return (select
	max(CollectID)
from
	collector.collections with (NOLOCK)
where
	servername=@servername)
END
GO
/****** Object:  Table [collector].[backuptime]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [collector].[backuptime](
	[servername] [varchar](32) NULL,
	[dbname] [varchar](128) NULL,
	[size] [decimal](18, 10) NULL,
	[comp_size] [decimal](18, 10) NULL,
	[runtime] [int] NULL,
	[rundate] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [collector].[Collections]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [collector].[Collections](
	[CollectID] [bigint] IDENTITY(1,1) NOT NULL,
	[CollectDate] [datetime] NULL DEFAULT (getdate()),
	[Servername] [sysname] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [collector].[databases]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [collector].[databases](
	[CollectID] [int] NULL,
	[CollectDate] [datetime] NULL,
	[Servername] [varchar](128) NULL,
	[CurrentHost] [varchar](250) NULL,
	[ClusterNodes] [varchar](250) NULL,
	[DB] [varchar](250) NULL,
	[FileType] [varchar](250) NULL,
	[Name] [varchar](250) NULL,
	[VolumeOrDrive] [varchar](250) NULL,
	[FileName] [varchar](250) NULL,
	[Filesize] [decimal](15, 2) NULL,
	[SpaceUsedInFile] [decimal](15, 2) NULL,
	[AvailableSpaceInFile] [decimal](15, 2) NULL,
	[createdate] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [collector].[diskdrive]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [collector].[diskdrive](
	[CollectID] [int] NULL,
	[CollectDate] [datetime] NULL CONSTRAINT [DF__diskdrive__Colle__0A9D95DB]  DEFAULT (getdate()),
	[Servername] [varchar](128) NULL,
	[Hostname] [varchar](128) NULL,
	[drivename] [varchar](255) NULL,
	[capacity] [float] NULL,
	[freespace] [float] NULL,
	[percentfree] [float] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [collector].[LastBackupStatus]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [collector].[LastBackupStatus](
	[CollectID] [int] NOT NULL,
	[CollectDate] [datetime] NULL DEFAULT (getdate()),
	[Servername] [nvarchar](128) NULL,
	[DatabaseName] [sysname] NOT NULL,
	[RecoveryLevel] [varchar](32) NOT NULL,
	[LastFullBackup] [datetime] NULL,
	[LastDiffBackup] [datetime] NULL,
	[LastLogBackup] [datetime] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [collector].[serverinfo]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [collector].[serverinfo](
	[CollectID] [int] NULL,
	[CollectDate] [datetime] NOT NULL,
	[Servername] [nvarchar](128) NULL,
	[IsClustered] [varchar](3) NOT NULL,
	[Edition] [sql_variant] NULL,
	[ProductVersion] [sql_variant] NULL,
	[VersionName] [nvarchar](300) NULL,
	[ProductLevel] [sql_variant] NULL,
	[Collation] [sql_variant] NULL,
	[ComputerNamePhysicalNetBIOS] [sql_variant] NULL,
	[MachineName] [sql_variant] NULL,
	[InstanceName] [sql_variant] NULL,
	[InstanceDefaultDataPath] [sql_variant] NULL,
	[InstanceDefaultLogPath] [sql_variant] NULL,
	[IsIntegratedSecurityOnly] [sql_variant] NULL,
	[IsHadrEnabled] [sql_variant] NULL,
	[HadrManagerStatus] [sql_variant] NULL,
	[Nodes] [varchar](128) NULL,
	[IsXTPSupported] [sql_variant] NULL,
	[SQL Server Install Date] [datetime] NOT NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [collector].[services]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [collector].[services](
	[CollectID] [int] NULL,
	[CollectDate] [datetime] NULL CONSTRAINT [DF__services__Collec__7B5B524B]  DEFAULT (getdate()),
	[Servername] [varchar](128) NULL,
	[Servicename] [varchar](128) NULL,
	[ServiceStatus] [varchar](128) NULL,
	[StartUp] [varchar](128) NULL,
	[ServiceAccount] [varchar](128) NULL,
	[BinaryPath] [varchar](256) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [collector].[sqlagentjobs]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [collector].[sqlagentjobs](
	[CollectID] [int] NULL,
	[CollectDate] [datetime] NULL DEFAULT (getdate()),
	[Servername] [nvarchar](128) NULL,
	[Job Name] [sysname] NOT NULL,
	[Job Enabled] [varchar](3) NULL,
	[Frequency] [varchar](27) NULL,
	[StartDateTime] [datetime] NULL,
	[Max Duration] [char](8) NULL,
	[Subday Frequency] [varchar](16) NULL,
	[FailuresLast7Days] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [collector].[traceflag]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [collector].[traceflag](
	[CollectID] [int] NULL,
	[CollectDate] [datetime] NULL DEFAULT (getdate()),
	[Servername] [varchar](128) NULL,
	[TraceFlag] [int] NULL,
	[TraceStatus] [char](1) NULL,
	[TraceGlobal] [char](1) NULL,
	[TraceSession] [char](1) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [collector].[get_collectID]    Script Date: 05-07-2018 12:13:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE proc [collector].[usp_NewCollection]
@Servername sysname,
@CollectID bigint OUTPUT
as
insert into collector.Collections (Servername)
values (@Servername)
set @CollectID=@@IDENTITY


GO
