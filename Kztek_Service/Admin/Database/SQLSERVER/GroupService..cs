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
    public class GroupService : IGroupService
    {
        private IGroupRepository _GroupRepository;
        public GroupService(IGroupRepository _GroupRepository)
        {
            this._GroupRepository = _GroupRepository;
        }
        public async Task<MessageReport> Create(Group model)
        {
            return await _GroupRepository.Add(model);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _GroupRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

        public async Task<List<Group>> GetAll()
        {
            var query =  from n in _GroupRepository.Table
                        select n;
            return await Task.FromResult( query.ToList());
        }

        public async Task<GridModel<Group>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in _GroupRepository.Table
                        select n;
            

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }



            var pageList = query.ToPagedList(pageNumber, pageSize);

            var model = GridModelHelper<Group>.GetPage(pageList.OrderByDescending(n => n.Name).ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }

        public async Task<SelectListModel_Chosen> GetaSelectModelChoseGroup(string id = "", string placeholder = "", string selecteds = "")
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
                IdSelectList = "GroupId",
                Selecteds = selecteds,
                Placeholder = placeholder,
                Data = cus.ToList(),
                isMultiSelect = false
            };
            return model;
        }

        public async Task<Group> GetById(string id)
        {
            return await _GroupRepository.GetOneById(id);
        }

        public async Task<Group> GetByName(string key)
        {
            var query = from n in _GroupRepository.Table
                        where n.Name == key
                        select n;
            return await Task.FromResult(   query.FirstOrDefault());
        }

        public async Task<MessageReport> Update(Group oldObj)
        {
            return await _GroupRepository.Update(oldObj);
        }
    }
}
