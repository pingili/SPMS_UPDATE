using BusinessEntities;
using BusinessLogic.Implementation;
using CoreComponents;
using MFIS.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MFIS.Web.Areas.Group.Controllers.TransactionControllers
{
    public class GroupOtherReceiptController : BaseController
    {
        #region Global Variables
        private readonly MasterService _masterService;
        private readonly GroupOtherReceiptService _groupOtherReceiptService;
        private readonly GroupOtherRecieptDto _groupOtherReceiptDto;
        GroupOtherRecieptUploadValidateInfo objUploadValidateInfo = new GroupOtherRecieptUploadValidateInfo();
        public GroupOtherReceiptController()
        {
            _masterService = new MasterService();
            _groupOtherReceiptService = new GroupOtherReceiptService();
            _groupOtherReceiptDto = new GroupOtherRecieptDto();
        }
        #endregion Global Variables

        [HttpGet]
        public ActionResult CreateOtherReceipt(string Id)
        {
            int AccountMasterId = string.IsNullOrEmpty(Id.DecryptString()) ? default(int) : Convert.ToInt32(Id.DecryptString());
            GroupOtherRecieptDto grpOtherReceiptDto = new GroupOtherRecieptDto();
            if (AccountMasterId > 0)
            {
                grpOtherReceiptDto = _groupOtherReceiptService.EditGroupOtherReceipt(AccountMasterId);
            }
            ViewBag.LockStatus = GroupInfo.LockStatus;
            grpOtherReceiptDto.AccountMasterID = AccountMasterId;
            
            LoadOtherReceiptDropDowns();

            return View(grpOtherReceiptDto);
        }

        public void LoadOtherReceiptDropDowns()
        {
            TypeQueryResult lst = _masterService.GetTypeQueryResult("GROUP_GOR_GL_HEADS");
            ViewBag.lstGLAcHeads = new SelectList(lst.OrderBy(a => a.Name).Where(b => b.Name.Split(new string[] { "::" }, StringSplitOptions.None)[1].StartsWith("1") || b.Name.Split(new string[] { "::" }, StringSplitOptions.None)[1].StartsWith("2") || b.Name.Split(new string[] { "::" }, StringSplitOptions.None)[1].StartsWith("3")), "Id", "Name");
            
            TypeQueryResult lstBankAh = _masterService.GetTypeQueryResult("GROUP_OR_BANK_AH", GroupInfo.GroupID.ToString());
            ViewBag.slBankAh = new SelectList(lstBankAh.OrderBy(a => a.Name), "Id", "Name");

            TypeQueryResult lstEmp = _masterService.GetTypeQueryResult("ACT_EMPLOYEES", GroupInfo.GroupID.ToString());
            ViewBag.slEmp = new SelectList(lstEmp, "Id", "Name", UserInfo.UserID);

            List<GroupMeetingDto> lstGroupMeetings = _groupOtherReceiptService.GetGroupOpenMeetingDates(GroupInfo.GroupID);
            ViewBag.MonthMeetings = new SelectList(lstGroupMeetings, "DisplayMeetingDate", "DisplayMeetingDate");
        }

        [HttpPost]
        public ActionResult CreateOtherReceipt(FormCollection form)
        {
            GroupOtherRecieptDto _groupOtherReceiptDto = new GroupOtherRecieptDto();
            try
            {
                _groupOtherReceiptDto.AccountMasterID = Convert.ToInt32(Request.Form["AccountMasterID"]);
                _groupOtherReceiptDto.TransactionMode = Convert.ToString(Request.Form["TransactionMode"]);

                string tranDate = _groupOtherReceiptDto.TransactionMode == "C" ? Request.Form["TransactionDate"] : Request.Form["txtTransactionDate"];
                DateTime dtTranDate = tranDate.ConvertToDateTime();
                _groupOtherReceiptDto.TransactionDate = dtTranDate;

                _groupOtherReceiptDto.VoucherRefNumber = Convert.ToString(Request.Form["VoucherRefNumber"]);
                _groupOtherReceiptDto.CollectionAgent = Convert.ToInt32(Request.Form["CollectionAgent"]);

                _groupOtherReceiptDto.GLAccountId = Convert.ToInt32(Request.Form["GLAccountId"]);
                _groupOtherReceiptDto.SLAccountId = Convert.ToInt32(Request.Form["SLAccountId"]);
                if (_groupOtherReceiptDto.TransactionMode == "BC")
                {
                    _groupOtherReceiptDto.ChequeNumber = Convert.ToString(Request.Form["ChequeNumber"]);
                    _groupOtherReceiptDto.ChequeDate = Request.Form["ChequeDate"].ConvertToDateTime();
                }
                bool isContra = Convert.ToBoolean((Request.Form["IsContra"]).Split(',')[0]);
                _groupOtherReceiptDto.Amount = Convert.ToDecimal(Request.Form["Amount"]);
                if (_groupOtherReceiptDto.TransactionMode != "C")
                    _groupOtherReceiptDto.BankEntryId = Convert.ToInt32(Request.Form["BankEntryId"]);
                if (_groupOtherReceiptDto.TransactionMode == "C" && isContra == true) {
                    _groupOtherReceiptDto.BankEntryId = Convert.ToInt32(Request.Form["BankEntryId"]);
                }

                _groupOtherReceiptDto.Narration = Convert.ToString(Request.Form["Narration"]);
                _groupOtherReceiptDto.UserId = UserInfo.UserID;
                //Save
                ResultDto resultDto = new ResultDto();
                int GroupId = GroupInfo.GroupID;
                
                resultDto = _groupOtherReceiptService.Insert(_groupOtherReceiptDto, GroupId,isContra);
                _groupOtherReceiptDto.VoucherNumber = resultDto.ObjectCode;
                _groupOtherReceiptDto.AccountMasterID = resultDto.ObjectId;
                ViewBag.LockStatus = GroupInfo.LockStatus;
                
                LoadOtherReceiptDropDowns();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(_groupOtherReceiptDto);
        }

        [HttpPost]
        public JsonResult GetGroupSubLedgerAccountHeadsByGLAHId(string glAHId)
        {
            var slAccountHeads = _groupOtherReceiptService.GetSLAccountHeads(int.Parse(glAHId));
            return Json(new { slAccountHeads = slAccountHeads });
        }

        public ActionResult GroupOtherReceiptLookUp()
        {
            List<GroupOtherReceiptLookUpDto> groupOtherReceiptLookUpDto = _groupOtherReceiptService.GroupOtherReceiptLookUp(UserInfo.UserID,GroupInfo.GroupID);
            return View(groupOtherReceiptLookUpDto);
        }

        public ActionResult DeleteGroupOtherReceipt(string Id)
        {
            int AccountmasterId = DecryptQueryString(Id);

            if (AccountmasterId < 1)
                return RedirectToAction("GroupOtherReceiptLookUp");

            ResultDto resultDto = _groupOtherReceiptService.DeleteGroupOtherReceipt(AccountmasterId, UserInfo.UserID);

            TempData["Result"] = resultDto;

            return RedirectToAction("GroupOtherReceiptLookUp");
        }

        public ActionResult GroupOtherReceiptView(string id)
        {
            int AccountmasterId = DecryptQueryString(id);

            if (AccountmasterId < 1)
                return RedirectToAction("GroupOtherReceiptLookUp");
            GroupOtherReceiptViewDto viewDto = _groupOtherReceiptService.GroupOtherReceiptView(AccountmasterId);
            return View(viewDto);
        }

        #region Group Other ReceiptUpload

        [HttpGet]
        public ActionResult OtherReceiptesUpload()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OtherReceiptesUpload(HttpPostedFileBase file)
        {
            DataSet ds = new DataSet();
            string errMessage = string.Empty;
            if (Request.Files["uploadreceipt"].ContentLength > 0)
            {
                string fileExtension = System.IO.Path.GetExtension(Request.Files["uploadreceipt"].FileName);

                if (fileExtension != ".xls" && fileExtension != ".xlsx")
                {
                    errMessage = "Please upload a valid excel file.";
                }
                else
                {
                    try
                    {
                        ds = ConvertExcelToDataSet();

                        if (ds.Tables.Count < 1)
                        {
                            errMessage = "Please upload a valid excel file.";
                        }
                    }
                    catch (Exception ex)
                    {
                        errMessage = "Please upload a valid excel file.";
                    }
                }
            }
            else
            {
                errMessage = "Please upload a excel file";
            }

            List<UploadErrorEntries> errAl = new List<UploadErrorEntries>();

            if (string.IsNullOrWhiteSpace(errMessage))
            {
                DataTable dtOtherReciept = ds.Tables[0];

                objUploadValidateInfo = _groupOtherReceiptService.GetGroupOtherRecieptValidateInfo(GroupInfo.GroupID);
                bool isValid = ValidateOtherRecieptUpload(dtOtherReciept, ref errAl);
                if (isValid)
                {
                    List<GroupOtherRecieptDto> lstGroupOtherReceiptDto = ConvertGroupOtherRecieptDataTableToList(dtOtherReciept);

                    foreach (var obj in lstGroupOtherReceiptDto)
                    {
                        ResultDto resultDto = new ResultDto();

                        int GroupId = GroupInfo.GroupID;
                        bool isContra = false;
                        resultDto = _groupOtherReceiptService.Insert(obj, GroupId, isContra);

                        obj.VoucherNumber = resultDto.ObjectCode;
                        obj.AccountMasterID = resultDto.ObjectId;
                    }
                }
            }

            bool isSucess = false;
            if (errMessage == string.Empty && errAl.Count < 1)
            {
                isSucess = true;
            }

            ViewBag.isSucess = isSucess;
            ViewBag.ErrorMessage = errMessage;
            ViewBag.DataErrorsList = errAl;

            return View();
        }

        private DataSet ConvertExcelToDataSet()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string fileExtension = System.IO.Path.GetExtension(Request.Files["uploadreceipt"].FileName);

            if (fileExtension == ".xls" || fileExtension == ".xlsx")
            {
                string fileLocation = Server.MapPath("~/Content/") + Request.Files["uploadreceipt"].FileName;
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }
                Request.Files["uploadreceipt"].SaveAs(fileLocation);
                string excelConnectionString = string.Empty;
                excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                //connection String for xls file format.
                if (fileExtension == ".xls")
                {
                    excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                }
                //connection String for xlsx file format.
                else if (fileExtension == ".xlsx")
                {
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                }
                //Create Connection to Excel work book and add oledb namespace
                OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                excelConnection.Open();

                dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int t = 0;
                //excel data saves in temp file here.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[t] = row["TABLE_NAME"].ToString();
                    t++;
                }
                OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                string query = string.Format("Select * from [{0}]", excelSheets[0]);
                using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                {
                    dataAdapter.Fill(ds);
                }
            }

            if (fileExtension.ToString().ToLower().Equals(".xml"))
            {
                string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                if (System.IO.File.Exists(fileLocation))
                {
                    System.IO.File.Delete(fileLocation);
                }

                Request.Files["FileUpload"].SaveAs(fileLocation);
                XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                // DataSet ds = new DataSet();
                ds.ReadXml(xmlreader);
                xmlreader.Close();
            }

            return ds;

            /*for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string conn = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
                SqlConnection con = new SqlConnection(conn);
                string query = "Insert into Person(Name,Email,Mobile) Values('" +
                ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() +
                "','" + ds.Tables[0].Rows[i][2].ToString() + "')";
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.ExecuteNonQuery();
                con.Close();
            }*/
        }

        private List<GroupOtherRecieptDto> ConvertGroupOtherRecieptDataTableToList(DataTable dtOtherReciept)
        {
            List<GroupOtherRecieptDto> lst = new List<GroupOtherRecieptDto>();
            GroupOtherRecieptDto obj = null;
            foreach (DataRow dr in dtOtherReciept.Rows)
            {
                if (String.IsNullOrWhiteSpace(Convert.ToString(dr["TransactionMode"]).Trim()))
                    break;

                obj = new GroupOtherRecieptDto();
                obj.TransactionMode = Convert.ToString(dr["TransactionMode"]).Trim().ToUpper();
                obj.TransactionDate = Convert.ToDateTime(dr["TransactionDate"]);
                obj.VoucherRefNumber = Convert.ToString(dr["VoucherReferenceNumber"]).Trim();

                string strCollectionAgent = Convert.ToString(dr["CollectionAgent"]).Trim().ToUpper();
                obj.CollectionAgent = objUploadValidateInfo.Employees.Find(l => l.EmployeeCode.Trim().ToUpper() == strCollectionAgent).EmployeeID;

                string strSLAccountCode = Convert.ToString(dr["SLAccountCode"]).Trim();
                obj.SLAccountId = objUploadValidateInfo.SlAccountHeads.Find(l => l.AHCode.ToUpper() == strSLAccountCode.ToUpper()).AHId;

                if (obj.TransactionMode == "BC")
                {
                    obj.ChequeNumber = Convert.ToString(dr["ChequeNumber"]).Trim().ToUpper();

                    string strChequeDate = Convert.ToString(dr["ChequeDate"]).Trim();
                    if (!String.IsNullOrWhiteSpace(strChequeDate))
                        obj.ChequeDate = Convert.ToDateTime(strChequeDate);
                }

                if (obj.TransactionMode == "BC" || obj.TransactionMode == "BD")
                {
                    string strBankCode = Convert.ToString(dr["BankCode"]).Trim().ToUpper();
                    obj.BankEntryId = objUploadValidateInfo.GroupBanks.Find(l => l.BankCode.ToUpper() == strBankCode.ToUpper()).BankEntryId;
                }

                string strAmount = Convert.ToString(dr["Amount"]).Trim();
                obj.Amount = Convert.ToDecimal(strAmount);

                obj.Narration = Convert.ToString(dr["Narration"]).Trim();
                obj.UserId = UserInfo.UserID;

                lst.Add(obj);
            }

            return lst;
        }

        private bool ValidateOtherRecieptUpload(DataTable dtOtherReciept, ref List<UploadErrorEntries> errAl)
        {

            errAl = new List<UploadErrorEntries>();
            string[] str = { "TransactionMode", "TransactionDate", "VoucherReferenceNumber", "CollectionAgent", "SLAccountCode", "ChequeNumber", "ChequeDate", "Amount", "BankCode", "Narration" };

            //Columns Validation
            foreach (string columnName in str)
            {
                if (!dtOtherReciept.Columns.Contains(columnName))
                    errAl.Add(new UploadErrorEntries() { ErrorMessage = columnName + " Column does Not Exists in the file", isGeneralError = true });
            }

            if (errAl.Count == 0)
            {
                int RowNumber = 1;
                foreach (DataRow dr in dtOtherReciept.Rows)
                {
                    string strTranscationMode = Convert.ToString(dr["TransactionMode"]).Trim().ToUpper();
                    string strTransactionDate = Convert.ToString(dr["TransactionDate"]).Trim();
                    string strVoucherReferenceNumber = Convert.ToString(dr["VoucherReferenceNumber"]).Trim();
                    string strCollectionAgent = Convert.ToString(dr["CollectionAgent"]).Trim().ToUpper();
                    string strSLAccountCode = Convert.ToString(dr["SLAccountCode"]).Trim();
                    string strChequeNumber = Convert.ToString(dr["ChequeNumber"]).Trim().ToUpper();
                    string strChequeDate = Convert.ToString(dr["ChequeDate"]).Trim();
                    string strAmount = Convert.ToString(dr["Amount"]).Trim();
                    string strBankCode = Convert.ToString(dr["BankCode"]).Trim().ToUpper();
                    string strNarration = Convert.ToString(dr["Narration"]).Trim();

                    if (String.IsNullOrWhiteSpace(strTranscationMode))
                        break;
 
                    if (!(strTranscationMode == "C" || strTranscationMode == "BC" || strTranscationMode == "BD"))
                    {
                        errAl.Add(new UploadErrorEntries() { ErrorMessage = "Please Enter Valid Transaction Mode(C or BC or BD)", RecordNumber = RowNumber });
                    }

                    DateTime dtTranDate = new DateTime();
                    DateTime dtChequeDate = new DateTime();
                    //if (!DateTime.TryParseExact(strTransactionDate, "MM/DD/YYYY", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtTranDate))
                    if (!DateTime.TryParse(strTransactionDate, out dtTranDate))
                    {
                        errAl.Add(new UploadErrorEntries() { ErrorMessage = "Please enter valid Transaction date Format : MM/DD/YYYY", RecordNumber = RowNumber });
                    }

                    if (string.IsNullOrWhiteSpace(strSLAccountCode))
                    {
                        errAl.Add(new UploadErrorEntries() { ErrorMessage = "Please enter valid SL Account Head Code", RecordNumber = RowNumber });
                    }

                    if (strTranscationMode == "BC")
                    {
                        if (string.IsNullOrWhiteSpace(strChequeNumber))
                        {
                            errAl.Add(new UploadErrorEntries() { ErrorMessage = "Please Provide Cheque Number", RecordNumber = RowNumber });
                        }
                        if (!string.IsNullOrWhiteSpace(strChequeDate))
                        {
                            if (!DateTime.TryParse(strChequeDate, out dtChequeDate))
                            // if (!DateTime.TryParseExact(strTransactionDate, "MM/DD/YYYY", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtChequeDate))
                            {
                                errAl.Add(new UploadErrorEntries() { ErrorMessage = "Please enter valid cheque date Format : MM/DD/YYYY", RecordNumber = RowNumber });
                            }
                        }
                    }

                    if (string.IsNullOrWhiteSpace(strAmount))
                    {
                        errAl.Add(new UploadErrorEntries() { ErrorMessage = "Please enter Amount", RecordNumber = RowNumber });
                    }
                    else
                    {
                        int amount = default(int);
                        int.TryParse(strAmount, out amount);
                    }

                    if (strTranscationMode == "BC" || strTranscationMode == "BD")
                    {
                        if (string.IsNullOrWhiteSpace(strBankCode))
                        {
                            errAl.Add(new UploadErrorEntries() { ErrorMessage = "Please enter BankCode", RecordNumber = RowNumber });
                        }
                    }

                    //Data validations againest DB - Start

                    if (strTranscationMode == "C")
                    {
                        if (!objUploadValidateInfo.GroupMeetings.Exists(l => l.MeetingDate.ToDisplayDateFormat() == dtTranDate.ToDisplayDateFormat()))
                        {
                            errAl.Add(new UploadErrorEntries() { ErrorMessage = "As Transaction Mode is Cash(C). Transaction Date should be Meeting date", RecordNumber = RowNumber });
                        }
                    }

                    if (!objUploadValidateInfo.Employees.Exists(l => l.EmployeeCode.Trim().ToUpper() == strCollectionAgent))
                    {
                        errAl.Add(new UploadErrorEntries() { ErrorMessage = "Entered Collection Agent Code Not Exists", RecordNumber = RowNumber });
                    }

                    if (!objUploadValidateInfo.SlAccountHeads.Exists(l => l.AHCode.ToUpper() == strSLAccountCode.ToUpper()))
                    {
                        errAl.Add(new UploadErrorEntries() { ErrorMessage = "Entered SLAccountCode Not Exists", RecordNumber = RowNumber });
                    }
                    if (strTranscationMode == "BC" || strTranscationMode == "BD")
                    {
                        if (!objUploadValidateInfo.GroupBanks.Exists(l => l.BankCode.ToUpper() == strBankCode.ToUpper()))
                        {
                            errAl.Add(new UploadErrorEntries() { ErrorMessage = "Entered BankCode not Exists", RecordNumber = RowNumber });
                        }
                    }
                    RowNumber++;
                    //Data validations againest DB - End
                }
            }
            return errAl.Count == 0;
        }
        #endregion

    }
}