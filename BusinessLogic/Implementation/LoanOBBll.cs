using BusinessEntities;
using DataLogic;
using System.Collections.Generic;
using System.Data;
namespace BusinessLogic
{
    public class LoanOBBll
    {
        public LoanOBDto InsertLoanOB(LoanOBDto loanOBDto)
        {
            for (int i = 0; i < 4; i++)
            {
                LoanOBByLoanDto loanOBByLoanDto = new LoanOBByLoanDto();
                if (i == 0 && loanOBDto.PrincipalOutstanding1 == 0)
                    continue;
                else if (i == 1 && loanOBDto.PrincipalOutstanding2 == 0)
                    continue;
                else if (i == 2 && loanOBDto.PrincipalOutstanding3 == 0)
                    continue;
                else if (i == 3 && loanOBDto.PrincipalOutstanding4 == 0)
                    continue;



                loanOBByLoanDto = ConvertLoanDtoToLoanByDto(loanOBDto, i);
                LoanOBDll dll = new LoanOBDll();

                bool isDisbursed = CheckForDisbursementStart(loanOBByLoanDto.LoanMasrterID);
                if (isDisbursed)
                {

                    switch (i)
                    {
                        case 0: loanOBDto.IsDisbursed1 = true;
                            break;
                        case 1: loanOBDto.IsDisbursed2 = true;
                            break;
                        case 2: loanOBDto.IsDisbursed3 = true;
                            break;
                        case 3: loanOBDto.IsDisbursed4 = true;
                            break;
                        default:
                            break;
                    }

                    continue;

                }

                loanOBByLoanDto = dll.InsertLoanOB(loanOBByLoanDto);

                if (i == 0)
                    loanOBDto.SLAccountNumberAHID1 = loanOBByLoanDto.SLAccountNumberAHID;
                else if (i == 1)
                    loanOBDto.SLAccountNumberAHID2 = loanOBByLoanDto.SLAccountNumberAHID;
                else if (i == 2)
                    loanOBDto.SLAccountNumberAHID3 = loanOBByLoanDto.SLAccountNumberAHID;
                else if (i == 3)
                    loanOBDto.SLAccountNumberAHID4 = loanOBByLoanDto.SLAccountNumberAHID;

                if (i == 0)
                    loanOBDto.LoanMasrterID1 = loanOBByLoanDto.LoanMasrterID;
                else if (i == 1)
                    loanOBDto.LoanMasrterID2 = loanOBByLoanDto.LoanMasrterID;
                else if (i == 2)
                    loanOBDto.LoanMasrterID3 = loanOBByLoanDto.LoanMasrterID;
                else if (i == 3)
                    loanOBDto.LoanMasrterID4 = loanOBByLoanDto.LoanMasrterID;
            }

            return loanOBDto;
        }
        public LoanOBDto GetByID(int GroupID)
        {
            return GetByID(GroupID, 'M');
        }
        public LoanOBDto GetByID(int GroupID, char LoanType)
        {
            LoanOBDto objdto = new LoanOBDto();
            LoanOBDll getdetails = new LoanOBDll();
            if (LoanType == 'G')
                objdto = getdetails.GetByID(GroupID, LoanType);
            else
                objdto = getdetails.GetByID(GroupID);
            return objdto;

        }

        public ResultDto DeleteLoanOBByMemberID(int MemberID)
        {
            ResultDto resultDto = new ResultDto();
            LoanOBDll deleteRecord = new LoanOBDll();
            resultDto = deleteRecord.DeleteLoanOBByMemberID(MemberID);
            return resultDto;

        }

