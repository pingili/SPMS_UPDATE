using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessLogic
{
    public class BranchService 
    {

        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;
        public BranchService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        #region GetAll
        public List<BranchDto> GetAll()
        {
            List<uspBranchGetAll_Result> lstuspBranchGetAll_Result = _dbContext.uspBranchGetAll().ToList();
            //List<BranchDto> lstBranchMasterDto = Mapper.Map<List<uspBranchGetAll_Result>, List<BranchDto>>(lstuspBranchGetAll_Result);

            List<BranchDto> lstBranchDto = new List<BranchDto>();
            foreach (var result in lstuspBranchGetAll_Result)
                lstBranchDto.Add(Mapper.Map<uspBranchGetAll_Result, BranchDto>(result));

            return lstBranchDto;

        }
        #endregion

        #region GetLookup
        public List<BranchLookupDto> GetLookup()
        {
            var lstBranchLookupDto = new List<BranchLookupDto>();
            var uspBranchLookupResults = _dbContext.uspBranchLookup().ToList();
            foreach (var branch in uspBranchLookupResults)
            {
                BranchLookupDto lookupDto = Mapper.Map<uspBranchLookup_Result, BranchLookupDto>(branch);
                lstBranchLookupDto.Add(lookupDto);
            }

            return lstBranchLookupDto;
        }
        #endregion

        #region GetByID
        public BranchDto GetByID(int branchId)
        {
            var objuspBranchGetByIdResult = _dbContext.uspBranchGetById(branchId).ToList().FirstOrDefault();

            BranchDto objBranchDto = AutoMapperEntityConfiguration.Cast<BranchDto>(objuspBranchGetByIdResult);

            return objBranchDto;
        }
        #endregion

        #region  Insert
        public ResultDto Insert(BranchDto branchDto)
        {
            return InsertUpdateBranchMaster(branchDto);
        }
        #endregion

        #region Update
        public ResultDto Update(BranchDto branchDto)
        {
            return InsertUpdateBranchMaster(branchDto);
        }
        #endregion

        #region InsertUpdateBranchMaster
        private ResultDto InsertUpdateBranchMaster(BranchDto branchDto)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "branch";
            try
            {
                ObjectParameter prmBranchId = new ObjectParameter("BranchID", branchDto.BranchID);
                ObjectParameter prmBranchCode = new ObjectParameter("BranchCode", string.Empty);
                int effectedRow = _dbContext.uspBranchInsertUpdate(prmBranchId, branchDto.BranchName, branchDto.TEBranchName, branchDto.StartDate, branchDto.Phone,
                    branchDto.Email, branchDto.Address, branchDto.AccountantID, branchDto.AccountantFromDate, branchDto.ManagerID, branchDto.ManagerFromDate, branchDto.UserID, prmBranchCode);

                resultDto.ObjectId = (int)prmBranchId.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)prmBranchCode.Value) ? branchDto.BranchCode : (string)prmBranchCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, resultDto.ObjectCode);
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
        #endregion

        #region GetBranchSelectList
        public List<SelectListDto> GetBranchSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspBranchGetAll_Result> lstuspBranchGetAll_Result = _dbContext.uspBranchGetAll().ToList();


            foreach (var branch in lstuspBranchGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = branch.BranchID,
                    Text = branch.BranchName.ToString()
                };

                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        #endregion

        #region Delete
        public ResultDto Delete(int branchId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "branch";

            try
            {
                ObjectParameter prmBranchID = new ObjectParameter("BranchID", branchId);
                ObjectParameter prmBranchCode = new ObjectParameter("BranchCode", string.Empty);

                int effectedCount = _dbContext.uspBranchDelete(prmBranchID, prmBranchCode, userId);

                resultDto.ObjectId = (int)prmBranchID.Value;
                resultDto.ObjectCode = (string)prmBranchCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details deleted successfully", obectName, resultDto.ObjectCode);
                //else if (resultDto.ObjectId == -1)
                //    resultDto.Message = string.Format("selected {0} : {1} aleready used in other transaction, in order to delete please remove the dependencies", obectName, resultDto.ObjectCode);
                else
                    resultDto.Message = string.Format("Error occured while deleting {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }
        #endregion

        #region ChangeStatus
        public ResultDto ChangeStatus(int branchId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Branch";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmBranchID = new ObjectParameter("BranchID", branchId);
                ObjectParameter prmBranchCode = new ObjectParameter("BranchCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspBranchChangeStatus(prmBranchID, prmBranchCode, prmStatusCode, userId);

                resultDto.ObjectId = (int)prmBranchID.Value;
                resultDto.ObjectCode = (string)prmBranchCode.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details {2} successfully", obectName, resultDto.ObjectCode, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
                else
                    resultDto.Message = string.Format("Error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }
        #endregion

        #region GetViewByID
        public BranchViewDto GetViewByID(int branchId)

        {
            var prmBranchId = new ObjectParameter("BranchID", branchId);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspBranchGetViewByID, prmBranchId)
                .With<BranchViewDto>()
                .Execute();

            var branchViewDto = (results[0] as List<BranchViewDto>)[0];
            return branchViewDto;
        }
        #endregion

    }
}
