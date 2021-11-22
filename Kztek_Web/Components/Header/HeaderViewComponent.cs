using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Service.Admin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kztek_Web.Components.Header
{
    public class HeaderViewComponent : ViewComponent
    {
        public HeaderViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await SessionCookieHelper.CurrentUser(HttpContext);

            //kiểm tra session xem có lưu ngôn ngữ không nếu có thì lấy không mặc định là "vi"
            string sessionValue = HttpContext.Session.GetString(SessionConfig.Kz_Language);
            if (string.IsNullOrWhiteSpace(sessionValue))
                sessionValue = HttpContext.Request.Cookies[CookieConfig.Kz_LanguageCookie];
            sessionValue = String.IsNullOrEmpty(sessionValue) ? "vi" : sessionValue;

            ViewBag.Lstlanguage = new SelectListModel_Chosen
            {
                Data = await StaticList.ListLanguage(),
                IdSelectList = "langCode_Parking",
                isMultiSelect = false,
                Selecteds = !string.IsNullOrEmpty(sessionValue) ? sessionValue : "vi"
            };

            return View(user);
        }
    }
}