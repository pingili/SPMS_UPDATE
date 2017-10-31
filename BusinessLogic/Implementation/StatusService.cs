////using BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using MFIEntityFrameWork;
using BusinessLogic.AutoMapper;
using AutoMapper;

namespace BusinessLogic
{
   public class StatusService
    {
       private readonly MFISDBContext _dbContext;

       public StatusService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
       public List<StatusMasterDto> GetAll()
        {
            List<uspStatusMasterGetAll_Result> lstuspStatusMasterGetAll_Result = _dbContext.uspStatusMasterGetAll().ToList();
            List<StatusMasterDto> lstStatusMasterDto = Mapper.Map<List<uspStatusMasterGetAll_Result>, List<StatusMasterDto>>(lstuspStatusMasterGetAll_Result);
            return lstStatusMasterDto;

        }

       public StatusMasterDto GetByStatusCode(string StatusCode)
        {
            List<uspStatusMasterGetbByStatusCode_Result> lstuspStatusMasterGetbByStatusCode_Result = _dbContext.uspStatusMasterGetbByStatusCode(StatusCode).ToList();
            StatusMasterDto objStatusMasterDto = Mapper.Map<List<uspStatusMasterGetbByStatusCode_Result>, StatusMasterDto>(lstuspStatusMasterGetbByStatusCode_Result);
            return objStatusMasterDto;
        }

       public List<SelectListDto> GetStatusSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspStatusMasterGetAll_Result> lstuspStatusMasterGetAll_Result = _dbContext.uspStatusMasterGetAll().ToList();
           foreach(var status in lstuspStatusMasterGetAll_Result)
           {
               SelectListDto objSelectListDto = new SelectListDto()
               {
                   ID=status.StatusID,
                   Text=status.Status
               };
               lstSelectListDto.Add(objSelectListDto);
           }
           return lstSelectListDto;
            
        }
    }
}
