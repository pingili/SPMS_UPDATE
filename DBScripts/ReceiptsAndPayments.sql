ALTER Proc [dbo].[uspGroupReceiptAndPayments]
(@GroupId int)
--Exec [uspGroupReceiptAndPayments] 282
AS
BEGIN

CREATE TABLE Receipts(AHID int ,SLAccountHead varchar(200), TotalByMonth decimal,monthname varchar(10 ))
CREATE TABLE Payments(AHID int ,SLAccountHead varchar(200), TotalByMonth decimal,monthname varchar(10 ))
;with
 ct1 as(select AH.AHID, AH.AHName SLAccountHead, PAH.AHName  As GLAccountHead ,sum(DrAmount) total,AM.TransactionDate from AccountTransactions AT
Join ReceiptAppropriationPriority RAP ON RAP.AHID=AT.AHID
JOIN AccountHead AH ON AH.AHID=RAP.AHID AND RAP.StatusId = 1 AND AH.StatusID = 1
INNER JOIN AccountHead PAH ON AH.ParentAHID = PAH.AHID AND PAH.StatusId = 1
JOIN AccountMaster AM ON AM.AccountMasterID=AT.AccountMasterID
Where AM.GroupId=@GroupId
Group By  AH.AHID, AH.AHName,PAH.AHName,AM.AccountMasterID,AM.TransactionDate)
Insert into Receipts
select AHID,SLAccountHead,sum(total),Month(TransactionDate) [Month]  from ct1 
Group By AHID,SLAccountHead,TransactionDate
Union All
SELECT distinct SLAccountId AHID, SLNAME  AS AHNAME , SUM(DrAmount),Month(Am.TransactionDate) [Month] FROM VWGroupAccountTree VW
JOIN AccountHead AH ON AH.AHID=VW.SLAccountId AND AH.StatusID = 1
 JOIN AccountTransactions AT ON AT.AHID=Vw.SLAccountId
INNER JOIN AccountHead PAH ON AH.ParentAHID = PAH.AHID AND PAH.StatusId = 1
 JOIN AccountMaster AM ON AM.AccountMasterID=AT.AccountMasterID
Where AM.GroupId=@GroupId AND  Vw.TransactionType in ('G','B')
GROUP BY SLAccountId,SLNAME,AM.TransactionDate

select Ahid,SLAccountHead
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
group by Ahid, SLAccountHead
Order By Ahid

;with
 ct1 as(select AH.AHID, AH.AHName SLAccountHead, PAH.AHName  As GLAccountHead ,sum(CrAmount) total,AM.TransactionDate from AccountTransactions AT
Join ReceiptAppropriationPriority RAP ON RAP.AHID=AT.AHID
JOIN AccountHead AH ON AH.AHID=RAP.AHID AND RAP.StatusId = 1 AND AH.StatusID = 1
INNER JOIN AccountHead PAH ON AH.ParentAHID = PAH.AHID AND PAH.StatusId = 1
JOIN AccountMaster AM ON AM.AccountMasterID=AT.AccountMasterID
Where AM.GroupId=@GroupId
Group By  AH.AHID, AH.AHName,PAH.AHName,AM.AccountMasterID,AM.TransactionDate)
Insert into Receipts
select AHID,SLAccountHead,sum(total),Month(TransactionDate) [Month]  from ct1 
Group By AHID,SLAccountHead,TransactionDate
Union All
SELECT distinct SLAccountId AHID, SLNAME  AS AHNAME , SUM(CrAmount),Month(Am.TransactionDate) [Month] FROM VWGroupAccountTree VW
JOIN AccountHead AH ON AH.AHID=VW.SLAccountId AND AH.StatusID = 1
 JOIN AccountTransactions AT ON AT.AHID=Vw.SLAccountId
INNER JOIN AccountHead PAH ON AH.ParentAHID = PAH.AHID AND PAH.StatusId = 1
 JOIN AccountMaster AM ON AM.AccountMasterID=AT.AccountMasterID
Where AM.GroupId=@GroupId AND  Vw.TransactionType in ('G','B')
GROUP BY SLAccountId,SLNAME,AM.TransactionDate

select Ahid,SLAccountHead
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
group by Ahid, SLAccountHead
Order by Ahid

Drop Table Receipts
Drop Table Payments
END