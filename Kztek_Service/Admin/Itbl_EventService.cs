using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface Itbl_EventService
    {
        Task<MessageReport> Create(tbl_Event obj);
        Task<MessageReport> DeleteById(string id);
        Task<tbl_Event> GetById(string id);
        Task<MessageReport> Update(tbl_Event oldObj);
        public Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int v, string statusID, string fromdate, string todate);
        Task<SelectListModel_Chosen> GetEventype(string id = "", string placeholder = "", string selecteds = "");
    }
}
