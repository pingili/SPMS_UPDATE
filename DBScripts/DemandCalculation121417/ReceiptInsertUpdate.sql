ALTER PROC [dbo].[uspGroupGroupMemberReceiptInsertUpdate_v1]
	@AccountMasterId	INT OUTPUT,
	@VoucherNumber		VARCHAR(32) = null OUTPUT,
	@TransactionMode	CHAR(2),
	@TransactionDate	DATETIME,
	@VoucherRefNumber	VARCHAR(32) = NULL,
	@CollectionAgent	INT,
	@ChequeNumber		INT = NULL,
	@ChequeDate			DateTime = NULL,
	@Amount				DECIMAL(18, 2),
	@BankEntryId		INT = NULL,
	@Narration			VARCHAR(1024),
	@UserId				INT,
	@GroupId			INT,
	@MemberId           INT,
	@TransactionXML		NVARCHAR(MAX)
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @AHID			INT
	IF(@TransactionMode = 'C')
		SELECT @AHID = CAST(SettingValue AS INT) FROM SystemSettings  WHERE SettingName = 'GROUP_CASHINHAND'
	ELSE
		SELECT @AHID = AHID FROM BankMaster WHERE BankEntryID = @BankEntryId

	IF  ISNULL(@AccountMasterId, 0) = 0
	BEGIN
		DECLARE @TransType		INT
		DECLARE @Sno			INT
		DECLARE @ActStatusId	INT = (SELECT StatusID FROM StatusMaster WHERE StatusCode = 'ACT')
		
		SELECT @TransType = RefID from RefValueMaster where RefCode = 'GMR' AND RefMasterID IN(select RefMasterID from RefMaster where RefMasterCode = 'TRANSACTION_TYPE')
		EXEC uspGenerateObjectCodeByEntityCode 'ACCOUNT_MASTER', @Sno OUTPUT, @VoucherNumber OUTPUT

		INSERT INTO AccountMaster(TransactionDate, CodeSno, VoucherNumber, 	VoucherRefNumber, EmployeeID, AHID,  TransactionType,
			 Amount, TransactionMode,  ChequeNumber, ChequeDate, BankAccount, Narration,  StatusID,  IsGroup, GroupID, CreatedBy, CreatedOn,  IsPairedRecord,MemberID)
		SELECT @TransactionDate, @Sno, @VoucherNumber, @VoucherRefNumber, @CollectionAgent, @AHID, @TransType,
			@Amount, LEFT(@TransactionMode, 1), @ChequeNumber, @ChequeDate, @BankEntryId, @Narration, @ActStatusId, 1, @GroupId, @UserId, GETDATE(), 0	,@MemberId
	
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
				ModifiedOn = GETDATE(),
				MemberId=@MemberId
		WHERE AccountMasterID = @AccountMasterId
	END

	DECLARE @Handler INT

	EXEC SP_XML_PREPAREDOCUMENT @Handler out, @TransactionXML        
        
	Declare @InputTransactions TABLE(AHID INT, CrAmount MONEY)

	INSERT INTO @InputTransactions
	SELECT 
		IIF(ISNULL(SLAHID, 0) > 0, SLAHID, AHID), CrAmount
	FROM OPENXML(@handler, N'ArrayOfGroupMemberReceiptTranDto/GroupMemberReceiptTranDto', 2)         
	WITH
	(
		SLAHID		INT			'SLAHID',
		AHID		INT			'AHID',
		CrAmount   MONEY		'Amount'
	)X
	EXEC SP_XML_REMOVEDOCUMENT @Handler
	
	--FROM ACC - START
	DELETE AT
	FROM AccountTransactions AT
	LEFT JOIN @InputTransactions IT ON AT.AHID = IT.AHID 
	WHERE AT.AccountMasterID = @AccountMasterId AND IT.AHID IS NULL

	UPDATE AT
		SET AT.CrAmount = IT.CrAmount
	FROM AccountTransactions AT
	INNER JOIN @InputTransactions IT ON AT.AHID = IT.AHID 
	WHERE AT.AccountMasterID = @AccountMasterId

	INSERT INTO AccountTransactions(AccountMasterID,  AHID,   CrAmount, DrAmount, IsActive,   CreatedBy, CreatedOn, IsMaster)
	SELECT
		@AccountMasterId, IT.AHID, it.CrAmount,0, 1, @UserId, GETDATE(), 	0
	FROM @InputTransactions IT
	LEFT JOIN AccountTransactions AT ON AT.AHID = IT.AHID AND AT.AccountMasterID = @AccountMasterId
	WHERE  AT.AHID IS NULL
	--FROM ACC - END

	IF NOT EXISTS (SELECT 1 FROM AccountTransactions WHERE AccountMasterID = @AccountMasterId AND IsMaster = 1)
	BEGIN
		--TO ACC
		INSERT INTO AccountTransactions(AccountMasterID,  AHID,   CrAmount, DrAmount, IsActive,   CreatedBy, CreatedOn, IsMaster)
		SELECT @AccountMasterId, @AHID,0, @Amount, 1, @UserId, GETDATE(), 1
	END
	ELSE
	BEGIN
		--TO ACC
		UPDATE AccountTransactions
		SET AHID	= @AHID,
			DrAmount = @Amount,
			ModifiedBy = @UserId,
			ModifiedOn = GETDATE()
		WHERE AccountMasterID = @AccountMasterId AND IsMaster = 1
	END

	EXEC uspLoanRepaymentInsertUpdate_v1  @AccountMasterID =@AccountMasterId ,@GroupID =@GroupId,@MemberID = @MemberId ,@IsMember =1,@TransactionDate=@TransactionDate

	SELECT @VoucherNumber = VoucherNumber FROM AccountMaster WHERE AccountMasterID = @AccountMasterId
END


