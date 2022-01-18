using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface IEventInService
    {
        Task<MessageReport> DeleteById(string id);
        Task<EventIn> GetById(string id);
        Task<MessageReport> Update(EventIn oldObj);
        Task<MessageReport> Create(EventIn model);
    }
}
