USE [SPMSUAT]
GO
/****** Object:  StoredProcedure [dbo].[uspBankLoanInsertUpdate]    Script Date: 11/18/2017 12:35:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 --uspBankLoanInsertUpdate 3,null,118,"4/7/2017",'4/12/2017','4/15/2017',244,242,185,1000,10000,10000,11,1,12345,124,'asdfasf',1,'4/7/2017',1000
ALTER PROC [dbo].[uspBankLoanInsertUpdate]
	@BankLoanId int OUTPUT,
	@Status	                     VARCHAR(50) =null OUTPUT,
	@GroupID                     INT,
	@LoanApplicationDate         DATE,
	@LoanAmountApprovedDate         DATE,
	@DisbursedDate         DATE,
	@SLAHId                 INT,
	@GLAHId                 INT,
	@BankEntryId                   INT,
	@LoanAmountApplied           BigInt,
	@LoanAmountApprovedAmount           BigInt,
	@DisbursedAmount           BigInt,
	@NoofInstallmentsProposed    TINYINT,
	@UserId                      INT,
	@LoanNumber          Varchar(56),
	@RefNumber					VARCHAR(50) = NULL,
	@Narration			VARCHAR(MAx),
	@InterstRate        int,
	@DueDate        Date,
	@EMI int
AS
BEGIN
	BEGIN TRY
		IF ISNULL(@BankLoanId,0)=0 
		BEGIN
		DECLARE @StatusID int
			SELECT @StatusID =StatusID FROM StatusMaster WHERE StatusCode = 'APP'
			INSERT BankLoanMaster
					(
					GroupId,BankEntryId,SLAHID,GLAHID,LinkageNumber,RequestDate,RequestedAmount,ApprovedDate,ApprovedAmount,DisbursementDate,DisbursementAmount,
					NoOfInStallments,EMI,Narration,DueDate,InterestRate,ReferenceNumber,Status,CreatedDate,CreatedBy
					)
					SELECT @GroupID,@BankEntryId,@SLAHId,@GLAHId,@LoanNumber,@LoanApplicationDate,@LoanAmountApplied,@LoanAmountApprovedDate,@LoanAmountApprovedAmount,@DisbursedDate,@DisbursedAmount,
					@NoofInstallmentsProposed,@EMI,@Narration,@DueDate,@InterstRate,@RefNumber,@StatusID, GEtdate(),@UserId

					Select @BankLoanId = SCOPE_IDENTITY()
		END
		ELSE
		BEGIN
		   UPDATE BankLoanMaster
		   SET 
					
					SLAHID=@SLAHId,
					 GLAHId=@GLAHId
					,LinkageNumber=@LoanNumber
					,RequestDate=@LoanApplicationDate
					,RequestedAmount=@LoanAmountApplied
					,ApprovedDate=@LoanAmountApprovedDate
					,ApprovedAmount=@LoanAmountApprovedAmount
					,DisbursementDate=@DisbursedDate
					,DisbursementAmount=@DisbursedAmount
					,NoOfInStallments=@NoofInstallmentsProposed
					,EMI=@EMI
					,Narration=@Narration
					,DueDate=@DueDate
					,InterestRate=@InterstRate
					,ReferenceNumber=@RefNumber
					--,Status=@StatusID
					,ModifiedDate=GETDATE()
					,ModifiedBy=@UserId
					where BankLoanId=@BankLoanId
		END
		SELECT @Status = S.Status FROM BankLoanMaster BL JOIN StatusMaster S ON S.StatusId= BL.[Status] WHERE BankLoanId = @BankLoanId
	END TRY
	BEGIN CATCH
		INSERT INTO SystemExceptions(ExpSource, EXCEPTION, StackTrace, ExpType, ExpDate)
		SELECT 'uspBankLoanInsertUpdate', ERROR_MESSAGE(), ERROR_LINE(),ERROR_SEVERITY(), GETDATE()	

		SET @BankLoanId=-99;
	END CATCH
END




