
ALTER Proc [dbo].[uspGroupIncomeAndExpendature]
(@GroupId int)
--Exec [uspGroupIncomeAndExpendature] 282
AS
BEGIN
if OBJECT_ID('tempdb..#Income') is not null drop table #Income
if OBJECT_ID('tempdb..#Expendature') is not null drop table #Expendature

	CREATE TABLE #Income(AHID int ,SLAccountHead varchar(200),OpeningBalance decimal, TotalByMonth decimal,monthname varchar(10 ))
	CREATE TABLE #Expendature(AHID int ,SLAccountHead varchar(200),OpeningBalance decimal, TotalByMonth decimal,monthname varchar(10 ))
;with
 ct1 as(
select AH.AHID, AH.AHName,Ah.OpeningBalance,AT.DrAmount,AT.CrAmount,AM.TransactionDate,AM.AccountMasterID from AccountHead AH
Left JOIN VWGroupAccountTree VA ON VA.GLAccountId=AH.AHID
Left JOIN AccountMaster AM ON AM.AHID=AH.AHID
Left JOIN AccountTransactions AT ON AT.AccountMasterID=AM.AccountMasterID
where Type in ('Income','Expenditure'))

, ct2 as(Select  AH.AHID, AH.AHName SLAccountHead, PAH.AHName  As GLAccountHead,Ah.OpeningBalance OpeningBalance,sum(DrAmount) total,AM.TransactionDate from ct1 c
Left JOIN AccountHead AH ON AH.AHID=c.AHID AND  AH.StatusID = 1
Left JOIN AccountHead PAH ON AH.ParentAHID = PAH.AHID AND PAH.StatusId = 1
Left JOIN AccountMaster AM ON AM.AccountMasterID=c.AccountMasterID AND AM.GroupID=@GroupId
--Where AM.GroupId=282
Group By  AH.AHID, AH.AHName,PAH.AHName,AM.AccountMasterID,AM.TransactionDate,Ah.OpeningBalance)
--Select * from ct2
Insert into #Income
select AHID,SLAccountHead,OpeningBalance,sum(total),Month(TransactionDate) [Month]  from ct2 
Group By AHID,SLAccountHead,TransactionDate,OpeningBalance

;with cte4 as(select Ahid,SLAccountHead
  ,isnull(OpeningBalance, 0) OpeningBalance
  ,isnull(sum(case when monthname = 1 then TotalByMonth end), 0) Jan
  ,isnull(sum(case when monthname = 2 then TotalByMonth end), 0) Feb 
  ,isnull(sum(case when monthname = 3 then TotalByMonth end), 0) Mar
  ,isnull(sum(case when monthname = 4 then TotalByMonth end), 0) Apr
  ,isnull(sum(case when monthname = 5 then TotalByMonth end), 0) May
  ,isnull(sum(case when monthname = 6 then TotalByMonth end), 0) Jun
  ,isnull(sum(case when monthname = 7 then TotalByMonth end), 0) Jul
  ,isnull(sum(case when monthname = 8 then TotalByMonth end), 0) Aug
  ,isnull(sum(case when monthname = 9 then TotalByMonth end), 0) Sep
  ,isnull(sum(case when monthname = 10 then TotalByMonth end), 0) Oct
  ,isnull(sum(case when monthname = 11 then TotalByMonth end), 0) Nov
  ,isnull(sum(case when monthname = 12 then TotalByMonth end), 0) Dec
from #income
group by Ahid, SLAccountHead,OpeningBalance)
select * from cte4 where SLAccountHead !='1'
union all
Select 0,'Total',sum(OpeningBalance),
sum(Jan),sum(Feb),Sum(Mar),Sum(Apr) ,Sum(May),Sum(Jun),Sum(Jul),Sum(Aug),Sum(Sep),Sum(Oct),Sum(Nov),Sum(Dec)from cte4
where SLAccountHead !='1'

;with
 ct1 as(
select AH.AHID, AH.AHName,Ah.OpeningBalance,AT.DrAmount,AT.CrAmount,AM.TransactionDate,AM.AccountMasterID from AccountHead AH
Left JOIN VWGroupAccountTree VA ON VA.GLAccountId=AH.AHID
Left JOIN AccountMaster AM ON AM.AHID=AH.AHID
Left JOIN AccountTransactions AT ON AT.AccountMasterID=AM.AccountMasterID
where Type in ('Income','Expenditure'))

, ct2 as(Select  AH.AHID, AH.AHName SLAccountHead, PAH.AHName  As GLAccountHead,Ah.OpeningBalance OpeningBalance,sum(CrAmount) total,AM.TransactionDate from ct1 c
Left JOIN AccountHead AH ON AH.AHID=c.AHID AND  AH.StatusID = 1
Left JOIN AccountHead PAH ON AH.ParentAHID = PAH.AHID AND PAH.StatusId = 1
Left JOIN AccountMaster AM ON AM.AccountMasterID=c.AccountMasterID AND AM.GroupId=@GroupId
--Where AM.GroupId=@GroupId
Group By  AH.AHID, AH.AHName,PAH.AHName,AM.AccountMasterID,AM.TransactionDate,Ah.OpeningBalance)
Insert into #Expendature
select AHID,SLAccountHead,OpeningBalance,sum(total),Month(TransactionDate) [Month]  from ct2 
Group By AHID,SLAccountHead,TransactionDate,OpeningBalance


;with cte4 as(select Ahid,SLAccountHead
  ,isnull(OpeningBalance, 0) OpeningBalance
  ,isnull(sum(case when monthname = 1 then TotalByMonth end), 0) Jan
  ,isnull(sum(case when monthname = 2 then TotalByMonth end), 0) Feb 
  ,isnull(sum(case when monthname = 3 then TotalByMonth end), 0) Mar
  ,isnull(sum(case when monthname = 4 then TotalByMonth end), 0) Apr
  ,isnull(sum(case when monthname = 5 then TotalByMonth end), 0) May
  ,isnull(sum(case when monthname = 6 then TotalByMonth end), 0) Jun
  ,isnull(sum(case when monthname = 7 then TotalByMonth end), 0) Jul
  ,isnull(sum(case when monthname = 8 then TotalByMonth end), 0) Aug
  ,isnull(sum(case when monthname = 9 then TotalByMonth end), 0) Sep
  ,isnull(sum(case when monthname = 10 then TotalByMonth end), 0) Oct
  ,isnull(sum(case when monthname = 11 then TotalByMonth end), 0) Nov
  ,isnull(sum(case when monthname = 12 then TotalByMonth end), 0) Dec
from #Expendature
group by Ahid, SLAccountHead,OpeningBalance)
select * from cte4 where SLAccountHead !='1'
union all
Select 0,'Total',sum(OpeningBalance),
sum(Jan),sum(Feb),Sum(Mar),Sum(Apr) ,Sum(May),Sum(Jun),Sum(Jul),Sum(Aug),Sum(Sep),Sum(Oct),Sum(Nov),Sum(Dec)from cte4
where SLAccountHead !='1'

END