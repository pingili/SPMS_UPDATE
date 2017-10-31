using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using CoreComponents;
using DataLogic.Implementation;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using Utilities;

namespace BusinessLogic
{
    public class EmployeeService 
    {
        private readonly MFISDBContext _dbContext;

        public EmployeeService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }


        public List<EmployeeDto> GetAll()
        {
            List<EmployeeDto> lstEmployeeDto = new List<EmployeeDto>();
            List<uspEmployeeGetAll_Result> lstuspEmployeeGetAll_Result = _dbContext.uspEmployeeGetAll().ToList();
            foreach (var employee in lstuspEmployeeGetAll_Result)
            {
                EmployeeDto EmployeeDto = Mapper.Map<uspEmployeeGetAll_Result, EmployeeDto>(employee);
                lstEmployeeDto.Add(EmployeeDto);
            }
            // lstEmployeeDto=Mapper.Map<List<uspEmployeeGetAll_Result>,List<EmployeeDto>>(lstuspEmployeeGetAll_Result);
            return lstEmployeeDto;
        }

        public EmployeeDto GetBranchByID(int branchID)
        {
            EmployeeDto EmployeeDto = new EmployeeDto();
            List<uspEmployeeGetByID_Result> lstuspEmployeeGetById_Result = _dbContext.uspEmployeeGetByID(branchID).ToList();
            EmployeeDto = Mapper.Map<List<uspEmployeeGetByID_Result>, List<EmployeeDto>>(lstuspEmployeeGetById_Result).FirstOrDefault();
            if (lstuspEmployeeGetById_Result.FirstOrDefault().DesignationFromDate.HasValue)
                EmployeeDto.FromDate = lstuspEmployeeGetById_Result.FirstOrDefault().DesignationFromDate.Value;
            if (lstuspEmployeeGetById_Result.FirstOrDefault().DesignationToDate.HasValue)
                EmployeeDto.ToDate = lstuspEmployeeGetById_Result.FirstOrDefault().DesignationToDate.Value;
            return EmployeeDto;
        }

        public ResultDto Insert(EmployeeDto employee)
        {
            return InsertUpdateEmployee(employee);
        }

        public ResultDto Update(EmployeeDto employee)
        {
            return InsertUpdateEmployee(employee);
        }

