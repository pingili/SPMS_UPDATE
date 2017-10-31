using BlackBeltCoder;
using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreComponents;

namespace DataLogic
{
    public class ReceiptDll
    {
        /* StartMember Receipt && Group  Receipt AccountDetails Start---- 07/17/2016  */

        public List<ReceiptTranscationDto> GetMemberReceiptAccountdetails(int memberID, string transactionDate)
        {

            List<ReceiptTranscationDto> lstGroupReceiptDto = new List<ReceiptTranscationDto>();
            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[1];
                parms[0] = new SqlParameter("@MemberID", memberID);
                parms[0].SqlDbType = SqlDbType.Int;

                //parms[1] = new SqlParameter("@TransactionDate", transactionDate);
                //parms[1].SqlDbType = SqlDbType.VarChar;

                SqlDataReader sdr = obj.ExecDataReaderProc("uspGetMemberReceiptTemplate", parms);
                while (sdr.Read())
                {

                    ReceiptTranscationDto objLoanOBLookup = new ReceiptTranscationDto();
                    objLoanOBLookup.AHID = Convert.ToInt32(sdr["AHID"]);
                    //objLoanOBLookup.ParentAHID = Convert.ToInt32(sdr["ParentAHID"]);
                    objLoanOBLookup.AHCode = Convert.ToString(sdr["AHCode"]);
                    objLoanOBLookup.AHName = sdr["AccountHead"].ToString();
                    objLoanOBLookup.SLAccName = sdr["SLAccountHead"].ToString();
                    objLoanOBLookup.OpeningBalance = Convert.ToDecimal(sdr["OpeningBalance"]);

                    lstGroupReceiptDto.Add(objLoanOBLookup);
                }
            }
            catch (Exception ex)
            {

            }
            return lstGroupReceiptDto;
        }

        public List<ReceiptTranscationDto> GetGroupReceiptAccountdetails(int groupID, string transactionDate)
        {
            List<ReceiptTranscationDto> lstGroupReceiptDto = new List<ReceiptTranscationDto>();
            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[2];
                parms[0] = new SqlParameter("@GroupID", groupID);
                parms[0].SqlDbType = SqlDbType.Int;

                parms[1] = new SqlParameter("@TransactionDate", transactionDate);
                parms[1].SqlDbType = SqlDbType.VarChar;


                SqlDataReader sdr = obj.ExecDataReaderProc("uspGetGroupReceiptTemplate", parms);
                while (sdr.Read())
                {
                    ReceiptTranscationDto objLoanOBLookup = new ReceiptTranscationDto();
                    objLoanOBLookup.AHID = Convert.ToInt32(sdr["AHID"]);
                    objLoanOBLookup.AHCode = Convert.ToString(sdr["AHCode"]);
                    objLoanOBLookup.AHName = sdr["AHName"].ToString();
                    objLoanOBLookup.ClosingBalance = Convert.ToDecimal(sdr["ClosingBalance"]);
                    objLoanOBLookup.SLAcNo = Convert.ToString(sdr["SLAccount"]);

                    lstGroupReceiptDto.Add(objLoanOBLookup);
                }
            }
            catch (Exception ex)
            {

            }
            return lstGroupReceiptDto;
        }
        /* End Member Receipt && Group  Receipt AccountDetails Start---- 07/17/2016  */


        public ResultDto MemberReceiptInsertUpdate(ReceiptMasterDto groupReceiptdto)
        {
            ResultDto result = new ResultDto();
            string receiptxml = CommonMethods.SerializeListDto<List<ReceiptTranscationDto>>(groupReceiptdto.lstGroupReceiptTranscationDto);
            List<ReceiptTranscationDto> lstGroupReceiptDto = new List<ReceiptTranscationDto>();
            try
            {
                AdoHelper obj = new AdoHelper();

                SqlParameter[] parms = new SqlParameter[]{
                 new SqlParameter ("@accountMasterID",groupReceiptdto.AccountMasterID){SqlDbType=SqlDbType.Int,Direction=ParameterDirection.Output},
                 new SqlParameter("@TransactionDate", groupReceiptdto.TransactionDate){SqlDbType= SqlDbType.DateTime},
                 new SqlParameter ("@voucherNumber",groupReceiptdto.VoucherNumber){SqlDbType=SqlDbType.VarChar,Direction=ParameterDirection.Output,Size=250},
                 new SqlParameter("@VoucherRefNumber", groupReceiptdto.VoucherRefNumber){ SqlDbType= SqlDbType.VarChar},
                 new SqlParameter("@CodeSno", groupReceiptdto.CodeSno){ SqlDbType= SqlDbType.Int},
                 new SqlParameter("@PartyName", groupReceiptdto.PartyName){ SqlDbType= SqlDbType.VarChar},
                 new SqlParameter("@EmployeeID", groupReceiptdto.EmployeeID){ SqlDbType= SqlDbType.Int},
                 new SqlParameter("@AHID",groupReceiptdto.AHID){ SqlDbType= SqlDbType.Int},
                 new SqlParameter("@SubHeadID", groupReceiptdto.SubHeadID){ SqlDbType= SqlDbType.Int},
                 new SqlParameter("@TransactionType", groupReceiptdto.TransactionType){ SqlDbType= SqlDbType.Int},
                 new SqlParameter("@Amount", groupReceiptdto.Amount){ SqlDbType= SqlDbType.Decimal},
                 new SqlParameter("@TransactionMode", groupReceiptdto.TransactionMode){ SqlDbType= SqlDbType.VarChar},
                 new SqlParameter("@ChequeNumber", groupReceiptdto.ChequeNumber){ SqlDbType= SqlDbType.VarChar},
                 new SqlParameter("@ChequeDate", groupReceiptdto.ChequeDate){ SqlDbType= SqlDbType.DateTime},
                 new SqlParameter("@BankAccount", groupReceiptdto.BankAccount){ SqlDbType= SqlDbType.Int},
                 new SqlParameter("@Narration", groupReceiptdto.Narration){ SqlDbType= SqlDbType.VarChar},
                 new SqlParameter("@TranscationXML",receiptxml){ SqlDbType= SqlDbType.Xml},
                 new SqlParameter("@IsGroup", groupReceiptdto.IsGroup){ SqlDbType= SqlDbType.Bit},
                 new SqlParameter("@GroupID", groupReceiptdto.GroupID){ SqlDbType= SqlDbType.Int},
                 new SqlParameter("@MemberID",groupReceiptdto.MemberId){ SqlDbType= SqlDbType.Int},
                 new SqlParameter("@UserId", groupReceiptdto.UserID){ SqlDbType= SqlDbType.Int}
                };
                SqlDataReader sdr = obj.ExecDataReaderProc("uspMemberReceiptInsertUpdate", parms);
                while (sdr.Read())
                {
                    result.ObjectId = Convert.ToInt32(sdr["accountMasterID"]);
                    result.ObjectCode = Convert.ToString(sdr["voucherNumber"]);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

    }
}
