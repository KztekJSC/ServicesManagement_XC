using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _UserRoleRepository;
     

        public UserRoleService(IUserRoleRepository _UserRoleRepository)
        {
            this._UserRoleRepository = _UserRoleRepository;
           
        }
    
        public async Task<IEnumerable<UserRole>> GetAllByUserId(string id)
        {
            var query = from n in _UserRoleRepository.Table
                        where n.UserId.Equals(id)
                        select n;
            return await Task.FromResult(query.ToList());
        }

    

        public async Task<UserRole> GetByUserId_RoleId(string userid, string roleid)
        {
            var query = from n in _UserRoleRepository.Table
                        where n.UserId.Equals(userid) && n.RoleId.Equals(roleid)
                        select n;
            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<IEnumerable<UserRole>> GetAllByRoleId(string id)
        {
            var query = from n in _UserRoleRepository.Table
                        where n.RoleId.Equals(id)
                        select n;
            return await Task.FromResult(query.ToList());
        }

        public async Task<MessageReport> Create(UserRole model)
        {
            return await _UserRoleRepository.Add(model);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _UserRoleRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return await Task.FromResult(result);
        }


        public async Task<UserRole> GetById(string id)
        {
            return await _UserRoleRepository.GetOneById(id);
        }

    
    }
}
