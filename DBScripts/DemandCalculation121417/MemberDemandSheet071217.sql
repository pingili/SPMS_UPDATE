USE [SPMSUAT]
GO
/****** Object:  StoredProcedure [dbo].[uspMCRptGetMemberDemandLoanDetails]    Script Date: 12/7/2017 10:50:15 PM ******/
--[uspMCRptGetMemberDemandLoanDetails_v1] 7857, '17/May/2017'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROC [dbo].[uspMCRptGetMemberDemandLoanDetails_v1]
	@MemberId			INT,
	@TransactionDate	DATE
AS
BEGIN

	DECLARE @FINANCIAL_YEAR_BEGIN DATE 

	SELECT @FINANCIAL_YEAR_BEGIN = DATEADD(DD, -1, CAST(SettingValue AS DATE)) FROM SystemSettings WHERE SettingName = 'FINANCIAL_YEAR_BEGIN'

	DECLARE @PrimarySavingsAhId		INT
	DECLARE @PrimarySavingsAmount	INT
	DECLARE @TBL TABLE(SEQ INT, SLAhId INT, SubAhId INT, DisbursementDate DATE, OutStandingAmount INT, LoanMasterId INT, Demand INT, MemberID INT, AHTechCode VARCHAR(100))

	SELECT @PrimarySavingsAhId = RegularSavingsAhId, @PrimarySavingsAmount = RegularSavingAmount FROM GroupMaster WHERE GROUPID  = (SELECT GroupID FROM Member WHERE MemberID = @MemberId)

	INSERT INTO @TBL
	SELECT
		ROW_NUMBER() OVER (ORDER BY RAP.[PRIORITY]) AS SEQ,
		AHSL.AHID AS SLAhId,
		SUB.LoanSLAccount AS SubAhId,
		DisbursementDate,
		OutStandingAmount,
		SUB.LoanMasterID,
		NULL AS DEMAND,
		@MemberId,
		SS.SettingName
	FROM ReceiptAppropriationPriority RAP
		INNER JOIN SystemSettings SS ON RAP.AHID = CONVERT(INT, SS.SettingValue)
		INNER JOIN AccountHead AHSL ON AHSL.AHID = RAP.AHID
		LEFT JOIN
		(
			SELECT * FROM(
				SELECT 
					A.AHID AS LoanSLAccount, 
					a.ParentAHID AS ParentSLAccount,
					LM.LoanMasterID,
					LM.DisbursementDate,
					LM.OutStandingAmount,
					LM.LastPaidDate,
					ROW_NUMBER() OVER(ORDER BY A.ParentAHID, LM.LOANMASTERID) AS ROWID
				FROM AccountHead A 
				INNER JOIN LoanMaster LM ON A.AHID=LM.SLAccountNumber 
				WHERE LM.MemberID = @MemberId and LM.StatusID = 1 AND A.StatusID = 1
			)A 
			WHERE A.ROWID = 1
		)Sub ON RAP.AHID=Sub.ParentSLAccount
		WHERE SS.SettingName IN ('BIGLOAN_PRINCIPAL','BIGLOAN_INTEREST','SMALLLOAN_PRINCIPAL','SMALLLOAN_INTEREST','HOUSINGLOAN_PRINCIPAL','HOUSINGLOAN_INTEREST','PRIMARY_SAVINGS','SPECIAL_SAVINGS', 'PRIMARY_SAVINGS_INT', 'SPECIAL_SAVINGS_INT')
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
		WHERE LoanMasterID = @LOANMASTERID AND LM.StatusID=1
			

		SELECT 
			--@PRINCIPAL_DEMAND = TOTAL.PrincipalBalance - ISNULL(PAID.PAID_PRINCIPLE, 0) 
			 @PRINCIPAL_DEMAND = TOTAL.PrincipalBalance
			,@INTEREST_DEMAND = IIF(TOTAL.InterestDemand<0 ,0,TOTAL.InterestDemand)
		FROM 
		(
		     SELECT 
			        LS.LoanMasterID,
					(LM.OutStandingAmount-(LM.DisbursedAmount- (DateDiff(M,LM.DisbursementDate,@TransactionDate)*LM.DisbursedAmount/LM.NoOfInstallments))) PrincipalBalance,
					(LM.OutStandingAmount * GIR.ROI * DATEDIFF(DAY, Lm.LastPaidDate, @TransactionDate))/36500 InterestDemand
				FROM AccountHead A 
				INNER JOIN LoanMaster LM ON A.AHID=LM.SLAccountNumber 
				LEFT JOIN LoanSchedule LS ON LS.LoanMasterID=LM.LoanMasterID
				INNER JOIN GroupInterestRates GIR ON LM.GroupInterstRateID = GIR.GroupInterestRateID
			    INNER JOIN GroupInterestMaster GIM ON GIM.GroupInterestID = GIR.GroupInterestID
				WHERE LM.MemberID = @MemberId and LM.StatusID = 1 AND A.StatusID = 1
				AND LM.LoanMasterID = @LoanMasterID AND LM.MemberID=@MemberId 
				AND DueDate > DATEADD(DD, -1, @FINANCIAL_YEAR_BEGIN)
			--SELECT 
			--	LS.LoanMasterID,
			--	ISNULL( SUM(PrincipalBalance),0) PrincipalBalance, 
			--	ISNULL( SUM(InterestDemand),0) InterestDemand 
			--FROM LoanSchedule LS 
			--LEFT JOIN LoanMaster LM ON LS.LoanMasterID=LM.LoanMasterID 
			--WHERE CAST(DueDate AS DATE) <= CAST(@TransactionDate AS DATE) 
			--	AND LM.LoanMasterID = @LoanMasterID AND LM.MemberID=@MemberId 
			--	AND DueDate > DATEADD(DD, -1, @FINANCIAL_YEAR_BEGIN)

			GROUP BY LS.LoanMasterID,LM.OutStandingAmount,GIR.ROI,Lm.LastPaidDate,Lm.DisbursedAmount,Lm.DisbursementDate,LM.NoOfInstallments
		)TOTAL
		--LEFT JOIN
		--(
		--	SELECT 
		--		LM.LoanMasterID,
		--		ISNULL(SUM(PrincipalAmount),0) PAID_PRINCIPLE,
		--		ISNULL( SUM(InterestAmount),0) PAID_INTEREST
		--	FROM LoanRepayment LR JOIN LoanMaster LM ON LR.LoanMasterID=LM.LoanMasterID 
		--	WHERE LM.MemberID = @LoanMasterID
		--		AND LR.TransactionDate > DATEADD(DD, -1, @FINANCIAL_YEAR_BEGIN) AND LM.StatusID=1
		--	GROUP BY LM.LoanMasterID
		--)PAID ON TOTAL.LoanMasterID = PAID.LoanMasterID
				
		IF ISNULL(@PRINCIPAL_DEMAND, 0) <> 0
			UPDATE @TBL SET DEMAND = @PRINCIPAL_DEMAND WHERE LoanMasterID = @LoanMasterID
		IF ISNULL(@INTEREST_DEMAND, 0) <> 0
			UPDATE @TBL SET DEMAND = isnull(DEMAND, 0) +  @INTEREST_DEMAND WHERE SLAhId = @INTEREST_AHID

		SET @I = @I + 1
	END

	DECLARE @SavingsAmountDemand  INT

    if(ABS(ABS(MONTH(@FINANCIAL_YEAR_BEGIN) - MONTH(@TRANSACTIONDATE))) * @PrimarySavingsAmount  - ISNULL((SELECT SUM(ISNULL(CrAmount, 0)) FROM DepositTransactions WHERE MemberID = @MemberId), 0)> @PrimarySavingsAmount)
	BEGIN
	    SET @SavingsAmountDemand = ABS(ABS(MONTH(@FINANCIAL_YEAR_BEGIN) - MONTH(@TRANSACTIONDATE))) * @PrimarySavingsAmount  - ISNULL((SELECT SUM(ISNULL(CrAmount, 0)) FROM DepositTransactions WHERE MemberID = @MemberId), 0)
	 END
	 ELSE 
	 BEGIN
	    SET @SavingsAmountDemand = @PrimarySavingsAmount
	 END


	UPDATE @TBL
		SET DEMAND = IIF(@SavingsAmountDemand > 0,  @SavingsAmountDemand , 0)
	WHERE SLAhId = @PrimarySavingsAhId

	SELECT * FROM @TBL
	ORDER BY SEQ
END

