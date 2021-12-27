using Kztek_Core.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface IColumTableService
    {
        Task<MessageReport> Create(ColumTable model);
        Task<ColumTable> GetDetailByController(string v1, string v2);
        Task<MessageReport> Update(ColumTable model);
    }
}
