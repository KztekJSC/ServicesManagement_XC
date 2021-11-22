using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface ItblCameraService
    {
        Task<tblCamera> GetById(string id);

        Task<MessageReport> Create(tblCamera obj);

        Task<MessageReport> Update(tblCamera obj);

        Task<MessageReport> DeleteById(string id);

        Task<IEnumerable<tblCamera>> GetAllActiveByPC(string id);

        Task<GridModel<tblCamera>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize);

        Task<tblCamera> GetByName(string name);

        Task<tblCamera> GetByName_Id(string name, string id);
        Task<List<tblCamera>> GetAllActive();
    }
}
