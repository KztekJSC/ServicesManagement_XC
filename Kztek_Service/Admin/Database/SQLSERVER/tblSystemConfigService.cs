using System;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Model.Models;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class tblSystemConfigService : ItblSystemConfigService
    {
        private ItblSystemConfigRepository _tblSystemConfigRepository;

        public tblSystemConfigService(ItblSystemConfigRepository _tblSystemConfigRepository)
        {
            this._tblSystemConfigRepository = _tblSystemConfigRepository;
        }

        public async Task<MessageReport> Create(tblSystemConfig obj)
        {
            return await _tblSystemConfigRepository.Add(obj);
        }

        public async Task<tblSystemConfig> GetDefault()
        {
            var query = from n in _tblSystemConfigRepository.Table
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<MessageReport> Update(tblSystemConfig obj)
        {
            return await _tblSystemConfigRepository.Update(obj);
        }
    }
}
