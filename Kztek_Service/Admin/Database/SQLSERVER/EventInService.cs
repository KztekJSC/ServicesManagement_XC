using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class EventInService : IEventInService
    {
        private IEventInRepository _EventInRepository;
        public EventInService(IEventInRepository _EventInRepository)
        {
            this._EventInRepository = _EventInRepository;
        }
        public async Task<MessageReport> Create(EventIn model)
        {
            return await _EventInRepository.Add(model);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _EventInRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

        public async Task<List<EventIn>> GetAll()
        {
            var query = from n in _EventInRepository.Table
                        select n;
            return await Task.FromResult(query.ToList());
        }

        public async Task<EventIn> GetById(string id)
        {
            return await _EventInRepository.GetOneById(id);
        }

        public async Task<MessageReport> Update(EventIn oldObj)
        {
            return await _EventInRepository.Update(oldObj);
        }
    }
}
