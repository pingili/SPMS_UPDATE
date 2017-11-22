ALTER Proc uspGetGroupMeetings
(@GroupID int)
AS
BEGIN
  select Distinct MeetingDate meetingDate from GroupMeeting where GroupID=@GroupId
END
--Exec uspGetGroupMeetings 282