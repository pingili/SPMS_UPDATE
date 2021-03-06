USE [SPMSUAT]
GO
/****** Object:  StoredProcedure [dbo].[uspBankLoanApplicationGetByBankLoanID]    Script Date: 11/18/2017 12:37:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[uspBankLoanApplicationGetByBankLoanID] 3
ALTER proc [dbo].[uspBankLoanApplicationGetByBankLoanID]
@BankLoanId int
AS

BEGIN
  SELECT 
    BankLoanId
,GroupId
,BankEntryId
,LoanMasterId
,SLAHID
,LinkageNumber
,RequestDate
,RequestedAmount
,ApprovedDate
,ApprovedAmount
,DisbursementDate
,DisbursementAmount
,NoOfInStallments
,EMI
,Narration
,DueDate
,InterestRate
,ReferenceNumber
,(SELECT S.Status FROM BankLoanMaster BL JOIN StatusMaster S ON S.StatusID = BL.Status WHERE BankLoanId = @BankLoanId) [Status]
,CreatedBy

  FROM BANKLOANMASTER where BankLoanId=@BankLoanId
END

