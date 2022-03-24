﻿CREATE TABLE dbo.Instances (
    InstanceID INT IDENTITY(1, 1) NOT NULL,
    Instance sysname NOT NULL,
    ConnectionID sysname NOT NULL,
    IsActive BIT NULL,
    BuildClrVersion NVARCHAR(128) NULL,
    Collation NVARCHAR(128) NULL,
    CollationID INT NULL,
    ComparisonStyle INT NULL,
    ComputerNamePhysicalNetBIOS NVARCHAR(128) NULL,
    Edition NVARCHAR(128) NULL,
    EditionID BIGINT NULL,
    EngineEdition INT NULL,
    FileStreamConfiguredLevel TINYINT NULL,
    FileStreamEffectiveLevel TINYINT NULL,
    FileStreamShareName NVARCHAR(128) NULL,
    HadrManagerStatus TINYINT NULL,
    InstanceDefaultDataPath NVARCHAR(260) NULL,
    InstanceDefaultLogPath NVARCHAR(260) NULL,
    InstanceName NVARCHAR(128) NULL,
    IsAdvancedAnalyticsInstalled BIT NULL,
    IsClustered INT NULL,
    IsFullTextInstalled INT NULL,
    IsHadrEnabled INT NULL,
    IsIntegratedSecurityOnly INT NULL,
    IsLocalDB INT NULL,
    IsPolybaseInstalled INT NULL,
    IsXTPSupported INT NULL,
    LCID INT NULL,
    LicenseType NVARCHAR(128) NULL,
    MachineName NVARCHAR(128) NULL,
    NumLicenses INT NULL,
    ProductBuild NVARCHAR(128) NULL,
    ProductBuildType NVARCHAR(128) NULL,
    ProductLevel NVARCHAR(128) NULL,
    ProductMajorVersion NVARCHAR(128) NULL,
    ProductUpdateLevel NVARCHAR(128) NULL,
    ProductUpdateReference NVARCHAR(128) NULL,
    ProductVersion NVARCHAR(128) NULL,
    ResourceLastUpdateDateTime DATETIME NULL,
    ResourceVersion NVARCHAR(128) NULL,
    ServerName NVARCHAR(128) NULL,
    SqlCharSet TINYINT NULL,
    SqlCharSetName NVARCHAR(128) NULL,
    SqlSortOrder TINYINT NULL,
    SqlSortOrderName NVARCHAR(128) NULL,
    ActivePowerPlanGUID UNIQUEIDENTIFIER NULL,
    ActivePowerPlan VARCHAR(16) NULL,
    ProcessorNameString NVARCHAR(512) NULL,
    SystemManufacturer NVARCHAR(512) NULL,
    SystemProductName NVARCHAR(512) NULL,
    IsAgentRunning BIT NULL,
    InstantFileInitializationEnabled BIT NULL,
    OfflineSchedulers INT NULL,
    affinity_type INT NULL,
    cores_per_socket INT NULL,
    cpu_count INT NULL,
    hyperthread_ratio INT NULL,
    ms_ticks BIGINT NULL,
    numa_node_count INT NULL,
    os_priority_class INT NULL,
    physical_memory_kb BIGINT NULL,
    socket_count INT NULL,
    softnuma_configuration INT NULL,
    sql_memory_model INT NULL,
    sqlserver_start_time DATETIME NULL,
    max_workers_count INT NULL,
    scheduler_count INT NULL,
    ResourceGovernorEnabled BIT NULL,
    UTCOffset INT NULL,
    host_release NVARCHAR(256) NULL,
    host_service_pack_level NVARCHAR(256) NULL,
    host_sku NVARCHAR(256) NULL,
    LastMemoryDump DATETIME NULL,
    MemoryDumpCount INT NULL,
    WindowsCaption NVARCHAR(256) NULL,
    host_distribution NVARCHAR(256) NULL,
    os_language_version INT NULL,
    host_platform NVARCHAR(256) NULL,
    DBMailStatus NVARCHAR(500) NULL,
    UptimeAckDate DATETIME NULL,
    CollectAgentID INT NULL,
    ImportAgentID INT NULL,
    Alias NVARCHAR(128) NULL,
    InstanceDisplayName AS ISNULL(Alias,ConnectionID),
    InstanceGroupName AS CASE WHEN EngineEdition=5 THEN Instance ELSE ISNULL(Alias,ConnectionID) END,
    CONSTRAINT PK_Instances PRIMARY KEY CLUSTERED (InstanceID ASC),
    CONSTRAINT FK_Instances_CollectAgent FOREIGN KEY(CollectAgentID) REFERENCES dbo.DBADashAgent(DBADashAgentID),
    CONSTRAINT FK_Instances_ImportAgent FOREIGN KEY(ImportAgentID) REFERENCES dbo.DBADashAgent(DBADashAgentID)
);

GO
CREATE UNIQUE NONCLUSTERED INDEX IX_Instances_ConnectionID
    ON dbo.Instances([ConnectionID] ASC);

GO
CREATE UNIQUE NONCLUSTERED INDEX IX_Instances_InstanceDisplayName ON dbo.Instances(InstanceDisplayName)