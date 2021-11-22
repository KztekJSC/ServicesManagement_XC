using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Model.Models;

namespace Kztek_Service.Admin
{
    public interface IMenuFunctionConfigService
    {
        Task<List<MenuFunctionConfig>> GetAll();

        Task<MessageReport> Create(MenuFunctionConfig obj);

        Task<bool> DeleteAll();
    }
}
