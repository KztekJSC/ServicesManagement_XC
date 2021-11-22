using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Model.Models;

namespace Kztek_Service.Api
{
    public interface ItblLaneService
    {
        Task<IEnumerable<tblLane>> GetAllByFirst(string key, string pcid);

        Task<tblLane> GetById(string id);

        Task<MessageReport> Create(tblLane_Submit model);

        Task<MessageReport> Update(tblLane_Submit model);

        Task<MessageReport> Remove(string id);
    }
}
