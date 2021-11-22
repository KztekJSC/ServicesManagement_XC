using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class MenuFunctionService : IMenuFunctionService
    {
        private IMenuFunctionRepository _MenuFunctionRepository;
        private IRoleMenuRepository _RoleMenuRepository;
        private IUserRoleRepository _UserRoleRepository;
        private IMenuFunctionConfigRepository _MenuFunctionConfigRepository;

        private IUserRepository _UserRepository;

        public MenuFunctionService(IMenuFunctionRepository _MenuFunctionRepository, IRoleMenuRepository _RoleMenuRepository, IUserRoleRepository _UserRoleRepository, IMenuFunctionConfigRepository _MenuFunctionConfigRepository, IUserRepository _UserRepository)
        {
            this._MenuFunctionRepository = _MenuFunctionRepository;
            this._RoleMenuRepository = _RoleMenuRepository;
            this._UserRoleRepository = _UserRoleRepository;
            this._MenuFunctionConfigRepository = _MenuFunctionConfigRepository;

            this._UserRepository = _UserRepository;
        }

        public async Task<IEnumerable<MenuFunction>> GetAllActiveByUserId(HttpContext context, SessionModel model)
        {
            var configs = await GetMenuConfigAll();

            var query = from n in _MenuFunctionRepository.Table
                        where n.Active == true && n.Deleted == false && configs.Any(m => m.MenuFunctionId == n.Id)
                        select n;

            if (model != null && model.isAdmin == false)
            {
                var auths = AuthHelper.MenuFunctionByUserId(model, context).Result;

                var list = auths.Select(n => n.Id).ToList();

                query = query.Where(n => list.Contains(n.Id));
            }

            return await Task.FromResult(query);
        }

        public async Task<List<MenuFunction>> GetAll()
        {
            var data = _MenuFunctionRepository.Table;
            return await Task.FromResult(data.ToList());
        }

        public async Task<List<MenuFunction>> GetAllActive()
        {
            var data = from n in _MenuFunctionRepository.Table
                       where n.Active == true
                       select n;

            return await Task.FromResult(data.ToList());
        }

        public async Task<List<MenuFunction>> GetAllActiveOrder()
        {
            var data = from n in _MenuFunctionRepository.Table
                       where n.Active == true
                       orderby n.OrderNumber
                       select n;

            return await Task.FromResult(data.ToList());
        }

        public async Task<MenuFunction> GetById(string id)
        {
            return await _MenuFunctionRepository.GetOneById(id);
        }

        public async Task<MenuFunction_Submit> GetCustomById(string id)
        {
            var model = new MenuFunction_Submit();

            var obj = GetById(id).Result;
            if (obj != null)
            {
                model = GetCustomByModel(obj).Result;
            }

            return await Task.FromResult(model);
        }

        public async Task<MenuFunction_Submit> GetCustomByModel(MenuFunction model)
        {
            var obj = new MenuFunction_Submit()
            {
                ActionName = model.ActionName,
                Active = model.Active,
                ControllerName = model.ControllerName,
                Icon = model.Icon,
                Id = model.Id,
                MenuName = model.MenuName,
                MenuType = model.MenuType,
                ParentId = model.ParentId,
                SortOrder = Convert.ToInt32(model.OrderNumber),
                OrderNumber = model.OrderNumber,
                MenuGroupListId = model.MenuGroupListId
            };

            return await Task.FromResult(obj);
        }

        public async Task<MessageReport> Create(MenuFunction model)
        {
            return await _MenuFunctionRepository.Add(model);
        }

        public async Task<MessageReport> Update(MenuFunction model)
        {
            return await _MenuFunctionRepository.Update(model);
        }

        public async Task<MessageReport> Delete(string ids)
        {

            var query = from n in _MenuFunctionRepository.Table
                        where ids.Contains(n.Id)
                        select n;


            var result = await _MenuFunctionRepository.Remove_Multi(query);

            return result;
        }

        public async Task<string> GetBreadcrumb(string id, string parentid, string lastvalue)
        {
            var list = GetAllActiveOrder().Result;

            if (string.IsNullOrWhiteSpace(parentid))
            {
                lastvalue += id;
            }
            else
            {
                var objParent = list.FirstOrDefault(n => n.Id.Equals(parentid));
                if (objParent != null)
                {
                    lastvalue += objParent.Id + ",";

                    var str = GetBreadcrumb(objParent.Id, objParent.ParentId.ToString(), lastvalue);

                    lastvalue = str.Result;
                }
            }

            return await Task.FromResult(lastvalue);
        }

        private List<string> GetListIdByUserId(string id)
        {
            //Danh sách quyền
            var roles = from r in _UserRoleRepository.Table
                        where r.UserId == id
                        select r.RoleId;

            var menus = from m in _RoleMenuRepository.Table
                        where roles.Contains(m.RoleId)
                        select m.MenuId;

            return menus.ToList();
        }

        public async Task<MessageReport> CreateMap(RoleMenu model)
        {
            return await _RoleMenuRepository.Add(model);
        }

        public async Task<MessageReport> DeleteMap(string roleid)
        {
            var t = from n in _RoleMenuRepository.Table
                    where n.RoleId == roleid
                    select n;

            return await _RoleMenuRepository.Remove_Multi(t);
        }

        public async Task<List<MenuFunction_Submit>> GetAllCustomActiveOrder()
        {
            var dt = new List<MenuFunction_Submit>();

            var data = from n in _MenuFunctionRepository.Table
                       where n.Active == true
                       orderby n.OrderNumber
                       select n;

            var k = data.ToList();

            foreach (var item in k)
            {
                dt.Add(GetCustomByModel(item).Result);
            }

            return await Task.FromResult(dt);
        }

        public async Task<IEnumerable<MenuFunction>> GetAllActiveByUserId(HttpContext context, SessionModel model, string area = "")
        {
            var configs = await GetMenuConfigAll();

            var query = from n in _MenuFunctionRepository.Table
                        where n.Active == true && n.Deleted == false && configs.Any(m => m.MenuFunctionId == n.Id)
                        select n;

            if (!string.IsNullOrWhiteSpace(area))
            {
                var objArea = StaticList.GroupMenuList().FirstOrDefault(n => n.AreaName == area);
                if (objArea != null)
                {
                    query = query.Where(n => n.MenuGroupListId != null && n.MenuGroupListId.Contains(objArea.ItemValue));
                }
            }
            else
            {
                query = query.Where(n => n.isSystem == true);
            }


            if (model != null && model.isAdmin == false)
            {
                var auths = AuthHelper.MenuFunctionByUserId(model, context).Result;

                var list = auths.Select(n => n.Id).ToList();

                query = query.Where(n => list.Contains(n.Id));
            }

            return await Task.FromResult(query);
        }

        public async Task<List<MenuFunction>> GetAllActive(string area = "")
        {
            //
            var objArea = StaticList.GroupMenuList().FirstOrDefault(n => n.AreaName == area);
            

            var data = from n in _MenuFunctionRepository.Table
                       where n.Active == true
                       select n;

            if (objArea != null)
            {
                data = data.Where(n => n.MenuGroupListId != null && n.MenuGroupListId.Contains(objArea.ItemValue));
            }

            return await Task.FromResult(data.ToList());
        }

        public async Task<List<MenuFunction>> GetAll(string area = "")
        {
            //
            var objArea = StaticList.GroupMenuList().FirstOrDefault(n => n.AreaName == area);

            var data = _MenuFunctionRepository.Table;

            if (objArea != null)
            {
                data = data.Where(n => n.MenuGroupListId != null && n.MenuGroupListId.Contains(objArea.ItemValue));
            }

            return await Task.FromResult(data.ToList());
        }

        public async Task<List<MenuFunction_Submit>> GetAllCustomActiveOrder(string area = "")
        {
            //
            var objArea = StaticList.GroupMenuList().FirstOrDefault(n => n.AreaName == area);

            var dt = new List<MenuFunction_Submit>();

            var data = from n in _MenuFunctionRepository.Table
                       where n.Active == true
                       orderby n.OrderNumber
                       select n;

            if (objArea != null)
            {
                data = data.Where(n => n.MenuGroupListId != null && n.MenuGroupListId.Contains(objArea.ItemValue)).OrderBy(n => n.OrderNumber);
            }

            var k = data.ToList();

            foreach (var item in k)
            {
                dt.Add(GetCustomByModel(item).Result);
            }

            return await Task.FromResult(dt);
        }

        public async Task<MessageReport> DeleteMap(string roleid, string area = "")
        {
            var ids = await GetMenuFunctionsByArea(area);

            var t = from n in _RoleMenuRepository.Table
                    where n.RoleId == roleid && ids.Contains(n.MenuId)
                    select n;

            return await _RoleMenuRepository.Remove_Multi(t);
        }

        private async Task<List<string>> GetMenuFunctionsByArea(string area = "")
        {
            var objArea = StaticList.GroupMenuList().FirstOrDefault(n => n.AreaName == area);

            var query = from n in _MenuFunctionRepository.Table
                        where n.MenuGroupListId != null
                        select n;

            if (objArea != null)
            {
                query = query.Where(n => n.MenuGroupListId.Contains(objArea.ItemValue));
            }

            var k = query.Select(n => n.Id).ToList();

            return await Task.FromResult(k);
        }

        private async Task<List<MenuFunctionConfig>> GetMenuConfigAll()
        {
            var data = _MenuFunctionConfigRepository.Table;
            return await Task.FromResult(data.ToList());
        }
    }
}