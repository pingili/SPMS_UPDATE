using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
//using BusinessLogic.Interface;
using BusinessLogic.AutoMapper;
using MFIEntityFrameWork;
using System.Data.Entity.Core.Objects;
using AutoMapper;

namespace BusinessLogic
{
    public class MemberKycService 
    {

      #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;
        public MemberKycService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion
        public MemberKYCDto GetByKycID(int memberkycId)
        {
            var objuspMemberKYCGetByMemberKYCIDResult = _dbContext.uspMemberKYCGetByMemberKYCID(memberkycId).ToList().FirstOrDefault();

            MemberKYCDto objMemberKYCDto = AutoMapperEntityConfiguration.Cast<MemberKYCDto>(objuspMemberKYCGetByMemberKYCIDResult);

            return objMemberKYCDto;
        }

        public List<MemberKYCDto> GetByMemberID(int memberId)
        {
            var lstuspMemberKYCGetByMemberIDResult = _dbContext.uspMemberKYCGetByMemberID(memberId).ToList();
            List<MemberKYCDto> lstMemberKYCDto = new List<MemberKYCDto>();
            foreach (var kyc in lstuspMemberKYCGetByMemberIDResult)
            {
                MemberKYCDto objMemberKYCDto = Mapper.Map<uspMemberKYCGetByMemberID_Result, MemberKYCDto>(kyc);
                lstMemberKYCDto.Add(objMemberKYCDto);
            }
            return lstMemberKYCDto;
        }

        public ResultDto Insert(MemberKYCDto memberKyc)
        {
            return InsertUpdateMemberKYC(memberKyc);
        }

        public ResultDto Update(MemberKYCDto memberKyc)
        {
            return InsertUpdateMemberKYC(memberKyc);
        }

        private ResultDto InsertUpdateMemberKYC(MemberKYCDto memberKYCDto)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "memberKyc";

            try
            {
                ObjectParameter prmMemberKYCID = new ObjectParameter("MemberKYCID",memberKYCDto.MemberKYCID);
                int count = _dbContext.uspMemberKYCInsertUpdate(prmMemberKYCID, memberKYCDto.MemberID, memberKYCDto.KYCNumber, memberKYCDto.KYCType, memberKYCDto.FileName, memberKYCDto.ActualFileName, memberKYCDto.UserID);

                resultDto.ObjectId = (int)prmMemberKYCID.Value;
                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully ", obectName);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", obectName);
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
