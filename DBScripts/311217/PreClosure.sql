 ALTER Procedure usp_LoanPreclosureDemand
  (@LoanMasterId int
 ,@TransactionDate Date
 )
 AS
 --Exec usp_LoanPreclosureDemand 5597, '16/Aug/2017'
 BEGIN
 
       SELECT 
			LoanMasterID,
		     IIF (TOTAL.OutStandingAmount < 0,0,TOTAL.OutStandingAmount) PrincipleDemand,
			 IIF(TOTAL.InterestDemand < 0, 0, TOTAL.InterestDemand+ TOTAL.InterestDue) InterestDemand
		FROM
		(
			SELECT 
				LM.LoanMasterID ,
				LM.OutStandingAmount, 
				(LM.OutStandingAmount * GIR.ROI * DATEDIFF(DAY, Lm.LastPaidDate, @TransactionDate))/36500 InterestDemand,
				LM.MonthlyPrincipalDemand,
				IIF(LM.InterestDue is null , 0 , LM.InterestDue) InterestDue
			FROM LoanMaster LM 
			INNER JOIN GroupInterestRates GIR ON LM.GroupInterstRateID = GIR.GroupInterestRateID
		    INNER JOIN GroupInterestMaster GIM ON GIM.GroupInterestID = GIR.GroupInterestID
			WHERE LM.LoanMasterID = @LoanMasterId 
		)TOTAL

		END


		