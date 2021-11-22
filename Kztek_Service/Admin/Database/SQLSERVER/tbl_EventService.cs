using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class tbl_EventService : Itbl_EventService
    {
        private Itbl_EventRepository _tbl_EventRepository;
        public tbl_EventService(Itbl_EventRepository _tbl_EventRepository)
        {
            this._tbl_EventRepository = _tbl_EventRepository;
        }

        public async Task<MessageReport> Create(tbl_Event obj)
        {
            return await _tbl_EventRepository.Add(obj);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = await GetById(id);
            if (obj != null)
            {
                return await _tbl_EventRepository.Remove(obj);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

    

        public async Task<tbl_Event> GetById(string id)
        {
            return await _tbl_EventRepository.GetOneById(id);
        }



        public async Task<MessageReport> Update(tbl_Event oldObj)
        {
            return await _tbl_EventRepository.Update(oldObj);
        }
    }
}
