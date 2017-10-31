using BusinessEntities;
using DataLogic;
using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class DepositOBBll
    {
        public DepositOBDto InsertLoanOB(DepositOBDto depositOBDto)
        {
            for (int i = 0; i < 4; i++)
            {
                DepositOBBySlAccountDto depositOBBySlAccountDto = new DepositOBBySlAccountDto();
                if (i == 0 && depositOBDto.DepositBalance1 == 0)
                    break;
                else if (i == 1 && depositOBDto.DepositBalance2 == 0)
                    break;
                else if (i == 2 && depositOBDto.DepositBalance3 == 0)
                    break;
                else if (i == 3 && depositOBDto.DepositBalance4 == 0)
                    break;

                depositOBBySlAccountDto = ConvertDepositDtoToDepositBySlAccountDto(depositOBDto, i);
                DepositOBDll dll = new DepositOBDll();
                depositOBBySlAccountDto = dll.InsertDepositOB(depositOBBySlAccountDto);

                if (i == 0)
                    depositOBDto.SLAccountNumberAHID1 = depositOBBySlAccountDto.SLAccountNumberAHID;
                else if (i == 1)
                    depositOBDto.SLAccountNumberAHID2 = depositOBBySlAccountDto.SLAccountNumberAHID;
                else if (i == 2)
                    depositOBDto.SLAccountNumberAHID3 = depositOBBySlAccountDto.SLAccountNumberAHID;
                else if (i == 3)
                    depositOBDto.SLAccountNumberAHID4 = depositOBBySlAccountDto.SLAccountNumberAHID;

                if (i == 0)
                    depositOBDto.Id1 = depositOBBySlAccountDto.ID;
                else if (i == 1)
                    depositOBDto.Id2 = depositOBBySlAccountDto.ID;
                else if (i == 2)
                    depositOBDto.Id3 = depositOBBySlAccountDto.ID;
                else if (i == 3)
                    depositOBDto.Id4 = depositOBBySlAccountDto.ID;
            }

            return depositOBDto;
        }
        public DepositOBBySlAccountDto ConvertDepositDtoToDepositBySlAccountDto(DepositOBDto loanOBDto, int index)
        {
            DepositOBBySlAccountDto loanOBByLoanDto = new DepositOBBySlAccountDto();
            loanOBByLoanDto.GroupId = loanOBDto.GroupId;
            loanOBByLoanDto.MemberId = loanOBDto.MemberId;
            loanOBByLoanDto.UserID = loanOBDto.UserID;
            if (loanOBDto.MemberId > 0)
                loanOBByLoanDto.IsMemberDeposit = true;
            else {
                loanOBByLoanDto.IsMemberDeposit = false;
            
            }
            if (index == 0)
            {
                loanOBByLoanDto.ID = loanOBDto.Id1;
                loanOBByLoanDto.DemandAmountPerMonth= loanOBDto.DemandAmountPerMonth1;
                loanOBByLoanDto.DepositAmount = loanOBDto.DepositBalance1;
                loanOBByLoanDto.DepositDate= loanOBDto.DepositDate1;
                loanOBByLoanDto.InterestAcmount= loanOBDto.IneterestDue1;
                loanOBByLoanDto.InterestMasterID= loanOBDto.Interest1;
                loanOBByLoanDto.LastPaidDate = loanOBDto.LastPaidDate1;
                loanOBByLoanDto.MeturityDate= loanOBDto.MeturityDate1;
                loanOBByLoanDto.Period = loanOBDto.Period1;
                loanOBByLoanDto.SLAccountNumberAHID= loanOBDto.SLAccountNumberAHID1;
                loanOBByLoanDto.SLAccountNumber = loanOBDto.SLAccountNumber1;

            }

            else if (index == 1)
            {
                loanOBByLoanDto.ID = loanOBDto.Id2;
                loanOBByLoanDto.DemandAmountPerMonth = loanOBDto.DemandAmountPerMonth2;
                loanOBByLoanDto.DepositAmount = loanOBDto.DepositBalance2;
                loanOBByLoanDto.DepositDate = loanOBDto.DepositDate2;
                loanOBByLoanDto.InterestAcmount = loanOBDto.IneterestDue2;
                loanOBByLoanDto.InterestMasterID = loanOBDto.Interest2;
                loanOBByLoanDto.LastPaidDate = loanOBDto.LastPaidDate2;
                loanOBByLoanDto.MeturityDate = loanOBDto.MeturityDate2;
                loanOBByLoanDto.Period = loanOBDto.Period2;
                loanOBByLoanDto.SLAccountNumberAHID = loanOBDto.SLAccountNumberAHID2;
                loanOBByLoanDto.SLAccountNumber = loanOBDto.SLAccountNumber2;
            }

            else if (index == 2)
            {

                loanOBByLoanDto.ID = loanOBDto.Id3;
                loanOBByLoanDto.DemandAmountPerMonth = loanOBDto.DemandAmountPerMonth3;
                loanOBByLoanDto.DepositAmount = loanOBDto.DepositBalance3;
                loanOBByLoanDto.DepositDate = loanOBDto.DepositDate3;
                loanOBByLoanDto.InterestAcmount = loanOBDto.IneterestDue3;
                loanOBByLoanDto.InterestMasterID = loanOBDto.Interest3;
                loanOBByLoanDto.LastPaidDate = loanOBDto.LastPaidDate3;
                loanOBByLoanDto.MeturityDate = loanOBDto.MeturityDate3;
                loanOBByLoanDto.Period = loanOBDto.Period3;
                loanOBByLoanDto.SLAccountNumberAHID = loanOBDto.SLAccountNumberAHID3;
                loanOBByLoanDto.SLAccountNumber = loanOBDto.SLAccountNumber3;
            }

            else if (index == 3)
            {
                loanOBByLoanDto.ID = loanOBDto.Id4;
                loanOBByLoanDto.DemandAmountPerMonth = loanOBDto.DemandAmountPerMonth4;
                loanOBByLoanDto.DepositAmount = loanOBDto.DepositBalance4;
                loanOBByLoanDto.DepositDate = loanOBDto.DepositDate4;
                loanOBByLoanDto.InterestAcmount = loanOBDto.IneterestDue4;
                loanOBByLoanDto.InterestMasterID = loanOBDto.Interest4;
                loanOBByLoanDto.LastPaidDate = loanOBDto.LastPaidDate4;
                loanOBByLoanDto.MeturityDate = loanOBDto.MeturityDate4;
                loanOBByLoanDto.Period = loanOBDto.Period4;
             //   loanOBByLoanDto.SLAccountAHID = loanOBDto.SLAccountNumberAHID4;
                loanOBByLoanDto.SLAccountNumberAHID = loanOBDto.SLAccountNumberAHID4;
                loanOBByLoanDto.SLAccountNumber = loanOBDto.SLAccountNumber4;
            }

            return loanOBByLoanDto;
        }

        public List<DepositOBLookup> DepositOBLookUpList(bool Type, int Groupid)
        {
            return new DepositOBDll().DepositOBLookUpList(Type, Groupid);
        }

        public DataSet DepositOBLookUpTable(bool Type, int Groupid)
        {
            return new DepositOBDll().DepositOBLookUpTable(Type, Groupid);
        }

        public DepositOBDto GetByID(int GroupID)
        {
            DepositOBDto objdto = new DepositOBDto();
            DepositOBDll getdetails = new DepositOBDll();
            objdto = getdetails.GetByID(GroupID);
            return objdto;
        }
        public DepositOBDto GetByMemberID(int MemberId)
        {
            DepositOBDto objdto = new DepositOBDto();
            DepositOBDll getdetails = new DepositOBDll();
            objdto = getdetails.GetByMemberID(MemberId);
            return objdto;
        }
    }
}
