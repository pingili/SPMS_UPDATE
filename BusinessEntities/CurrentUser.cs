using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
    public class CurrentUser
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string RoleCode { get; set; }
        public string Photo { get; set; }
        public List<ModuleDto> modules { get; set; }
        public List<ModuleActionDto> moduleActions { get; set; }
        public DateTime FinancialYearBegin { get; set; }
        public DateTime FinancialYearEnd { get; set; }
    }

    public class UploadErrorEntries
    {
        public int RecordNumber { get; set; }
        public string ErrorMessage { get; set; }
        public bool isGeneralError { get; set; }
    }
}
