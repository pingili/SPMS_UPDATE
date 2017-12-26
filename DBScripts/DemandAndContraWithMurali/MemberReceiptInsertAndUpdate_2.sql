
ALTER PROCEDURE [dbo].[uspLoanRepaymentInsertUpdate_v2]
	@AccountMasterID INT,
	@GroupID INT=NULL,
	@MemberID INT=NULL,
	@IsMember BIT,
	@TransactionDate DateTime
AS
BEGIN

	IF @IsMember =1
	BEGIN
	
		DELETE LoanRepayment WHERE ReceiptID=@AccountMasterID

		DECLARE @PrincipalAHID INT,@InterestAHID INT,@LoanMasterID INT,@InterestAmount DECIMAL(18,2),@RepayAmount DECIMAL(18,2),@OpeningBalance DECIMAL(18,2),@Collection DECIMAL(18,2)

		DECLARE CurLoans CURSOR FOR 
		SELECT  GM.PrincipalAHID, GM.InterestAHID, LM.LoanMasterID 
		FROM LoanMaster LM JOIN Member M ON M.MemberID=LM.MemberID 
		JOIN GroupInterestMaster GM ON GM.GroupInterestID=LM.GroupInterstRateID
		WHERE LoanType = CASE WHEN @IsMember=1 THEN  'M' ELSE 'G' END AND M.MemberID =@MemberID

		OPEN CurLoans 

		FETCH NEXT FROM CurLoans INTO @PrincipalAHID,@InterestAHID,@LoanMasterID

		WHILE (@@Fetch_Status =0)
		BEGIN

			SELECT @PrincipalAHID= SLAccountNumber FROM LoanMaster WHERE LoanMasterID = @LoanMasterID 

			SELECT @InterestAmount= CASE WHEN AT.DrAmount>0 THEN AT.DrAmount ELSE AT.CrAmount END 
			FROM AccountMaster AM 
			JOIN AccountTransactions AT ON AM.AccountMasterID = AT .AccountMasterID 
			WHERE AM.AccountMasterID=@AccountMasterID AND AT.AHID= @InterestAHID 

			SELECT @RepayAmount =  CASE WHEN AT.DrAmount>0 THEN AT.DrAmount ELSE AT.CrAmount END 
			FROM AccountMaster AM 
			JOIN AccountTransactions AT ON AM.AccountMasterID = AT .AccountMasterID 
			WHERE AM.AccountMasterID=@AccountMasterID  AND AT.AHID = @PrincipalAHID

			DECLARE @LoanOutStandingAmount DECIMAL(18, 2) 
			SELECT @LoanOutStandingAmount = OutStandingAmount FROM LoanMaster WHERE LoanMasterID= @LoanMasterID

			IF @LoanOutStandingAmount - @RepayAmount <= 0 -- LOAN CLOSED
			BEGIN
				UPDATE LoanMaster 
					SET StatusID = (SELECT StatusID FROM StatusMaster WHERE StatusCode = 'CLOSED'),
						PrincipalDue = 0,
						InterestDue = 0,
						LoanClosingDate = @TransactionDate,
						LastPaidDate = @TransactionDate,
						ModifiedOn=GetDate(),
						LoanClosedBy = (SELECT ModifiedBy FROM AccountMaster WHERE AccountMasterID = @AccountMasterID),
						OutStandingAmount = @LoanOutStandingAmount - @RepayAmount
				WHERE LoanMasterID = @LoanMasterID
			END
			ELSE
			BEGIN
			
				UPDATE LoanMaster 
				SET LastPaidDate = @TransactionDate , 
				    ModifiedOn=GetDate(),
					OutStandingAmount=OutStandingAmount - @RepayAmount
				WHERE LoanMasterID = @LoanMasterID
			END


			--un used logic of repayment
			SELECT @Collection = SUM(PrincipalAmount) FROM LoanRepayment WHERE LoanMasterID=@LoanMasterID

			SELECT @OpeningBalance =SUM(PrincipalBalance) 
			FROM LoanSchedule 
			WHERE LoanMasterID=@LoanMasterID AND DueDate>(SELECT DATEADD(DD, -1, CAST(SettingValue AS DATE)) FROM SystemSettings WHERE SettingName = 'FINANCIAL_YEAR_BEGIN')
			

			SET @LoanOutStandingAmount = ISNULL(@OpeningBalance,0)-ISNULL(@Collection,0)-ISNULL(@RepayAmount,0)

			INSERT INTO LoanRepayment 
			( 
				LoanMasterID,ReceiptID,LoanReferenceNumber,XGroupID,EmployeeID,TransactionDate,PrincipalAmount,InterestAmount,PenaltyAmount,PenalInterestAmount
				,OverDueAmount,LoanOutStandingAmount,OpeningBalance,StatusID,CreatedBy,CreatedOn
			)
			SELECT @LoanMasterID,@AccountMasterID ,NULL,GroupID,EmployeeID,TransactionDate,@RepayAmount,@InterestAmount,0,0 
			,0,ISNULL( @OpeningBalance,0)-ISNULL( @Collection,0)-ISNULL( @RepayAmount,0),ISNULL( @OpeningBalance,0)-ISNULL( @Collection,0),1,CreatedBy,CreatedOn
			FROM AccountMaster WHERE AccountMasterID=@AccountMasterID


			--IF @LoanOutStandingAmount <= 0
			--BEGIN
			--	UPDATE LoanMaster 
			--		SET StatusID = (SELECT StatusID FROM StatusMaster WHERE StatusCode = 'CLOSED'),
			--			PrincipalDue = 0,
			--			InterestDue = 0,
			--			LoanClosingDate = @TransactionDate,
			--			LastPaidDate = @TransactionDate,
			--			LoanClosedBy = (SELECT ModifiedBy FROM AccountMaster WHERE AccountMasterID = @AccountMasterID)
			--	WHERE LoanMasterID = @LoanMasterID
			--END
			--ELSE
			--BEGIN
			--	UPDATE LoanMaster SET LastPaidDate = @TransactionDate , OutStandingAmount=OutStandingAmount - @RepayAmount WHERE LoanMasterID = @LoanMasterID
			--END
 
		FETCH NEXT FROM CurLoans INTO @PrincipalAHID,@InterestAHID,@LoanMasterID

		END
		CLOSE CurLoans
		DEALLOCATE CurLoans

		-----Savings part--------------
		DELETE DepositTransactions WHERE ReceiptID=@AccountMasterID

		DECLARE @DepostAHID INT
		
		SELECT @DepostAHID=RegularSavingsAhId FROM GroupMaster WHERE GroupID = (SELECT GroupID FROM Member WHERE MemberID = @MemberID )

		INSERT INTO DepositTransactions(MemberID,GroupID,ReceiptID,AHID,DrAmount,CrAmount,StatusID,CreatedBy,CreatedOn)
		SELECT @MemberID,@GroupID,@AccountMasterID,@DepostAHID,DrAmount,CrAmount,1,AM.CreatedBy ,AM.CreatedOn 
		FROM AccountTransactions AT JOIN AccountMaster AM ON AT.AccountMasterID = AM.AccountMasterID 
		WHERE AM.AccountMasterID=@AccountMasterID AND AT.AHID = @DepostAHID 
	END
END





