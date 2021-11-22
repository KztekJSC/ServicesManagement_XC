using System;
using System.Threading.Tasks;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;

namespace Kztek_Service.Admin
{
    public interface IUser_AuthGroupService
    {
        Task<User_AuthGroup> GetByUserId(string userid);

        Task<string> GetAuthCardGroupIds(HttpContext context);
    }
}
