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
    public class ServiceService : IServiceService
    {
        private IServiceRepository _ServiceRepository;
        public ServiceService(IServiceRepository _ServiceRepository)
        {
            this._ServiceRepository = _ServiceRepository;
        }
        public async Task<MessageReport> Create(Service model)
        {
            return await _ServiceRepository.Add(model);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _ServiceRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

        public async Task<List<Service>> GetAll()
        {
            var query = from n in _ServiceRepository.Table
                        select n;
            return await Task.FromResult(query.ToList());
        }

        public async Task<GridModel<Service>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in _ServiceRepository.Table
                        select n;


            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }



            var pageList = query.ToPagedList(pageNumber, pageSize);

            var model = GridModelHelper<Service>.GetPage(pageList.OrderByDescending(n => n.Name).ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }

       

        public async Task<Service> GetById(string id)
        {
            return await _ServiceRepository.GetOneById(id);
        }

        public async Task<Service> GetByName(string key)
        {
            var query = from n in _ServiceRepository.Table
                        where n.Name == key
                        select n;
            return query.FirstOrDefault();
        }

        public async Task<SelectListModel_Chosen> SelectChoseService(string id = "", string placeholder = "", string selecteds = "")
        {
            var data = await GetAll();
            var cus = new List<SelectListModel>();
            var lst = data;
            if (lst != null && lst.Count > 0)
            {
                cus.Add(new SelectListModel()
                {
                    ItemText = "---- Lựa chọn ----",
                    ItemValue = "00"
                });

                cus.AddRange(data.Select(n => new SelectListModel()
                {
                    ItemText = n.Name,
                    ItemValue = n.Id
                }));
            }

            var model = new SelectListModel_Chosen()
            {
                IdSelectList = "ServiceId",
                Selecteds = selecteds,
                Placeholder = placeholder,
                Data = cus.ToList(),
                isMultiSelect = false
            };
            return model;
        }

        public async Task<MessageReport> Update(Service oldObj)
        {
            return await _ServiceRepository.Update(oldObj);
        }
    }
}
