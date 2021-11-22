using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Model.Models;

namespace Kztek_Service.Api
{
    public interface ItblSystemConfigService
    {
        Task<List<tblSystemConfig>> GetDefault();

        Task<MessageReport> Create(tblSystemConfig_POST model);

        Task<MessageReport> Update(tblSystemConfig_PUT model);

        Task<MessageReport> Remove(string id);
    }
}
