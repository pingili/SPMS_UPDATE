CREATE PROC [dbo].[uspMemberLoanConfirm_v1]
	@LoanMasterId	INT OUTPUT,
	@UserId		INT,
	@GroupId	INT
AS
BEGIN
		DECLARE @AccountMasterId INT, @VoucherNumber VARCHAR(50), @Narration VARCHAR(500), @SLAHID INT, @PrincipalAHId INT, @InterestRate INT
		DECLARE @TransactionMode VARCHAR(50), @BankEntryId INT, @DisbursementDate DATETIME, @VoucherRefNumber VARCHAR(100), @DisbursedAmount INT, @ChequeNumber VARCHAR(50), @ChequeDate DATE,
			@GroupInterestRateId INT, @LoanCode VARCHAR(50), @NoOfInstallments INT, @MemberId INT, @InstallmentStartFrom DATETIME, @GroupInterestId INT

		SELECT @TransactionMode = TransactionMode,
				@BankEntryId = BankEntryId,
				@DisbursementDate = DisbursementDate,
				@VoucherRefNumber = l.LoanRefNumber,
				@DisbursedAmount = l.DisbursedAmount,
				@ChequeNumber = L.ChequeNumber,
				@ChequeDate = L.ChequeDate,
				@Narration = IIF(ISNULL(L.DisbursementComments, '') = '', 'MEMBER LOAN VOUCHER (' + L.LoanCode + ')', L.DisbursementComments),
				@GroupInterestRateId = L.GroupInterstRateID,
				@NoOfInstallments = l.NoOfInstallments,
				@LoanCode = L.LoanCode,
				@MemberId = L.MemberID,
				@InstallmentStartFrom = L.InstallmentStartFrom
		FROM LoanMaster L WHERE LoanMasterID = @LoanMasterId

		SELECT @PrincipalAHId = GIM.PrincipalAHID, @InterestRate = GIR.ROI, @GroupInterestId = GIM.GroupInterestID
		FROM GroupInterestRates GIR
		INNER JOIN GroupInterestMaster GIM ON GIR.GroupInterestID = GIM.GroupInterestID
		WHERE GIR.GroupInterestRateID = @GroupInterestRateId

		DECLARE @IsLoanExists BIT = 0

		EXEC DBO.uspisexistedLoan @MemberID = @MemberId, @InterestId = @GroupInterestId, @IsLoanExists = @IsLoanExists OUTPUT

		IF @IsLoanExists = 1
		BEGIN
			SELECT @LoanMasterId = -98
			RETURN;
		END

		DECLARE @AHID			INT
		IF(LEFT(@TransactionMode, 1) = 'C')
			SELECT @AHID = CAST(SettingValue AS INT) FROM SystemSettings  WHERE SettingName = 'GROUP_CASHINHAND'
		ELSE
			SELECT @AHID = AHID FROM BankMaster WHERE BankEntryID = @BankEntryId
		
		declare @asondate date = dateadd(dd, 1, @DisbursementDate)
		DECLARE @OpeningBalance	DECIMAL(18, 2)
		EXEC uspGetGroupOpeningBalanceAsOnDate @AHID, @GroupId, @asondate, @OpeningBalance OUTPUT

		IF @DisbursedAmount > @OpeningBalance
		BEGIN
			SELECT @LoanMasterId = -97
			RETURN;
		END

	BEGIN TRY
		BEGIN TRANSACTION
		

		----------------I. SL ACCOUNT NUMBER GENERATION---------------------
 		IF NOT EXISTS(Select 1 From LoanMaster where LoanMasterID=@LoanMasterId AND SLAccountNumber IS NOT NULL)
		BEGIN
 			EXEC [dbo].[uspGenerateSlAccount] @LoanMasterId = @LoanMasterId, @LoanAmount = @DisbursedAmount, @PrincipalAHId = @PrincipalAHId, @UserID = @UserID,
 				@SLAccountName = @LoanCode, @AH_ORIGIN_CODE = 'LOAN_DISBURSEMENT_GROUP_LOGIN', @AH_ORIGIN_ID = @LoanMasterId, @SLAHID= @SLAHID OUTPUT
		END
 		ELSE
		BEGIN
			SELECT @SLAHID =SLAccountNumber FROM LoanMaster WHERE LoanMasterID =@LoanMasterId
		END

		----------------II. VOUCHER GENERATION ---------------------
		
		----------------ACCOUNT MASTER---------------------
		
	
		DECLARE @TransType INT, @Sno INT, @ActStatusId INT = (SELECT StatusID FROM StatusMaster WHERE StatusCode = 'ACT')
		
		SELECT @TransType = RefID FROM RefValueMaster where RefCode = 'GMP' AND RefMasterID IN(SELECT RefMasterID from RefMaster where RefMasterCode = 'TRANSACTION_TYPE')
		EXEC uspGenerateObjectCodeByEntityCode 'ACCOUNT_MASTER', @Sno OUTPUT, @VoucherNumber OUTPUT

		INSERT INTO AccountMaster(TransactionDate, CodeSno, VoucherNumber, 	VoucherRefNumber, EmployeeID, AHID,  TransactionType, LoanMasterID,
				Amount, TransactionMode,  ChequeNumber, ChequeDate, BankAccount, Narration,  StatusID,  IsGroup, GroupID, MemberID, CreatedBy, CreatedOn,  IsPairedRecord)
		SELECT @DisbursementDate, @Sno, @VoucherNumber, @VoucherRefNumber, @UserId, @AHID, @TransType, @LoanMasterId,
			@DisbursedAmount, LEFT(@TransactionMode, 1), @ChequeNumber, @ChequeDate, @BankEntryId, @Narration, @ActStatusId, 1, @GroupId, @MemberId, @UserId, GETDATE(), 0
	
		SET @AccountMasterId = SCOPE_IDENTITY()		

		----------------ACCOUNT TRANSACTION---------------------

		INSERT INTO AccountTransactions(AccountMasterID, AHID, CrAmount, DrAmount, IsActive, CreatedBy, CreatedOn,IsMaster)        
		SELECT @AccountMasterID, @SLAHID, 0,@DisbursedAmount, 1, @UserID, GETDATE(), 0

		INSERT INTO AccountTransactions(AccountMasterID, AHID, CrAmount, DrAmount, IsActive, CreatedBy, CreatedOn, IsMaster)        
		SELECT @AccountMasterID, @AHID, @DisbursedAmount,0, 1, @UserID, GETDATE(), 1


		---------------------- SCHEDULE GENERATION ------------------------
		EXEC dbo.[uspCreateSchedule]
			@LoanMasterID = @LoanMasterId, 
			@LoanAmount = @DisbursedAmount,
			@InterestRate = @InterestRate,
			@PenelInterestRate = 0,
			@LoanPeriod = @NoOfInstallments,
			@StartPaymentDate = @InstallmentStartFrom,
			@UserID = @UserId,
			@CalculationStartDate = @DisbursementDate
		
		UPDATE LoanMaster 
			SET SLAccountNumber = @SLAHID, 
			AccountMasterID = @AccountMasterId, 
			OutStandingAmount = @DisbursedAmount,
			StatusID = @ActStatusId,
			ModifiedBy = @UserId,
			ModifiedOn = GETDATE(),
			LastPaidDate=@DisbursementDate
		WHERE LoanMasterID = @LoanMasterId
		
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION    
	  
		INSERT INTO SystemExceptions(ExpSource, EXCEPTION, StackTrace, ExpType, ExpDate)        
		SELECT 'uspMemberLoanConfirm', ERROR_MESSAGE(), ERROR_LINE(),ERROR_SEVERITY(), GETDATE()         
        
		SELECT @LoanMasterId = -99        
 END CATCH    
END


