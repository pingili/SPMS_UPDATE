using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using CoreComponents;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class RAPService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;

        public RAPService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        public List<RAPDto> GetRAPByType(bool isGroup)
        {
            var lstRapDetails = _dbContext.uspGetRAPByType(isGroup).ToList();

            List<RAPDto> lstRapDto = new List<RAPDto>();
            foreach (var rap in lstRapDetails)
            {
                lstRapDto.Add(new RAPDto()
                {
                    AHID = rap.AHID,
                   // AccountType = rap.AccountType,
                    AHCode = rap.AHCode,
                    AHName = rap.AHName,
                    Priority = rap.Priority == null ? default(byte) : rap.Priority.Value,
                   // AccountNumber = rap.AccountNumber
                    Category = rap.Category
                });
            }
            return lstRapDto;
        }

        public ResultDto ManageRAPByType(bool isGroup, List<RAPDto> lstRapDto, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = isGroup ? "Group RAP" : "Federation RAP";
            try
            {
                ObjectParameter prmRetVal = new ObjectParameter("RetVal", default(int));

                string rapXML = CommonMethods.SerializeListDto<List<RAPDto>>(lstRapDto);

                int effectedCount = _dbContext.uspManageRAP(rapXML, isGroup, userId, prmRetVal);

                resultDto.ObjectId = (int)prmRetVal.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully.", obectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", obectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }
    }
}
