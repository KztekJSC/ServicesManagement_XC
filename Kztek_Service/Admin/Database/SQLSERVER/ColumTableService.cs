using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class ColumTableService : IColumTableService

    {
        private IColumTableRepository _ColumTableRepository;
        public ColumTableService(IColumTableRepository _ColumTableRepository) {
            this._ColumTableRepository = _ColumTableRepository;
           }
        public async Task<MessageReport> Create(ColumTable model)
        {
            return await _ColumTableRepository.Add(model);
        }

        public async Task<ColumTable> GetDetailByController(string controller, string action)
        {
            var query = from n in _ColumTableRepository.Table
                        where n.Controller == controller && n.Action == action
                        select n;
            return await Task.FromResult( query.FirstOrDefault());

        }

        public async Task<MessageReport> Update(ColumTable model)
        {
            return await _ColumTableRepository.Update(model);
        }
    }
}
