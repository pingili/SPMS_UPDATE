using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using BusinessLogic;
//using BusinessLogic.Interface;
using MFIS.Web.Areas.Group.Models;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Utilities;
using CoreComponents;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

namespace MFIS.Web.Areas.Group.Controllers
{
    public class MemberController : BaseController
    {
        #region Global Variables
        private readonly OccupationService _occupationService;
        private readonly GroupService _groupService;
        private readonly AccountHeadService _accountheadservice;
        private readonly PanchayatService _panchayatService;
        private readonly MemberService _memberservice;
        private readonly MemberKycService _memberKycService;
        private readonly ReferenceValueService _referenceValueService;
        public MemberController()
        {
            _occupationService = new OccupationService();
            _groupService = new GroupService();
            _accountheadservice = new AccountHeadService();
            _memberservice = new MemberService();
            _memberKycService = new MemberKycService();
            _referenceValueService = new ReferenceValueService();
            _panchayatService = new PanchayatService();
        }

        #endregion Global Variables
        [HttpGet]
        public ActionResult CreateMember(string id)
        {
            int memberId = string.IsNullOrEmpty(id.DecryptString()) ? default(int) : Convert.ToInt32(id.DecryptString());

            OrganizationService _organizationService = new OrganizationService();
            OrganizationDto org = _organizationService.GetAll();
            ViewBag.MemberRetirementAge = org != null ? org.MemberRetirementAge : 60;

            ViewBag.EducationQualification = GetDropDownListByMasterCode(Enums.RefMasterCodes.EDUCATION_QUALIFICATION);

            SelectList nomineeRelationship = GetDropDownListByMasterCode(Enums.RefMasterCodes.NOMINEE_RELATIONSHIP);
            ViewBag.nomineeRelationship = nomineeRelationship;

            SelectList parentGuardionRelationship = GetDropDownListByMasterCode(Enums.RefMasterCodes.PARENT_GUARDIAN_RELATIONSHIP);
            ViewBag.parentGuardionRelationship = parentGuardionRelationship;

            SelectList socialCategory = GetDropDownListByMasterCode(Enums.RefMasterCodes.SOCIAL_CATEGORY);
            ViewBag.socialCategory = socialCategory;

            SelectList monthlyIncome = GetDropDownListByMasterCode(Enums.RefMasterCodes.MONTHLY_INCOME);
            ViewBag.monthlyIncome = monthlyIncome;

            SelectList incomefrequency = GetDropDownListByMasterCode(Enums.RefMasterCodes.INCOME_FREQUENCY);
            ViewBag.incomefrequency = incomefrequency;

            SelectList religion = GetDropDownListByMasterCode(Enums.RefMasterCodes.RELIGION);
            ViewBag.religion = religion;

            List<SelectListDto> lstoccupationDto = _occupationService.GetOccupationSelectList();
            SelectList lstoccupation = new SelectList(lstoccupationDto, "ID", "Text");
            ViewBag.OccupationName = lstoccupation;

            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;

            List<SelectListDto> lstaccountheaddto = _accountheadservice.GetAccountHeadSelectList();
            SelectList lstaccounthead = new SelectList(lstaccountheaddto, "ID", "Text");
            ViewBag.accounthead = lstaccounthead;

            SelectList FederationDesigination = GetDropDownListByMasterCode(Enums.RefMasterCodes.FED_DESG);
            ViewBag.FED_DESG = FederationDesigination;

            SelectList GroupDesigination = GetDropDownListByMasterCode(Enums.RefMasterCodes.GROUP_DESG);
            ViewBag.GROUP_DESG = GroupDesigination;

            SelectList ClusterDesigination = GetDropDownListByMasterCode(Enums.RefMasterCodes.CLUSTER_DESG);
            ViewBag.CLUSTER_DESG = ClusterDesigination;

            MemberDto memberdto = new MemberDto();
            if (memberId > 0)
            {
                memberdto = _memberservice.GetById(memberId);
                if (memberdto.DOB != DateTime.MinValue)
                {
                    memberdto.DateOfRetirement = memberdto.DOB.AddYears(org.MemberRetirementAge);
                }
            }
            MemberModel memberModel = Mapper.Map<MemberDto, MemberModel>(memberdto);
            List<MemberKYCDto> memberkycdto = new List<MemberKYCDto>();
            if (memberId > 0)
            {
                memberkycdto = _memberKycService.GetByMemberID(memberId);
                if (memberkycdto != null && memberkycdto.Count > 0)
                {
                    List<ReferenceValueDto> kycTypeRefereces = _referenceValueService.GetByRefMasterCode(Enums.RefMasterCodes.KYCTYPE.ToString());

                    //Fill Member Model With Kyc Aadhar details 
                    var aarharType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.AADHAR.ToString()));
                    MemberKYCDto AadharKYC = memberkycdto.Find(a => a.KYCType == aarharType.RefID);
                    if (AadharKYC != null)
                    {
                        memberModel.AadharNo = AadharKYC.KYCNumber;
                        memberModel.AadharImagePath = AadharKYC.FileName;
                    }

                    //Fill Member Model With Kyc PAN details 
                    var panType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.PAN.ToString()));
                    MemberKYCDto panKYC = memberkycdto.Find(a => a.KYCType == panType.RefID);
                    if (panKYC != null)
                    {
                        memberModel.PANNo = panKYC.KYCNumber;
                        memberModel.PANImagePath = panKYC.FileName;
                    }

