using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BusinessLogic.Interface;
using BusinessEntities;
using MFIEntityFrameWork;
using BusinessLogic.AutoMapper;
using AutoMapper;
using System.Data.Entity.Core.Objects;

namespace BusinessLogic
{
    public class StateService 
    {
        private readonly MFISDBContext _dbContext;

        public StateService()
        {
            _dbContext = new MFISDBContext();
            AutoMapperEntityConfiguration.Configure();
        }

        
        public List<StateDto> GetAll()
        {
            List<StateDto> lstStatesDto = new List<StateDto>();
            List<uspStateGetAll_Result> lstuspStateGetAll_Result = _dbContext.uspStateGetAll().ToList();
            lstStatesDto = Mapper.Map<List<uspStateGetAll_Result>, List<StateDto>>(lstuspStateGetAll_Result);
            return lstStatesDto;
        }

        public StateDto GetByID(int stateID)
        {
            StateDto stateDto = new StateDto();
            List<uspStateGetByStateId_Result> lstuspStateGetByStateId_Result = _dbContext.uspStateGetByStateId(stateID).ToList();
            stateDto = Mapper.Map<uspStateGetByStateId_Result, StateDto>(lstuspStateGetByStateId_Result.FirstOrDefault());
            return stateDto;
        }

        public bool Insert(StateDto state)
        {
            return InsertUpdateState(state);
        }

        public bool Update(StateDto state)
        {
            throw new NotImplementedException();
        }

        public List<SelectListDto> GetStateSelectList()
        {
            
            List<SelectListDto> lstSelectListDto = new List<SelectListDto>();

            List<uspStateGetAll_Result> lstuspStateGetAll_Result = _dbContext.uspStateGetAll().ToList();
            foreach (var state in lstuspStateGetAll_Result)
            {
                SelectListDto objSelectListDto = new SelectListDto()
                {
                    ID = state.StateID,
                    Text = state.State
                };
                lstSelectListDto.Add(objSelectListDto);
            }
            return lstSelectListDto;
        }

        private bool InsertUpdateState(StateDto stateDto)
        {
            bool isSuccess = true;
            try
            {
                ObjectParameter paramStateCode = new ObjectParameter("stateCode", "");
                int effectcount = _dbContext.uspStateInsert
                    (stateDto.StateID, stateDto.State, stateDto.TEStateName, stateDto.IsActive, stateDto.UserID, DateTime.Now, paramStateCode);

                if (effectcount > 0)
                    isSuccess = true;
                else
                    isSuccess = false;
            }
            catch (Exception)
            {
                isSuccess = false;
            }
            return isSuccess;
        }



    }
}
