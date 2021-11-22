using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Library.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class LoginController : Controller
    {
        public LoginController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new AuthModel();

            //kiểm tra session xem có lưu ngôn ngữ không nếu có thì lấy không mặc định là "vi"
            string sessionValue = HttpContext.Session.GetString(SessionConfig.Kz_Language);
            if (string.IsNullOrWhiteSpace(sessionValue))
                sessionValue = HttpContext.Request.Cookies[CookieConfig.Kz_LanguageCookie];
            sessionValue = String.IsNullOrEmpty(sessionValue) ? "vi" : sessionValue;
            LanguageHelper.GetLang(sessionValue);

            return View(await Task.FromResult(model));
        }

        [HttpPost]
        public async Task<IActionResult> Index(AuthModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Load tài khoản admin
            var username = await AppSettingHelper.GetStringFromAppSetting("Administrator:Username");
            var pass = await AppSettingHelper.GetStringFromAppSetting("Administrator:Password");

            if (!model.Username.Equals(username))
            {
                ModelState.AddModelError("Username", "Tên tài khoản không chính xác");
                return View(model);
            }

            //Giải mã pass
            var decryptPass = CryptoHelper.DecryptPass_Admin(pass);
            if (!model.Password.Equals(decryptPass))
            {
                ModelState.AddModelError("Password", "Mật khẩu không chính xác");
                return View(model);
            }

            //Lưu session
            Session_Cookie(model);

            return RedirectToAction("Index", "Support", new { Area = "Admin" });
        }

        private void Session_Cookie(AuthModel model)
        {
            var host = Request.Host.Host;

            //Session
            var userSession = new SessionModel
            {
                UserId = "Administrator",
                Name = "Administrator",
                Username = model.Username,
                Avatar = "",
                isAdmin = true
            };


            //CacheMenuFunctionByUser(userSession);

            //Mã hóa
            var serializeModel = JsonConvert.SerializeObject(userSession);
            var encryptModel = CryptoHelper.EncryptSessionCookie_Admin(serializeModel);

            //Lưu lại trong session
            HttpContext.Session.SetString(SessionConfig.Kz_AdminSession, encryptModel);

        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionConfig.Kz_AdminSession);

            return RedirectToAction("Index", "Login", new { Area = "" });
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
    }
}
