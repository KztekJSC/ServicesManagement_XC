using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Model.Models;

namespace Kztek_Service.Api
{
    public interface ItblCameraService
    {
        Task<IEnumerable<tblCamera>> GetAllByFirst(string key, string pcid);

        Task<List<tblCamera_View>> GetAllCustomByFirst(string key, string pcid);

        Task<tblCamera> GetById(string id);

        Task<tblCamera_View> GetCustomById(string id);

        Task<tblCamera_View> GetCustomByModel(tblCamera model);

        Task<MessageReport> Create(tblCamera_POST model);

        Task<MessageReport> Update(tblCamera_PUT model);

        Task<MessageReport> Remove(string id);
    }
}
