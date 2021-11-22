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
    public interface IUserService
    {
        Task<List<User>> GetAll();

        Task<GridModel<User>> GetPaging(string key, int pageNumber, int pageSize);

        Task<User> GetById(string id);

        Task<User> GetByUsername(string username);

        Task<User> GetByUsername_notId(string username, string id);

        User_Submit GetCustomById(string id);

        User_Submit GetCustomByModel(User model);

        Task<MessageReport> Create(User model);

        Task<MessageReport> Update(User model);

        Task<MessageReport> Delete(string id);

        Task<MessageReport> Login(AuthModel model, out User user);
        Task<IEnumerable<User>> GetAllActiveByListId(string ids);
    }
}