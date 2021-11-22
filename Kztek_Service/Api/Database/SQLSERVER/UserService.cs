using System;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Extensions;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Library.Security;
using Kztek_Model.Models;

namespace Kztek_Service.Api.Database.SQLSERVER
{
    public class UserService : IUserService
    {
        private IUserRepository _UserRepository;
        private IMenuFunctionRepository _MenuFunctionRepository;
        private IUserRoleRepository _UserRoleRepository;
        private IRoleMenuRepository _RoleMenuRepository;

        public UserService(IUserRepository _UserRepository, IMenuFunctionRepository _MenuFunctionRepository, IUserRoleRepository _UserRoleRepository, IRoleMenuRepository _RoleMenuRepository)
        {
            this._UserRepository = _UserRepository;
            this._MenuFunctionRepository = _MenuFunctionRepository;
            this._UserRoleRepository = _UserRoleRepository;
            this._RoleMenuRepository = _RoleMenuRepository;
        }

        public async Task<bool> CheckPermission(string userid, string controllername, string actionname)
        {
            //Lấy menu
            var objMenuFunction = await GetByControllerName_ActionName(controllername, actionname);

            if (objMenuFunction == null)
            {
                return false;
            }

            //Check tồn tại
            var existed = await IsExistedMenuInUser(userid, objMenuFunction.Id);

            return existed;
        }

        public async Task<MessageReport> SignIn(AuthModel model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                //Kiểm tra tk
                var objUser = GetByUsername(model.Username);
                if (objUser == null)
                {
                    result = new MessageReport(false, "Tài khoản không tồn tại");
                    return await Task.FromResult(result);
                }

                //Kiểm tra trạng thái
                if (objUser.Active == false)
                {
                    result = new MessageReport(false, "Tài khoản đang bị khóa");
                    return await Task.FromResult(result);
                }

                //Kiểm tra mk
                //var pass = CryptoHelper.DecryptPass_User(objUser.Password, objUser.PasswordSalat);
                //if (pass != model.Password)
                //{
                //    result = new MessageReport(false, "Mật khẩu không khớp");
                //    return await Task.FromResult(result);
                //}
                var pass = model.Password.PasswordHashed(objUser.PasswordSalat);

                if (objUser.Password != pass)
                {
                    result = new MessageReport(false, "Mật khẩu không khớp");
                    return await Task.FromResult(result);
                }

                //Tạo token
                var token = ApiHelper.GenerateJSON_MobileToken(objUser.Id);

                result = new MessageReport(true, token);
            }
            catch (System.Exception ex)
            {
                result = new MessageReport(false, string.Format("Message: {0} - Details: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));
            }

            return await Task.FromResult(result);
        }

        public async Task<MessageReport> SignIn(AuthModel_LowSecurity model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                //Kiểm tra tk
                var objUser = GetByUsername(model.Username);
                if (objUser == null)
                {
                    result = new MessageReport(false, "Tài khoản không tồn tại");
                    return await Task.FromResult(result);
                }

                //Kiểm tra trạng thái
                if (objUser.Active == false)
                {
                    result = new MessageReport(false, "Tài khoản đang bị khóa");
                    return await Task.FromResult(result);
                }

                //Kiểm tra mk
                //var pass = CryptoHelper.DecryptPass_User(objUser.Password, objUser.PasswordSalat);
                //if (pass != model.Password)
                //{
                //    result = new MessageReport(false, "Mật khẩu không khớp");
                //    return await Task.FromResult(result);
                //}

                //if (!string.IsNullOrWhiteSpace(model.Password) || objUser.Admin)
                //{
                    
                //}

                var pass = model.Password.PasswordHashed(objUser.PasswordSalat);

                if (objUser.Password != pass)
                {
                    result = new MessageReport(false, "Mật khẩu không khớp");
                    return await Task.FromResult(result);
                }

                //Tạo token
                var token = ApiHelper.GenerateJSON_MobileToken(objUser.Id);

                result = new MessageReport(true, token);
            }
            catch (System.Exception ex)
            {
                result = new MessageReport(false, string.Format("Message: {0} - Details: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));
            }

            return await Task.FromResult(result);
        }

        private User GetByUsername(string username)
        {
            var query = from n in _UserRepository.Table
                        where n.Username == username
                        select n;

            return query.FirstOrDefault();
        }

        private async Task<MenuFunction> GetByControllerName_ActionName(string controllername, string actionname)
        {
            var query = from n in _MenuFunctionRepository.Table
                        where !n.Deleted
                        select n;

            var data = query.FirstOrDefault(n => n.ControllerName.Equals(controllername) && n.ActionName.Equals(actionname));

            return await Task.FromResult(data);
        }

        private async Task<bool> IsExistedMenuInUser(string userid, string menuid)
        {
            var objUser = await _UserRepository.GetOneById(userid);
            if (objUser != null)
            {
                if (objUser.Admin)
                {
                    return true;
                }
            }

            //Lấy quyền người dùng
            var query = from n in _UserRoleRepository.Table
                        where n.UserId == userid
                        select n;

            if (query.Any() == false)
            {
                return await Task.FromResult(false);
            }

            //Lấy danh sách menu theo các quyền
            var roles = query.Select(n => n.RoleId).ToList();

            var k = from n in _RoleMenuRepository.Table
                    where roles.Contains(n.RoleId) && n.MenuId == menuid
                    select n;

            //Kiểm tra
            return k.FirstOrDefault() != null ? await Task.FromResult(true) : await Task.FromResult(false);
        }

        public async Task<MessageReport> Login(AuthModel_LowSecurity model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                //Kiểm tra tk
                var objUser = GetByUsername(model.Username);
                if (objUser == null)
                {
                    result = new MessageReport(false, "Tài khoản không tồn tại");
                    return await Task.FromResult(result);
                }

                //Kiểm tra trạng thái
                if (objUser.Active == false)
                {
                    result = new MessageReport(false, "Tài khoản đang bị khóa");
                    return await Task.FromResult(result);
                }

                //Kiểm tra mk
                //var pass = CryptoHelper.DecryptPass_User(objUser.Password, objUser.PasswordSalat);
                //if (pass != model.Password)
                //{
                //    result = new MessageReport(false, "Mật khẩu không khớp");
                //    return await Task.FromResult(result);
                //}
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var pass = model.Password.PasswordHashed(objUser.PasswordSalat);

                    if (objUser.Password != pass)
                    {
                        result = new MessageReport(false, "Mật khẩu không khớp");
                        return await Task.FromResult(result);
                    }
                }

                //Tạo token
                var token = ApiHelper.GenerateJSON_MobileToken(objUser.Id);

                result = new MessageReport(true, token);
            }
            catch (System.Exception ex)
            {
                result = new MessageReport(false, string.Format("Message: {0} - Details: {1}", ex.Message, ex.InnerException != null ? ex.InnerException.Message : ""));
            }

            return await Task.FromResult(result);
        }
    }
}
