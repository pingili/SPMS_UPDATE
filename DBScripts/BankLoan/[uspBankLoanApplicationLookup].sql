USE [SPMSUAT]
GO
/****** Object:  StoredProcedure [dbo].[uspBankLoanApplicationLookup]    Script Date: 11/18/2017 12:39:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--uspBankLoanApplicationLookup 188, 1
ALTER PROC [dbo].[uspBankLoanApplicationLookup]
	@GroupId	INT,
	@UserId		INT
AS
BEGIN
	
	SELECT BankLoanId,
		BM.BankName,
		GLAHID,
		AH.AHName ,
		LinkageNumber,
		RequestDate,
		RequestedAmount,
		ApprovedDate,
		ApprovedAmount,
		DisbursementDate,
		DisbursementAmount,
		NoOfInStallments,
		EMI,
		SM.[Status],
		SM.StatusCode
	FROM BankLoanMaster LM
		INNER JOIN StatusMaster SM ON SM.StatusID = LM.Status AND StatusCode != 'DISC'
		Inner Join BankMaster BM ON BM.BankEntryID=LM.BankEntryId
		LEFT JOIN AccountHead AH ON AH.AHID = LM.SLAHID
	WHERE LM.GroupID = @GroupId
		
END

