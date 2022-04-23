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
    public class GroupServieService : IGroupServieService
    {
        private IServiceRepository _ServiceRepository;
        private Itbl_EventService _tbl_EventService;
        private ItblGroupServiceRepository _tblGroupServiceRepository;
        public GroupServieService(IServiceRepository _ServiceRepository, Itbl_EventService _tbl_EventService , ItblGroupServiceRepository _tblGroupServiceRepository)
        {
            this._ServiceRepository = _ServiceRepository;
            this._tbl_EventService = _tbl_EventService;
            this._tblGroupServiceRepository = _tblGroupServiceRepository;
        }

        public async Task<MessageReport> Create(Service model)
        {
            return await _ServiceRepository.Add(model);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));
            var obj1 = await _tbl_EventService.GetByService(id);

            if (obj1 != null) // đã tồn tại k được xoá
            {
                result = new MessageReport(false, "Không xoá được");
            }
            else
            {
                var obj = GetById(id);
                if (obj.Result != null)
                {
                    return await _ServiceRepository.Remove(obj.Result);
                }
                else
                {
                    result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
                }
            }
            
          

            return await Task.FromResult(result);
          
        }

       
        public async Task<GridModel<Service>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in _ServiceRepository.Table
                        select n;


            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key) || n.Code.Contains(key));
            }



            var pageList = query.ToPagedList(pageNumber, pageSize);

            var model = GridModelHelper<Service>.GetPage(pageList.OrderByDescending(n => n.Name).ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }

        public async Task<Service> GetById(string id)
        {
            return await _ServiceRepository.GetOneById(id);
        }

        public async Task<Service_Submit> GetCustomeById(string id)
        {
            var model = await GetById(id);

            var obj = new Service_Submit()
            {
                Id = model.Id,
                Code = model.Code,  
                Groups = new List<string>(),
                Description = model.Description,
                Name = model.Name,
                
            };

            obj.Groups = (from n in _tblGroupServiceRepository.Table
                         where n.ServiceId == model.Id
                         select n.GroupId).ToList();

            return obj;
        }

        public async Task<MessageReport> Update(Service oldObj)
        {
            return await _ServiceRepository.Update(oldObj);
        }
    }
}
