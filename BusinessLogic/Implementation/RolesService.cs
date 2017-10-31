//using BusinessLogic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessEntities;
using MFIEntityFrameWork;
using BusinessLogic.AutoMapper;
using System.Data.Entity.Core.Objects;
using AutoMapper;
using DataLogic;

namespace BusinessLogic
{
    public class RolesService 
    {
        #region Global Variables
        private readonly RoleDal _roleDal;
        public RolesService()
        {
            _roleDal = new RoleDal();
        }
        #endregion

      

        
        private ResultDto InsertUpdate(RoleDto role)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Role";
            try
            {
                int roleId = role.RoleId;

                int rowAffected = _roleDal.InsertUpdate(ref roleId, role.RoleName, role.RoleCode);

                resultDto.ObjectId = roleId;

                if (resultDto.ObjectId > 0)
                    resultDto.Message = string.Format("{0} details saved successfully", objectName);
                else
                    resultDto.Message = string.Format("Error occured while saving {0} details", objectName);
            }
            catch (Exception)
            {
                resultDto.Message = string.Format("Service layer error occured while saving the {0} details", objectName);
                resultDto.ObjectId = -98;
            }

            return resultDto;
        }


        public List<RoleDto> GetLookUp()
        {
            List<RoleDto> lstDto = _roleDal.GetLookUp();

            return lstDto;
        }


        public ResultDto Insert(RoleDto Role)
        {
            return InsertUpdate(Role);
        }

        public ResultDto Update(RoleDto Role)
        {
            return InsertUpdate(Role);
            
        }

        public RoleDto GetById(int? ID)
        {
            RoleDto lstrole = _roleDal.GetById(ID);
            return lstrole;
        }


        public ResultDto RoleDelete(int id)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Roles";
            try
            {
                int effectedCount = _roleDal.DeleteRole(ref id);
                resultDto.ObjectId = id;
                if (resultDto.ObjectId > 0)
                {
                    resultDto.Message = string.Format("{0}:details Deleted successfully", objectName);
                }
                else
                {
                    resultDto.Message = string.Format("Error occured while deleting {0} Details", objectName);
                }

            }
            catch (Exception)
            {
                resultDto.Message = string.Format("service layer error occured while deleting{0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;

        }


        public ResultDto ChangeStatus(int id)
        {
            ResultDto resultDto = new ResultDto();
            string objectName = "Roles";
            string StatusCode = string.Empty;
            try
            {
                int effectedCount = _roleDal.ChangeStatus(ref id, ref StatusCode);
                resultDto.ObjectId = id;
                if (resultDto.ObjectId > 0)
                {
                    resultDto.Message = string.Format("{0}:details Deleted successfully", objectName);
                }
                else
                {
                    resultDto.Message = string.Format("Error occured while deleting {0} Details", objectName);
                }

            }
            catch (Exception)
            {
                resultDto.Message = string.Format("service layer error occured while deleting{0} details", objectName);
                resultDto.ObjectId = -98;

            }
            return resultDto;

        }

    }
}


