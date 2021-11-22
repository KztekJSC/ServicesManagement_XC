using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Model.Models;

namespace Kztek_Service.Api
{
    public interface ItblPCService
    {
        Task<IEnumerable<tblPC>> GetAllByFirst(string key, string gateid);

        Task<tblPC> GetById(string id);

        Task<MessageReport> Create(tblPC_POST model);

        Task<MessageReport> Update(tblPC_POST model);

        Task<MessageReport> Remove(string id);
    }
}
