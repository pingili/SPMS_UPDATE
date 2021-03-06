USE [SPMSUAT]
GO
/****** Object:  StoredProcedure [dbo].[uspGroupMeetingLock]    Script Date: 11/3/2017 1:45:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Proc [dbo].[uspGroupMeetingLock]
(@GroupId int,
 @UserId int,
 @type varchar(50),
 @meetingId int)
 
AS 
BEGIN
   IF(@type='L')
   BEGIN
   INSERT INTO GroupmeetingLockHistory (GroupMeetingId, LockedBy,LockedDate)
       SELECT GroupMeetingId, @UserId, Getdate() FROM GroupMeeting 
	   where GroupId= @GroupId and LockStatus= (SELECT StatusID From StatusMaster where StatusCode='OPEN')

	   UPDATE GroupMeeting 
   SET LockStatus = (SELECT StatusID From StatusMaster where StatusCode='LOCKED'),
   ModifiedBy=@UserId,
   ModifiedOn=GETDATE()
   WHERE GROUPID=@GroupId and GroupMeetingID=@meetingId
   END
   IF(@type='U')
   BEGIN
    INSERT INTO GroupmeetingLockHistory (GroupMeetingId, LockedBy,LockedDate)
       SELECT GroupMeetingId, @UserId, Getdate() FROM GroupMeeting 
	   where GroupId= @GroupId and LockStatus= (SELECT StatusID From StatusMaster where StatusCode='UNLOCK')

	   UPDATE GroupMeeting 
   SET LockStatus = (SELECT StatusID From StatusMaster where StatusCode='OPEN'),
   ModifiedBy=@UserId,
   ModifiedOn=GETDATE()
   WHERE GROUPID=@GroupId and GroupMeetingID=@meetingId
   END
END
