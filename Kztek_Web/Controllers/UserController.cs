using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kztek_Web.Models;
using Kztek_Service.Admin;
using Kztek_Model.Models;
using Kztek_Library.Helpers;
using Kztek_Library.Security;
using Kztek.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Kztek_Web.Attributes;
using System.IO;
using OfficeOpenXml;
using System.Globalization;
using Kztek_Library.Models;
using Kztek_Library.Configs;
using OfficeOpenXml.Style;
using Kztek_Library.Extensions;

namespace Kztek_Web.Controllers
{
    public class UserController : Controller
    {
        private IUserService _UserService;
        private IRoleService _RoleService;
        private IHostingEnvironment _hostingEnvironment;

        public UserController(IUserService _UserService, IRoleService _RoleService, IHostingEnvironment _hostingEnvironment)
        {
            this._UserService = _UserService;
            this._RoleService = _RoleService;
            this._hostingEnvironment = _hostingEnvironment;
        }

        [CheckSessionCookie]
        public async Task<IActionResult> Index(string key= "",int page = 1, string export = "0", string AreaCode = "")
        {
            var gridmodel = await _UserService.GetPaging(key, page, 10);

            ViewBag.keyValue = key;
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("User", this.HttpContext);
            ViewBag.AreaCodeValue = AreaCode;

            if (export == "1")
            {
                await ExportFile(this.HttpContext);

                //return View(gridmodel);
            }
            ViewBag.Roles = await _RoleService.GetAllUserRoles();
            return View(gridmodel);
        }

        // public IActionResult Export()
        // {
        //     ();
        //     return Json("");

        //     // return File(
        //     //     fileContents: data,
        //     //     contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //     //     fileDownloadName: excelName
        //     // );

        //     //return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        // }

        private async Task<bool> ExportFile(HttpContext context)
        {
            //column header
            var Data_ColumnHeader = new List<SelectListModel_Print_Column_Header>();
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "ID" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Họ tên" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Tài khoản" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "MK" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "MKSA" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Là admin" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Hoạt động" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Ảnh" });

            //
            var printConfig = PrintHelper.Template_Excel_V1(PrintConfig.HeaderType.TwoColumns, "Danh sách người dùng", DateTime.Now, SessionCookieHelper.CurrentUser(this.HttpContext).Result, "Kztek", Data_ColumnHeader, 4, 5, 5);

            //
            var gridmodel = await _UserService.GetPaging("", 1, 10);

            return await PrintHelper.Excel_Write<User>(context, gridmodel.Data, "User_" + DateTime.Now.ToString("ddMMyyyyHHmmss"), printConfig);
        }

        [CheckSessionCookie]
        [HttpGet]
        public async Task<IActionResult> Create(User_Submit model, string AreaCode = "")
        {
            model = model == null ? new User_Submit() : model;
            model.Data_Role = await _RoleService.GetAllActiveOrder();
            ViewBag.AreaCodeValue = AreaCode;

            return View(model);
        }

        [CheckSessionCookie]
        [HttpPost]
        public async Task<IActionResult> Create(User_Submit model, bool SaveAndCountinue = false, string AreaCode = "")
        {
            model.Data_Role = await _RoleService.GetAllActiveOrder();
            ViewBag.AreaCodeValue = AreaCode;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //
            var existed = await _UserService.GetByUsername(model.Username);
            if (existed != null)
            {
                ModelState.AddModelError("Username", "Tài khoản tồn tại");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                model.Password = "123456";
            }
            else
            {
                if (model.Password != model.RePassword)
                {
                    ModelState.AddModelError("RePassword", "Mật khẩu không khớp");
                    return View(model);
                }
            }

            var obj = new User()
            {
                Active = model.Active,
                Id = Guid.NewGuid().ToString(),
                Password = model.Password,
                PasswordSalat = Guid.NewGuid().ToString(),
                Name = model.Name,
                Username = model.Username,
                Admin = model.isAdmin
            };

            if (!string.IsNullOrWhiteSpace(model.RoleIds))
            {
                var ks = model.RoleIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                model.Roles = new List<string>();
                foreach (var item in ks)
                {
                    model.Roles.Add(item);
                }

                foreach (var item in model.Roles)
                {
                    var t = new UserRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = item,
                        UserId = obj.Id
                    };

                    await _RoleService.CreateMap(t);
                }
            }

            //Mã hóa pass
            //obj.Password = CryptoHelper.EncryptPass_User(obj.Password, obj.PasswordSalat);
            obj.Password = obj.Password.PasswordHashed(obj.PasswordSalat);