        public LoanOBByLoanDto ConvertLoanDtoToLoanByDto(LoanOBDto loanOBDto, int index)
        {
            LoanOBByLoanDto loanOBByLoanDto = new LoanOBByLoanDto();
            loanOBByLoanDto.GroupId = loanOBDto.GroupId;
            loanOBByLoanDto.LoanType = loanOBDto.LoanType;
            loanOBByLoanDto.MemberId = loanOBDto.MemberID;
            loanOBByLoanDto.UserID = loanOBDto.UserID;
            loanOBByLoanDto.DueDay = loanOBDto.DueDay;
            if (index == 0)
            {
                loanOBByLoanDto.LoanMasrterID = loanOBDto.LoanMasrterID1;
                loanOBByLoanDto.FinalInstallmentDate = loanOBDto.FinalInstallmentDate1;
                loanOBByLoanDto.Fundsource = loanOBDto.Fundsource1;
                loanOBByLoanDto.Interest = loanOBDto.Interest1;
                loanOBByLoanDto.InterestDue = loanOBDto.InterestDue1;
                loanOBByLoanDto.InterestOD = loanOBDto.InterestOD1;
                loanOBByLoanDto.LastPaidDate = loanOBDto.LastPaidDate1;
                loanOBByLoanDto.LoanAmountGiven = loanOBDto.LoanAmountGiven1;
                loanOBByLoanDto.LoanDisbursedDate = loanOBDto.LoanDisbursedDate1;
                loanOBByLoanDto.LoanMasrterID = loanOBDto.LoanMasrterID1;
                loanOBByLoanDto.MonthlyDemand = loanOBDto.MonthlyDemand1;
                loanOBByLoanDto.Period = loanOBDto.Period1;
                loanOBByLoanDto.PrincipalOD = loanOBDto.PrincipalOD1;
                loanOBByLoanDto.PrincipalOutstanding = loanOBDto.PrincipalOutstanding1;
                loanOBByLoanDto.Project = loanOBDto.Project1;
                loanOBByLoanDto.Purpose = loanOBDto.Purpose1;
                loanOBByLoanDto.SLAccountNumber = loanOBDto.SLAccountNumber1;
                loanOBByLoanDto.SLAccountNumberAHID = loanOBDto.SLAccountNumberAHID1;
                loanOBByLoanDto.PrincipalAHID = loanOBDto.PrincipalAHID1;
            }



            else if (index == 1)
            {
                loanOBByLoanDto.LoanMasrterID = loanOBDto.LoanMasrterID2;
                loanOBByLoanDto.FinalInstallmentDate = loanOBDto.FinalInstallmentDate2;
                loanOBByLoanDto.Fundsource = loanOBDto.Fundsource2;
                loanOBByLoanDto.Interest = loanOBDto.Interest2;
                loanOBByLoanDto.InterestDue = loanOBDto.InterestDue2;
                loanOBByLoanDto.InterestOD = loanOBDto.InterestOD2;
                loanOBByLoanDto.LastPaidDate = loanOBDto.LastPaidDate2;
                loanOBByLoanDto.LoanAmountGiven = loanOBDto.LoanAmountGiven2;
                loanOBByLoanDto.LoanDisbursedDate = loanOBDto.LoanDisbursedDate2;
                loanOBByLoanDto.LoanMasrterID = loanOBDto.LoanMasrterID2;
                loanOBByLoanDto.MonthlyDemand = loanOBDto.MonthlyDemand2;
                loanOBByLoanDto.Period = loanOBDto.Period2;
                loanOBByLoanDto.PrincipalOD = loanOBDto.PrincipalOD2;
                loanOBByLoanDto.PrincipalOutstanding = loanOBDto.PrincipalOutstanding2;
                loanOBByLoanDto.Project = loanOBDto.Project2;
                loanOBByLoanDto.Purpose = loanOBDto.Purpose2;
                loanOBByLoanDto.SLAccountNumber = loanOBDto.SLAccountNumber2;
                loanOBByLoanDto.SLAccountNumberAHID = loanOBDto.SLAccountNumberAHID2;
                loanOBByLoanDto.PrincipalAHID = loanOBDto.PrincipalAHID2;


            }

            else if (index == 2)
            {
                loanOBByLoanDto.LoanMasrterID = loanOBDto.LoanMasrterID3;
                loanOBByLoanDto.FinalInstallmentDate = loanOBDto.FinalInstallmentDate3;
                loanOBByLoanDto.Fundsource = loanOBDto.Fundsource3;
                loanOBByLoanDto.Interest = loanOBDto.Interest3;
                loanOBByLoanDto.InterestDue = loanOBDto.InterestDue3;
                loanOBByLoanDto.InterestOD = loanOBDto.InterestOD3;
                loanOBByLoanDto.LastPaidDate = loanOBDto.LastPaidDate3;
                loanOBByLoanDto.LoanAmountGiven = loanOBDto.LoanAmountGiven3;
                loanOBByLoanDto.LoanDisbursedDate = loanOBDto.LoanDisbursedDate3;
                loanOBByLoanDto.LoanMasrterID = loanOBDto.LoanMasrterID3;
                loanOBByLoanDto.MonthlyDemand = loanOBDto.MonthlyDemand3;
                loanOBByLoanDto.Period = loanOBDto.Period3;
                loanOBByLoanDto.PrincipalOD = loanOBDto.PrincipalOD3;
                loanOBByLoanDto.PrincipalOutstanding = loanOBDto.PrincipalOutstanding3;
                loanOBByLoanDto.Project = loanOBDto.Project3;
                loanOBByLoanDto.Purpose = loanOBDto.Purpose3;
                loanOBByLoanDto.SLAccountNumber = loanOBDto.SLAccountNumber3;
                loanOBByLoanDto.SLAccountNumberAHID = loanOBDto.SLAccountNumberAHID3;
                loanOBByLoanDto.PrincipalAHID = loanOBDto.PrincipalAHID2;



            }

            else if (index == 3)
            {
                loanOBByLoanDto.LoanMasrterID = loanOBDto.LoanMasrterID4;
                loanOBByLoanDto.FinalInstallmentDate = loanOBDto.FinalInstallmentDate4;
                loanOBByLoanDto.Fundsource = loanOBDto.Fundsource4;
                loanOBByLoanDto.Interest = loanOBDto.Interest4;
                loanOBByLoanDto.InterestDue = loanOBDto.InterestDue4;
                loanOBByLoanDto.InterestOD = loanOBDto.InterestOD4;
                loanOBByLoanDto.LastPaidDate = loanOBDto.LastPaidDate4;
                loanOBByLoanDto.LoanAmountGiven = loanOBDto.LoanAmountGiven4;
                loanOBByLoanDto.LoanDisbursedDate = loanOBDto.LoanDisbursedDate4;
                loanOBByLoanDto.LoanMasrterID = loanOBDto.LoanMasrterID4;
                loanOBByLoanDto.MonthlyDemand = loanOBDto.MonthlyDemand4;
                loanOBByLoanDto.Period = loanOBDto.Period4;
                loanOBByLoanDto.PrincipalOD = loanOBDto.PrincipalOD4;
                loanOBByLoanDto.PrincipalOutstanding = loanOBDto.PrincipalOutstanding4;
                loanOBByLoanDto.Project = loanOBDto.Project4;
                loanOBByLoanDto.Purpose = loanOBDto.Purpose4;
                loanOBByLoanDto.SLAccountNumber = loanOBDto.SLAccountNumber4;
                loanOBByLoanDto.SLAccountNumberAHID = loanOBDto.SLAccountNumberAHID4;
                loanOBByLoanDto.PrincipalAHID = loanOBDto.PrincipalAHID4;



            }

            return loanOBByLoanDto;
        }
        public bool CheckForDisbursementStart(int LoanMasterId)
        {
            bool isDisbursed = false;
            LoanDisbursementDataAccess dl = new LoanDisbursementDataAccess();
            isDisbursed = dl.CheckLoanDisbursed(LoanMasterId);

            return isDisbursed;
        }
        public List<LoanOBLookup> LoanOBLookUp(char LoanType)
        {
            return new LoanOBDll().LoanOBLookUp(LoanType);
        }
        public List<LoanOBLookup> LoanOBLookUpList(char LoanType, int GroupID)
        {
            return new LoanOBDll().LoanOBLookUpList(LoanType, GroupID);
        }

        public DataSet LoanOBLookUpTable(char LoanType, int GroupID)
        {
            return new LoanOBDll().LoanOBLookUpTable(LoanType, GroupID);
        }
    }
}
