using BusinessEntities;
using DataLogic.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementation
{
    public class MasterService
    {
        #region GetTypeQueryResult - Overloaded Methods
        /// <summary>
        /// Used to get dynamic result of typed quries in db table based on type code
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="param1"></param>
        /// <param name="param2"></param>
        /// <param name="param3"></param>
        /// <param name="param4"></param>
        /// <param name="param5"></param>
        /// <param name="param6"></param>
        /// <returns></returns>
        public TypeQueryResult GetTypeQueryResult(string typeCode, string param1 = null, string param2 = null, string param3 = null, string param4 = null, string param5 = null, string param6 = null)
        {
            return _objMasterDal.GetTypeQueryResult(typeCode, param1, param2, param3, param4, param5, param6);
        }
        #endregion

        /// <summary>
        /// By passing ref code to get Dynamic dropdown values
        /// </summary>
        /// <param name="refCode"></param>
        /// <returns></returns>

        public CurrentUser GetLoginMasterInfo(int userId, bool isFederation)
        {
            return _objMasterDal.GetLoginMasterInfo(userId, isFederation);
        }

        public List<SelectListDto> GetMasterDropDownResult(string refCode)
        {
            return _objMasterDal.GetMasterDropDownResult(refCode);
        }

        private MasterDal _objMasterDal
        {
            get
            {
                return new MasterDal();
            }
        }
    }
}
