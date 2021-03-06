USE [SPMSUAT]
GO
/****** Object:  StoredProcedure [dbo].[uspGetMemberReceiptDemands_v2]    Script Date: 12/23/2017 11:29:50 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[uspGetMemberReceiptDemands_v1]
	@MemberId			INT,
	@TransactionDate	DATE,
	@RecieptId			INT = 0
AS
--uspGetMemberReceiptDemands_v2 7857, '17/APR/2017'
BEGIN
	--SET @MemberId = 2221
	--SET @TransactionDate= '19/AUG/2016'
	--SET @RecieptId = NULL
	--Declare @MemberId int 
	--Set @MemberId=5479
	DECLARE @FINANCIAL_YEAR_BEGIN DATE 

	SELECT @FINANCIAL_YEAR_BEGIN = DATEADD(DD, -1, CAST(SettingValue AS DATE)) FROM SystemSettings WHERE SettingName = 'FINANCIAL_YEAR_BEGIN'

	DECLARE @PrimarySavingsAhId		INT
	DECLARE @PrimarySavingsAmount	INT
	DECLARE @TBL TABLE(SEQ INT, GLAhId INT, GLAhName VARCHAR(200), SLAhId INT, SLAhName VARCHAR(200), SubAhId INT, SubAhName VARCHAR(200), ReferenceNumber VARCHAR(200), LoanMasterId INT, Demand INT, MemberID INT)

	SELECT @PrimarySavingsAhId = RegularSavingsAhId, @PrimarySavingsAmount = RegularSavingAmount FROM GroupMaster WHERE GROUPID  = (SELECT GroupID FROM Member WHERE MemberID = @MemberId)

	INSERT INTO @TBL
	SELECT
		ROW_NUMBER() OVER (ORDER BY RAP.[PRIORITY]) AS SEQ,
		--RAP.[Priority] AS SEQ,
		AHGL.AHID AS GLAhId,
		AHGL.AHName + '::' + AHGL.AHCode AS GLAhName,
		AHSL.AHID AS SLAhId,
		AHSL.AHName + '::' + AHSL.AHCode AS SLAhName,
		SUB.LoanSLAccount AS SubAhId,
		SUB.LOANSLAHNAME AS SubAhName,
		SUB.LoanCode AS ReferenceNumber,
		SUB.LoanMasterID,
		NULL AS DEMAND,
		@MemberId
	FROM ReceiptAppropriationPriority RAP
		INNER JOIN AccountHead AHSL ON AHSL.AHID = RAP.AHID
		LEFT JOIN AccountHead AHGL ON AHSL.ParentAHID = AHGL.AHID
		LEFT JOIN
		(
			SELECT * FROM(
				SELECT 
					A.AHID AS LoanSLAccount, 
					A.AHName AS LOANSLAHNAME, 
					a.ParentAHID AS ParentSLAccount,
					LM.LoanCode,
					LM.LoanMasterID,
					LM.OutStandingAmount,
					LM.LastPaidDate,
					ROW_NUMBER() OVER(ORDER BY A.ParentAHID, LM.LOANMASTERID) AS ROWID
				FROM AccountHead A 
				INNER JOIN LoanMaster LM ON A.AHID=LM.SLAccountNumber 
				WHERE LM.MemberID = @MemberId and LM.StatusID = 1 AND A.StatusID = 1
			)A 
			WHERE A.ROWID = 1
		)Sub ON RAP.AHID=Sub.ParentSLAccount
	ORDER BY RAP.[Priority]

	DECLARE @I INT = (SELECT MIN(SEQ) FROM @TBL)
	DECLARE @MAX INT = (SELECT MAX(SEQ) FROM @TBL)

	WHILE @I <= @MAX
	BEGIN

		IF NOT EXISTS (SELECT 1 FROM @TBL WHERE SEQ = @I AND LOANMASTERID IS NOT NULL)
		BEGIN
			SET @I = @I + 1
			CONTINUE;
		END
		DECLARE @LoanMasterID	INT

		SELECT TOP 1 @LoanMasterID = LOANMASTERID FROM @TBL WHERE SEQ = @I
		
		DECLARE @PRINCIPAL_AHID	INT
		DECLARE @INTEREST_AHID	INT
		DECLARE @PRINCIPAL_DEMAND	INT
		DECLARE @INTEREST_DEMAND	INT

		SELECT @PRINCIPAL_AHID = GIM.PrincipalAHID,
			@INTEREST_AHID = GIM.InterestAHID
		FROM LoanMaster LM
			INNER JOIN GroupInterestRates GIR ON LM.GroupInterstRateID = GIR.GroupInterestRateID
			INNER JOIN GroupInterestMaster GIM ON GIM.GroupInterestID = GIR.GroupInterestID
		WHERE LoanMasterID = @LOANMASTERID

		SELECT 
		     @PRINCIPAL_DEMAND = IIF (TOTAL.OutStandingAmount + InterestDemand <= MonthlyPrincipalDemand, TOTAL.OutStandingAmount + InterestDemand, MonthlyPrincipalDemand - InterestDemand)
			,@INTEREST_DEMAND = IIF(TOTAL.InterestDemand < 0, 0, TOTAL.InterestDemand+TOTAL.InterestDue)

		FROM
		(
			SELECT 
				LM.LoanMasterID ,
				LM.OutStandingAmount, 
				(LM.OutStandingAmount * GIR.ROI * DATEDIFF(DAY, Lm.LastPaidDate, @TransactionDate))/36500 InterestDemand,
				LM.MonthlyPrincipalDemand,
				LM.InterestDue
			FROM LoanMaster LM 
			INNER JOIN GroupInterestRates GIR ON LM.GroupInterstRateID = GIR.GroupInterestRateID
		    INNER JOIN GroupInterestMaster GIM ON GIM.GroupInterestID = GIR.GroupInterestID
			WHERE LM.LoanMasterID = @LoanMasterID AND LM.MemberID = @MemberId 
		)TOTAL
				
		IF ISNULL(@PRINCIPAL_DEMAND, 0) <> 0
			UPDATE @TBL SET DEMAND = @PRINCIPAL_DEMAND WHERE LoanMasterID = @LoanMasterID
		IF ISNULL(@INTEREST_DEMAND, 0) <> 0
			UPDATE @TBL SET DEMAND = isnull(DEMAND, 0) +  @INTEREST_DEMAND WHERE SLAhId = @INTEREST_AHID

		SET @I = @I + 1
	END

	DECLARE @SavingsAmountDemand  INT

	SET @SavingsAmountDemand = ABS(ABS(MONTH(@FINANCIAL_YEAR_BEGIN) - MONTH(@TRANSACTIONDATE))) * @PrimarySavingsAmount  - ISNULL((SELECT SUM(ISNULL(CrAmount, 0)) FROM DepositTransactions WHERE MemberID = @MemberId AND ReceiptID <> @RecieptId), 0)

	UPDATE @TBL
		SET DEMAND = IIF(@SavingsAmountDemand > 0,  @SavingsAmountDemand , 0)
	WHERE SLAhId = @PrimarySavingsAhId

	SELECT * FROM @TBL
	ORDER BY SEQ


	/* old logic
	DECLARE @FINANCIAL_YEAR_BEGIN DATE 

	SELECT @FINANCIAL_YEAR_BEGIN = DATEADD(DD, -1, CAST(SettingValue AS DATE)) FROM SystemSettings WHERE SettingName = 'FINANCIAL_YEAR_BEGIN'

	SELECT AHID, round(PrincipalBalance, 0) PrincipalBalance
	FROM(
			SELECT 
				AH.ParentAHID AS AHID
				,ISNULL(SUM(PrincipalBalance),0) AS PrincipalBalance
			FROM LoanSchedule LS 
				JOIN LoanMaster LM ON LS.LoanMasterID = LM.LoanMasterID 
				JOIN GroupInterestMaster GIM ON LM.GroupInterstRateID = GIM.GroupInterestID
				JOIN AccountHead AH ON AH.AHID = LM.SLAccountNumber
			WHERE DueDate <= @TransactionDate AND LM.MemberID = @MemberID AND LS.DueDate > @FINANCIAL_YEAR_BEGIN
			GROUP BY LS.LoanMasterID,ParentAHID,GIM.PrincipalAHID ,GIM.InterestAHID

		UNION ALL

			SELECT 
				GIM.InterestAHID AS AHID
				,ISNULL( SUM(InterestDemand),0) as InterestBalance
			FROM LoanSchedule LS 
				JOIN LoanMaster LM ON LS.LoanMasterID = LM.LoanMasterID 
				JOIN GroupInterestMaster GIM ON LM.GroupInterstRateID=GIM.GroupInterestID
			WHERE DueDate<=@TransactionDate AND LM.MemberID = @MemberID AND LS.DueDate > @FINANCIAL_YEAR_BEGIN
			GROUP BY LS.LoanMasterID,SLAccountNumber,GIM.PrincipalAHID ,GIM.InterestAHID

		UNION ALL

			SELECT ISNULL( RegularSavingsAhId,0),ISNULL(RegularSavingAmount ,0)
			FROM GroupMaster 
			WHERE GroupID = (SELECT GroupID FROM Member where MemberID = @MemberID)
	)A
	*/
END



