ALTER PROCEDURE [dbo].[uspMemberByGroupId]
@GroupId Int
AS
--=========================================
--Author : Amarender 
--Creation Date : 6 Feb, 2016
--Purpose : To Get the MemberByGroupId.
--Exec [uspMemberByGroupId] 282
--===========================================
--Modified By	Modified On		Purpose
--xxxxxxx		xxxxxxxxxx		xxxxxxxxx.	
--==========================================

BEGIN
	SELECT 
MemberID,
MemberCode,
MemberName+' ('+MemberCode+')' as MemberName,
G.GroupName,
Gender,
DateOfAdmission,
DOB,
O.Occupation,
ParentGuardianName,
SM.[Status]
	FROM Member	m	
	    JOIN GroupMaster G on G.GroupID=m.GroupID
	    LEFT JOIN Occupation O on O.OccupationID=m.OccupationID  
		JOIN StatusMaster SM on SM.StatusID = m.StatusID WHERE M.GroupID=@GroupId AND StatusCode = 'ACT'
	ORDER BY MemberID DESC		
END
	
	


