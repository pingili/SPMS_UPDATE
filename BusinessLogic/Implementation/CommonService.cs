using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MFIEntityFrameWork;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using System.Dynamic;
using System.Reflection;
using CoreComponents;
using System.Data.Entity.Core.Objects;

namespace BusinessLogic
{
    public class CommonService 
    {
        private MFISDBContext _dbContext;

        public CommonService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }

        public List<ReferenceValueDto> PrepareSelectListByRefCode(string refMasterCode)
        {
            List<ReferenceValueDto> referenceValues = new List<ReferenceValueDto>();
            ReferenceValueService _referenceValueService = new ReferenceValueService();
            referenceValues= _referenceValueService.GetByRefMasterCode(refMasterCode);
            return referenceValues;
        }

        public List<SelectListDto> GetRefDropDownByRefMasterCode(string refMasterCode)
        {
            var lstSelectListDto = new List<SelectListDto>();
            var referenceValues = new List<ReferenceValueDto>();
            ReferenceValueService _referenceValueService = new ReferenceValueService();
            referenceValues = _referenceValueService.GetByRefMasterCode(refMasterCode);

            foreach (var refvalue in referenceValues)
            {
                SelectListDto selectlist = new SelectListDto()
                {
                    ID = refvalue.RefID,
                    Text = refvalue.RefValue
                };
                lstSelectListDto.Add(selectlist);
            }
            return lstSelectListDto;
        }

        /// <summary>
        /// objectId - EmployeeId/OrgId/GroupId
        /// Entity - Employee/Org/Group
        /// </summary>
        /// <param name="lstbank"></param>
        /// <param name="objectId"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ResultDto InsertBankDetails(int objectId, Utilities.Enums.EntityCodes enumEntityCode, int userid, List<BankMasterDto> lstbank)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "bank";
            try
            {
                string bankxml = CommonMethods.SerializeListDto<List<BankMasterDto>>(lstbank);

                ObjectParameter prmObjectId = new ObjectParameter("ObjectId", objectId);

                int effectedRow = _dbContext.uspObjectBanksInsertUpdate(prmObjectId, enumEntityCode.ToString(), userid, bankxml);

                resultDto.ObjectId = (int)prmObjectId.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, resultDto.ObjectCode);
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
