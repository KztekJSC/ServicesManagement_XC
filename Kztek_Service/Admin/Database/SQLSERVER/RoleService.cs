using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Model.Models;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class RoleService : IRoleService
    {
        private IRoleRepository _RoleRepository;
        private IRoleMenuRepository _RoleMenuRepository;
        private IUserRoleRepository _UserRoleRepository;

        public RoleService(IRoleRepository _RoleRepository, IRoleMenuRepository _RoleMenuRepository, IUserRoleRepository _UserRoleRepository)
        {
            this._RoleRepository = _RoleRepository;
            this._RoleMenuRepository = _RoleMenuRepository;
            this._UserRoleRepository = _UserRoleRepository;
        }

        public async Task<List<Role>> GetAll()
        {
            var data = _RoleRepository.Table;
            return await Task.FromResult(data.ToList());
        }

        public async Task<List<Role>> GetAllActiveOrder()
        {
            var data = from n in _RoleRepository.Table
                       where n.Active == true
                       orderby n.RoleName
                       select n;

            return await Task.FromResult(data.ToList());
        }

        public async Task<Role> GetById(string id)
        {
            return await _RoleRepository.GetOneById(id);
        }

        public Role_Submit GetCustomById(string id)
        {
            var model = new Role_Submit();

            var obj = _RoleRepository.GetOneById(id).Result;
            if (obj != null)
            {
                model = GetCustomByModel(obj);
            }

            return model;
        }

        public Role_Submit GetCustomByModel(Role model)
        {
            var obj = new Role_Submit()
            {
                Id = model.Id.ToString(),
                Active = model.Active,
                Description = model.Description,
                RoleName = model.RoleName,
                MenuFunctions = new List<string>()
            };

            obj.MenuFunctions = (from n in _RoleMenuRepository.Table
                                 where n.RoleId == model.Id
                                 select n.MenuId).ToList();

            return obj;
        }

        public async Task<MessageReport> Create(Role model)
        {
            return await _RoleRepository.Add(model);
        }

        public async Task<MessageReport> Update(Role model)
        {
            return await _RoleRepository.Update(model);
        }

        public async Task<MessageReport> Delete(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _RoleRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

        public async Task<MessageReport> CreateMap(UserRole model)
        {
            return await _UserRoleRepository.Add(model);
        }

        public async Task<MessageReport> DeleteMap(string userid)
        {
            var t = from n in _UserRoleRepository.Table
                    where n.UserId == userid
                    select n;

            return await _UserRoleRepository.Remove_Multi(t);
        }
        public async Task<List<Role_Custom>> GetAllUserRoles()
        {
            var query = new StringBuilder();
            query.AppendLine("select u.Id as UserId,r.* from UserRole ur left join[Role] r on ur.RoleId = r.Id left join[User] u on ur.UserId = u.Id");

            var list = DatabaseHelper.ExcuteCommandToList<Role_Custom>(query.ToString());

            return await Task.FromResult(list);
        }
    }
}