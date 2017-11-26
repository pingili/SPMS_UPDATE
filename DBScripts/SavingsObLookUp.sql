ALTER proc [dbo].[uspDepositsLookupTable] 
	@Type      BIT,
	@GroupId  int
AS
BEGIN 
	DECLARE @DynamicPivotQuery  NVARCHAR(MAX)
	DECLARE @ColumnName NVARCHAR(MAX)

	---Get distinct values of the PIVOT Column 
	SELECT @ColumnName= ISNULL(@ColumnName + ',','') + QUOTENAME(AHName) 
	FROM (
		
SELECT DISTINCT (
	SELECT AHName from AccountHead where AHID = ( SELECT ParentAHID FROM AccountHead WHERE AHID = SLAccountAHID)
	) as AHName 
		from Deposits WHERE SLAccountAHID IS NOT NULL
	) AS A

	IF ISNULL(@ColumnName, '') = ''
	BEGIN
		RETURN;
	END

	SET @DynamicPivotQuery = 'SELECT *
	FROM (
		SELECT M.MemberId, M.MemberName+''(''+M.MemberCode+'')'' MemberName, MAH.AHName AS AccountHeadName, DepositAmount
		FROM (
			SELECT MemberID, SLAccountAHID, SUM(ISNULL(DepositAcmount, 0)) as DepositAmount
			FROM Deposits d JOIN AccountHead ah on d.SLAccountAHID = AHID
			where (d.isOpeningBalance = 1 OR d.IsOpeningBalance is null) AND  d.GroupID = '+  CAST(@GroupId AS VARCHAR)--3-- AND MemberID = 70
			+'AND IsMemberDeposit = '+ CAST(@Type AS VARCHAR)
			+'group by d.MemberID, SLAccountAHID

		)A
		JOIN Member M ON M.MemberID = A.MemberID
		JOIN AccountHead AH ON AH.AHID = SLAccountAHID 
		JOIN AccountHead MAH ON MAH.AHID = AH.ParentAHID
		
	) as s
	PIVOT
	(
		MIN(DepositAmount)
		FOR AccountHeadName  IN (' + @ColumnName + ')
	)AS pvt
	'

	--print @DynamicPivotQuery
	--Execute the Dynamic Pivot Query
	EXEC sp_executesql @DynamicPivotQuery
END


