using System;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Model.Models;

namespace Kztek_Service.Admin
{
    public interface ItblSystemConfigService
    {
        Task<tblSystemConfig> GetDefault();

        Task<MessageReport> Create(tblSystemConfig obj);

        Task<MessageReport> Update(tblSystemConfig obj);
    }
}
