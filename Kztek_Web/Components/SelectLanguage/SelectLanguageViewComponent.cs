using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Components.Language
{
    public class SelectLanguageViewComponent : ViewComponent
    {
        public SelectLanguageViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string path)
        {
            //kiểm tra session xem có lưu ngôn ngữ không nếu có thì lấy không mặc định là "vi"
            string sessionValue = HttpContext.Session.GetString(SessionConfig.Kz_Language);
            if (string.IsNullOrWhiteSpace(sessionValue))
                sessionValue = HttpContext.Request.Cookies[CookieConfig.Kz_LanguageCookie];
            sessionValue = String.IsNullOrEmpty(sessionValue) ? "vi" : sessionValue;

            ViewBag.Lstlanguage = new SelectListModel_Chosen
            {
                Data = await StaticList.ListLanguage(),
                IdSelectList = "langCode",
                isMultiSelect = false,
                Selecteds = !string.IsNullOrEmpty(sessionValue) ? sessionValue : "vi"
            };
            return View();
        }
    }
}
