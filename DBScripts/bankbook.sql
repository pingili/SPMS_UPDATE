ALTER PROC [dbo].[uspGroupBankBook_v1]
	@StartDate			DATE = NULL,
	@EndDate			DATE = NULL,
	@GroupId			INT,
	@OrgAddress			VARCHAR(1000) OUTPUT
AS
/*
DECLARE @A VARCHAR(1000), @B VARCHAR(500)
--EXEC USPGENERALLEDGERDREPORT 365683  ,'2016-01-01','2016-07-29', @A OUTPUT, @B OUTPUT
EXEC [uspGroupBankBook_v1] '2017-04-01','2017-05-29',282,''
SELECT @A, @B
*/
BEGIN
	declare @ahId INT = 36
	DECLARE @AHID_TBL TABLE(AHID INT, PARENTAHID INT NULL)

	DECLARE @STATUSID INT = (SELECT StatusID FROM StatusMaster WHERE StatusCode = 'ACT')

	SELECT TOP 1 @OrgAddress = [Address] FROM Organization WHERE IsActive = 1

	DECLARE @AHCODE VARCHAR(500)
	DECLARE @AHNAME VARCHAR(500)
	DECLARE @AHLEVEL INT
	DECLARE @TDATE	DATE = CAST(YEAR(@StartDate) AS VARCHAR) + '-04-01'
	DECLARE @OBTYPE	VARCHAR(50) 
	DECLARE @CROB		DECIMAL(16, 2) = 0
	DECLARE @DROB		DECIMAL(16, 2) = 0
	DECLARE @BAL		DECIMAL(18,2) = 0
	DECLARE @VoucherNumber VARCHAR(500)

	SELECT 
		@AHCODE = AHCODE,
		@AHNAME = 'OPENING BALANCE',--AHName,
		@AHLEVEL =  AHLevel,
		@OBTYPE = OpeningBalanceType,
		@CROB = IIF(OpeningBalanceType = 'CR', ISNULL(OpeningBalance, 0), 0),
		@DROB = IIF(OpeningBalanceType = 'DR', ISNULL(OpeningBalance, 0), 0)
	FROM AccountHead WHERE AHID = @AHID


	IF @AHLEVEL = 5
	BEGIN
		INSERT INTO @AHID_TBL(AHID, PARENTAHID)
		SELECT AHID, ParentAHID FROM AccountHead WHERE ParentAHID = @AHID OR AHID = @AHID
	END
	ELSE
	BEGIN
		INSERT INTO @AHID_TBL(AHID, PARENTAHID)
		SELECT @AHID, @AHID
	END

	SELECT
		@CROB += SUM(ISNULL(CrAmount, 0)),
		@DROB += SUM(ISNULL(DrAmount, 0))
	FROM 
	(
		SELECT 
			SUM(ISNULL(D.DepositAcmount, 0)) as CrAmount,
			SUM(ISNULL(L.LoanAmountApplied, 0)) as DrAmount
		FROM AccountHead A
			LEFT JOIN Deposits D ON A.AHID = D.SLAccountAHID
			LEFT JOIN LoanMaster L ON L.SLAccountNumber = A.AHID
		WHERE IsFederation = 0 
			AND ORIGIN IN ('LOAN_OB_GROUP_LOGIN', 'DEPOSIT_GROUP_LOGIN_OB')
			AND ParentAHID IN (SELECT AHID FROM @AHID_TBL)
			AND (D.SLAccountAHID IS NOT NULL OR L.SLAccountNumber IS NOT NULL)
			AND ISNULL(D.GroupID, L.GroupID) = @GroupID
		UNION ALL
	
		SELECT 
			SUM(IIF( OpeningBalanceType = 'CR', ISNULL(OpeningBalance, 0), 0)) as CrAmount,
			SUM(IIF( OpeningBalanceType = 'DR', ISNULL(OpeningBalance, 0), 0)) as DrAmount
		FROM GroupOB 
		WHERE AHID IN (SELECT AHID FROM @AHID_TBL)
		AND GroupID = @GroupID
	)A


	;WITH ENTRIES AS(
		SELECT 0 ROWID, @AHID AHID, @AHCODE AHCODE, @AHNAME AHNAME, @TDATE TDATE,@VoucherNumber VoucherNumber, @DROB DrAmount, @CROB CrAmount, Bal =  IIF(@OBTYPE = 'CR', @CROB, -@DROB)
		
		UNION ALL

		SELECT ROW_NUMBER() OVER(ORDER BY AHID, TDATE) rowid, 
				AHID, AHCode, IIF(TransactionMode= 'C', 'CASH', 'BANK') AS AHName, TDATE,VoucherNumber,
				-SUM(ISNULL(DrAmount, 0)) AS DrAmount, 
				SUM(ISNULL(CrAmount, 0)) AS CrAmount,
				BALANCE = (- SUM(ISNULL(DrAmount, 0))) + SUM(ISNULL(CrAmount, 0))
		FROM(
				SELECT 
					AM.AccountMasterID, A.AccountTranID,
					A.AHID,C.AHCode,C.AHName, AM.TransactionDate TDATE,AM.VoucherNumber,
					A.CrAmount, A.DrAmount, AM.Transactiontype, AM.TransactionMode
				FROM AccountTransactions A 
					JOIN AccountHead C ON A.AHID =C.AHID
					JOIN AccountMaster AM ON AM.AccountMasterID = A.AccountMasterID
					JOIN BankMaster BM ON BM.AHID=AM.AHID
					WHERE --A.AHID=@AHID 
					 C.ParentAHID IN (SELECT AHID FROM @AHID_TBL)
					AND (A.CrAmount > 0 OR A.DrAmount > 0)
					--AND A.IsActive = 1
					AND ((@StartDate IS NULL OR @EndDate IS NULL) OR CAST(AM.TransactionDate AS DATE) BETWEEN @StartDate AND @EndDate)
					AND AM.StatusID = @STATUSID
					AND C.StatusID = @STATUSID
					AND AM.GroupID = @GroupID
		)ABC
		GROUP BY AHID,AHCode,AHName, TDATE,VoucherNumber, Transactiontype, TransactionMode--, CrAmount, DrAmount
	)

	SELECT A.AHID, A.AHCODE, A.AHNAME,AH.AHName AS BankName, B.AccountNumber,B.BranchName ,TDATE,VoucherNumber, ABS(A.CrAmount) AS CrAmount, ABS(A.DrAmount) DrAmount, (SELECT ABS(SUM(BAL)) FROM ENTRIES B WHERE B.ROWID <= A.ROWID) AS Balance
	FROM ENTRIES a JOIN AccountHead AH ON AH.AHID=a.AHID  JOIN BankMaster B ON B.AHID=a.AHID
END

