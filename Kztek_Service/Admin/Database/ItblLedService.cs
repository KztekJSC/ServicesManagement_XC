using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin.Database
{
    public interface ItblLedService
    {
        Task<GridModel<tblLED>> GetAllCustomPagingByFirst(string key, string pc, int page, int v);

        Task<tblLED> GetByName(string led_Name);

        Task<MessageReport> Create(tblLED obj);

        Task<tblLED_Submit> GetByCustomId(string id);

        Task<tblLED> GetByID(string id);

        Task<tblLED> GetByName_Id(string led_Name, string id);

        Task<MessageReport> Update(tblLED oldObj);

        Task<MessageReport> DeleteById(string id);
    }
}
