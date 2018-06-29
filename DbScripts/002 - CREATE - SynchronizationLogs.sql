-- USE [ZusContribution]
-- GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SynchronizationLogs]
(
	[SynchronizationLogId] [int] IDENTITY(1,1) NOT NULL,
	[ValidityEndDate] [datetime] NOT NULL,
	[SynchronizationDate] [datetime] NOT NULL,
	[IsSuccess] [bit] NOT NULL
		CONSTRAINT [PK_SynchronizationLog] PRIMARY KEY CLUSTERED 
(
	[SynchronizationLogId] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO