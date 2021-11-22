using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin.Database
{
    public interface Itbl_Lane_PCService
    {
        Task<GridModel<tbl_Lane_PC>> GetAllCustomPagingByFirst(string key, string pc, int page, int v);

        Task<MessageReport> Create(tbl_Lane_PC obj);

        Task<MessageReport> Update(tbl_Lane_PC obj);

        Task<tbl_Lane_PC> GetById(string id);

        Task<tbl_Lane_PC_Custom> GetByCustomId(string id);
        Task<MessageReport> DeleteById(string id);

        Task<tbl_Lane_PC> getByPc_ID(string pc_ID);
    }
}
