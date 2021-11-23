using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Api
{
    public interface Itbl_EventService
    {
        Task<MessageReport> Create(tbl_Event_POST model);
        Task<MessageReport> Update(tbl_Event_POST model);
        Task<MessageReport> Delete(string id);
    }
}
