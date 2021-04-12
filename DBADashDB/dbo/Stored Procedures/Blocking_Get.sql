﻿CREATE PROC [dbo].[Blocking_Get](@BlockingSnapshotID INT,@blocking_session_id INT)
AS
SELECT 
       BSS.session_id,
       BSS.blocking_session_id,
       ISNULL(BSS.Txt,'') Txt,
       BSS.start_time_utc,
       BSS.command,
       BSS.database_id,
       BSS.database_name,
       BSS.host_name,
       BSS.program_name,
       BSS.wait_time,
       BSS.login_name,
       BSS.wait_resource,
       BSS.Status,
	   stat.BlockCountRecursive,
       stat.WaitTimeRecursive,
       stat.BlockCount,
       stat.BlockWaitTime
FROM dbo.BlockingSnapshot BSS
OUTER APPLY dbo.BlockingSnapshotRecursiveStats(BSS.BlockingSnapshotID,BSS.session_id,BSS.SnapshotDateUTC) stat
WHERE BSS.BlockingSnapshotID = @BlockingSnapshotID
AND (BSS.blocking_session_id=@blocking_session_id
		OR (@blocking_session_id=0
			/* 
			Session is not blocked by another sessionid in this snapshot. 
			(Either root blocker with blocking_session_id = 0, or we did not capture the root blocker)
			*/
			AND NOT EXISTS(SELECT 1 
						FROM dbo.BlockingSnapshot BSS2 
						WHERE BSS2.BlockingSnapshotID=BSS.BlockingSnapshotID
						AND BSS2.session_id=BSS.blocking_session_id
						)
			)
	)