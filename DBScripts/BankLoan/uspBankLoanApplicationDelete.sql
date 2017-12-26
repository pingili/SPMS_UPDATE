Create proc  [uspBankLoanApplicationDelete]
	@BannkLoanId INT OUTPUT,
	@UserId	 INT
AS
BEGIN

	BEGIN TRY
		DECLARE @StatusId	INT

		SELECT @StatusId = StatusID FROM StatusMaster WHERE StatusCode = 'DISC'

		UPDATE BankLoanMaster SET [Status] = @StatusId, ModifiedBy = @UserId, ModifiedDate = GETDATE()  WHERE BankLoanId = @BannkLoanId

		--SELECT @LoanCode = LoanCode FROM LoanMaster WHERE LoanMasterID = @LoanMasterID

	END TRY
	BEGIN CATCH
		INSERT INTO SystemExceptions(ExpSource, EXCEPTION, StackTrace, ExpType, ExpDate)
		SELECT '[uspBankLoanApplicationDelete]', ERROR_MESSAGE(), ERROR_LINE(),ERROR_SEVERITY(), GETDATE()	

		SET @BannkLoanId=-99;
	END CATCH

END