        public List<SelectListDto> GetEmployeeSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspEmployeeGetAll_Result> lstuspEmployeeGetAll_Result = _dbContext.uspEmployeeGetAll().ToList();
            foreach (var objuspEmployeeGetAll_Result in lstuspEmployeeGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = objuspEmployeeGetAll_Result.EmployeeID,
                    Text = objuspEmployeeGetAll_Result.EmployeeName
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        public List<SelectListDto> GetEmployeeCodeSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspEmployeeGetAll_Result> lstuspEmployeeGetAll_Result = _dbContext.uspEmployeeGetAll().ToList();
            foreach (var objuspEmployeeGetAll_Result in lstuspEmployeeGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = objuspEmployeeGetAll_Result.EmployeeID,
                    Text = objuspEmployeeGetAll_Result.EmployeeCode
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        private ResultDto InsertUpdateEmployee(EmployeeDto employee)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Employee";

            try
            {
                ObjectParameter paramEmployeeId = new ObjectParameter("EmployeeID", employee.EmployeeID);
                ObjectParameter paramEmployeeCode = new ObjectParameter("EmployeeCode", string.Empty);
                int effectedCount = _dbContext.uspEmployeeInsertUpdate(paramEmployeeId, paramEmployeeCode, employee.EmployeeRefCode, employee.SurName, employee.TESurName,
                    employee.EmployeeName, employee.TEEmployeeName, employee.Photo, employee.BranchID, employee.ClusterID, employee.Gender, employee.DOJ,
                    employee.EducationQualification, employee.MobileNumber, employee.Email, employee.DOB, employee.Designation, employee.FromDate, employee.ToDate, employee.Disability,
                    employee.BloodGroup, employee.MaritalStatus, employee.SocialCategory, employee.PresentAddress, employee.PermanentAddress,
                    employee.EmergencyContactNumber, employee.EmergencyContactName, employee.UserID, employee.Religion, employee.DateOfRetirement);

                resultDto.ObjectId = (int)paramEmployeeId.Value;


                resultDto.ObjectCode = (string)paramEmployeeCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", objectName, resultDto.ObjectCode);
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
        public EmployeeDto GetByID(int EmployeeId)
        {
            List<uspEmployeeGetByID_Result> lstuspEmployeeGetByID_Result = _dbContext.uspEmployeeGetByID(EmployeeId).ToList();
            EmployeeDto EmployeeDto = Mapper.Map<uspEmployeeGetByID_Result, EmployeeDto>(lstuspEmployeeGetByID_Result.FirstOrDefault());
            if (lstuspEmployeeGetByID_Result.FirstOrDefault().DesignationFromDate.HasValue)
                EmployeeDto.FromDate = lstuspEmployeeGetByID_Result.FirstOrDefault().DesignationFromDate.Value;
            if (lstuspEmployeeGetByID_Result.FirstOrDefault().DesignationToDate.HasValue)
                EmployeeDto.ToDate = lstuspEmployeeGetByID_Result.FirstOrDefault().DesignationToDate.Value;
            return EmployeeDto;
        }
        public EmployeeDto GetEmployeeNameByID(int EmployeeId)
        {
            List<uspEmployeeGetByID_Result> lstuspEmployeeGetByID_Result = _dbContext.uspEmployeeGetByID(EmployeeId).ToList();
            EmployeeDto EmployeeDto = Mapper.Map<uspEmployeeGetByID_Result, EmployeeDto>(lstuspEmployeeGetByID_Result.FirstOrDefault());
        
            return EmployeeDto;
        }
        public List<EmployeeLookupDto> Lookup()
        {
            List<EmployeeLookupDto> lstEmployeeLookupDto = new List<EmployeeLookupDto>();
            List<uspEmployeeLookUp_Result> lstuspEmployeeLookUp_Result = _dbContext.uspEmployeeLookUp().ToList();
            foreach (var objspresult in lstuspEmployeeLookUp_Result)
            {
                var objEmployeeLookupDto = Mapper.Map<uspEmployeeLookUp_Result, EmployeeLookupDto>(objspresult);
                lstEmployeeLookupDto.Add(objEmployeeLookupDto);
            }
            return lstEmployeeLookupDto;
        }


        public ResultDto UpdateFamily(EmployeeDto empDto)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "EmployeeFamilyDetails";
            try
            {
                _dbContext.uspEmployeeFamilyDetailsUpdate(empDto.EmployeeID, empDto.NomineeName, empDto.NomineeRelationship, empDto.ParentGuardianName, empDto.ParentGuardianRelationship
                    , empDto.SocialCategory, empDto.FamilyIncome, empDto.EarningMembersInFamily, empDto.NonEarningMembersInFamily, empDto.AssetsInNameOfEmployee, empDto.UserID, empDto.DateOfRetirement, empDto.Religion);
                resultDto.ObjectId = (int)empDto.EmployeeID;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", objectName, empDto.EmployeeCode);
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



        public ResultDto UpdateLoginDetails(EmployeeDto empDto)
        {

            ResultDto resultDto = new ResultDto();
            string objectName = "Login Credentials";
            try
            {
                //empDto.RoleId = 3013;
                _dbContext.uspEmployeeLoginDetailsUpdate(empDto.EmployeeID,empDto.RoleId,empDto.LoginUserName,empDto.LoginPassWord ,empDto.UserID);
                resultDto.ObjectId = (int)empDto.EmployeeID;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", objectName, empDto.EmployeeCode);
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
        public ResultDto ChangeStatus(int employeeId, int CurrentUserID)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Employee";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmEmployeeId = new ObjectParameter("EmployeeID", employeeId);
                ObjectParameter prmEmployeeCode = new ObjectParameter("EmployeeCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspEmployeeChangeStatus(prmEmployeeId, prmEmployeeCode, prmStatusCode, CurrentUserID);

                resultDto.ObjectId = (int)prmEmployeeId.Value;
                resultDto.ObjectCode = (string)prmEmployeeCode.Value;
                statusCode = (string)prmStatusCode.Value;
                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details {2} successfully", objectName, resultDto.ObjectCode, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
                else
                    resultDto.Message = string.Format("Error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", objectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }
        public EmployeeViewDto GetViewById(int employeeId)
        {
            List<uspEmployeeGetViewByID_Result> lstuspEmployeeGetViewByID_Result = _dbContext.uspEmployeeGetViewByID(employeeId).ToList();
            EmployeeViewDto employeeViewDto = new EmployeeViewDto();
            employeeViewDto = Mapper.Map<uspEmployeeGetViewByID_Result, EmployeeViewDto>(lstuspEmployeeGetViewByID_Result.FirstOrDefault());

            List<EmployeeKYCDto> lstEmployeeKYCDto = new List<EmployeeKYCDto>();
            List<uspEmployeeKYCGetViewByID_Result> lstuspEmployeeKYCGetViewByID_Result = _dbContext.uspEmployeeKYCGetViewByID(employeeId).ToList();
            foreach (var Item in lstuspEmployeeKYCGetViewByID_Result)
            {
                EmployeeKYCDto objEmployeeKYCDto = new EmployeeKYCDto();
                objEmployeeKYCDto.EmployeeID = Item.EmployeeID;
                objEmployeeKYCDto.KYCNumber = Item.KYCNumber;
                objEmployeeKYCDto.FileName = Item.FileName;
                lstEmployeeKYCDto.Add(objEmployeeKYCDto);
            }
            employeeViewDto.EmployeeKYC = lstEmployeeKYCDto;
            return employeeViewDto;
        }


        public List<SelectListDto> GetRoleSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();

            List<uspGetRoles_Result> lstuspGetRoles_Result = _dbContext.uspGetRoles().ToList();
            foreach (var role in lstuspGetRoles_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = role.RoleId,
                    Text = role.rolename
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        public ResultDto Delete(int EmployeeId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Employee";

            try
            {
                ObjectParameter prmEmployeeId = new ObjectParameter("EmployeeID", EmployeeId);
                ObjectParameter prmEmployeeCode = new ObjectParameter("EmployeeCode", string.Empty);

                int effectedCount = _dbContext.uspEmployeeDelete(prmEmployeeId, prmEmployeeCode, userId);

                resultDto.ObjectId = (int)prmEmployeeId.Value;
                resultDto.ObjectCode = (string)prmEmployeeCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details deleted successfully", obectName, resultDto.ObjectCode);
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

        public ResultDto ManageClusterAssignments(List<ClusterAssignmentDto> lstClusterAssignement, int employeeId)
        {
            string strClustersVsEmpXML = CommonMethods.SerializeListDto<List<ClusterAssignmentDto>>(lstClusterAssignement);

            return objEmployeeDal.ManageClusterAssignments(employeeId, strClustersVsEmpXML);
        }

        public List<ClusterAssignmentDto> ClusterGetByEmpID(int ID)
        {
            return objEmployeeDal.ClusterGetByEmpID(ID);
        }

        public List<ClusterAssignmentDto> GetAllClusterAssignments()
        {
            return objEmployeeDal.GetAllClusterAssignments();
        }

        private EmployeeDAL _objEmployeeDal;
        private EmployeeDAL objEmployeeDal
        {
            get
            {
                if (_objEmployeeDal == null)
                    _objEmployeeDal = new EmployeeDAL();
                return _objEmployeeDal;
            }
        }


    }
}
