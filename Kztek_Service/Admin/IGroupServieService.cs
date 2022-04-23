using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface IGroupServieService
    {
        Task<GridModel<Service>> GetAllCustomPagingByFirst(string key, string pc, int page, int v);

        Task<MessageReport> Create(Service model);

        Task<Service> GetById(string id);

        Task<MessageReport> Update(Service oldObj);

        Task<MessageReport> DeleteById(string id);

        Task<Service_Submit> GetCustomeById(string id);
    }
}
