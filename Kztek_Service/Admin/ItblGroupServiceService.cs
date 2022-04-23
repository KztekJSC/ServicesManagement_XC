using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface ItblGroupServiceService
    {
        Task<MessageReport> CreateMap(tblGroupService t);
        Task<MessageReport> DeleteMap(string id);
        Task<List<tblGroupService>> GetAllByService(string service);
    }
}
