﻿CREATE TABLE [dbo].[Instances] (
    [InstanceID]                   INT            IDENTITY (1, 1) NOT NULL,
    [Instance]                     [sysname]      NOT NULL,
    [ConnectionID]                 [sysname]      NOT NULL,
    [IsActive]                     BIT            NULL,
    [LogRestoreSnapshotDate]       DATETIME2 (2)  NULL,
    [BuildClrVersion]              NVARCHAR (128) NULL,
    [Collation]                    NVARCHAR (128) NULL,
    [CollationID]                  INT            NULL,
    [ComparisonStyle]              INT            NULL,
    [ComputerNamePhysicalNetBIOS]  NVARCHAR (128) NULL,
    [Edition]                      NVARCHAR (128) NULL,
    [EditionID]                    BIGINT         NULL,
    [EngineEdition]                INT            NULL,
    [FileStreamConfiguredLevel]    TINYINT        NULL,
    [FileStreamEffectiveLevel]     TINYINT        NULL,
    [FileStreamShareName]          NVARCHAR (128) NULL,
    [HadrManagerStatus]            TINYINT        NULL,
    [InstanceDefaultDataPath]      NVARCHAR (260) NULL,
    [InstanceDefaultLogPath]       NVARCHAR (260) NULL,
    [InstanceName]                 NVARCHAR (128) NULL,
    [IsAdvancedAnalyticsInstalled] BIT            NULL,
    [IsClustered]                  INT            NULL,
    [IsFullTextInstalled]          INT            NULL,
    [IsHadrEnabled]                INT            NULL,
    [IsIntegratedSecurityOnly]     INT            NULL,
    [IsLocalDB]                    INT            NULL,
    [IsPolybaseInstalled]          INT            NULL,
    [IsXTPSupported]               INT            NULL,
    [LCID]                         INT            NULL,
    [LicenseType]                  NVARCHAR (128) NULL,
    [MachineName]                  NVARCHAR (128) NULL,
    [NumLicenses]                  INT            NULL,
    [ProductBuild]                 NVARCHAR (128) NULL,
    [ProductBuildType]             NVARCHAR (128) NULL,
    [ProductLevel]                 NVARCHAR (128) NULL,
    [ProductMajorVersion]          NVARCHAR (128) NULL,
    [ProductUpdateLevel]           NVARCHAR (128) NULL,
    [ProductUpdateReference]       NVARCHAR (128) NULL,
    [ProductVersion]               NVARCHAR (128) NULL,
    [ResourceLastUpdateDateTime]   DATETIME       NULL,
    [ResourceVersion]              NVARCHAR (128) NULL,
    [ServerName]                   NVARCHAR (128) NULL,
    [SqlCharSet]                   TINYINT        NULL,
    [SqlCharSetName]               NVARCHAR (128) NULL,
    [SqlSortOrder]                 TINYINT        NULL,
    [SqlSortOrderName]             NVARCHAR (128) NULL,
    [SnapshotDate]                 DATETIME       NULL,
    CONSTRAINT [PK_Instances] PRIMARY KEY CLUSTERED ([InstanceID] ASC)
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Instances_ConnectionID]
    ON [dbo].[Instances]([ConnectionID] ASC);

