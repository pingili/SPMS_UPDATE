using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using MFIEntityFrameWork;
using BusinessLogic.AutoMapper;
using System.Data.Entity.Core.Objects;
using AutoMapper;

namespace BusinessLogic
{
    public class EmployeeKycService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;
        public EmployeeKycService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion
        public EmployeeKYCDto GetByKycID(int employeekycId)
        {
            var objuspEmployeeKYCGetByEmployeeKYCIDResult = _dbContext.uspEmployeeKYCGetByEmployeeKYCID(employeekycId).ToList().FirstOrDefault();

            EmployeeKYCDto objEmployeeKYCDto = AutoMapperEntityConfiguration.Cast<EmployeeKYCDto>(objuspEmployeeKYCGetByEmployeeKYCIDResult);

            return objEmployeeKYCDto;
        }

        public List<EmployeeKYCDto> GetByEmployeeID(int employeeId)
        {
            var objuspEmployeeKYCGetByEmployeeIDResult = _dbContext.uspEmployeeKYCGetByEmployeeID(employeeId).ToList();
            List<EmployeeKYCDto> lstEmployeeKYCDto = new List<EmployeeKYCDto>();
            foreach (var kyc in objuspEmployeeKYCGetByEmployeeIDResult)
            {
                EmployeeKYCDto objEmployeeKYCDto = Mapper.Map<uspEmployeeKYCGetByEmployeeID_Result, EmployeeKYCDto>(kyc);
                lstEmployeeKYCDto.Add(objEmployeeKYCDto);
            }
            return lstEmployeeKYCDto;

        }



        public ResultDto Insert(EmployeeKYCDto employeeKyc)
        {
            return InsertUpdateEmployeeKYC(employeeKyc);

        }

        public ResultDto Update(EmployeeKYCDto employeeKyc)
        {
            return InsertUpdateEmployeeKYC(employeeKyc);
        }

        private ResultDto InsertUpdateEmployeeKYC(EmployeeKYCDto employeeKYCDto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "EmployeeKYC";

            try
            {
                ObjectParameter prmEmployeeKYCID = new ObjectParameter("EmployeeKYCID", employeeKYCDto.EmployeeKYCID);


                int count = _dbContext.uspEmployeeKYCInsertUpdate(prmEmployeeKYCID, employeeKYCDto.EmployeeID, employeeKYCDto.KYCType, employeeKYCDto.KYCNumber, employeeKYCDto.FileName, employeeKYCDto.ActualFileName, employeeKYCDto.StatusID, employeeKYCDto.UserID);

                resultDto.ObjectId = (int)prmEmployeeKYCID.Value;
                //resultDto.ObjectCode = string.IsNullOrEmpty((string)prmMandalCode.Value) ? mandalDto.MandalCode : (string)prmMandalCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully ", objectName);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", objectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", objectName);

            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

    }
}
