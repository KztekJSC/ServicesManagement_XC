using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kztek_Web.Models;
using Kztek_Service.Admin;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Library.Configs;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Kztek_Library.Security;
using Kztek_Library.Helpers;
using Kztek_Library.Functions;
using Kztek.Security;
using System.IO;
using Kztek_Library.Extensions;

namespace Kztek_Web.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _UserService;

        public LoginController(IUserService _UserService)
        {
            this._UserService = _UserService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await _UserService.GetAll();

            var model = new AuthModel();
            model.isAny = data.Any();
            model.Data_Service = Data_Service();
            model.AreaCode = "Admin";
            updateDatabase();
            //kiểm tra session xem có lưu ngôn ngữ không nếu có thì lấy không mặc định là "vi"
            string sessionValue = HttpContext.Session.GetString(SessionConfig.Kz_Language);
            if (string.IsNullOrWhiteSpace(sessionValue))
                sessionValue = HttpContext.Request.Cookies[CookieConfig.Kz_LanguageCookie];
            sessionValue = String.IsNullOrEmpty(sessionValue) ? "vi" : sessionValue;
            LanguageHelper.GetLang(sessionValue);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AuthModel model)
        {
            var data = await _UserService.GetAll();

            model.isAny = data.Any();
            model.Data_Service = Data_Service();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var objUser = new User();
            var result = _UserService.Login(model, out objUser).Result;

            if (result.isSuccess)
            {
                Session_Cookie(model, objUser);
                CookieAreaLastLogin(model.AreaCode);
                await LogHelper.WriteLog(objUser.Id, ActionConfig.Login,"User", JsonConvert.SerializeObject(objUser.Name + " đăng nhập thành công"), HttpContext);
                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
        }
        // private void CacheMenuFunctionByUser(SessionModel model)
        // {

        //     AuthHelper.MenuFunctionByUserId(model);
        // }

        private void CookieAreaLastLogin(string areaCode)
        {
            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Response.Cookies.Append(CookieConfig.Kz_AreaCookie, areaCode);
        }

        private void Session_Cookie(AuthModel model, User user)
        {
            var host = Request.Host.Host;

            //Session
            var userSession = new SessionModel
            {
                UserId = user.Id,
                Name = user.Name,
                Username = user.Username,
                Avatar = user.UserAvatar,
                isAdmin = user.Admin
            };


            //CacheMenuFunctionByUser(userSession);

            //Mã hóa
            var serializeModel = JsonConvert.SerializeObject(userSession);
            var encryptModel = CryptoHelper.EncryptSessionCookie_User(serializeModel);

            //Lưu lại trong session
            HttpContext.Session.SetString(SessionConfig.Kz_UserSession, encryptModel);

            //Kiểm tra có lưu cookie
            if (model.isRemember)
            {
                var option = new CookieOptions();
                option.Expires = DateTime.Now.AddMonths(1);
                HttpContext.Response.Cookies.Append(CookieConfig.Kz_UserCookie, encryptModel);
            }
        }

        public IActionResult Logout(string userid)
        {
            HttpContext.Session.Remove(SessionConfig.Kz_UserSession);
            HttpContext.Response.Cookies.Delete(CookieConfig.Kz_UserCookie);

            CacheFunction.Clear(this.HttpContext, string.Format(CacheConfig.Kz_User_MenuFunctionCache_Key, userid, SecurityModel.Cache_Key));

            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterModel();
            model.isAny = _UserService.GetAll().Result.Any();
            if (model.isAny)
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            model.isAny = _UserService.GetAll().Result.Any();
            if (model.isAny)
            {
                return RedirectToAction("Index", "Login");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Kiểm tra mật khẩu
            if (model.Password != model.RePassword)
            {
                ModelState.AddModelError("", "Mật khẩu không khớp");
                return View(model);
            }

            var salat = Guid.NewGuid().ToString();

            var obj = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Username = model.Username,
                Active = true,
                UserAvatar = "",
                Password = model.Password.PasswordHashed(salat),
                PasswordSalat = salat,
                Admin = true
            };

            var result = _UserService.Create(obj).Result;

            if (result.isSuccess)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
        }


        public IActionResult ChangeLanguage(string lang)
        {
            var host = Request.Host.Host;

            //Lưu lại trong session
            HttpContext.Session.SetString(SessionConfig.Kz_Language, lang);

            //Kiểm tra có lưu cookie

            var option = new CookieOptions();
            option.Expires = DateTime.Now.AddMonths(1);
            HttpContext.Response.Cookies.Append(CookieConfig.Kz_LanguageCookie, lang);
            LanguageHelper.GetLang(lang);

            //kiểm tra session/cookies xem đã  lưu ngôn ngữ chưa
            string sessionValue = HttpContext.Session.GetString(SessionConfig.Kz_Language);
            if (string.IsNullOrWhiteSpace(sessionValue))
                sessionValue = HttpContext.Request.Cookies[CookieConfig.Kz_LanguageCookie];

            return Json(new { success = !String.IsNullOrEmpty(sessionValue) ? true : false });
        }

        private List<SelectListModel> Data_Service()
        {
            //Lấy danh sách nhóm được sử dụng
            var groups = AppSettingHelper.GetStringFromAppSetting("FunctionGroupAllow").Result;

            //
            var data = new List<SelectListModel>();

            var list = StaticList.GroupMenuList().Where(n => groups.Contains(n.ItemValue)).ToList();

            foreach (var item in list)
            {
                data.Add(new SelectListModel()
                {
                    ItemText = item.ItemText,
                    ItemValue = item.AreaName
                });
            }

            return data;
        }

        private void updateDatabase()
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "sqlscript", "Script.sql");
            //thêm trường
            string script01 = System.IO.File.ReadAllText(file);

            DatabaseHelper.ExcuteCommandToBool(script01);
        }
    }
}
