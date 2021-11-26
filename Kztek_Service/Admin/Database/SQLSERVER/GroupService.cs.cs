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

        public async Task<Group> GetById(string id)
        {
            return await _GroupRepository.GetOneById(id);
        }

        public async Task<MessageReport> Update(Group oldObj)
        {
            return await _GroupRepository.Update(oldObj);
        }
    }
}
