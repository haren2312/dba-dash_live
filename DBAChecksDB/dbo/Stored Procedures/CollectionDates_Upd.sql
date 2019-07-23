﻿CREATE PROC CollectionDates_Upd(@InstanceID INT,@Reference VARCHAR(30),@SnapshotDate DATETIME)
AS
UPDATE dbo.CollectionDates
SET SnapshotDate=@SnapshotDate 
WHERE InstanceID=@InstanceID 
AND Reference = @Reference
IF @@ROWCOUNT=0
BEGIN
	INSERT INTO dbo.CollectionDates
	(
		InstanceID,
		Reference,
		SnapshotDate
	)
	VALUES
	(   @InstanceID,
		@Reference,
		@SnapshotDate)
END