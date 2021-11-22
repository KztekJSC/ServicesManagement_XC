using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface ItblPCService
    {
        Task<IEnumerable<tblPC>> GetAllActive();

        Task<IEnumerable<tblPC>> GetAllByGateId(string id);

        Task<GridModel<tblPC>> GetAllCustomPagingByFirst(string key, string gate, int pageNumber, int pageSize);

        Task<tblPC> GetById(string id);

        Task<MessageReport> Create(tblPC obj);

        Task<MessageReport> Update(tblPC obj);

        Task<MessageReport> DeleteById(string id);

        Task<tblPC> GetByName(string name);

        Task<tblPC> GetByName_Id(string name, string id);
    }
}
