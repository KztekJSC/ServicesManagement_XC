using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek.Security;
using Kztek_Library.Configs;
using Kztek_Library.Functions;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;

namespace Kztek_Library.Helpers
{
    public class AuthHelper
    {
        public static Task<AuthActionModel> CheckAuthAction(string controller, HttpContext context, string area = "")
        {
            var Auth = new AuthActionModel();

            //Get list auth of current user
            var currentUser = SessionCookieHelper.CurrentUser(context).Result;
            if (currentUser != null)
            {
                //Get list all menufunction by controller
                var menus = MenuFunctionByUserId(currentUser, context, area).Result;

                if (currentUser.isAdmin)
                {
                    Auth.Create_Auth = 1;
                    Auth.Update_Auth = 1;
                    Auth.Delete_Auth = 1;
                    Auth.Remove_Auth = 1;
                    Auth.LockCard_Auth = 1;
                    Auth.OpenCard_Auth = 1;
                    Auth.DeleteCard_Auth = 1;
                }
                else
                {
                    //Action Create
                    var objCreate = menus.FirstOrDefault(n => n.ControllerName == controller && n.ActionName == "Create");
                    if (objCreate != null)
                    {
                        Auth.Create_Auth = 1;
                    }

                    //Action Update
                    var objUpdate = menus.FirstOrDefault(n => n.ControllerName == controller && n.ActionName == "Update");
                    if (objUpdate != null)
                    {
                        Auth.Update_Auth = 1;
                    }

                    //Action Delete
                    var objDelete = menus.FirstOrDefault(n => n.ControllerName == controller && n.ActionName == "Delete");
                    if (objDelete != null)
                    {
                        Auth.Delete_Auth = 1;
                    }

                    //Action Delete //xóa không điều kiện
                    var objRemove = menus.FirstOrDefault(n => n.ControllerName == controller && n.ActionName == "Remove");
                    if (objRemove != null)
                    {
                        Auth.Remove_Auth = 1;
                    }

                    //khóa thẻ
                    var objLockCard = menus.FirstOrDefault(n => n.ControllerName == controller && n.ActionName == "LockCard");
                    if (objLockCard != null)
                    {
                        Auth.LockCard_Auth = 1;
                    }

                    //mở thẻ
                    var objOpenCard = menus.FirstOrDefault(n => n.ControllerName == controller && n.ActionName == "OpenCard");
                    if (objOpenCard != null)
                    {
                        Auth.OpenCard_Auth = 1;
                    }

                    //xóa thẻ
                    var objDeleteCard = menus.FirstOrDefault(n => n.ControllerName == controller && n.ActionName == "DeleteCard");
                    if (objDeleteCard != null)
                    {
                        Auth.DeleteCard_Auth = 1;
                    }
                }


            }

            return Task.FromResult(Auth);
        }

        public static Task<List<MenuFunction>> MenuFunctionByUserId(SessionModel user, HttpContext context, string area = "")
        {
            //
            var identify = string.Format(CacheConfig.Kz_User_MenuFunctionCache_Key, user.UserId, SecurityModel.Cache_Key);

            var modelCache = new List<MenuFunction>();

            //var cache = context.RequestServices.GetService<IMemoryCache>();

            var existed = CacheFunction.TryGet<List<MenuFunction>>(context, identify, out modelCache);

            //var k = cache.Get<List<SY_MenuFunction>>(identify);

            if (existed == false)
            {

                if (user.isAdmin)
                {
                    var cmdMenus = "SELECT * FROM menufunction WHERE Active = 1";

                    if (!string.IsNullOrWhiteSpace(area))
                    {
                        cmdMenus += string.Format(" AND MenuGroupListId LIKE '%{0}%'", area);
                    }

                    modelCache = Kztek_Library.Helpers.DatabaseHelper.ExcuteCommandToList<MenuFunction>(cmdMenus);
                }
                else
                {
                    var cmdRole = string.Format("SELECT * FROM userrole WHERE UserId = '{0}'", user.UserId);

                    var roles = Kztek_Library.Helpers.DatabaseHelper.ExcuteCommandToList<UserRole>(cmdRole);

                    var str_roles = new List<string>();
                    foreach (var item in roles)
                    {
                        str_roles.Add(string.Format("'{0}'", item.RoleId));
                    }

                    //Danh sách menu của tài khoản với roleids = > danh sách menu
                    var cmdRoleMenus = string.Format("SELECT * FROM rolemenu WHERE RoleId IN ({0})", roles.Any() ? string.Join(",", str_roles) : "'0'");

                    var rolemenus = Kztek_Library.Helpers.DatabaseHelper.ExcuteCommandToList<RoleMenu>(cmdRoleMenus);

                    //Lấy danh sách menu quyền
                    var menuids = "";
                    var count1 = 0;
                    foreach (var item in rolemenus)
                    {
                        count1++;
                        menuids += string.Format("'{0}'{1}", item.MenuId, count1 == rolemenus.Count ? "" : ",");
                    }

                    var cmdMenus = string.Format("SELECT * FROM menufunction WHERE Active = 1 AND Id IN ({0})", string.IsNullOrWhiteSpace(menuids) ? "'0'" : menuids);

                    if (!string.IsNullOrWhiteSpace(area))
                    {
                        cmdMenus += string.Format(" AND MenuGroupListId LIKE '%{0}%'", area);
                    }

                    modelCache = Kztek_Library.Helpers.DatabaseHelper.ExcuteCommandToList<MenuFunction>(cmdMenus);
                }

                //Save lại vào cache
                if (modelCache == null)
                {
                    modelCache = new List<MenuFunction>();
                }

                CacheFunction.Add<List<MenuFunction>>(context, identify, modelCache, CacheConfig.Kz_User_MenuFunctionCache_Time);
                //cache.Set<List<SY_MenuFunction>>(identify, modelCache, DateTime.Now.AddHours(8));
            }

            return Task.FromResult(modelCache);
        }
    }

}