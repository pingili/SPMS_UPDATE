using BusinessEntities;
using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementation
{
   public class FederationLoanApplicationService
    {
        FederationLoanApplicationDal fedLoandal = new FederationLoanApplicationDal();
        public FederationLoanApplicationDto GetGroupSLAccountByFedSLAcount(string Id)
        {
            FederationLoanApplicationDto dto = fedLoandal.GetGroupSLAccountByFedSLAcount(Id);
            return dto;
        }

        public ResultDto CreateFedLoanAppln(FederationLoanApplicationDto objDto)
        {
            ResultDto resultDto = new ResultDto();
            resultDto= fedLoandal.CreateFedLoanAppln(objDto);
            return resultDto;
        }

        public int GetMemberCountByGroupId(int Id)
        { 
         int i= fedLoandal.GetMemberCountByGroupId(Id);
            return i;
        }
    }
}
