using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRole>> GetAllByUserId(string id);
        Task<IEnumerable<UserRole>> GetAllByRoleId(string id);       
        Task<UserRole> GetByUserId_RoleId(string userid, string roleid);
        Task<UserRole> GetById(string id);

        Task<MessageReport> Create(UserRole obj);

        Task<MessageReport> DeleteById(string id);
    }
}
