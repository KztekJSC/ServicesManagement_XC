using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace Kztek_Service.Admin
{
    public interface ItblLogService
    {
        Task<GridModel<tblLog>> GetAllPagingByFirst(string key, string users, string actions, string fromdate, string todate, int pageNumber, int pageSize, string appcode = "");
    }
}
