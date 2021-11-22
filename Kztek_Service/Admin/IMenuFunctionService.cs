using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;

namespace Kztek_Service.Admin
{
    public interface IMenuFunctionService
    {
        Task<IEnumerable<MenuFunction>> GetAllActiveByUserId(HttpContext context, SessionModel model);

        Task<IEnumerable<MenuFunction>> GetAllActiveByUserId(HttpContext context, SessionModel model, string area = "");

        Task<List<MenuFunction>> GetAll();

        Task<List<MenuFunction>> GetAll(string area = "");

        Task<List<MenuFunction>> GetAllActive();

        Task<List<MenuFunction>> GetAllActive(string area = "");

        Task<List<MenuFunction>> GetAllActiveOrder();

        Task<List<MenuFunction_Submit>> GetAllCustomActiveOrder();

        Task<List<MenuFunction_Submit>> GetAllCustomActiveOrder(string area = "");

        Task<MenuFunction> GetById(string id);

        Task<MenuFunction_Submit> GetCustomById(string id);

        Task<MenuFunction_Submit> GetCustomByModel(MenuFunction model);

        Task<MessageReport> Create(MenuFunction model);

        Task<MessageReport> Update(MenuFunction model);

        Task<MessageReport> Delete(string ids);

        Task<string> GetBreadcrumb(string id, string parentid, string lastvalue);

        Task<MessageReport> CreateMap(RoleMenu model);

        Task<MessageReport> DeleteMap(string roleid);

        Task<MessageReport> DeleteMap(string roleid, string area = "");
    }
}