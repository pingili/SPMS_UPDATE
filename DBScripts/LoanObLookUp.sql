ALTER proc [dbo].[uspLoanMasterGetGroupOldLoansLookUpTable] 
	@LoanType		CHAR(1) ,
	@GroupId		INT 
AS
--uspLoanMasterGetGroupOldLoansLookUpTable 'M', 282
BEGIN 
	DECLARE @DynamicPivotQuery  NVARCHAR(MAX)
	DECLARE @ColumnName			NVARCHAR(MAX)

	---Get distinct values of the PIVOT Column 
	SELECT @ColumnName = ISNULL(@ColumnName + ',','')  + QUOTENAME(AHName) 
	FROM (
		SELECT DISTINCT MA.AHName  
		FROM LoanMaster L  INNER JOIN AccountHead AH ON L.SLAccountNumber=AH.AHID INNER JOIN AccountHead MA ON MA.AHID = AH.ParentAHID 
		WHERE GroupID = @GroupId
	) AS A

	IF(ISNULL(@ColumnName, '') = '')
	BEGIN
		RETURN;
	END

	SET @DynamicPivotQuery = 'SELECT *
	FROM (
		SELECT M.MemberId, M.MemberName+''(''+M.MemberCode+'')'' MemberName, MA.AHName AS AccountHeadName,DisbursementDate, isnull( cast(DisbursedAmount as varchar), '''')  + ''~''+ isnull( cast(OutStandingAmount as varchar), '''') as DisbursedAmount
		FROM (
			SELECT L.MemberID, 
				L.SLAccountNumber,
				SUM(L.DisbursedAmount) AS DisbursedAmount, 
				SUM(L.OutStandingAmount) AS OutStandingAmount,
				L.DisbursementDate
			FROM LoanMaster L 
			INNER JOIN Member M ON L.MemberID = M.MemberID
			INNER JOIN Statusmaster SM ON L.StatusID=SM.StatusID
			WHERE IsOldLoan=1 AND LoanType=''' + CAST(@LoanType AS VARCHAR) + ''' AND L.GroupID= '+ CAST(@GroupId AS VARCHAR)+ '
			AND L.StatusID<>3 AND M.StatusID <>3
			GROUP BY L.MemberID, SLAccountNumber,L.DisbursementDate
		)L
		INNER JOIN Member M ON L.MemberID  = M.MemberID
		INNER JOIN AccountHead AH ON L.SLAccountNumber=AH.AHID
		INNER JOIN AccountHead MA ON MA.AHID = AH.ParentAHID
	) as s
	PIVOT
	(
		MIN(DisbursedAmount)
		FOR AccountHeadName  IN (' + @ColumnName + ')
	)AS pvt
	'

	--print @DynamicPivotQuery
	--Execute the Dynamic Pivot Query
	EXEC sp_executesql @DynamicPivotQuery
END


