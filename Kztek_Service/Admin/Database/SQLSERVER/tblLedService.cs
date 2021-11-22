using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class tblLedService : ItblLedService
    {
        private ItblLEDRepository _tblLEDRepository;
        public tblLedService(ItblLEDRepository _tblLEDRepository)
        {
            this._tblLEDRepository = _tblLEDRepository;
        }

        public async Task<MessageReport> Create(tblLED obj)
        {
            return await _tblLEDRepository.Add(obj);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = await GetByID(id);
            if (obj != null)
            {
                return await _tblLEDRepository.Remove(obj);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

        public async Task<GridModel<tblLED>> GetAllCustomPagingByFirst(string key, string pc, int page, int pageSize)
        {
            var query = from n in _tblLEDRepository.Table
                        select n;


            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.led_Name.Contains(key));
            }

            var pageList = query.ToPagedList(page, pageSize);

            var model = GridModelHelper<tblLED>.GetPage(pageList.OrderByDescending(n => n.led_Name).ToList(), page, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }

        public async Task<tblLED_Submit> GetByCustomId(string id)
        {
            var obj = await GetByID(id);
            var model = new tblLED_Submit()
            {
                id = id.ToString(),
                led_Code = obj.led_Code,
                led_Name = obj.led_Name,
                ip_Address = obj.ip_Address,
                FunctionLed = obj.led_Function.ToString(),
                description = obj.description,
                port = obj.port,
                controller_Type = obj.controller_Type,

            };
            return model;

        }

        public async Task<tblLED> GetByID(string id)
        {
            return await _tblLEDRepository.GetOneById(Guid.Parse(id));
        }

        public async Task<tblLED> GetByName(string led_Name)
        {
            var query = from n in _tblLEDRepository.Table
                        where n.led_Name == led_Name
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<tblLED> GetByName_Id(string led_Name, string id)
        {
            var query = from n in _tblLEDRepository.Table
                        where n.led_Name == led_Name && n.id.ToString() != id
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<MessageReport> Update(tblLED oldObj)
        {
            return await _tblLEDRepository.Update(oldObj);
        }
    }
}
