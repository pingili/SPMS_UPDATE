using BusinessEntities;
using BusinessLogic;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GroupDepositOBController : BaseController
    {
        private readonly MemberService _memberService;
        private readonly InterestService _interestService;

        public GroupDepositOBController()
        {
            _memberService = new MemberService();
            _interestService = new InterestService();
        }
        [HttpGet]
        public ActionResult CreateDepositOB(int? id)
        {
            int MemberId = Convert.ToInt32(id);
            DepositOBDto lstDepositOBDto = new DepositOBDto();
            DepositOBBll objDepositOBBll = new DepositOBBll();
            if (MemberId > 0)
            {
                lstDepositOBDto = objDepositOBBll.GetByMemberID(MemberId);
            }
            ViewBag.clusters = GroupInfo.Cluster;
            ViewBag.village = GroupInfo.Village;
            ViewBag.GroupName = GroupInfo.GroupName;
            ViewBag.MeetingDay = GroupInfo.MeetingDay;
            int GroupId = GroupInfo.GroupID;

            List<MemberLookupDto> lstMembers = _memberService.GetByGroupId(GroupInfo.GroupID);
            SelectList slMembers = null;

            if (lstDepositOBDto.MemberId == 0)
            {
                DepositOBBll depositOBBll = new DepositOBBll();
                List<DepositOBLookup> lstDepositOB = depositOBBll.DepositOBLookUpList(true,GroupInfo.GroupID);
                foreach (var member in lstDepositOB)
                {
                    lstMembers.Remove(lstMembers.Find(l => l.MemberID == member.MemberId));
                }
                slMembers = new SelectList(lstMembers, "MemberID", "MemberName");
            }
            else
            {
                List<MemberLookupDto> lstMembersTemp = new List<MemberLookupDto>();
                foreach (var member in lstMembers)
                {
                    if (member.MemberID == lstDepositOBDto.MemberId)
                    {
                        lstMembersTemp.Add(lstMembers.Find(l => l.MemberID == member.MemberID));
                    }
                }
                slMembers = new SelectList(lstMembersTemp, "MemberID", "MemberName");
            }

            ViewBag.Members = slMembers;
            List<SelectListDto> lstInterests = _interestService.GetDepositInterestsSelectList(GroupInfo.GroupID);
            SelectList slInterests = new SelectList(lstInterests, "ID", "Text");
            ViewBag.interests = slInterests;

            //ViewBag.lastpaidDate = string.Format("{0}/Mar/2016", GroupInfo.MeetingDay);
            //lstDepositOBDto.LastPaidDate1 =Convert.ToDateTime(ViewBag.lastpaidDate);
            //lstDepositOBDto.LastPaidDate2 = Convert.ToDateTime(ViewBag.lastpaidDate);
            //lstDepositOBDto.LastPaidDate3 = Convert.ToDateTime(ViewBag.lastpaidDate);
            //lstDepositOBDto.LastPaidDate4 = Convert.ToDateTime(ViewBag.lastpaidDate);

            //var DB1 = (lstDepositOBDto.DepositBalance1) > 0 ?lstDepositOBDto.DepositBalance1 : 0;
            //var DB2 = (lstDepositOBDto.IneterestDue1) > 0 ? lstDepositOBDto.IneterestDu*e1 : 0;
            //ViewBag.Total1 = DB1 + DB2;
            //var DB3 = (lstDepositOBDto.DepositBalance2) > 0 ? lstDepositOBDto.DepositBalance2 : 0;
            //var DB4 = (lstDepositOBDto.IneterestDue2) > 0 ? lstDepositOBDto.IneterestDue2 : 0;
            //ViewBag.Total2 = DB3 + DB4;
            //var DB5 = (lstDepositOBDto.DepositBalance3) > 0 ? lstDepositOBDto.DepositBalance3 : 0;
            //var DB6 = (lstDepositOBDto.IneterestDue3) > 0 ? lstDepositOBDto.IneterestDue3 : 0;
            //ViewBag.Total3 = DB5 + DB6;
            //var DB7 = (lstDepositOBDto.DepositBalance4) > 0 ? lstDepositOBDto.DepositBalance4 : 0;
            //var DB8 = (lstDepositOBDto.IneterestDue4) > 0 ? lstDepositOBDto.IneterestDue4 : 0;
            //ViewBag.Total4 = DB7 + DB8;

            int Id = lstDepositOBDto.MemberId;

            MemberDto obj = _memberService.GetById(Id);
            lstDepositOBDto.MemberCode = obj==null ? string.Empty: obj.MemberCode;

            return View(lstDepositOBDto);
        }
        public JsonResult GetMemberName(int Id)
        {
            MemberDto obj = _memberService.GetById(Id);
            return Json(new { MemberCode = obj.MemberCode });
        }
        [HttpPost]
        public JsonResult SaveDepositOB(DepositOBDto depositOBDto)
        {
            DepositOBBll objDepositOBBll = new DepositOBBll();
            depositOBDto.UserID = UserInfo.UserID;
            depositOBDto.GroupId = GroupInfo.GroupID;
            depositOBDto = objDepositOBBll.InsertLoanOB(depositOBDto);
            string Message = null;
            if (depositOBDto.Id1 != 0 || depositOBDto.Id2 != 0 || depositOBDto.Id3 != 0 || depositOBDto.Id4 != 0)
            {
                Message = "Successfully Inserted";

            }
            else
            {
                Message = "Not Inserted";
            }
            return Json(new { result = Message, Id1 = depositOBDto.Id1, Id2 = depositOBDto.Id2, Id3 = depositOBDto.Id3, Id4 = depositOBDto.Id4 });
        }
        public ActionResult DepositLookup()
        {
            DepositOBBll DepositOBBll = new DepositOBBll();
            DataSet dsDepositOB = DepositOBBll.DepositOBLookUpTable(true, GroupInfo.GroupID);
            /*List<DepositOBLookup> lstDepositOB = DepositOBBll.DepositOBLookUp(true, GroupInfo.GroupID);
            List<DepositOBPPivot> lstDepositOBPPivot = new List<DepositOBPPivot>();
            var lstLoanOBGroup = lstDepositOB.GroupBy(l => l.MemberId).Select(m => m.Key).ToList();
            foreach (var member in lstLoanOBGroup)
            {

                var memberLoans = lstDepositOB.FindAll(l => l.MemberId == member);
                DepositOBPPivot depositOBPPivot = new DepositOBPPivot();
                depositOBPPivot.MemberName = lstDepositOB.Find(l => l.MemberId == member).MemberName;
                depositOBPPivot.MemberId = member;
                foreach (var loan in memberLoans)
                {
                    if (memberLoans.IndexOf(loan) == 0)
                    {
                        depositOBPPivot.DepositAmount1 = loan.DepositAmount;
                    }
                    else if (memberLoans.IndexOf(loan) == 1)
                    {
                        depositOBPPivot.DepositAmount2 = loan.DepositAmount;
                    }
                    else if (memberLoans.IndexOf(loan) == 2)
                    {
                        depositOBPPivot.DepositAmount3 = loan.DepositAmount;
                    }
                    else
                        break;
                }

                lstDepositOBPPivot.Add(depositOBPPivot);
            }
            var amount1 = lstDepositOBPPivot.Sum(l => l.DepositAmount1);
            var amount2 = lstDepositOBPPivot.Sum(l => l.DepositAmount2);
            var amount3 = lstDepositOBPPivot.Sum(l => l.DepositAmount3);
            ViewBag.DepositAmount1 = amount1;
            ViewBag.DepositAmount2 = amount2;
            ViewBag.DepositAmount3 = amount3;
            return View(lstDepositOBPPivot);
            */
            ViewBag.DSDEPOSITOB = dsDepositOB;
            return View();
        }

        public JsonResult GetInterestDetails(int id)
        {
            InterestService _interestService = new InterestService();
            InterestMasterDto interestMasterDto = _interestService.GetByIDExt(id);
            return Json(new { result = interestMasterDto });
        }
    }
}
