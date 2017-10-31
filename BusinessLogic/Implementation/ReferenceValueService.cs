using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
//using BusinessLogic.Interface;
using MFIEntityFrameWork;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLogic
{
    public class ReferenceValueService 
    {
        private readonly MFISDBContext _dbContext;

        public ReferenceValueService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        public List<ReferenceValueDto> GetByRefMasterCode(string refMasterCode)
        {
            var listRefValues = new List<ReferenceValueDto>();
            var lstuspRefValuesByRefMasterCodeResult = _dbContext.uspRefValuesByRefMasterCode(refMasterCode).ToList();
            foreach (var refvalueProc in lstuspRefValuesByRefMasterCodeResult)
            {
                var refvalue = Mapper.Map<uspRefValuesByRefMasterCode_Result,ReferenceValueDto>(refvalueProc);
                listRefValues.Add(refvalue);
            }
            return listRefValues;
        }

        
    }
}
