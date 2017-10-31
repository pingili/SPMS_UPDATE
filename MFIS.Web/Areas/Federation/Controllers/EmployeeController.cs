
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CoreComponents;
//using BusinessLogic.Interface;
using BusinessLogic;
using BusinessEntities;
using MFIS.Web.Controllers;
using MFIS.Web.Areas.Federation.Models;
using AutoMapper;
using Utilities;
using System.IO;
using BusinessLogic.Implementation;


namespace MFIS.Web.Areas.Federation.Controllers
{
    public class EmployeeController : BaseController
    {

        #region Global Variables

        private readonly BranchService _branchService;
        private readonly EmployeeService _employeeService;
        private readonly ClusterService _ClusterService;
        private readonly EmployeeKycService _employeeKycService;
        private readonly ReferenceValueService _referenceValueService;
        private readonly CommonService _commonService;
        private readonly MasterService _masterService;

        public EmployeeController()
        {
            _branchService = new BranchService();
            _employeeService = new EmployeeService();
            _ClusterService = new ClusterService();
            _employeeKycService = new EmployeeKycService();
            _referenceValueService = new ReferenceValueService();
            _commonService = new CommonService();
            _masterService = new MasterService();
        }
        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateEmployee(string ID)
        {
            int EmployeeId = string.IsNullOrEmpty(ID.DecryptString()) ? default(int) : Convert.ToInt32(ID.DecryptString());

            #region Dropdowwn Binding
            EmployeeDto objEmployeeDto = new EmployeeDto();

            List<BranchDto> BranchDto = _branchService.GetAll();
            SelectList branchSelectList = new SelectList(BranchDto, "BranchID", "BranchCode", "Select BranchCode");
            ViewBag.BranchCode = branchSelectList;

            List<ClusterDto> clusterSelectList = _ClusterService.GetAll();
            SelectList lStClusters = new SelectList(clusterSelectList, "ClusterID", "ClusterName", objEmployeeDto.ClusterID);
            ViewBag.clusters = lStClusters;

            SelectList educationQualification = GetDropDownListByMasterCode(Enums.RefMasterCodes.EDUCATION_QUALIFICATION);
            ViewBag.educationQualification = educationQualification;

            SelectList socialCategory = GetDropDownListByMasterCode(Enums.RefMasterCodes.SOCIAL_CATEGORY);
            ViewBag.socialCategory = socialCategory;

            SelectList bankName = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_NAME);
            ViewBag.bankName = bankName;

            SelectList AccountType = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_ACCOUNT_TYPE);
            ViewBag.AhType = AccountType;

            SelectList Religion = GetDropDownListByMasterCode(Enums.RefMasterCodes.RELIGION);
            ViewBag.Religion = Religion;

            SelectList Desigination = GetDropDownListByMasterCode(Enums.RefMasterCodes.DESIGNATION);
            ViewBag.Desigination = Desigination;

            SelectList BloodGroup = GetDropDownListByMasterCode(Enums.RefMasterCodes.BLOODGROUP);
            ViewBag.BloodGroup = BloodGroup;

            SelectList nomineeRelationship = GetDropDownListByMasterCode(Enums.RefMasterCodes.NOMINEE_RELATIONSHIP);
            ViewBag.nomineeRelationship = nomineeRelationship;

            SelectList parentGuardionRelationship = GetDropDownListByMasterCode(Enums.RefMasterCodes.PARENT_GUARDIAN_RELATIONSHIP);
            ViewBag.parentGuardionRelationship = parentGuardionRelationship;

            ViewBag.familyIncome = GetDropDownListByMasterCode(Enums.RefMasterCodes.MONTHLY_INCOME);


           var  roleSelectListDto = _employeeService.GetRoleSelectList();
           var roleSelectList = new SelectList(roleSelectListDto, "ID", "Text",objEmployeeDto.RoleId);
            ViewBag.roles = roleSelectList;
           
            #endregion Dropdowwn Binding

            if (EmployeeId > 0)
            {
                objEmployeeDto = _employeeService.GetByID(EmployeeId);
            }

            EmployeeModel employeeModel = Mapper.Map<EmployeeDto, EmployeeModel>(objEmployeeDto);

