--EXEC [uspMemberConfirmationReport] 282
ALTER PROC [dbo].[uspMemberConfirmationReport]
	@GroupId	INT ,
	@AsOnDate	DATE = NULL,
	@UserId		INT = NULL
AS
BEGIN
 --   Declare @GroupId int
	--SET @GroupId  = 282

	DECLARE @MeetingDate DATE
	SELECT @MeetingDate =  MAX(MeetingDate)
	FROM GroupMeeting GMT JOIN StatusMaster SM ON SM.StatusID = GMT.LockStatus WHERE StatusCode = 'OPEN' AND GroupID = @GroupId

	DECLARE @AHID_OB_TBL TABLE(AHID INT, SLAHID INT, CrAmount DECIMAL(18, 2), DrAmount DECIMAL(18, 2), OpeningBalanceType VARCHAR(50), AHTechCode VARCHAR(50))
	DECLARE @FYStartDate DATE = (SELECT SettingValue FROM SystemSettings WHERE SettingName=  'FINANCIAL_YEAR_BEGIN')
	DECLARE @STATUSID INT = (SELECT StatusID FROM StatusMaster WHERE StatusCode = 'ACT')

	DECLARE @FINAL_TBL TABLE(MemberId INT, MemberCode VARCHAR(500), MemberName VARCHAR(200), PSPrincipal INT, PSInt INT, SSPrincipal INT, SSInt INT, SLoanDate DATE, SLoanPrincipal INT, SLoanInt INT,
		BLoanDate DATE, BLoanPrincipal INT, BLoanInt INT, HLoanDate DATE, HLoanPrincipal INT, HLoanInt INT)

	INSERT INTO @AHID_OB_TBL(AHID, OpeningBalanceType, AHTechCode)
	SELECT
		AH.AHID, OpeningBalanceType, SettingName--, AH.AHCode, AH.AHName, AH.ParentAHID, AH.OpeningBalanceType, SAH.*
	FROM SystemSettings SS left JOIN AccountHead AH ON AH.AHID = CONVERT(INT, SS.SettingValue)-- JOIN AccountHead SAH ON SAH.ParentAHID = AH.AHID
	WHERE SettingName IN (--'BIGLOAN_PRINCIPAL','BIGLOAN_INTEREST','SMALLLOAN_PRINCIPAL','SMALLLOAN_INTEREST','HOUSINGLOAN_PRINCIPAL','HOUSINGLOAN_INTEREST', 
		'PRIMARY_SAVINGS','SPECIAL_SAVINGS', 'PRIMARY_SAVINGS_INT', 'SPECIAL_SAVINGS_INT')
	AND AH.IsFederation = 0

	INSERT INTO @AHID_OB_TBL(AHID, SLAHID, AHTechCode)
	SELECT A.ParentAHID, A.AHID, T.AHTechCode  FROM AccountHead A JOIN @AHID_OB_TBL T ON ParentAHID = t.AHID JOIN Deposits D ON D.SLAccountAHID = A.AHID
	WHERE D.GroupID = @GroupId 

	DECLARE @TBL_TOTAL_ASONDATE TABLE(AHID INT, MemberID INT, AHTechCode VARCHAR(100), CrAmount INT, DrAmount INT, BALANCE INT)

	IF OBJECT_ID('tempdb..#ENTRIES') IS NOT NULL DROP TABLE #ENTRIES

	SELECT * INTO #ENTRIES 
	FROM(
		--SELECT 0 ROWID, @AHID AHID, @AHCODE AHCODE, @AHNAME AHNAME, @TDATE TDATE, @DROB DrAmount, @CROB CrAmount, Bal =  IIF(@OBTYPE = 'CR', @CROB, - @DROB)

		SELECT
			--ROW_NUMBER() OVER(ORDER BY T.AHID) rowid, 
			T.AHID AS AHID,
			t.AHTechCode,
			M.MemberID,
			@FYStartDate AS TDATE,
			SUM(ISNULL(D.DepositAcmount, 0)) AS CrAmount,
			SUM(ISNULL(L.DisbursedAmount, 0)) AS DrAmount,
			IIF(a.OpeningBalanceType = 'CR', 
				SUM(ISNULL(D.DepositAcmount, 0)),
				-SUM(ISNULL(L.DisbursedAmount, 0)) 
			) AS BALANCE
		FROM AccountHead A
			LEFT JOIN Deposits D ON A.AHID = D.SLAccountAHID
			LEFT JOIN LoanMaster L ON L.SLAccountNumber = A.AHID
			INNER JOIN Member M ON M.MemberID = ISNULL(D.MemberID, L.MemberID)
			INNER JOIN @AHID_OB_TBL T ON A.AHID =ISNULL(SLAHID, T.AHID)
		WHERE IsFederation = 0
			AND ORIGIN IN ('LOAN_OB_GROUP_LOGIN', 'DEPOSIT_GROUP_LOGIN_OB')
			AND (D.SLAccountAHID IS NOT NULL OR L.SLAccountNumber IS NOT NULL)
			AND M.GroupID = @GroupId
		GROUP BY T.AHID, AHTechCode, M.MemberID, A.OpeningBalanceType
		
		UNION ALL

		SELECT --ROW_NUMBER() OVER(ORDER BY AHID) + 10000 rowid, 
				AHID, 
				AHTechCode,
				MemberID,
				TDATE,
				SUM(ISNULL(DrAmount, 0)) AS DrAmount, 
				SUM(ISNULL(CrAmount, 0)) AS CrAmount,
				BALANCE = (- SUM(ISNULL(DrAmount, 0))) + SUM(ISNULL(CrAmount, 0))
		FROM(
				SELECT 
					AM.AccountMasterID, 
					A.AccountTranID, 
					AM.MemberID,
					T.AHID, 
					AHTechCode,
					C.AHCode,
					C.AHName, 
					AM.TransactionDate TDATE,
					A.CrAmount, 
					A.DrAmount, 
					AM.Transactiontype, 
					AM.TransactionMode
				FROM AccountTransactions A 
					INNER JOIN AccountHead C ON A.AHID =C.AHID
					INNER JOIN AccountMaster AM ON AM.AccountMasterID = A.AccountMasterID
					INNER JOIN @AHID_OB_TBL T ON A.AHID = ISNULL(SLAHID, T.AHID)
				WHERE --A.AHID=@AHID 
					(A.CrAmount > 0 OR A.DrAmount > 0)
					--AND A.IsActive = 1
					AND ((@FYStartDate IS NULL OR @MeetingDate IS NULL) OR CAST(AM.TransactionDate AS DATE) BETWEEN @FYStartDate AND @MeetingDate)
					AND AM.StatusID = @STATUSID
					AND C.StatusID = @STATUSID
					AND AM.GroupID = @GroupID
		)ABC
		GROUP BY AHID, AHTechCode, MemberID, TDATE
	)A

	DECLARE @MonthEnd DATE = DATEADD(DD, -1, CAST(CAST(YEAR(DATEADD(MM, 1, @MeetingDate)) AS VARCHAR) + '-' + CAST(MONTH(DATEADD(MM, 1, @MeetingDate)) AS VARCHAR) + '-1' AS DATE))
	
	;WITH ENTRIES AS(
		select *, ROW_NUMBER() over(partition by MEMBERID, interestahid order by MEMBERID, TDATE)  as rowid  
		from (
			SELECT 
				InterestAHID, t.AHTechCode, MemberID, TDATE, CAST(ROI AS INT) AS ROI,
				CAST(SUM(BALANCE) AS INT) balance
			FROM GroupInterestMaster GIM
			INNER JOIN GroupInterestRates GIR ON GIM.GroupInterestID = GIR.GroupInterestID
			INNER JOIN @AHID_OB_TBL T ON GIM.InterestAHID = T.AHID
			INNER JOIN #ENTRIES E ON E.AHID = GIM.PrincipalAHID
			WHERE 
				T.AHTechCode IN ('PRIMARY_SAVINGS_INT', 'SPECIAL_SAVINGS_INT') 
				AND E.AHTechCode IN ('SPECIAL_SAVINGS', 'PRIMARY_SAVINGS')
				AND GroupID = @GroupId
				AND StatusID = 1
				AND (TDATE >= GIR.FromDate AND (GIR.ToDate IS NULL OR GIR.ToDate <= TDATE))
			GROUP BY InterestAHID, t.AHTechCode, MemberID, TDATE, ROI
		)A
	)
	,
	Entries_Interest as(

		SELECT rowid, AHTechCode, InterestAHID, MemberID, TDATE, ROI, balance, balance total, (SELECT A.TDATE FROM ENTRIES A WHERE A.rowid = T.rowid + 1 AND A.MemberID = T.MemberID AND T.InterestAHID = A.InterestAHID) AS CALDATE
		FROM ENTRIES T WHERE rowid = 1
		
		UNION ALL
		
		SELECT e.rowid, E.AHTechCode, e.InterestAHID, e.MemberID, e.TDATE, e.ROI, e.balance, (e.balance + a.balance) total,(SELECT A.TDATE FROM ENTRIES A WHERE A.rowid = E.rowid + 1 AND A.MemberID = e.MemberID AND e.InterestAHID = A.InterestAHID) AS CALDATE
		FROM ENTRIES e
		INNER JOIN Entries_Interest a on 
		e.InterestAHID = a.InterestAHID 
		and 
		a.MemberID =  e.MemberID
		WHERE e.rowid = a.rowid + 1
	)
	--select * from Entries_Interest
	INSERT INTO #ENTRIES(AHID, AHTechCode, MemberID,TDATE, DrAmount, CrAmount, BALANCE)
	select InterestAHID, AHTechCode, MemberID, TDATE, 
		--caldate, balance, ROI, isnull(caldate, @MonthEnd), 
		--datediff(dd, tdate, isnull(caldate, @MonthEnd)) DIFF,
		0 as CrAmount,
		CAST((total * ROI * datediff(dd, tdate, isnull(caldate, @MonthEnd)))/36500 AS INT) DrAmount
		,CAST((total * ROI * datediff(dd, tdate, isnull(caldate, @MonthEnd)))/36500 AS INT) INTEREST_Bal
	from Entries_Interest
	order by MemberID, InterestAHID, TDATE
	--select * from #ENTRIES
	INSERT INTO @TBL_TOTAL_ASONDATE
	SELECT T.AHID, 
		A.MemberID, 
		t.AHTechCode,
		A.CrAmount, 
		A.DrAmount,
		A.BALANCE
	FROM @AHID_OB_TBL T
	LEFT JOIN
	(
		SELECT AHID, AHTechCode, MemberID,
			ABS(SUM(A.CrAmount)) AS CrAmount, 
			ABS(SUM(A.DrAmount)) DrAmount,
			SUM(BALANCE) AS BALANCE
		FROM #ENTRIES A
		GROUP BY AHID, AHTechCode, MemberID 
	)A ON A.AHID = T.AHID

	--SELECT * FROM @TBL_TOTAL_ASONDATE

	DECLARE @MemberID	INT

	DECLARE @PSPrincipal INT, @PSInt INT, @SSPrincipal INT, @SSInt INT, @SLoanDate DATE, @SLoanPrincipal INT, @SLoanInt INT,
		@BLoanDate DATE, @BLoanPrincipal INT, @BLoanInt INT, @HLoanDate DATE, @HLoanPrincipal INT, @HLoanInt INT

	DECLARE @TBL TABLE(SEQ INT, SLAhId INT, SubAhId INT, DisbursementDate DATE, OutStandingAmount INT, LoanMasterId INT, Demand INT, MemberID INT, AHTechCode VARCHAR(100))
	DECLARE MEMBER_CURSOR CURSOR FOR
	SELECT MemberID FROM Member WHERE GroupID = @GroupId AND StatusID = @STATUSID

	OPEN MEMBER_CURSOR
	FETCH NEXT FROM MEMBER_CURSOR INTO @MemberID
	WHILE @@Fetch_Status = 0
	BEGIN
		SELECT @PSPrincipal = 0, @PSInt = 0, @SSPrincipal= 0, @SSInt= 0, @SLoanDate = NULL, @SLoanPrincipal= 0, @SLoanInt= 0, @BLoanDate = NULL, @BLoanPrincipal= 0, @BLoanInt= 0, @HLoanDate = NULL, 
		@HLoanPrincipal = 0, @HLoanInt = 0
		
		INSERT INTO @TBL
		EXEC dbo.uspMCRptGetMemberDemandLoanDetails @MemberID, @MeetingDate

		SELECT @PSPrincipal = BALANCE FROM @TBL_TOTAL_ASONDATE WHERE AHTechCode = 'PRIMARY_SAVINGS'  AND MemberID = @MemberID 
		SELECT @SSPrincipal = BALANCE FROM @TBL_TOTAL_ASONDATE WHERE AHTechCode = 'SPECIAL_SAVINGS' AND MemberID = @MemberID 

		SELECT @PSInt = BALANCE FROM @TBL_TOTAL_ASONDATE WHERE AHTechCode = 'PRIMARY_SAVINGS_INT' AND MemberID = @MemberID 
		SELECT @SSInt = BALANCE FROM @TBL_TOTAL_ASONDATE WHERE AHTechCode = 'SPECIAL_SAVINGS_INT' AND MemberID = @MemberID 


		SELECT @SLoanDate = DisbursementDate, @SLoanPrincipal = OutStandingAmount
		FROM @TBL  WHERE AHTechCode = 'SMALLLOAN_PRINCIPAL' AND MemberID = @MemberID 

		SELECT @SLoanInt = Demand FROM @TBL  WHERE AHTechCode = 'SMALLLOAN_INTEREST' AND MemberID = @MemberID 

		SELECT @BLoanDate = DisbursementDate, @BLoanPrincipal = OutStandingAmount
		FROM @TBL  WHERE AHTechCode = 'BIGLOAN_PRINCIPAL' AND MemberID = @MemberID 

		SELECT @BLoanInt = Demand FROM @TBL  WHERE AHTechCode = 'BIGLOAN_INTEREST' AND MemberID = @MemberID 

		SELECT @HLoanDate = DisbursementDate, @HLoanPrincipal = OutStandingAmount
		FROM @TBL  WHERE AHTechCode = 'HOUSINGLOAN_PRINCIPAL' AND MemberID = @MemberID 

		SELECT @HLoanInt = Demand FROM @TBL  WHERE AHTechCode = 'HOUSINGLOAN_INTEREST' AND MemberID = @MemberID 

		INSERT INTO @FINAL_TBL
		SELECT @MemberID, IIF(ISNULL(MemberRefCode, '') = '', MemberCode, MemberRefCode), MemberName, @PSPrincipal, @PSInt, @SSPrincipal, @SSInt, @SLoanDate, @SLoanPrincipal, @SLoanInt, @BLoanDate, @BLoanPrincipal, @BLoanInt, @HLoanDate, @HLoanPrincipal, @HLoa
nInt
		FROM Member 
		WHERE MemberID = @MemberID

		--'PRIMARY_SAVINGS','SPECIAL_SAVINGS', 'PRIMARY_SAVINGS_INT', 'SPECIAL_SAVINGS_INT'
		--'BIGLOAN_PRINCIPAL','BIGLOAN_INTEREST','SMALLLOAN_PRINCIPAL','SMALLLOAN_INTEREST','HOUSINGLOAN_PRINCIPAL','HOUSINGLOAN_INTEREST', 

	FETCH NEXT FROM MEMBER_CURSOR INTO @MemberID
	END
	CLOSE MEMBER_CURSOR
	DEALLOCATE MEMBER_CURSOR

	SELECT * FROM @FINAL_TBL
	UNION ALL
	SELECT 0, '', 'TOTAL', SUM(PSPrincipal), SUM(PSInt), SUM(SSPrincipal), SUM(SSInt), NULL, SUM(SLoanPrincipal), SUM(SLoanInt), 
		NULL, SUM(BLoanPrincipal), SUM(BLoanInt), NULL, SUM(HLoanPrincipal), SUM(HLoanInt)
	FROM @FINAL_TBL

END


