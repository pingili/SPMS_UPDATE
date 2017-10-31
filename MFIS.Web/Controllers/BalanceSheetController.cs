using BusinessEntities;
using BusinessLogic;
using CrystalDecisions.CrystalReports.Engine;
using DataLogic.Implementation;
using MFIEntityFrameWork;
using MFIS.Web.Models;
using MFIS.Web.Views.TrialBalance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MFIS.Web.Controllers
{


    public class BalanceSheetController : BaseController
    {
        private readonly MFISDBContext _dbContext;

        public BalanceSheetController()
        {

            _dbContext = new MFISDBContext();
        }

        #region Group Balancesheet Report
        [HttpGet]
        public ActionResult BalanceSheet_Group()
        {
            ReportsDal objDal = new ReportsDal();

            List<TrialBalanceDto> lstRecords = objDal.GetAllTrialBalanceReport_Group(GroupInfo.GroupID);
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvmAssets = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "ASSETS");
            RefValueMaster rvmLiabilities = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "LIABILITIES");

            List<TrialBalanceDto> lstAssets = lstRecords.FindAll(l => l.AhType == rvmAssets.RefID);
            List<TrialBalanceDto> lstLiabilities = lstRecords.FindAll(l => l.AhType == rvmLiabilities.RefID);
            int RecordCount = lstRecords.Count > lstLiabilities.Count ? lstRecords.Count : lstLiabilities.Count;
            ViewBag.RecordCount = RecordCount;
            ViewBag.Assets = lstAssets;
            ViewBag.Liabilities = lstLiabilities;

            var Debit1 = lstAssets.Sum(l => l.Debit3);
            var Credit1 = lstAssets.Sum(l => l.Credit3);
            ViewBag.TotalAssetsBalance = Debit1 - Credit1;
            var Debit2 = lstLiabilities.Sum(M => M.Debit3);
            var Credit2 = lstLiabilities.Sum(M => M.Credit3);
            ViewBag.TotalLiabilitiesbalance = Credit2-Debit2;
            return View("BalanceSheet");
        }
        [HttpGet]
        public ActionResult ExportsReports_Group()
        {
            ReportsDal objDal = new ReportsDal();
            OrganizationService objIOrganizationService = new OrganizationService();
            OrganizationDto organizationDto = objIOrganizationService.GetAll();
            List<TrialBalanceDto> lstRecords = objDal.GetAllTrialBalanceReport_Group(GroupInfo.GroupID);
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvmAssets = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "ASSETS");
            RefValueMaster rvmLiabilities = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "LIABILITIES");
            
            List<TrialBalanceDto> lstAssets = lstRecords.FindAll(l => l.AhType == rvmAssets.RefID);
            List<TrialBalanceDto> lstLiabilities = lstRecords.FindAll(l => l.AhType == rvmLiabilities.RefID);
            int recordCount = lstLiabilities.Count > lstAssets.Count ? lstLiabilities.Count : lstAssets.Count;
            List<BalanceSheetModel> reportData = new List<BalanceSheetModel>();
            for (int idx = 0; idx < recordCount; idx++)
            {
                TrialBalanceDto objAsset = lstAssets.Count > idx ? lstAssets[idx] : null;
                TrialBalanceDto objLiability = lstLiabilities.Count > idx ? lstLiabilities[idx] : null;
                BalanceSheetModel objBalanceSheetModel= new BalanceSheetModel() ;
                objBalanceSheetModel.OrgName = organizationDto.OrgName;
                objBalanceSheetModel.OrgAddress = organizationDto.Address;
                if(objAsset!=null)
                {
                    objBalanceSheetModel.Asset_AHCode =objAsset.Ahcode;
                    objBalanceSheetModel.Asset_AHName = objAsset.Accounthaedname;
                    objBalanceSheetModel.Asset_CrAmount = objAsset.Credit3;
                    objBalanceSheetModel.Asset_DrAmount = objAsset.Debit3;
                }
                if (objLiability != null)
                {
                    objBalanceSheetModel.Liability_AHCode = objLiability.Ahcode;
                    objBalanceSheetModel.Liability_AHName = objLiability.Accounthaedname;
                    objBalanceSheetModel.Liability_CrAmount = objLiability.Credit3;
                    objBalanceSheetModel.Liability_DrAmount = objLiability.Debit3;
                }
                reportData.Add(objBalanceSheetModel);
            }

            double assetSum = reportData.Sum(l => l.Asset_DrAmount - l.Asset_CrAmount);
            double liabilitySum = reportData.Sum(l => l.Liability_CrAmount-l.Liability_DrAmount);

            reportData.ForEach(l => l.Liability_Sum = liabilitySum);
            reportData.ForEach(l => l.Asset_Sum = assetSum);
            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/FinancialRports/BalanceSheet.rpt"));
            rd.SetDataSource(reportData);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "BalanceSheet.pdf");
        }
        #endregion Group Balancesheet Report

        #region Federation Balancesheet Report
        [HttpGet]
        public ActionResult BalanceSheet()
        {

            TrialBalanceController TBC = new TrialBalanceController();
            List<TrialBalanceDto> lstRecords = TBC.GetAllTrialBalanceReport();
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvmAssets = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "ASSETS");
            RefValueMaster rvmLiabilities = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "LIABILITIES");

            List<TrialBalanceDto> lstAssets = lstRecords.FindAll(l => l.AhType == rvmAssets.RefID);
            List<TrialBalanceDto> lstLiabilities = lstRecords.FindAll(l => l.AhType == rvmLiabilities.RefID);
            int RecordCount = lstRecords.Count > lstLiabilities.Count ? lstRecords.Count : lstLiabilities.Count;
            ViewBag.RecordCount = RecordCount;
            ViewBag.Assets = lstAssets;
            ViewBag.Liabilities = lstLiabilities;

            var Debit1 = lstAssets.Sum(l => l.Debit3);
            var Credit1 = lstAssets.Sum(l => l.Credit3);
            ViewBag.TotalAssetsBalance = Debit1 + Credit1;
            var Debit2 = lstLiabilities.Sum(M => M.Debit3);
            var Credit2 = lstLiabilities.Sum(M => M.Credit3);
            ViewBag.TotalLiabilitiesbalance = Debit2 + Credit2;
            return View();
        }
        [HttpGet]
        public ActionResult ExportsReports()
        {
            TrialBalanceController TBC = new TrialBalanceController();
            List<TrialBalanceDto> lstRecords = TBC.GetAllTrialBalanceReport();
            RefMaster rm = _dbContext.RefMasters.ToList().Find(r => r.RefMasterCode.ToUpper() == "AHTYPE");
            RefValueMaster rvmAssets = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "ASSETS");
            RefValueMaster rvmLiabilities = _dbContext.RefValueMasters.ToList().Find(f => f.RefMasterID == rm.RefMasterID && f.RefCode.ToUpper() == "LIABILITIES");
            List<TrialBalanceDto> lstAssets = lstRecords.FindAll(l => l.AhType == rvmAssets.RefID);
            List<TrialBalanceDto> lstLiabilities = lstRecords.FindAll(l => l.AhType == rvmLiabilities.RefID);
            var Debit1 = lstAssets.Sum(l => l.Debit3);
            var Credit1 = lstAssets.Sum(l => l.Credit3);
            var Debit2 = lstLiabilities.Sum(M => M.Debit3);
            var Credit2 = lstLiabilities.Sum(M => M.Credit3);

            BalanceSheetDto lst = new BalanceSheetDto();
            lst.lstAssects = lstAssets;
            lst.lstLiabilities = lstLiabilities;
            lst.Debit1 = Debit1;
            lst.Credit1 = Credit1;
            lst.Debit2 = Debit2;
            lst.Credit2 = Credit2;
            ReportDocument rd = new ReportDocument();
            rd.Load(Server.MapPath("~/Reports/FinancialRports/BalanceSheet.rpt"));
            rd.SetDataSource(lst);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();
            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "BalanceSheet.pdf");
        }
        #endregion Federation Balancesheet Reports
    }
}
