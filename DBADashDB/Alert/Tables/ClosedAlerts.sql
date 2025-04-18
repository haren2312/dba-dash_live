﻿CREATE TABLE Alert.ClosedAlerts(
	AlertID BIGINT NOT NULL,
	InstanceID INT NOT NULL,
	Priority TINYINT NOT NULL,
	AlertType VARCHAR(50) NOT NULL,
	AlertKey NVARCHAR(128) NOT NULL,
	FirstMessage NVARCHAR(MAX) NOT NULL,
	LastMessage NVARCHAR(MAX) NOT NULL,
	TriggerDate DATETIME2 NOT NULL,
	UpdatedDate DATETIME2 NOT NULL,
	FirstNotification DATETIME2 NULL,
	LastNotification DATETIME2 NULL,
	UpdateCount INT NOT NULL CONSTRAINT DF_Alert_ClosedAlerts_UpdateCount DEFAULT(0),
	ResolvedCount INT NOT NULL CONSTRAINT DF_Alert_ClosedAlerts_ResolvedCount DEFAULT(0),
	NotificationCount INT NOT NULL CONSTRAINT DF_Alert_ClosedAlerts_NotificationCount DEFAULT(0),
	FailedNotificationCount INT NOT NULL CONSTRAINT DF_Alert_ClosedAlerts_FailedNotificationCount DEFAULT(0),
	IsAcknowledged BIT NOT NULL CONSTRAINT DF_Alert_ClosedAlerts_IsAcknowledged DEFAULT(0),
	IsResolved BIT NOT NULL CONSTRAINT DF_Alert_ClosedAlerts_IsRevolved DEFAULT(0),
	ResolvedDate DATETIME2 NULL,
	ClosedDate DATETIME2 NOT NULL CONSTRAINT DF_Alert_ClosedAlerts_ClosedDate DEFAULT(SYSUTCDATETIME()),
	Notes NVARCHAR(MAX) NULL,
	RuleID INT NULL,
	AcknowledgedDate DATETIME2 NULL,
	CONSTRAINT FK_ClosedAlerts_Instances FOREIGN KEY(InstanceID) REFERENCES dbo.Instances(InstanceID),
	CONSTRAINT PK_ClosedAlerts PRIMARY KEY(AlertID)
)
GO
CREATE NONCLUSTERED INDEX IX_Alert_ClosedAlerts_ClosedDate ON Alert.ClosedAlerts(ClosedDate)