            FillEmployeeKYC(ref employeeModel);

            if (employeeModel.EmployeeID > 0)
            {
                JsonResult BranchData = GetBranchName(objEmployeeDto.BranchID);
                employeeModel.BranchName = BranchData.Data.GetType().GetProperty("BranchName").GetValue(BranchData.Data, null).ToString();
            }

            if (EmployeeId > 0)
            {
                int employeeid = objEmployeeDto.BranchID;
                ViewBag.EmployeeId = employeeid;
                if (UserInfo.UserID == employeeid)
                {
                    UserInfo.Photo = employeeModel.Photo;
                    UserInfo.UserName = employeeModel.EmployeeName;
                }
            }

            return View(employeeModel);
        }

        public void FillEmployeeKYC(ref EmployeeModel employeeModel)
        {
            List<EmployeeKYCDto> objEmployeekyc = new List<EmployeeKYCDto>();
            if (employeeModel.EmployeeID > 0)
                objEmployeekyc = _employeeKycService.GetByEmployeeID(employeeModel.EmployeeID);

            if (objEmployeekyc != null && objEmployeekyc.Count > 0)
            {
                List<ReferenceValueDto> kycTypeRefereces = _referenceValueService.GetByRefMasterCode(Enums.RefMasterCodes.KYCTYPE.ToString());

                //Fill Employee Model With Kyc Aadhar details 
                var aarharType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.AADHAR.ToString()));
                EmployeeKYCDto AadharKYC = objEmployeekyc.Find(a => a.KYCType == aarharType.RefID);
                if (AadharKYC != null)
                {
                    employeeModel.AadharNo = AadharKYC.KYCNumber;
                    employeeModel.AadharImagePath = AadharKYC.FileName;
                }

                //Fill Employee Model With Kyc PAN details 
                var panType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.PAN.ToString()));
                EmployeeKYCDto panKYC = objEmployeekyc.Find(a => a.KYCType == panType.RefID);
                if (panKYC != null)
                {
                    employeeModel.PANNo = panKYC.KYCNumber;
                    employeeModel.PANImagePath = panKYC.FileName;
                }

                //Fill Employee Model With Kyc Voter details 
                var voterType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.VOTERID.ToString()));
                EmployeeKYCDto voterKYC = objEmployeekyc.Find(a => a.KYCType == voterType.RefID);
                if (voterKYC != null)
                {
                    employeeModel.VoterNo = voterKYC.KYCNumber;
                    employeeModel.VoterImagePath = voterKYC.FileName;
                }

                //Fill Employee Model With Kyc Ration details 
                var rationType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.RATIONCARD.ToString()));
                EmployeeKYCDto rationKYC = objEmployeekyc.Find(a => a.KYCType == rationType.RefID);
                if (rationKYC != null)
                {
                    employeeModel.RationNo = rationKYC.KYCNumber;
                    employeeModel.RationImagePath = rationKYC.FileName;
                }

                //Fill Employee Model With Kyc Ration details 
                var bankaccountType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.BANKACCOUNT.ToString()));
                EmployeeKYCDto bankKYC = objEmployeekyc.Find(a => a.KYCType == bankaccountType.RefID);
                if (bankKYC != null)
                {
                    employeeModel.BankAccountNo = bankKYC.KYCNumber;
                    employeeModel.BankImagePath = bankKYC.FileName;
                }
            }
        }

        [HttpPost]
        public ActionResult CreateEmployee(EmployeeModel empModel, List<HttpPostedFileBase> fileUpload, FormCollection form)
        {

            string CurrentTab = Request.Form.Get("CurrentTab");
            ViewBag.CurrentTab = CurrentTab;

            var resultDto = new ResultDto();
            EmployeeDto objEmployeeDto = new EmployeeDto();
            EmployeeModel fileModelTemp = new EmployeeModel();

            #region DropDowns

            List<BranchDto> BranchDto = _branchService.GetAll();
            ViewBag.BranchCode = new SelectList(BranchDto, "BranchID", "BranchCode", "Select BranchCode");

            List<ClusterDto> clusterSelectList = _ClusterService.GetAll();
            SelectList lStClusters = new SelectList(clusterSelectList, "ClusterID", "ClusterName", objEmployeeDto.ClusterID);
            ViewBag.clusters = lStClusters;

            SelectList educationQualification = GetDropDownListByMasterCode(Enums.RefMasterCodes.EDUCATION_QUALIFICATION);
            ViewBag.educationQualification = educationQualification;

            SelectList socialCategory = GetDropDownListByMasterCode(Enums.RefMasterCodes.SOCIAL_CATEGORY);
            ViewBag.socialCategory = socialCategory;

            SelectList bankName = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_NAME);
            ViewBag.bankName = bankName;

            SelectList AccountType = GetDropDownListByMasterCode(Enums.RefMasterCodes.BANK_ACCOUNT_TYPE);
            ViewBag.AhType = AccountType;

            SelectList Religion = GetDropDownListByMasterCode(Enums.RefMasterCodes.RELIGION);
            ViewBag.Religion = Religion;

            SelectList Desigination = GetDropDownListByMasterCode(Enums.RefMasterCodes.DESIGNATION);
            ViewBag.Desigination = Desigination;

            SelectList BloodGroup = GetDropDownListByMasterCode(Enums.RefMasterCodes.BLOODGROUP);
            ViewBag.BloodGroup = BloodGroup;

            SelectList nomineeRelationship = GetDropDownListByMasterCode(Enums.RefMasterCodes.NOMINEE_RELATIONSHIP);
            ViewBag.nomineeRelationship = nomineeRelationship;

            SelectList parentGuardionRelationship = GetDropDownListByMasterCode(Enums.RefMasterCodes.PARENT_GUARDIAN_RELATIONSHIP);
            ViewBag.parentGuardionRelationship = parentGuardionRelationship;

            ViewBag.familyIncome = GetDropDownListByMasterCode(Enums.RefMasterCodes.MONTHLY_INCOME);

            var roleSelectListDto = _employeeService.GetRoleSelectList();
            var roleSelectList = new SelectList(roleSelectListDto, "ID", "Text", objEmployeeDto.RoleId);
            ViewBag.roles = roleSelectList;

            #endregion DropDowns

            EmployeeDto empDto = new EmployeeDto();
            if (CurrentTab.ToUpper() == Enums.MemberTabs.PERSONALDETAILS.ToString().ToUpper())
            {
                string directoryPath = "AssetUploads/Employee/" + empModel.EmployeeID;
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath)))
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath));

                var file = Request.Files["Photo"];
                if (file != null && file.FileName != string.Empty)
                {
                    string guid = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    string fullFileName = directoryPath + "/" + guid + extension;

                    file.SaveAs(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fullFileName));
                    empModel.Photo = fullFileName;
                }
                else
                {
                    empModel.Photo = Request.Form["hdnPhoto"];
                }
                empDto = Mapper.Map<EmployeeModel, EmployeeDto>(empModel);

                empDto.Disability = Convert.ToBoolean(form["optionsRadios"]);
                empDto.UserID = UserInfo.UserID;
                if (empDto.EmployeeID == 0)
                {
                    resultDto = _employeeService.Insert(empDto);
                }
                else
                    resultDto = _employeeService.Update(empDto);
            }
            else if (CurrentTab.ToUpper() == Enums.MemberTabs.FAMILYDETAILS.ToString().ToUpper())
            {
                empDto = Mapper.Map<EmployeeModel, EmployeeDto>(empModel);
                empDto.UserID = UserInfo.UserID;
                resultDto = _employeeService.UpdateFamily(empDto);
                resultDto.ObjectCode = empModel.EmployeeCode;
            }

            else if (CurrentTab.ToUpper() == Enums.MemberTabs.PROOFS.ToString().ToUpper())
            {
                #region FileUploads
                List<EmployeeKYCDto> lstKyc = new List<EmployeeKYCDto>();
                EmployeeKYCDto objkycempdto = new EmployeeKYCDto();

                lstKyc = _employeeKycService.GetByEmployeeID(empModel.EmployeeID);

                List<ReferenceValueDto> lstReferenceValueDto = _referenceValueService.GetByRefMasterCode(Enums.RefMasterCodes.KYCTYPE.ToString());

                //Aa
                var aadharref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.AADHAR.ToString().ToUpper());
                var fileAadhar = Request.Files["fileAadhar"];
                if (fileAadhar != null && fileAadhar.ContentLength > 0)
                {
                    objkycempdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["AADHAR"]) ? Request.Form["AADHAR"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == aadharref.RefID) != null)
                    {
                        objkycempdto.EmployeeKYCID = lstKyc.Find(f => f.KYCType == aadharref.RefID).EmployeeKYCID;
                    }
                    EmployeeKYCDto obj = SaveKYC(fileAadhar, empModel.EmployeeID, aadharref.RefID, objkycempdto.KYCNumber, objkycempdto.EmployeeKYCID);
                    fileModelTemp.AadharImagePath = obj.FileName;
                    fileModelTemp.AadharNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == aadharref.RefID) != null)
                    {
                        EmployeeKYCDto obj = lstKyc.Find(f => f.KYCType == aadharref.RefID);
                        fileModelTemp.AadharImagePath = obj.FileName;
                        fileModelTemp.AadharNo = obj.KYCNumber;
                    }
                }


                //vo
                var voterref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.VOTERID.ToString().ToUpper());
                var fileVoter = Request.Files["fileVoter"];
                if (fileVoter != null && fileVoter.ContentLength > 0)
                {
                    objkycempdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["VoterID"]) ? Request.Form["VoterID"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == voterref.RefID) != null)
                    {
                        objkycempdto.EmployeeKYCID = lstKyc.Find(f => f.KYCType == voterref.RefID).EmployeeKYCID;
                    }
                    EmployeeKYCDto obj = SaveKYC(fileVoter, empModel.EmployeeID, voterref.RefID, objkycempdto.KYCNumber, objkycempdto.EmployeeKYCID);
                    fileModelTemp.VoterImagePath = obj.FileName;
                    fileModelTemp.VoterNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == voterref.RefID) != null)
                    {
                        EmployeeKYCDto obj = lstKyc.Find(f => f.KYCType == voterref.RefID);
                        fileModelTemp.VoterImagePath = obj.FileName;
                        fileModelTemp.VoterNo = obj.KYCNumber;
                    }
                }
                //pan
                var panref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.PAN.ToString().ToUpper());
                var filepan = Request.Files["filePan"];

                if (filepan != null && filepan.ContentLength > 0)
                {
                    objkycempdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["PANCard"]) ? Request.Form["PANCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == panref.RefID) != null)
                    {
                        objkycempdto.EmployeeKYCID = lstKyc.Find(f => f.KYCType == panref.RefID).EmployeeKYCID;
                    }
                    EmployeeKYCDto obj = SaveKYC(filepan, empModel.EmployeeID, panref.RefID, objkycempdto.KYCNumber, objkycempdto.EmployeeKYCID);
                    fileModelTemp.PANImagePath = obj.FileName;
                    fileModelTemp.PANNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == panref.RefID) != null)
                    {
                        EmployeeKYCDto obj = lstKyc.Find(f => f.KYCType == panref.RefID);
                        fileModelTemp.PANImagePath = obj.FileName;
                        fileModelTemp.PANNo = obj.KYCNumber;
                    }
                }

                //Ration
                var rationref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.RATIONCARD.ToString().ToUpper());
                var fileration = Request.Files["fileRation"];

                if (fileration != null && fileration.ContentLength > 0)
                {
                    objkycempdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["RationCard"]) ? Request.Form["RationCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == rationref.RefID) != null)
                    {
                        objkycempdto.EmployeeKYCID = lstKyc.Find(f => f.KYCType == rationref.RefID).EmployeeKYCID;
                    }
                    EmployeeKYCDto obj = SaveKYC(fileration, empModel.EmployeeID, rationref.RefID, objkycempdto.KYCNumber, objkycempdto.EmployeeKYCID);
                    fileModelTemp.RationImagePath = obj.FileName;
                    fileModelTemp.RationNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == rationref.RefID) != null)
                    {
                        EmployeeKYCDto obj = lstKyc.Find(f => f.KYCType == rationref.RefID);
                        fileModelTemp.RationImagePath = obj.FileName;
                        fileModelTemp.RationNo = obj.KYCNumber;
                    }
                }
                //Bankaccount
                var bankaccountref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.BANKACCOUNT.ToString().ToUpper());
                var filebank = Request.Files["fileBankaccount"];
                if (filebank != null && filebank.ContentLength > 0)
                {
                    objkycempdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["BankAccount"]) ? Request.Form["BankAccount"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == bankaccountref.RefID) != null)
                    {
                        objkycempdto.EmployeeKYCID = lstKyc.Find(f => f.KYCType == bankaccountref.RefID).EmployeeKYCID;
                    }
                    EmployeeKYCDto obj = SaveKYC(filebank, empModel.EmployeeID, bankaccountref.RefID, objkycempdto.KYCNumber, objkycempdto.EmployeeKYCID);
                    fileModelTemp.BankImagePath = obj.FileName;
                    fileModelTemp.BankAccountNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == bankaccountref.RefID) != null)
                    {
                        EmployeeKYCDto obj = lstKyc.Find(f => f.KYCType == bankaccountref.RefID);
                        fileModelTemp.BankImagePath = obj.FileName;
                        fileModelTemp.BankAccountNo = obj.KYCNumber;
                    }
                }
                #endregion FileUploads

                resultDto.Message = "Employee KYC data saved successfully for Employee code : " + empModel.EmployeeCode;

                resultDto.ObjectId = empModel.EmployeeID;
                resultDto.ObjectCode = empModel.EmployeeCode;
            }

            else if (CurrentTab.ToUpper() == Enums.Employeetabs.BANKAccountDetails.ToString().ToUpper())
            {


                List<BankMasterDto> lstBanks = GetBanksList(form);
                int employeeId = Convert.ToInt32(Request.Form["hdnObjectID"]);

                ResultDto resultdto = _commonService.InsertBankDetails(employeeId, Enums.EntityCodes.EMPLOYEE, UserInfo.UserID, lstBanks);

                resultDto.Message = resultdto.ObjectId > 0 ? "Employee bank details saved successfully for Employee code : " + empModel.EmployeeCode : resultdto.Message;
                resultDto.ObjectId = empModel.EmployeeID;
                resultDto.ObjectCode = empModel.EmployeeCode;
            }

            else if (CurrentTab.ToUpper() == Enums.Employeetabs.CreateLogin.ToString().ToUpper())
            {
                empDto = Mapper.Map<EmployeeModel, EmployeeDto>(empModel);
                empDto.UserID = UserInfo.UserID;
                resultDto = _employeeService.UpdateLoginDetails(empDto);
                resultDto.ObjectCode = empModel.EmployeeCode;
            }


            if (resultDto.ObjectId > 0)
            {
                empDto = _employeeService.GetByID(resultDto.ObjectId);
                empModel = Mapper.Map<EmployeeDto, EmployeeModel>(empDto);
                resultDto.ObjectCode = empDto.EmployeeCode;
                resultDto.ObjectId = empDto.EmployeeID;

                //REFILL THE KYC
                FillEmployeeKYC(ref empModel);

                //REFILL THE BANKS

            }
            ViewBag.Result = resultDto;

            ModelState.Clear();
            return View(empModel);
        }

        public JsonResult GetBranchName(int id)
        {
            BranchDto BranchDtoDto = _branchService.GetByID(id);
            return Json(new { BranchName = BranchDtoDto.BranchName });
        }

        [HttpGet]
        public ActionResult EmployeeLookUp()
        {
            var lstEmployee = _employeeService.Lookup();
            return View(lstEmployee);
        }

        public EmployeeKYCDto SaveKYC(HttpPostedFileBase file, int employeeId, int refid, string KYCNumber, int EmployeeKYCID)
        {
            string directoryPath = "AssetUploads/Employee/" + employeeId;
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath)))
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath));

            string guid = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(file.FileName);
            string fullFileName = directoryPath + '/' + guid + extension;

            file.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "/" + fullFileName);

            EmployeeKYCDto objEmployeeKYCDto = new EmployeeKYCDto()
            {
                EmployeeID = employeeId,
                EmployeeKYCID = EmployeeKYCID,
                KYCNumber = KYCNumber,
                KYCType = refid,
                FileName = fullFileName,
                ActualFileName = Path.GetFileName(file.FileName),
                UserID = UserInfo.UserID
            };
            _employeeKycService.Insert(objEmployeeKYCDto);


            if (objEmployeeKYCDto.EmployeeKYCID > 0)
            {
                _employeeKycService.Update(objEmployeeKYCDto);

            }
            else
            {
                _employeeKycService.Insert(objEmployeeKYCDto);
            }
            return objEmployeeKYCDto;
        }


        [HttpGet]
        public ActionResult ActiveInactiveEmployee(string Id)
        {
            int employeeId = DecryptQueryString(Id);

            if (employeeId < 1)
                return RedirectToAction("EmployeeLookUp");

            ResultDto resultDto = _employeeService.ChangeStatus(employeeId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("EmployeeLookUp");
        }


        [HttpGet]
        public ActionResult ViewEmployee(string Id)
        {
            int employeeId = DecryptQueryString(Id);
            if (employeeId <= 0)
                return RedirectToAction("EmployeeLookUp");
            EmployeeViewDto employeeViewDto = _employeeService.GetViewById(employeeId);
            return View(employeeViewDto);
        }
        [HttpGet]
        public ActionResult DeleteEmployee(string Id)
        {
            int EmployeeId = DecryptQueryString(Id);

            if (EmployeeId < 1)
                return RedirectToAction("EmployeeLookUp");

            ResultDto resultDto = _employeeService.Delete(EmployeeId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("EmployeeLookUp");
        }

        [HttpGet]
        public ActionResult AssignClusterToEmployee()
        {
            TypeQueryResult lstEmp = _masterService.GetTypeQueryResult("CLUSTER_ASSIGNMENT_EMP");
            ViewBag.EmployeeList = new SelectList(lstEmp, "Id", "Name");

            List<SelectListDto> clusterlist = _ClusterService.GetClusterSelectList();
            SelectList clusterSelectList = new SelectList(clusterlist, "ID", "Text");
            ViewBag.Clusterlist = clusterSelectList;
            return View();
        }
        [HttpPost]
        public ActionResult AssignClusterToEmployee(FormCollection form)
        {
            TypeQueryResult lstEmp = _masterService.GetTypeQueryResult("CLUSTER_ASSIGNMENT_EMP");
            ViewBag.EmployeeList = new SelectList(lstEmp, "Id", "Name");

            List<SelectListDto> clusterlist = _ClusterService.GetClusterSelectList();
            SelectList clusterSelectList = new SelectList(clusterlist, "ID", "Text");
            ViewBag.Clusterlist = clusterSelectList;

            ResultDto resultDto = new ResultDto();

            int employeeId = Convert.ToInt32(form["EmployeeID"]);
            int maxIndex = Convert.ToInt32(form["hdnIndex"]);

            List<ClusterAssignmentDto> lstClusterAssignement = new List<ClusterAssignmentDto>();
            for (int i = 1; i <= maxIndex; i++)
            {
                //  if (form["hdnClusterID_" + i] == null) continue;


                if (form["Checkcluster_" + i] == "on")
                    lstClusterAssignement.Add(new ClusterAssignmentDto()
                    {
                        ClusterID = Convert.ToInt32(form["hdnClusterID_" + i])
                    });
            }
            resultDto = _employeeService.ManageClusterAssignments(lstClusterAssignement, employeeId);
            return View();
        }
        public JsonResult Getcluster(int ID)
        {
            List<ClusterAssignmentDto> clusterlistdto = _employeeService.ClusterGetByEmpID(ID);
            return Json(new { ClusterCheck = clusterlistdto });
        }

        public ActionResult EmployeeClusterDetails()
        {
            List<ClusterAssignmentDto> listassignclustertoemployee = new List<ClusterAssignmentDto>();
            listassignclustertoemployee = _employeeService.GetAllClusterAssignments();
            return View(listassignclustertoemployee);
        }

    }
}