                    //Fill Member Model With Kyc Voter details 
                    var voterType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.VOTERID.ToString()));
                    MemberKYCDto voterKYC = memberkycdto.Find(a => a.KYCType == voterType.RefID);
                    if (voterKYC != null)
                    {
                        memberModel.VoterNo = voterKYC.KYCNumber;
                        memberModel.VoterImagePath = voterKYC.FileName;
                    }

                    //Fill Member Model With Kyc Ration details 
                    var rationType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.RATIONCARD.ToString()));
                    MemberKYCDto rationKYC = memberkycdto.Find(a => a.KYCType == rationType.RefID);
                    if (rationKYC != null)
                    {
                        memberModel.RationNo = rationKYC.KYCNumber;
                        memberModel.RationImagePath = rationKYC.FileName;
                    }



                    //Fill Member Model With Kyc Ration details 
                    var jobcardType = (kycTypeRefereces.Find(k => k.RefCode.ToUpper() == Utilities.Enums.KycType.JOBCARD.ToString()));
                    MemberKYCDto jobcardKYC = memberkycdto.Find(a => a.KYCType == jobcardType.RefID);
                    if (jobcardKYC != null)
                    {
                        memberModel.JobcardNo = jobcardKYC.KYCNumber;
                        memberModel.JobcardImagePath = jobcardKYC.FileName;
                    }
                }
            }

            if (memberId > 0)
            {
                ViewBag.MemberID = memberId;
            }

            int groupid = GroupInfo.GroupID;
            PanchayatLookupDto panchayatlookupDto = _panchayatService.GetByGroupID(groupid);
            memberModel.GroupID = GroupInfo.GroupID;
            memberModel.GroupName = GroupInfo.GroupName;
            memberModel.GroupCode=GroupInfo.GroupCode;
            memberModel.cluster = GroupInfo.Cluster;
            memberModel.village = GroupInfo.Village;
            memberModel.panchayat = panchayatlookupDto.Panchayat;


            //int GroupId = GroupInfo.GroupID;
            var lstmemberleadership = _memberservice.LeaderShipLookUp(memberId);
            ViewBag.LeadershipDetails = lstmemberleadership;
            //return View(lstmemberleadership);



            return View(memberModel);
        }
        [HttpPost]
        public ActionResult CreateMember(MemberModel objmembermodel, List<HttpPostedFileBase> fileUpload,FormCollection form)
        {
            string CurrentTab = Request.Form.Get("CurrentTab");
            ViewBag.CurrentTab = CurrentTab;
            var resultDto = new ResultDto();

            #region Drop Downs
            SelectList educationQualification = GetDropDownListByMasterCode(Enums.RefMasterCodes.EDUCATION_QUALIFICATION);
            ViewBag.educationQualification = educationQualification;

            SelectList nomineeRelationship = GetDropDownListByMasterCode(Enums.RefMasterCodes.NOMINEE_RELATIONSHIP);
            ViewBag.nomineeRelationship = nomineeRelationship;

            SelectList parentGuardionRelationship = GetDropDownListByMasterCode(Enums.RefMasterCodes.PARENT_GUARDIAN_RELATIONSHIP);
            ViewBag.parentGuardionRelationship = parentGuardionRelationship;

            SelectList socialCategory = GetDropDownListByMasterCode(Enums.RefMasterCodes.SOCIAL_CATEGORY);
            ViewBag.socialCategory = socialCategory;

            SelectList monthlyIncome = GetDropDownListByMasterCode(Enums.RefMasterCodes.MONTHLY_INCOME);
            ViewBag.monthlyIncome = monthlyIncome;

            SelectList incomefrequency = GetDropDownListByMasterCode(Enums.RefMasterCodes.INCOME_FREQUENCY);
            ViewBag.incomefrequency = incomefrequency;

            SelectList religion = GetDropDownListByMasterCode(Enums.RefMasterCodes.RELIGION);
            ViewBag.religion = religion;

            List<SelectListDto> lstoccupationDto = _occupationService.GetOccupationSelectList();
            SelectList lstoccupation = new SelectList(lstoccupationDto, "ID", "Text");
            ViewBag.OccupationName = lstoccupation;

            List<GroupLookupDto> lstGroupDto = _groupService.Lookup();
            SelectList lstgroup = new SelectList(lstGroupDto, "GroupID", "GroupCode");
            ViewBag.GroupNames = lstgroup;

            List<SelectListDto> lstaccountheaddto = _accountheadservice.GetAccountHeadSelectList();
            SelectList lstaccounthead = new SelectList(lstaccountheaddto, "ID", "Text");
            ViewBag.accounthead = lstaccounthead;

            SelectList FederationDesigination = GetDropDownListByMasterCode(Enums.RefMasterCodes.FED_DESG);
            ViewBag.FED_DESG = FederationDesigination;

            SelectList GroupDesigination = GetDropDownListByMasterCode(Enums.RefMasterCodes.GROUP_DESG);
            ViewBag.GROUP_DESG = GroupDesigination;

            SelectList ClusterDesigination = GetDropDownListByMasterCode(Enums.RefMasterCodes.CLUSTER_DESG);
            ViewBag.CLUSTER_DESG = ClusterDesigination;
            #endregion Drop Downs


            MemberDto memberDto = new MemberDto();
            MemberModel fileModelTemp = new MemberModel();
            // Insert Or Update Member Personal Details
            if (CurrentTab.ToUpper() == Enums.MemberTabs.PERSONALDETAILS.ToString().ToUpper())
            {

                //BEGIN PHOTO SAVING
                string directoryPath = "AssetUploads/Member/" + objmembermodel.MemberID;
                if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath)))
                    Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath));

                var file = Request.Files["Photo"];
                string guid = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(file.FileName);
                string fullFileName = directoryPath + "/" + guid + extension;

                file.SaveAs(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fullFileName));
                objmembermodel.Photo = fullFileName;
                //END PHOTO SAVING
                memberDto = Mapper.Map<MemberModel, MemberDto>(objmembermodel);
                memberDto.UserID = UserInfo.UserID;
                if (memberDto.MemberID == 0)
                {
                    resultDto = _memberservice.Insert(memberDto);
                }
                else
                    resultDto = _memberservice.Update(memberDto);
            }
            else if (CurrentTab.ToUpper() == Enums.MemberTabs.FAMILYDETAILS.ToString().ToUpper())
            {
                memberDto = Mapper.Map<MemberModel, MemberDto>(objmembermodel);
                memberDto.UserID = UserInfo.UserID;

                resultDto = _memberservice.UpdateFamily(memberDto);
                resultDto.ObjectId = objmembermodel.MemberID;
                resultDto.ObjectCode = objmembermodel.MemberCode;
                List<MemberKYCDto> lstKyc = new List<MemberKYCDto>();
                MemberKycModel objkycdto = new MemberKycModel();

                lstKyc = _memberKycService.GetByMemberID(objmembermodel.MemberID);
                List<ReferenceValueDto> lstReferenceValueDto = _referenceValueService.GetByRefMasterCode(Enums.RefMasterCodes.KYCTYPE.ToString());

                //AADHAR
                var aadharref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.AADHAR.ToString().ToUpper());
                var fileAadhar = Request.Files["fileAadhar"];
                if (fileAadhar != null && fileAadhar.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["AADHAR"]) ? Request.Form["AADHAR"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == aadharref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == aadharref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileAadhar, objmembermodel.MemberID, aadharref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.AadharImagePath = obj.FileName;
                    fileModelTemp.AadharNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == aadharref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == aadharref.RefID);
                        fileModelTemp.AadharImagePath = obj.FileName;
                        fileModelTemp.AadharNo = obj.KYCNumber;
                    }
                }
                //VOTER
                var voterref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.VOTERID.ToString().ToUpper());
                var fileVoter = Request.Files["fileVoter"];
                if (fileVoter != null && fileVoter.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["VoterID"]) ? Request.Form["VoterID"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == voterref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == voterref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileVoter, objmembermodel.MemberID, voterref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.VoterImagePath = obj.FileName;
                    fileModelTemp.VoterNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == voterref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == voterref.RefID);
                        fileModelTemp.VoterImagePath = obj.FileName;
                        fileModelTemp.VoterNo = obj.KYCNumber;
                    }
                }

                //PAN
                var panref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.PAN.ToString().ToUpper());
                var filepan = Request.Files["filePan"];
                if (filepan != null && filepan.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["PANCard"]) ? Request.Form["PANCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == panref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == panref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(filepan, objmembermodel.MemberID, panref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.PANImagePath = obj.FileName;
                    fileModelTemp.PANNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == panref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == panref.RefID);
                        fileModelTemp.PANImagePath = obj.FileName;
                        fileModelTemp.PANNo = obj.KYCNumber;
                    }
                }

                //RATION
                var rationref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.RATIONCARD.ToString().ToUpper());
                var fileration = Request.Files["fileRation"];
                if (fileration != null && fileration.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["RationCard"]) ? Request.Form["RationCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == rationref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == rationref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileration, objmembermodel.MemberID, rationref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.RationImagePath = obj.FileName;
                    fileModelTemp.RationNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == rationref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == rationref.RefID);
                        fileModelTemp.RationImagePath = obj.FileName;
                        fileModelTemp.RationNo = obj.KYCNumber;
                    }
                }

                //JOB CARD
                var Jobcardref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.JOBCARD.ToString().ToUpper());
                var fileJobcard = Request.Files["fileJobcard"];
                if (fileJobcard != null && fileJobcard.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["JobCard"]) ? Request.Form["JobCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == Jobcardref.RefID) != null)
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == Jobcardref.RefID).MemberKYCID;
                    MemberKYCDto obj = SaveKYC(fileJobcard, objmembermodel.MemberID, Jobcardref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.JobcardImagePath = obj.FileName;
                    fileModelTemp.JobcardNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == Jobcardref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == Jobcardref.RefID);
                        fileModelTemp.JobcardImagePath = obj.FileName;
                        fileModelTemp.JobcardNo = obj.KYCNumber;
                    }
                }


            }
            else if (CurrentTab.ToUpper() == Enums.MemberTabs.GENERALACCOUNTHEAD.ToString().ToUpper())
            {
                memberDto = Mapper.Map<MemberModel, MemberDto>(objmembermodel);
                memberDto.UserID = UserInfo.UserID;

                resultDto = _memberservice.UpdateAccountHead(memberDto);
                resultDto.ObjectId = objmembermodel.MemberID;
                resultDto.ObjectCode = objmembermodel.MemberCode;

                List<MemberKYCDto> lstKyc = new List<MemberKYCDto>();
                MemberKycModel objkycdto = new MemberKycModel();

                lstKyc = _memberKycService.GetByMemberID(resultDto.ObjectId);
                List<ReferenceValueDto> lstReferenceValueDto = _referenceValueService.GetByRefMasterCode(Enums.RefMasterCodes.KYCTYPE.ToString());

                //AADHAR
                var aadharref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.AADHAR.ToString().ToUpper());
                var fileAadhar = Request.Files["fileAadhar"];
                if (fileAadhar != null && fileAadhar.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["AADHAR"]) ? Request.Form["AADHAR"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == aadharref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == aadharref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileAadhar, objmembermodel.MemberID, aadharref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.AadharImagePath = obj.FileName;
                    fileModelTemp.AadharNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == aadharref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == aadharref.RefID);
                        fileModelTemp.AadharImagePath = obj.FileName;
                        fileModelTemp.AadharNo = obj.KYCNumber;
                    }
                }
                //VOTER
                var voterref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.VOTERID.ToString().ToUpper());
                var fileVoter = Request.Files["fileVoter"];
                if (fileVoter != null && fileVoter.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["VoterID"]) ? Request.Form["VoterID"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == voterref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == voterref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileVoter, objmembermodel.MemberID, voterref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.VoterImagePath = obj.FileName;
                    fileModelTemp.VoterNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == voterref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == voterref.RefID);
                        fileModelTemp.VoterImagePath = obj.FileName;
                        fileModelTemp.VoterNo = obj.KYCNumber;
                    }
                }

                //PAN
                var panref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.PAN.ToString().ToUpper());
                var filepan = Request.Files["filePan"];
                if (filepan != null && filepan.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["PANCard"]) ? Request.Form["PANCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == panref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == panref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(filepan, objmembermodel.MemberID, panref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.PANImagePath = obj.FileName;
                    fileModelTemp.PANNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == panref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == panref.RefID);
                        fileModelTemp.PANImagePath = obj.FileName;
                        fileModelTemp.PANNo = obj.KYCNumber;
                    }
                }

                //RATION
                var rationref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.RATIONCARD.ToString().ToUpper());
                var fileration = Request.Files["fileRation"];
                if (fileration != null && fileration.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["RationCard"]) ? Request.Form["RationCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == rationref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == rationref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileration, objmembermodel.MemberID, rationref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.RationImagePath = obj.FileName;
                    fileModelTemp.RationNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == rationref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == rationref.RefID);
                        fileModelTemp.RationImagePath = obj.FileName;
                        fileModelTemp.RationNo = obj.KYCNumber;
                    }
                }

                //JOB CARD
                var Jobcardref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.JOBCARD.ToString().ToUpper());
                var fileJobcard = Request.Files["fileJobcard"];
                if (fileJobcard != null && fileJobcard.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["JobCard"]) ? Request.Form["JobCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == Jobcardref.RefID) != null)
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == Jobcardref.RefID).MemberKYCID;
                    MemberKYCDto obj = SaveKYC(fileJobcard, objmembermodel.MemberID, Jobcardref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.JobcardImagePath = obj.FileName;
                    fileModelTemp.JobcardNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == Jobcardref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == Jobcardref.RefID);
                        fileModelTemp.JobcardImagePath = obj.FileName;
                        fileModelTemp.JobcardNo = obj.KYCNumber;
                    }
                }

            }
            else if (CurrentTab.ToUpper() == Enums.MemberTabs.PROOFS.ToString().ToUpper())
            {
                List<MemberKYCDto> lstKyc = new List<MemberKYCDto>();
                MemberKycModel objkycdto = new MemberKycModel();

                lstKyc = _memberKycService.GetByMemberID(objmembermodel.MemberID);
                List<ReferenceValueDto> lstReferenceValueDto = _referenceValueService.GetByRefMasterCode(Enums.RefMasterCodes.KYCTYPE.ToString());

                //AADHAR
                var aadharref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.AADHAR.ToString().ToUpper());
                var fileAadhar = Request.Files["fileAadhar"];
                if (fileAadhar != null && fileAadhar.ContentLength > 0)
                {   
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["AADHAR"]) ? Request.Form["AADHAR"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == aadharref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == aadharref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileAadhar, objmembermodel.MemberID, aadharref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.AadharImagePath = obj.FileName;
                    fileModelTemp.AadharNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == aadharref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == aadharref.RefID);
                        fileModelTemp.AadharImagePath = obj.FileName;
                        fileModelTemp.AadharNo = obj.KYCNumber;
                    }
                }

                //VOTER
                var voterref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.VOTERID.ToString().ToUpper());
                var fileVoter = Request.Files["fileVoter"];
                if (fileVoter != null && fileVoter.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["VoterID"]) ? Request.Form["VoterID"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == voterref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == voterref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileVoter, objmembermodel.MemberID, voterref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.VoterImagePath = obj.FileName;
                    fileModelTemp.VoterNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == voterref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == voterref.RefID);
                        fileModelTemp.VoterImagePath = obj.FileName;
                        fileModelTemp.VoterNo = obj.KYCNumber;
                    }
                }

                //PAN
                var panref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.PAN.ToString().ToUpper());
                var filepan = Request.Files["filePan"];
                if (filepan != null && filepan.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["PANCard"]) ? Request.Form["PANCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == panref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == panref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(filepan, objmembermodel.MemberID, panref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.PANImagePath = obj.FileName;
                    fileModelTemp.PANNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == panref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == panref.RefID);
                        fileModelTemp.PANImagePath = obj.FileName;
                        fileModelTemp.PANNo = obj.KYCNumber;
                    }
                }

                //RATION
                var rationref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.RATIONCARD.ToString().ToUpper());
                var fileration = Request.Files["fileRation"];
                if (fileration != null && fileration.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["RationCard"]) ? Request.Form["RationCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == rationref.RefID) != null)
                    {
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == rationref.RefID).MemberKYCID;
                    }
                    MemberKYCDto obj = SaveKYC(fileration, objmembermodel.MemberID, rationref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.RationImagePath = obj.FileName;
                    fileModelTemp.RationNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == rationref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == rationref.RefID);
                        fileModelTemp.RationImagePath = obj.FileName;
                        fileModelTemp.RationNo = obj.KYCNumber;
                    }
                }

                //JOB CARD
                var Jobcardref = lstReferenceValueDto.Find(l => l.RefCode.ToUpper() == Enums.KycType.JOBCARD.ToString().ToUpper());
                var fileJobcard = Request.Files["fileJobcard"];
                if (fileJobcard != null && fileJobcard.ContentLength > 0)
                {
                    objkycdto.KYCNumber = !string.IsNullOrEmpty(Request.Form["JobCard"]) ? Request.Form["JobCard"] : "";
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == Jobcardref.RefID) != null)
                        objkycdto.MemberKYCID = lstKyc.Find(f => f.KYCType == Jobcardref.RefID).MemberKYCID;
                    MemberKYCDto obj = SaveKYC(fileJobcard, objmembermodel.MemberID, Jobcardref.RefID, objkycdto.KYCNumber, objkycdto.MemberKYCID);
                    fileModelTemp.JobcardImagePath = obj.FileName;
                    fileModelTemp.JobcardNo = obj.KYCNumber;
                }
                else
                {
                    if (lstKyc != null && lstKyc.Count > 0 && lstKyc.Find(f => f.KYCType == Jobcardref.RefID) != null)
                    {
                        MemberKYCDto obj = lstKyc.Find(f => f.KYCType == Jobcardref.RefID);
                        fileModelTemp.JobcardImagePath = obj.FileName;
                        fileModelTemp.JobcardNo = obj.KYCNumber;
                    }
                }

                resultDto.Message = "Member KYC data saved successfully for member code : " + objmembermodel.MemberCode;
                resultDto.ObjectId = objmembermodel.MemberID;
                resultDto.ObjectCode = objmembermodel.MemberCode;
            }
            else if (CurrentTab.ToUpper() == Enums.MemberTabs.LEADERSHIP.ToString().ToUpper())
            {
                memberDto.leaderShipLevel = Convert.ToString(Request.Form["Leadership"]);
                memberDto.designation = Convert.ToInt32(Request.Form["FederationDesigination"]);
                memberDto.FromDate = Convert.ToDateTime(Request.Form["FromDate"]);
                memberDto.MemberID = objmembermodel.MemberID;


                resultDto = _memberservice.UpdateMemberLeaderShip(memberDto.MemberID,memberDto.leaderShipLevel,memberDto.designation,memberDto.FromDate,UserInfo.UserID);
                resultDto.ObjectId = objmembermodel.MemberID;
                resultDto.ObjectCode = objmembermodel.MemberCode;
                resultDto.Message = "Leadership details saved successfully for member code : " + objmembermodel.MemberCode;

                var lstmemberleadership = _memberservice.LeaderShipLookUp(objmembermodel.MemberID);
                ViewBag.LeadershipDetails = lstmemberleadership;

            }

            if (resultDto.ObjectId > 0)
            {
                memberDto = _memberservice.GetById(resultDto.ObjectId);
                objmembermodel = AutoMapperEntityConfiguration.Cast<MemberModel>(memberDto);

                OrganizationService _organizationService = new OrganizationService();
                OrganizationDto org = _organizationService.GetAll();
                ViewBag.MemberRetirementAge = org != null ? org.MemberRetirementAge : 60;

                //fill values get back
                objmembermodel.JobcardNo = fileModelTemp.JobcardNo;
                objmembermodel.JobcardImagePath = fileModelTemp.JobcardImagePath;

                objmembermodel.VoterNo = fileModelTemp.VoterNo;
                objmembermodel.VoterImagePath = fileModelTemp.VoterImagePath;
                objmembermodel.AadharNo = fileModelTemp.AadharNo;
                objmembermodel.AadharImagePath = fileModelTemp.AadharImagePath;
                objmembermodel.PANNo = fileModelTemp.PANNo;
                objmembermodel.PANImagePath = fileModelTemp.PANImagePath;
                objmembermodel.RationNo = fileModelTemp.RationNo;
                objmembermodel.RationImagePath = fileModelTemp.RationImagePath;
                if (objmembermodel.DOB != DateTime.MinValue)
                    objmembermodel.DateOfRetirement = objmembermodel.DOB.AddYears(org.MemberRetirementAge);

                resultDto.ObjectCode = memberDto.MemberCode;
            }

            ModelState.Clear();
            int groupid = GroupInfo.GroupID;
            PanchayatLookupDto panchayatlookupDto = _panchayatService.GetByGroupID(groupid);
            objmembermodel.GroupID = GroupInfo.GroupID;
            objmembermodel.GroupName = GroupInfo.GroupName;
            objmembermodel.GroupCode = GroupInfo.GroupCode;
            objmembermodel.cluster = GroupInfo.Cluster;
            objmembermodel.village = GroupInfo.Village;
            objmembermodel.panchayat = panchayatlookupDto.Panchayat;
            ViewBag.Result = resultDto;
            return View(objmembermodel);
        }
        [HttpGet]
        public ActionResult MemberLookUp()
        {
            int GroupId = GroupInfo.GroupID;
            var lstmemberlookup = _memberservice.LookUp(GroupId);
            return View(lstmemberlookup);
        }

        public MemberKYCDto SaveKYC(HttpPostedFileBase file, int memberId, int refid, string KYCNumber, int MemberKYCID)
        {
            string directoryPath = "AssetUploads/Member/" + memberId;
            if (!Directory.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath)))
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryPath));

            string guid = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(file.FileName);
            string fullFileName = directoryPath + '/' + guid + extension;

            file.SaveAs(AppDomain.CurrentDomain.BaseDirectory + "/" + fullFileName);

            MemberKYCDto objMemberKYCDto = new MemberKYCDto()
            {
                MemberID = memberId,
                MemberKYCID = MemberKYCID,
                KYCNumber = KYCNumber,
                KYCType = refid,
                FileName = fullFileName,
                ActualFileName = Path.GetFileName(file.FileName),
                UserID = UserInfo.UserID
            };
            if (objMemberKYCDto.MemberKYCID > 0)
            {
                _memberKycService.Update(objMemberKYCDto);

            }
            else
            {
                _memberKycService.Insert(objMemberKYCDto);
            }
            return objMemberKYCDto;

        }

        [HttpGet]
        public ActionResult ViewMember(string id)
        {
            int memberID = DecryptQueryString(id);

            if (memberID <= 0)
                return RedirectToAction("MemberLookUp");
            var memberViewDto = _memberservice.GetViewByID(memberID);

            OrganizationService _organizationService = new OrganizationService();
            OrganizationDto org = _organizationService.GetAll();
            if (memberViewDto.DOB != DateTime.MinValue)
                memberViewDto.DateOfRetirement = memberViewDto.DOB.AddYears(org.MemberRetirementAge);

            return View(memberViewDto);
        }

        [HttpGet]
        public ActionResult DeleteMember(string Id)
        {
            int memberId = DecryptQueryString(Id);

            if (memberId < 1)
                return RedirectToAction("MemberLookUp");

            ResultDto resultDto = _memberservice.Delete(memberId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberLookUp");
        }

        [HttpGet]
        public ActionResult ActiveInactiveMember(string Id)
        {
            int memberId = DecryptQueryString(Id);

            if (memberId < 1)
                return RedirectToAction("MemberLookUp");

            ResultDto resultDto = _memberservice.ChangeStatus(memberId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("MemberLookUp");
        }

        public ActionResult BindDesginations(string flag)
        {
            StringBuilder sbOptions = new StringBuilder();
            Enums.RefMasterCodes level = flag == "F" ? Enums.RefMasterCodes.FED_DESG : flag == "C" ? Enums.RefMasterCodes.CLUSTER_DESG : Enums.RefMasterCodes.GROUP_DESG;

            SelectList lstDesignations = GetDropDownListByMasterCode(level);
            foreach (var item in lstDesignations)
            {
                sbOptions.Append("<option value=" + item.Value + ">" + item.Text + "</option>");
            }

            return Content(sbOptions.ToString());
        }
    }
}
