using AutoMapper;
using BusinessEntities;
using BusinessLogic.AutoMapper;
using MFIEntityFrameWork;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace BusinessLogic
{
    public class ClusterService 
    {
        #region Global Variables
        private readonly MFISDBContext _dbContext;
        private readonly CommonService _commonService;
        public ClusterService()
        {
            _dbContext = new MFISDBContext();
            _commonService = new CommonService();
            AutoMapperEntityConfiguration.Configure();
        }
        #endregion

        public List<ClusterDto> GetAll()
        {
            var lstClusterDto = new List<ClusterDto>();
            var lstuspClusterGetAll_Result = _dbContext.uspClusterGetAll().ToList();
            foreach (var cluster in lstuspClusterGetAll_Result)
            {
                ClusterDto objCluster = Mapper.Map<uspClusterGetAll_Result, ClusterDto>(cluster);
                lstClusterDto.Add(objCluster);
            }

            return lstClusterDto;
        }

        public List<ClusterLookupDto> Lookup()
        {
            var lstClusterLookupDto = new List<ClusterLookupDto>();
            var lstuspClusterLookup_Result = _dbContext.uspClusterLookup().ToList();
            foreach (var cluster in lstuspClusterLookup_Result)
            {
                ClusterLookupDto lookup = Mapper.Map<uspClusterLookup_Result, ClusterLookupDto>(cluster);
                lstClusterLookupDto.Add(lookup);
            }

            return lstClusterLookupDto;
        }
        public ClusterDto GetByID(int clusterID)
        {
            var objuspClusterGetByIdResult = _dbContext.uspClusterGetByID(clusterID).ToList().FirstOrDefault();

            ClusterDto objClusterDto = AutoMapperEntityConfiguration.Cast<ClusterDto>(objuspClusterGetByIdResult);

            return objClusterDto;
        }
        public ResultDto Insert(ClusterDto clusterDto)
        {
            return InsertUpdate(clusterDto);
        }
        public ResultDto Update(ClusterDto clusterDto)
        {
            return InsertUpdate(clusterDto);
        }
        public ResultDto InsertUpdate(ClusterDto cluster)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "cluster";
            try
            {
                ObjectParameter prmClusterID = new ObjectParameter("ClusterID", cluster.ClusterID);
                ObjectParameter prmClusterCode = new ObjectParameter("ClusterCode", string.Empty);

                int effectedRow = _dbContext.uspClusterInsertUpdate(prmClusterID, cluster.ClusterName, cluster.TEClusterName, cluster.StartDate, cluster.MandalID,
                    cluster.BranchID, cluster.Phone, cluster.Address, cluster.Leader, cluster.LeaderFromDate, cluster.UserID, prmClusterCode);

                resultDto.ObjectId = (int)prmClusterID.Value;
                resultDto.ObjectCode = string.IsNullOrEmpty((string)prmClusterCode.Value) ? cluster.ClusterCode : (string)prmClusterCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully with code : {1}", obectName, resultDto.ObjectCode);
                else if (resultDto.ObjectId == -1)
                    resultDto.Message = string.Format("Error occured while generating {0} code", obectName);
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
        public List<SelectListDto> GetClusterSelectList()
        {
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();
            List<uspClusterGetAll_Result> lstCluster = _dbContext.uspClusterGetAll().ToList();
            foreach (var cluster in lstCluster)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = cluster.ClusterID,
                    Text = cluster.ClusterName
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }
        public List<SelectListDto> GetClusterByMandalID(int Id)
        {
            var lstClusters = GetAll().FindAll(l => l.MandalID == Id);

            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();

            lstClusters.ForEach(cluster =>
            {
                lstSelectListDto.Add(new SelectListDto()
                {
                    ID = cluster.ClusterID,
                    Text = cluster.ClusterName
                });
            });

            return lstSelectListDto;
        }
        public ResultDto Delete(int ClusterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "Cluster";

            try
            {
                ObjectParameter prmResultID = new ObjectParameter("Result", resultDto.Result);
                ObjectParameter prmMessage = new ObjectParameter("Message", string.Empty);

                _dbContext.uspClusterDelete(ClusterId, userId, prmResultID, prmMessage);

                resultDto.Result = (bool)prmResultID.Value;
                resultDto.Message = (string)prmMessage.Value;
                resultDto.ObjectId = (int)ClusterId;                

            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while deleting the {0} details", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }

        public ResultDto ChangeStatus(int clusterId, int userId)
        {
            ResultDto resultDto = new ResultDto();
            string obectName = "cluster";
            string statusCode = string.Empty;
            try
            {
                ObjectParameter prmClusterId = new ObjectParameter("ClusterID", clusterId);
                ObjectParameter prmClusterCode = new ObjectParameter("ClusterCode", string.Empty);
                ObjectParameter prmStatusCode = new ObjectParameter("StatusCode", string.Empty);

                int effectedCount = _dbContext.uspClusterChangeStatus(prmClusterId, prmClusterCode, prmStatusCode,userId);

                resultDto.ObjectId = (int)prmClusterId.Value;
                resultDto.ObjectCode = (string)prmClusterCode.Value;
                statusCode = (string)prmStatusCode.Value;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} : {1} details {2} successfully", obectName, resultDto.ObjectCode, statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated");
                else
                    resultDto.Message = string.Format("Error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", obectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while {0} {1} details", statusCode == Constants.StatusCodes.Active ? "activated" : "inactivated", obectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;
        }
        public ClusterViewDto GetViewByID(int clusterId)
        {
            var prmClusterId = new ObjectParameter("ClusterID", clusterId);

            var results = new MFISDBContext()
                .MultipleResults(CustomProcNames.uspClusterGetViewByID, prmClusterId)
                .With<ClusterViewDto>()
                .Execute();

            var clusterViewDto = (results[0] as List<ClusterViewDto>)[0];
            return clusterViewDto;
        }

        public List<SelectListDto> GetClusterSelectListByBranchID(int Id)
        {

            var lstClusters = GetAll().FindAll(l => l.BranchID == Id);

            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();

            lstClusters.ForEach(cluster =>
            {
                lstSelectListDto.Add(new SelectListDto()
                {
                    ID = cluster.ClusterID,
                    Text = cluster.ClusterName
                });
            });

            return lstSelectListDto;
        }      
    }
}