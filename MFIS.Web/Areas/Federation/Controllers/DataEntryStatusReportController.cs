using BusinessEntities;
using DataLogic.Implementation;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Areas.Federation.Controllers
{
    public class DataEntryStatusReportController : BaseController
    {
        //
        // GET: /Federation/DataEntryStatusReport/
        List<DataEntryStatusDBDto> lstDbDto = null;

        [HttpGet]
        public ActionResult DataEntryStatusReport()
        {
            FedReportsDll objDal = new FedReportsDll();
            if (GroupInfo != null && GroupInfo.GroupID > 0)
            {
                ViewBag.IsGroup = true;
                lstDbDto = objDal.GetDataEntryStatusReport(GroupInfo.GroupID);
            }
            else
            {
                lstDbDto = objDal.GetDataEntryStatusReport();
            }


            var lstReport = GetFinalReport();

            return View(lstReport);
        }

        private List<DataEntryStatusReportDto> GetFinalReport()
        {
            List<DataEntryStatusReportDto> lstReport = new List<DataEntryStatusReportDto>();
            var lst = lstDbDto.OrderBy(l => l.GroupId);
            foreach (var dto in lst)
            {
                DataEntryStatusReportDto obj = null;
                if (!lstReport.Exists(l => l.GroupId == dto.GroupId))
                {
                    obj = new DataEntryStatusReportDto();
                    obj.GroupId = dto.GroupId;
                    obj.GroupCode = dto.GroupCode;
                    obj.GroupName = dto.GroupName;
                    obj.ClusterCode = dto.ClusterCode;
                    obj.ClusterName = dto.ClusterName;
                    obj.GroupOBStatus = dto.GroupOBStatus;
                    obj.SavingsMemberCount = dto.SavingsMemberCount;
                    lstReport.Add(obj);
                }
                else
                {
                    obj = lstReport.Find(l => l.GroupId == dto.GroupId);
                }


                if (dto.isConducted)
                {
                    obj.AprC = dto.Apr;
                    obj.MayC = dto.May;
                    obj.JunC = dto.Jun;
                    obj.JulC = dto.Jul;
                    obj.AugC = dto.Aug;
                    obj.SepC = dto.Sep;
                    obj.OctC = dto.Oct;
                    obj.NovC = dto.Nov;
                    obj.DecC = dto.Dec;
                    obj.JanC = dto.Jan;
                    obj.FebC = dto.Feb;
                    obj.MarC = dto.Mar;
                }
                else
                {
                    obj.AprN = dto.Apr;
                    obj.MayN = dto.May;
                    obj.JunN = dto.Jun;
                    obj.JulN = dto.Jul;
                    obj.AugN = dto.Aug;
                    obj.SepN = dto.Sep;
                    obj.OctN = dto.Oct;
                    obj.NovN = dto.Nov;
                    obj.DecN = dto.Dec;
                    obj.JanN = dto.Jan;
                    obj.FebN = dto.Feb;
                    obj.MarN = dto.Mar;
                }

            }
            return lstReport;
        }
    }
}
