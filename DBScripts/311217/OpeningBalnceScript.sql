   insert into TypeQueries(TypeCode,TypeQuery,Param1)
   Values('GROUP_OR_BANK_AH_OB',
   'DECLARE @StatusId INT = (SELECT StatusId FROM StatusMaster WHERE StatusCode = ''ACT'') 
   select bm.AHID AS Id, AHName + '':'' + AHCode as Name  from BankMaster bm  inner join BankDetails bd on bm.BankEntryID = bd.BankEntryID AND BM.StatusID = @StatusId  
   INNER JOIN EntityMaster EM ON EM.EntityID = BD.EntityID AND EntityCode = ''GROUP_MASTER''  
   INNER JOIN AccountHead AH ON AH.AHID = BM.AHID AND AH.StatusID = @StatusId  Where ObjectID  = @Param1','@Param1')

