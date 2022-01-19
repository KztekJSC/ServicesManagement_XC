using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface IHomeService
    {
        Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int v, string groupid, string fromdate, string todate, string StatusID);
    }
}
