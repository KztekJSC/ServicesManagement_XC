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

namespace Kztek_Service.Api.Database.MYSQL
{
    public class UserService
    {
        private IUserRepository _UserRepository;
        private IUserRoleRepository _UserRoleRepository;

        public UserService(IUserRepository _UserRepository, IUserRoleRepository _UserRoleRepository)
        {
            this._UserRepository = _UserRepository;
            this._UserRoleRepository = _UserRoleRepository;
        }

        public async Task<List<User>> GetAll()
        {
            var data = _UserRepository.Table;
            return await Task.FromResult(data.ToList());
        }

        public async Task<GridModel<User>> GetPaging(string key, int pageNumber, int pageSize)
        {

            var query = from n in _UserRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLower();
                query = query.Where(n => n.Name.ToLower().Contains(key) || n.Username.ToLower().Contains(key));
            }

            var pageList = query.ToPagedList(pageNumber, pageSize);

            //
            // var model = new GridModel<User>()
            // {
            //     Data = pageList.ToList(),
            //     PageIndex = pageNumber,
            //     PageSize = pageSize,
            //     TotalIem = pageList.TotalItemCount,
            //     TotalPage = pageList.PageCount
            // };

            var model = GridModelHelper<User>.GetPage(pageList.ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }

        public async Task<User> GetById(string id)
        {
            return await _UserRepository.GetOneById(id);
        }

        public async Task<User> GetByUsername(string username)
        {
            var query = from n in _UserRepository.Table
                        where n.Username == username
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<User> GetByUsername_notId(string username, string id)
        {
            var query = from n in _UserRepository.Table
                        where n.Username == username && n.Id != id
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public User_Submit GetCustomById(string id)
        {
            var model = new User_Submit();

            var obj = _UserRepository.GetOneById(id).Result;
            if (obj != null)
            {
                model = GetCustomByModel(obj);
            }

            return model;
        }

        public User_Submit GetCustomByModel(User model)
        {
            var obj = new User_Submit()
            {
                Id = model.Id,
                Active = model.Active,
                Name = model.Name,
                Roles = new List<string>(),
                Username = model.Username,
                isAdmin = model.Admin,
                Avatar = model.UserAvatar
            };

            obj.Roles = (from n in _UserRoleRepository.Table
                         where n.UserId == model.Id
                         select n.RoleId).ToList();

            return obj;
        }

        public async Task<MessageReport> Create(User model)
        {
            return await _UserRepository.Add(model);
        }

        public async Task<MessageReport> Update(User model)
        {
            return await _UserRepository.Update(model);
        }

        public async Task<MessageReport> Delete(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _UserRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return await Task.FromResult(result);
        }

        public Task<MessageReport> Login(AuthModel model, out User user)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                //Kiểm tra username
                var objUser = GetByUsername(model.Username).Result;
                if (objUser == null)
                {
                    user = null;
                    result = new MessageReport(false, "Tài khoản không tồn tại");
                    return Task.FromResult(result);
                }

                //Giải mã
                //var pass = CryptoHelper.DecryptPass_User(objUser.Password, objUser.PasswordSalat);
                var pass = objUser.Password;

                //Check mật khẩu
                if (pass != model.Password.PasswordHashed(objUser.PasswordSalat))
                {
                    user = null;
                    result = new MessageReport(false, "Mật khẩu không khớp");
                    return Task.FromResult(result);
                }

                //Gán lại user
                user = objUser;
                result = new MessageReport(true, "Đăng nhập thành công");
            }
            catch (System.Exception ex)
            {
                user = null;
                result = new MessageReport(false, ex.Message);
            }

            return Task.FromResult(result);
        }
    }
}
