using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface ItblLaneService
    {
        Task<IEnumerable<tblLane>> GetAll();
        Task<IEnumerable<tblLane>> GetAllActive();

        Task<tblLane> GetActiveLoop();

        Task<IEnumerable<tblLane>> GetAllActiveByListId(string ids);
        Task<GridModel<tblLane>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize);

        Task<tblLane> GetById(string id);

        Task<MessageReport> Create(tblLane obj);

        Task<MessageReport> Update(tblLane obj);

        Task<MessageReport> DeleteById(string id);

        Task<tblLane> GetByName(string name);

        Task<tblLane> GetByName_Id(string name, string id);
        Task<string> GetTitle(string landids);

        Task<tblLane_Submit> GetByCustomId(string id);
    }
}
