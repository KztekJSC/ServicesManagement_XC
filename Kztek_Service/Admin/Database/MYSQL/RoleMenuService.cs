using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Extensions;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Library.Security;
using Kztek_Model.Models;
using X.PagedList;

namespace Kztek_Service.Admin.Database.MYSQL
{
    public class RoleMenuService : IRoleMenuService
    {
        private readonly IRoleMenuRepository _RoleMenuRepository;
     

        public RoleMenuService(IRoleMenuRepository _RoleMenuRepository)
        {
            this._RoleMenuRepository = _RoleMenuRepository;
           
        }
        public async Task<MessageReport> Create(RoleMenu model)
        {
            return await _RoleMenuRepository.Add(model);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _RoleMenuRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return await Task.FromResult(result);
        }
     

        public async Task<IEnumerable<RoleMenu>> GetAllByMenuId(string id)
        {
            var query = from n in _RoleMenuRepository.Table
                        where n.MenuId.Equals(id)
                        select n;
            return await Task.FromResult(query.ToList());
        }

        public async Task<IEnumerable<RoleMenu>> GetAllByRoleId(string id)
        {
            var query = from n in _RoleMenuRepository.Table
                        where n.RoleId.Equals(id)
                        select n;
            return await Task.FromResult(query.ToList());
        }

        public async Task<IEnumerable<RoleMenu>> GetAllByRoleId(string id, List<string> menuids)
        {
            var query = from n in _RoleMenuRepository.Table
                        where n.RoleId.Equals(id) && menuids.Contains(n.MenuId)
                        select n;
            return await Task.FromResult(query.ToList());
        }
        public async Task<RoleMenu> GetById(string id)
        {
            return await _RoleMenuRepository.GetOneById(id);
        }

    }
}
