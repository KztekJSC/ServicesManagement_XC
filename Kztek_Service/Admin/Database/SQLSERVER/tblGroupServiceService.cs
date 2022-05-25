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
   
      public  class tblGroupServiceService : ItblGroupServiceService
    {
        private ItblGroupServiceRepository _tblGroupServiceRepository;
        public tblGroupServiceService(ItblGroupServiceRepository _tblGroupServiceRepository)
        {
            this._tblGroupServiceRepository = _tblGroupServiceRepository;
        }

        public async Task<MessageReport> CreateMap(tblGroupService t)
        {
            return await Task.FromResult(await _tblGroupServiceRepository.Add(t));
        }

        public async Task<MessageReport> DeleteMap(string id)
        {
            var t = from n in _tblGroupServiceRepository.Table
                    where n.ServiceId == id
                    select n;

            return await _tblGroupServiceRepository.Remove_Multi(t);
        }

        public async Task<List<tblGroupService>> GetAllByService(string service)
        {
           var qu = from n in _tblGroupServiceRepository.Table
                    where n.ServiceId == service  
                    select n;
            return await Task.FromResult(qu.ToList());
        }
    }
}
