using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogic.Implementation
{
   public  class AccountHeadMappingDll
    {
       public ResultDto AccountHeadMappnig(DataTable dt)
       {
           ResultDto objResult = new ResultDto();

           AdoHelper obj = new AdoHelper();
           SqlParameter[] parms = new SqlParameter[1];

           parms[0] = new SqlParameter("@DataTable", dt);
           parms[0].SqlDbType = System.Data.SqlDbType.Structured;

           int i = Convert.ToInt16(obj.ExecNonQueryProc("uspAccountHeadMappingInsert", parms));
           if (i > 0)
           {
               objResult.ObjectCode = "TRUE";
               objResult.Message = "Additional Security Details Inserted Successfullly";
               objResult.ObjectId = 0;

           }
           else
           {

               objResult.ObjectCode = "FALSE";
               objResult.Message = "Fail To Insert Additional Security Details";
               objResult.ObjectId = 0;

           }
           return objResult;
       }
       public List<AccountheadMappingDto> GetAllAccountHead()
       {
           List<AccountheadMappingDto> lstAccountHeadDto = new List<AccountheadMappingDto>();
           try
           {
               AdoHelper obj = new AdoHelper();
               SqlDataReader sdr = obj.ExecDataReaderProc("uspGetAllAccountHeadMapping");
               while (sdr.Read())
               {
                   AccountheadMappingDto objAccountHeadDto = new AccountheadMappingDto();
                   objAccountHeadDto.FedAhid = Convert.ToInt32(sdr["FedAhiD"]);
                   objAccountHeadDto.GroupAHId = Convert.ToInt32(sdr["GroupAHID"]);
                   lstAccountHeadDto.Add(objAccountHeadDto);
               }
           }
           catch (Exception ex)
           {

           }
           return lstAccountHeadDto;
       }
    }
}
