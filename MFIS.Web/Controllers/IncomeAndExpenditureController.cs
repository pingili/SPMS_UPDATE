using BusinessEntities;
using BusinessLogic;
using CrystalDecisions.CrystalReports.Engine;
using DataLogic.Implementation;
using MFIEntityFrameWork;
using MFIS.Web.Models;
using MFIS.Web.Views.TrialBalance;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Controllers
{
    public class IncomeAndExpenditureController : BaseController
    {
        private readonly MFISDBContext _dbContext;

        public IncomeAndExpenditureController()
        {
            _dbContext = new MFISDBContext();
        }
          #region fed IncomeandExpenditue
        [HttpGet]
        public ActionResult IncomeAndExpenditure()
        {
            TrialBalanceController TBC = new TrialBalanceController();
            List<TrialBalanceDto> lstRecords = TBC.GetAllTrialBalanceReport_Group();
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvmIncomes = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "INCOME");
            RefValueMaster rvmExpenditure = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "EXPENDITURE");

            List<TrialBalanceDto> lstIncomes = lstRecords.FindAll(l => l.AhType == rvmIncomes.RefID);
            List<TrialBalanceDto> lstExpenditure = lstRecords.FindAll(l => l.AhType == rvmExpenditure.RefID);
            int RecordCount = lstRecords.Count > lstExpenditure.Count ? lstRecords.Count : lstExpenditure.Count;
            ViewBag.RecordCount = RecordCount;
            ViewBag.Incomes = lstIncomes;
            ViewBag.Expenditure = lstExpenditure;
            var Debit1 = lstIncomes.Sum(l => l.Debit3);
            var Credit1 = lstIncomes.Sum(l => l.Credit3);
            ViewBag.TotalIncomesBalance = Debit1 + Credit1;
            var Debit2 = lstExpenditure.Sum(M => M.Debit3);
            var Credit2 = lstExpenditure.Sum(M => M.Credit3);
            ViewBag.TotalExpenditurebalance = Debit2 + Credit2;
            return View();
        }
           #endregion
        #region Group IncomeandExpenditue Report
        [HttpGet]
        public ActionResult IncomeAndExpenditure_Group()
        {
            ReportsDal objDal = new ReportsDal();
            List<TrialBalanceDto> lstRecords = objDal.GetAllTrialBalanceReport_Group(GroupInfo.GroupID);
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvmIncomes = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "INCOME");
            RefValueMaster rvmExpenditure = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "EXPENDITURE");

            List<TrialBalanceDto> lstIncomes = lstRecords.FindAll(l => l.AhType == rvmIncomes.RefID);
            List<TrialBalanceDto> lstExpenditure = lstRecords.FindAll(l => l.AhType == rvmExpenditure.RefID);
            int RecordCount = lstRecords.Count > lstExpenditure.Count ? lstRecords.Count : lstExpenditure.Count;
            ViewBag.RecordCount = RecordCount;
            ViewBag.Incomes = lstIncomes;
            ViewBag.Expenditure = lstExpenditure;
            var Debit1 = lstIncomes.Sum(l => l.Debit3);
            var Credit1 = lstIncomes.Sum(l => l.Credit3);
            ViewBag.TotalIncomesBalance = Debit1 + Credit1;
            var Debit2 = lstExpenditure.Sum(M => M.Debit3);
            var Credit2 = lstExpenditure.Sum(M => M.Credit3);
            ViewBag.TotalExpenditurebalance = Debit2 + Credit2;
            return View("IncomeAndExpenditure");
        }
        [HttpGet]
        public ActionResult ExportsReports_Group()
        {
            ReportsDal objDal = new ReportsDal();
            OrganizationService objIOrganizationService = new OrganizationService();
            OrganizationDto organizationDto = objIOrganizationService.GetAll();
            List<TrialBalanceDto> lstRecords = objDal.GetAllTrialBalanceReport_Group(GroupInfo.GroupID);
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvmIncomes = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "INCOME");
            RefValueMaster rvmExpenditure = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "EXPENDITURE");

            List<TrialBalanceDto> lstIncomes = lstRecords.FindAll(l => l.AhType == rvmIncomes.RefID);
            List<TrialBalanceDto> lstExpenditure = lstRecords.FindAll(l => l.AhType == rvmExpenditure.RefID);
            int recordCount = lstExpenditure.Count > lstIncomes.Count ? lstExpenditure.Count : lstIncomes.Count;
            List<IncomeAndExpenditureModel> reportData = new List<IncomeAndExpenditureModel>();
            for (int idx = 0; idx < recordCount; idx++)
            {
                TrialBalanceDto objAsset = lstIncomes.Count > idx ? lstIncomes[idx] : null;
                TrialBalanceDto objLiability = lstExpenditure.Count > idx ? lstExpenditure[idx] : null;
                IncomeAndExpenditureModel objIncomeAndExpenditureModel = new IncomeAndExpenditureModel();
                objIncomeAndExpenditureModel.OrgName = organizationDto.OrgName;
                objIncomeAndExpenditureModel.OrgAddress = organizationDto.Address;
                if (objAsset != null)
                {
                    objIncomeAndExpenditureModel.Income_AHCode = objAsset.Ahcode;
                    objIncomeAndExpenditureModel.Income_AHName = objAsset.Accounthaedname;
                    objIncomeAndExpenditureModel.Income_CrAmount = objAsset.Credit3;
                    objIncomeAndExpenditureModel.Income_DrAmount = objAsset.Debit3;
                }
                if (objLiability != null)
                {
                    objIncomeAndExpenditureModel.Expenditure_AHCode = objLiability.Ahcode;
                    objIncomeAndExpenditureModel.Expenditure_AHName = objLiability.Accounthaedname;
                    objIncomeAndExpenditureModel.Expenditure_CrAmount = objLiability.Credit3;
                    objIncomeAndExpenditureModel.Expenditure_DrAmount = objLiability.Debit3;
                }
                reportData.Add(objIncomeAndExpenditureModel);
            }

            double incomeSum = reportData.Sum(l => l.Income_CrAmount - l.Income_DrAmount);
            double expenditureSum = reportData.Sum(l => l.Expenditure_DrAmount - l.Expenditure_CrAmount);

            reportData.ForEach(l => l.Expenditure_Sum = expenditureSum);
            reportData.ForEach(l => l.Income_Sum = incomeSum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/FinancialRports/IncomeAndExpenditure.rpt"));
            rd.SetDataSource(reportData);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "IncomeAndExpenditure.pdf");
        }
        #endregion Group IncomeandExpenditue Report
    }
}
