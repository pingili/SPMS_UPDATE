ALTER PROC [dbo].[uspMemberDemandSheetReport]
	@GroupId	INT ,
	@AsOnDate	DATE = NULL,
	@UserId		INT = NULL,
	@MeetingDate DateTime
AS
BEGIN

	--SET @GroupId  = 170

	DECLARE @ActStatusID INT = (SELECT StatusID FROM StatusMaster WHERE StatusCode = 'ACT')

	DECLARE @FINAL_TBL TABLE(MemberId INT, MemberCode VARCHAR(500), MemberName VARCHAR(200), PSDemand INT, SSDemand INT, SLoanPrincipal INT, SLoanInt INT,
		BLoanPrincipal INT, BLoanInt INT, HLoanPrincipal INT, HLoanInt INT, TotalDemand INT)
	
	--DECLARE @MeetingDate DATE
	--SELECT @MeetingDate =  DATEADD(MM, 1, MAX(MeetingDate)) 
	--FROM GroupMeeting GMT JOIN StatusMaster SM ON SM.StatusID = GMT.LockStatus WHERE StatusCode = 'OPEN' AND GroupID = @GroupId
	--SET @MeetingDate = CAST(YEAR(@MeetingDate) AS VARCHAR) + '-' + CAST(MONTH(@MeetingDate) AS VARCHAR) + '-' + (SELECT CAST(MeetingDay AS VARCHAR) FROM GroupMaster WHERE GroupID = @GroupId)

	DECLARE @MemberID	INT
	DECLARE @PSDemand INT, @SSDemand INT, @SLoanPrincipal INT, @SLoanInt INT, @BLoanPrincipal INT, @BLoanInt INT, @HLoanPrincipal INT, @HLoanInt INT

	DECLARE @TBL TABLE(SEQ INT, SLAhId INT, SubAhId INT, DisbursementDate DATE, OutStandingAmount INT, LoanMasterId INT, Demand INT, MemberID INT, AHTechCode VARCHAR(100))
	DECLARE MEMBER_CURSOR CURSOR FOR
	SELECT MemberID FROM Member WHERE GroupID = @GroupId AND StatusID = @ActStatusID

	OPEN MEMBER_CURSOR
	FETCH NEXT FROM MEMBER_CURSOR INTO @MemberID
	WHILE @@Fetch_Status = 0
	BEGIN
		SELECT @PSDemand = 0, @SSDemand = 0, @SLoanPrincipal= 0, @SLoanInt= 0, @BLoanPrincipal= 0, @BLoanInt= 0, @HLoanPrincipal = 0, @HLoanInt = 0
		
		INSERT INTO @TBL
		EXEC dbo.uspMCRptGetMemberDemandLoanDetails @MemberID, @MeetingDate

		SELECT @PSDemand = ISNULL(Demand, 0) FROM @TBL WHERE AHTechCode = 'PRIMARY_SAVINGS' AND MemberID = @MemberID
		SELECT @SSDemand = ISNULL(Demand, 0)  FROM @TBL WHERE AHTechCode = 'SPECIAL_SAVINGS' AND MemberID = @MemberID

		SELECT @SLoanPrincipal = ISNULL(Demand, 0)  FROM @TBL  WHERE AHTechCode = 'SMALLLOAN_PRINCIPAL' AND MemberID = @MemberID
		SELECT @SLoanInt = ISNULL(Demand, 0)  FROM @TBL  WHERE AHTechCode = 'SMALLLOAN_INTEREST' AND MemberID = @MemberID

		SELECT @BLoanPrincipal = ISNULL(Demand, 0)  FROM @TBL  WHERE AHTechCode = 'BIGLOAN_PRINCIPAL' AND MemberID = @MemberID
		SELECT @BLoanInt = ISNULL(Demand, 0)  FROM @TBL  WHERE AHTechCode = 'BIGLOAN_INTEREST' AND MemberID = @MemberID

		SELECT @HLoanPrincipal = ISNULL(Demand, 0)  FROM @TBL  WHERE AHTechCode = 'HOUSINGLOAN_PRINCIPAL' AND MemberID = @MemberID
		SELECT @HLoanInt = ISNULL(Demand, 0)  FROM @TBL  WHERE AHTechCode = 'HOUSINGLOAN_INTEREST' AND MemberID = @MemberID

		INSERT INTO @FINAL_TBL
		SELECT @MemberID, IIF(ISNULL(MemberRefCode, '') = '', MemberCode, MemberRefCode), MemberName, @PSDemand, @SSDemand, @SLoanPrincipal, @SLoanInt, @BLoanPrincipal, @BLoanInt, @HLoanPrincipal, @HLoanInt, 
			(@PSDemand + @SSDemand + @SLoanPrincipal + @SLoanInt + @BLoanPrincipal + @BLoanInt + @HLoanPrincipal + @HLoanInt) AS TotalDemand
		FROM Member WHERE MemberID = @MemberID 
		
		--'PRIMARY_SAVINGS','SPECIAL_SAVINGS', 'PRIMARY_SAVINGS_INT', 'SPECIAL_SAVINGS_INT'
		--'BIGLOAN_PRINCIPAL','BIGLOAN_INTEREST','SMALLLOAN_PRINCIPAL','SMALLLOAN_INTEREST','HOUSINGLOAN_PRINCIPAL','HOUSINGLOAN_INTEREST', 
	FETCH NEXT FROM MEMBER_CURSOR INTO @MemberID
	END
	CLOSE MEMBER_CURSOR
	DEALLOCATE MEMBER_CURSOR

	SELECT * FROM @FINAL_TBL
	UNION ALL
	SELECT 0, '', 'TOTAL', SUM(PSDemand), SUM(SSDemand), SUM(SLoanPrincipal), SUM(SLoanInt), SUM(BLoanPrincipal), SUM(BLoanInt), SUM(HLoanPrincipal), SUM(HLoanInt), sum(TotalDemand)
	FROM @FINAL_TBL
END

