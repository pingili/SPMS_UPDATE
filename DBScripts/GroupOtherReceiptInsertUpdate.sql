ALTER PROC [dbo].[uspGroupOtherRecieptInsertUpdate]
	@AccountMasterId	INT OUTPUT,
	@VoucherNumber		VARCHAR(32) = null OUTPUT,
	@TransactionMode	CHAR(2),
	@TransactionDate	DATETIME,
	@VoucherRefNumber	VARCHAR(32) = NULL,
	@CollectionAgent	INT,
	@GLAccountId		INT,
	@SLAccountId		INT,
	@ChequeNumber		INT = NULL,
	@ChequeDate			DateTime = NULL,
	@Amount				DECIMAL(18, 2),
	@BankEntryId		INT = NULL,
	@Narration			VARCHAR(1024),
	@UserId				INT,
	@GroupId			INT,
	@IsContra           BIT
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @AHID			INT
	IF(@TransactionMode = 'C')
		SELECT @AHID = CAST(SettingValue AS INT) FROM SystemSettings  WHERE SettingName = 'GROUP_CASHINHAND'
	ELSE
		SELECT @AHID = AHID FROM BankMaster WHERE BankEntryID = @BankEntryId
    IF (@IsContra=1 and @TransactionMode='C')
		SELECT @AHID = AHID FROM BankMaster WHERE BankEntryID = @BankEntryId
    


	IF  ISNULL(@AccountMasterId, 0) = 0
	BEGIN
		DECLARE @TransType		INT
		DECLARE @Sno			INT
		DECLARE @ActStatusId	INT = (SELECT StatusID FROM StatusMaster WHERE StatusCode = 'ACT')
		IF @IsContra=1
		BEGIN
		 select @TransType = RefID from RefValueMaster where RefCode = 'CCW' AND RefMasterID IN(select RefMasterID from RefMaster where RefMasterCode = 'TRANSACTION_TYPE')
		END
		ELSE
		BEGIN
		 select @TransType = RefID from RefValueMaster where RefCode = 'GOR' AND RefMasterID IN(select RefMasterID from RefMaster where RefMasterCode = 'TRANSACTION_TYPE')
		END
		EXEC uspGenerateObjectCodeByEntityCode 'ACCOUNT_MASTER', @Sno OUTPUT, @VoucherNumber OUTPUT

		INSERT INTO AccountMaster(TransactionDate, CodeSno, VoucherNumber, 	VoucherRefNumber, EmployeeID, AHID,  TransactionType,
			 Amount, TransactionMode,  ChequeNumber, ChequeDate, BankAccount, Narration,  StatusID,  IsGroup, GroupID, CreatedBy, CreatedOn,  IsPairedRecord)
		SELECT @TransactionDate, @Sno, @VoucherNumber, @VoucherRefNumber, @CollectionAgent, @AHID, @TransType, 
			@Amount, LEFT(@TransactionMode, 1), @ChequeNumber, @ChequeDate, @BankEntryId, @Narration, @ActStatusId, 1, @GroupId, @UserId, GETDATE(), 0
	
		SET @AccountMasterId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN
		UPDATE AccountMaster
			SET TransactionDate = @TransactionDate,
				VoucherRefNumber = @VoucherRefNumber,
				EmployeeID = @CollectionAgent,
				AHID = @AHID,
				Amount = @Amount, 
				TransactionMode = LEFT(@TransactionMode, 1),  
				ChequeNumber = @ChequeNumber, 
				ChequeDate = @ChequeDate, 
				BankAccount = @BankEntryId, 
				Narration = @Narration,
				ModifiedBy = @UserId,
				ModifiedOn = GETDATE()
		WHERE AccountMasterID = @AccountMasterId
	END

	IF NOT EXISTS (SELECT 1 FROM AccountTransactions WHERE AccountMasterID = @AccountMasterId)
	BEGIN
		--FROM ACC
		INSERT INTO AccountTransactions(AccountMasterID,  AHID,   CrAmount, DrAmount, IsActive,   CreatedBy, CreatedOn, IsMaster)
		SELECT @AccountMasterId, @SLAccountId,  @Amount, 0, 1, @UserId, GETDATE(), 	0

		--TO ACC
		INSERT INTO AccountTransactions(AccountMasterID,  AHID,   CrAmount, DrAmount, IsActive,   CreatedBy, CreatedOn, IsMaster)
		SELECT @AccountMasterId, @AHID,  0, @Amount, 1, @UserId, GETDATE(), 1
	END
	ELSE
	BEGIN
		--FROM ACC
		UPDATE AccountTransactions
		SET AHID	= @SLAccountId,
			CrAmount = @Amount,
			ModifiedBy = @UserId,
		ModifiedOn = GETDATE()
		WHERE AccountMasterID = @AccountMasterId AND IsMaster = 0

		--TO ACC
		UPDATE AccountTransactions
		SET AHID	= @AHID,
			DrAmount = @Amount,
			ModifiedBy = @UserId,
			ModifiedOn = GETDATE()
		WHERE AccountMasterID = @AccountMasterId AND IsMaster = 1
	END
END


