using System;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Library.Models;

namespace Kztek_Service.Api
{
    public interface IUserService
    {
        Task<MessageReport> SignIn(AuthModel model);

        Task<MessageReport> SignIn(AuthModel_LowSecurity model);

        Task<bool> CheckPermission(string userid, string controllername, string actionname);
    }
}
