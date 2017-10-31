using BusinessEntities;
using BusinessLogic.AutoMapper;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CoreComponents;
using System.Data.Entity.Core.Objects;

namespace BusinessLogic
{
    public class DepreciationService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;

        public DepreciationService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        public List<DepreciationDto> GetDepreciation()
        {
            var objRpaResult = _dbContext.uspDepreciation().ToList();

            List<DepreciationDto> lstDepreciationDto = new List<DepreciationDto>();
            foreach (var obj in objRpaResult)
            {
                lstDepreciationDto.Add(new DepreciationDto()
                {
                    AHID = obj.AHID,
                    AHName = obj.AHName,
                    Rate = obj.Rate == null ? default(byte) : obj.Rate.Value
                });
            }

            return lstDepreciationDto;
        }

        public ResultDto InsertUpdateDepreciationRecords(List<DepreciationDto> lstDepreciationDto, int userID)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "depreciation";
            try
            {
                ObjectParameter prmRetVal = new ObjectParameter("RetVal", default(int));
                
                string depreciationXML = CommonMethods.SerializeListDto<List<DepreciationDto>>(lstDepreciationDto);
                
                int effectedCount = _dbContext.uspManageDepreciation(depreciationXML, userID, prmRetVal);

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
