using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using Kztek_Library.Configs;
using Kztek_Library.Extensions;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Kztek_Web.Attributes
{
    public class CheckSessionCookie : Attribute, IAuthorizationFilter
    {
        IHttpContextAccessor HttpContextAccessor;
        private string AreaCode;
        private bool isSupport;
        public CheckSessionCookie()
        {
            HttpContextAccessor = new HttpContextAccessor();
        }

        public CheckSessionCookie(string AreaCode,bool isSupport = false)
        {
            HttpContextAccessor = new HttpContextAccessor();
            this.AreaCode = AreaCode;
            this.isSupport = isSupport;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //string sessionValue = HttpContextAccessor.HttpContext.Session.GetString(SessionConfig.Kz_Language);
            //sessionValue = String.IsNullOrEmpty(sessionValue) ? "vi" : sessionValue;
            //LanguageHelper.GetLang(sessionValue);

            //Kiểm tra nếu AreaCode = Admin thì cần check login riêng
            if (isSupport)
            {
                var currentAdmin = SessionCookieHelper.CurrentAdmin(HttpContextAccessor.HttpContext).Result;
                if (currentAdmin == null)
                {
                    context.Result = new RedirectResult("/Admin/Login/Index");
                    return;
                }
            }
            else
            {
                //Kiểm tra hệ thống có sử dụng hệ thống này ko
                if (!string.IsNullOrWhiteSpace(AreaCode))
                {
                    var groups = AppSettingHelper.GetStringFromAppSetting("FunctionGroupAllow").Result;

                    var list = StaticList.GroupMenuList().Where(n => groups.Contains(n.ItemValue)).ToList();

                    var objArea = list.FirstOrDefault(n => n.AreaName == AreaCode);
                    if (objArea == null)
                    {
                        context.Result = new RedirectResult("/Home/NotAuthorize");
                        return;
                    }
                }

                //Check có session / cookie
                var currentUser = SessionCookieHelper.CurrentUser(HttpContextAccessor.HttpContext).Result;
                if (currentUser == null)
                {
                    context.Result = new RedirectResult("/Login/Index");
                    return;
                }

                //Check tk tồn tại trong db
                var cmdq = string.Format("SELECT * FROM dbo.[user] WHERE Id = '{0}'", currentUser.UserId);

                var result = Kztek_Library.Helpers.DatabaseHelper.ExcuteCommandToBool(cmdq);

                if (result == false)
                {
                    context.Result = new RedirectResult("/Login/Index");
                    return;
                }

                //Check quyền
                if (currentUser.isAdmin == false)
                {
                    //Id menu hiện tại
                    var controller = (string)context.RouteData.Values["Controller"];
                    var action = (string)context.RouteData.Values["Action"];

                    var modelCache = AuthHelper.MenuFunctionByUserId(currentUser, HttpContextAccessor.HttpContext).Result;

                    if (!modelCache.Any(n => n.ControllerName == controller && n.ActionName == action))
                    {
                        context.Result = new RedirectResult("/Home/NotAuthorize");
                        return;
                    }
                }
            }


        }
    }
}