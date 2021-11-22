using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Library.Security;
using Kztek_Model.Models;
using X.PagedList;
namespace Kztek_Service.Admin
{
    public interface IRoleMenuService
    {
        Task<IEnumerable<RoleMenu>> GetAllByMenuId(string id);
        Task<IEnumerable<RoleMenu>> GetAllByRoleId(string id);
        Task<IEnumerable<RoleMenu>> GetAllByRoleId(string id, List<string> menuids);

        Task<MessageReport> Create(RoleMenu obj);

        Task<MessageReport> DeleteById(string id);
    }
}
