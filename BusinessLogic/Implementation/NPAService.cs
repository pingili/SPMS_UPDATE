namespace BusinessLogic
{
    using BusinessEntities;
    using BusinessLogic.AutoMapper;
    //using BusinessLogic.Interface;
    using MFIEntityFrameWork;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.Objects;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml.Serialization;

    public class NPAService 
    {

        #region Global Variables
        private readonly MFISDBContext _dbContext;

        public NPAService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        public List<NPADto> GetNPADetails()
        {
            var lstNpaDetails = _dbContext.uspGetNPADetails().ToList();

            List<NPADto> lstNpaDto = new List<NPADto>();
            foreach (var npa in lstNpaDetails)
            {
                lstNpaDto.Add(new NPADto()
                {
                    OverDuePeriod = npa.OverDuePeriod,
                    OverDuePeriodID = npa.OverDuePeriodID,
                    Rate = npa.Rate.HasValue ? npa.Rate.Value : default(byte)
                });

            }
            return lstNpaDto;
        }

        public ResultDto InsertUpdateNPARecords(List<NPADto> lstNpaDto, int userID)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "NPA";
            try
            {
                ObjectParameter prmRetVal = new ObjectParameter("RetVal", default(int));
                string npaXML = CoreComponents.CommonMethods.SerializeListDto<List<NPADto>>(lstNpaDto);
                int effectedCount = _dbContext.uspManageNPA(npaXML, userID, prmRetVal);

                resultDto.ObjectId = (int)prmRetVal.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully.", obectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", obectName);
                resultDto.ObjectId = -98;
            }
            return resultDto;
        }
    }
}