            //Thực hiện thêm mới
            var result = await _UserService.Create(obj);
            if (result.isSuccess)
            {
                if (SaveAndCountinue)
                {
                    TempData["Success"] = "Thêm mới thành công";
                    return RedirectToAction("Create", new { AreaCode = AreaCode });
                }

                return RedirectToAction("Index", new { AreaCode = AreaCode });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        [CheckSessionCookie]
        [HttpGet]
        public async Task<IActionResult> Update(string id, string AreaCode = "")
        {
            var model = _UserService.GetCustomById(id);
            model.Data_Role = await _RoleService.GetAllActiveOrder();
            ViewBag.AreaCodeValue = AreaCode;

            return View(model);
        }

        [CheckSessionCookie]
        [HttpPost]
        public async Task<IActionResult> Update(User_Submit model, string AreaCode = "")
        {
            model.Data_Role = await _RoleService.GetAllActiveOrder();
            ViewBag.AreaCodeValue = AreaCode;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var oldObj = await _UserService.GetById(model.Id);
            if (oldObj == null)
            {
                ModelState.AddModelError("", "Bản ghi không tồn tại");
                return View(model);
            }

            //
            var existed = await _UserService.GetByUsername_notId(model.Username, model.Id);
            if (existed != null)
            {
                ModelState.AddModelError("Username", "Tài khoản tồn tại");
                return View(model);
            }

            oldObj.Active = model.Active;
            oldObj.Name = model.Name;
            oldObj.Username = model.Username;
            oldObj.Admin = model.isAdmin;

            //Kiểm tra mật khẩu mới
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (model.Password != model.RePassword)
                {
                    ModelState.AddModelError("RePassword", "Mật khẩu không khớp");
                    return View(model);
                }

                //Sinh mã salat mới
                oldObj.PasswordSalat = Guid.NewGuid().ToString();

                //
                //oldObj.Password = CryptoHelper.EncryptPass_User(model.Password, oldObj.PasswordSalat);
                oldObj.Password = model.Password.PasswordHashed(oldObj.PasswordSalat);
            }

            //
            await _RoleService.DeleteMap(oldObj.Id);

            if (!string.IsNullOrWhiteSpace(model.RoleIds))
            {
                var ks = model.RoleIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                model.Roles = new List<string>();
                foreach (var item in ks)
                {
                    model.Roles.Add(item);
                }

                foreach (var item in model.Roles)
                {
                    var t = new UserRole()
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = item,
                        UserId = oldObj.Id
                    };

                    await _RoleService.CreateMap(t);
                }
            }

            var result = await _UserService.Update(oldObj);
            if (result.isSuccess)
            {
                return RedirectToAction("Index", new { AreaCode = AreaCode });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
        }

        [CheckSessionCookie]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _UserService.Delete(id);

            return Json(result);
        }

        [HttpGet]
        public IActionResult AccountInfo(string id, string AreaCode = "")
        {
            var model = _UserService.GetCustomById(id);

            ViewBag.AreaCodeValue = AreaCode;

            return View(model);
        }

        [HttpPost]
        public IActionResult AccountInfo(User_Submit model, IFormFile FileAvatar, string AreaCode = "")
        {
            ViewBag.AreaCodeValue = AreaCode;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var oldObj = _UserService.GetById(model.Id).Result;
            if (oldObj == null)
            {
                ModelState.AddModelError("", "Tài khoản không tồn tại");
                return View(model);
            }

            var existedUser = _UserService.GetByUsername_notId(model.Username, model.Id).Result;
            if (existedUser != null)
            {
                ModelState.AddModelError("", "Tài khoản đã tồn tại");
                return View(model);
            }

            //Có đổi mật khẩu
            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                if (string.IsNullOrWhiteSpace(model.OldPassword))
                {
                    ModelState.AddModelError("", "Vui lòng nhập mật khẩu cũ");
                    return View(model);
                }

                if (model.Password != model.RePassword)
                {
                    ModelState.AddModelError("", "Vui lòng nhập lại chính xác mật khẩu mới");
                    return View(model);
                }

                //Tiến hành check mật khẩu cũ
                //var oldpass = CryptoHelper.DecryptPass_User(oldObj.Password, oldObj.PasswordSalat);
                var oldpass = oldObj.Password;

                if (oldpass != model.OldPassword.PasswordHashed(oldObj.PasswordSalat))
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không chính xác");
                    return View(model);
                }

                //Tạo mk mới
                oldObj.PasswordSalat = Guid.NewGuid().ToString();
                //oldObj.Password = CryptoHelper.EncryptPass_User(model.Password, oldObj.PasswordSalat);
                oldObj.Password = model.Password.PasswordHashed(oldObj.PasswordSalat);
            }

            //Có upload file ảnh lên
            if (FileAvatar != null)
            {
                var filePath = _hostingEnvironment.WebRootPath + "/uploads";
                var res = UploadHelper.UploadFile(FileAvatar, filePath).Result;

                if (res.isSuccess == false)
                {
                    return View(model);
                }

                model.Avatar = "/uploads/" + FileAvatar.FileName;
                oldObj.UserAvatar = "/uploads/" + FileAvatar.FileName;
            }

            oldObj.Username = model.Username;
            oldObj.Name = model.Name;

            var result = _UserService.Update(oldObj).Result;
            if (result.isSuccess)
            {
                TempData["Success"] = "Cập nhật thành công";
                return RedirectToAction("AccountInfo", "User", new { id = oldObj.Id, AreaCode = "Parking" });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
        }
    }
}
