ALTER Proc [dbo].[uspGroupReceiptAndPayments]
(@GroupId int)
--Exec [uspGroupReceiptAndPayments] 282
AS
BEGIN
if OBJECT_ID('tempdb..#Receipts') is not null drop table #receipts
if OBJECT_ID('tempdb..#Payments') is not null drop table #Payments

	CREATE TABLE #Receipts(AHID int ,SLAccountHead varchar(200),OpeningBalance decimal, TotalByMonth decimal,monthname varchar(10 ))
	CREATE TABLE #Payments(AHID int ,SLAccountHead varchar(200),OpeningBalance decimal, TotalByMonth decimal,monthname varchar(10 ))
;with
 ct1 as(select * from AccountTransactions AT
--Join ReceiptAppropriationPriority RAP ON RAP.AHID=AT.AHID
where AccountMasterID in( 
select AccountMasterID from AccountMaster where TransactionType in (
select RefID from RefValueMaster where  refmasterid = 3027 and refcode in ('gop', 'GMR', 'GOR', 'GMP'))
))
, ct2 as(Select  AH.AHID, AH.AHName SLAccountHead, PAH.AHName  As GLAccountHead,Ah.OpeningBalance OpeningBalance,sum(DrAmount) total,AM.TransactionDate from ct1 c
JOIN AccountHead AH ON AH.AHID=c.AHID AND  AH.StatusID = 1
INNER JOIN AccountHead PAH ON AH.ParentAHID = PAH.AHID AND PAH.StatusId = 1
JOIN AccountMaster AM ON AM.AccountMasterID=c.AccountMasterID
Where AM.GroupId=@GroupId
Group By  AH.AHID, AH.AHName,PAH.AHName,AM.AccountMasterID,AM.TransactionDate,Ah.OpeningBalance)
Insert into Receipts
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
from Receipts
group by Ahid, SLAccountHead,OpeningBalance)
select * from cte4 where SLAccountHead !='1'
union all
Select 0,'Total',sum(OpeningBalance),
sum(Jan),sum(Feb),Sum(Mar),Sum(Apr) ,Sum(May),Sum(Jun),Sum(Jul),Sum(Aug),Sum(Sep),Sum(Oct),Sum(Nov),Sum(Dec)from cte4
where SLAccountHead !='1'

;with
 ct1 as(select * from AccountTransactions AT
--Join ReceiptAppropriationPriority RAP ON RAP.AHID=AT.AHID
where AccountMasterID in( 
select AccountMasterID from AccountMaster where TransactionType in (
select RefID from RefValueMaster where  refmasterid = 3027 and refcode in ('gop', 'GMR', 'GOR', 'GMP'))
))
, ct2 as(Select  AH.AHID, AH.AHName SLAccountHead, PAH.AHName  As GLAccountHead,Ah.OpeningBalance OpeningBalance,sum(CrAmount) total,AM.TransactionDate from ct1 c
JOIN AccountHead AH ON AH.AHID=c.AHID AND  AH.StatusID = 1
INNER JOIN AccountHead PAH ON AH.ParentAHID = PAH.AHID AND PAH.StatusId = 1
JOIN AccountMaster AM ON AM.AccountMasterID=c.AccountMasterID
Where AM.GroupId=@GroupId
Group By  AH.AHID, AH.AHName,PAH.AHName,AM.AccountMasterID,AM.TransactionDate,Ah.OpeningBalance)
Insert into Payments
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
from Payments
group by Ahid, SLAccountHead,OpeningBalance)
select * from cte4 where SLAccountHead !='1'
union all
Select 0,'Total',sum(OpeningBalance),
sum(Jan),sum(Feb),Sum(Mar),Sum(Apr) ,Sum(May),Sum(Jun),Sum(Jul),Sum(Aug),Sum(Sep),Sum(Oct),Sum(Nov),Sum(Dec)from cte4
where SLAccountHead !='1'

END