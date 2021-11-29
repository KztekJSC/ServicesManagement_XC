using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Model.Models;

namespace Kztek_Service.Api
{
    public interface Itbl_EventService
    {
        Task<MessageReport> Create(tbl_Event_POST model);
        Task<MessageReport> Update(tbl_Event_POST model);
        Task<MessageReport> Delete(tbl_Event_POST model);
        Task<MessageReport> VehicleStatusIn(API_VehicleStatus model);
        Task<MessageReport> VehicleStatusOut(API_VehicleStatus model);
    }
}

