using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface IGroupService
    {
        Task<GridModel<Group>> GetAllCustomPagingByFirst(string key, string pc, int page, int v);
        Task<MessageReport> DeleteById(string id);
        Task<Group> GetById(string id);
        Task<MessageReport> Update(Group oldObj);
        Task<MessageReport> Create(Group model);
    }
}
