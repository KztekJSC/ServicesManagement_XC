using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Kztek_Web.Controllers
{
    public class tblLogController : Controller
    {
        private ItblLogService _tblLogService;
        private IUserService _UserService;

        public tblLogController(ItblLogService _tblLogService, IUserService _UserService)
        {
            this._tblLogService = _tblLogService;
            this._UserService = _UserService;
        }
        public async Task<SelectListModel_Multi> GetUser_Multi(string id, string selecteds = "")
        {
            var list = new List<SelectListModel> { };
            var lst = await _UserService.GetAll();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.Username.ToString(), ItemText = item.Username });
                }
            }

            var a = new SelectListModel_Multi
            {
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = id,
                Selecteds = !string.IsNullOrEmpty(selecteds) ? selecteds : "",
                Data = list
            };

            return a;
        }
        public async Task<SelectListModel_Multi> GetAction_Multi(string id, string selecteds = "")
        {
            var a = new SelectListModel_Multi
            {
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = id,
                Selecteds = !string.IsNullOrEmpty(selecteds) ? selecteds : "",
                Data = await StaticList.ActionLog()
            };

            return a;
        }
        [CheckSessionCookie]
        public async Task<IActionResult> Index(string key, string user, string actions, string fromdate, string todate, int page = 1, string group = "", string AreaCode = "")
        {
            var name = FunctionHelper.getCurrentGroup(group);

            if (string.IsNullOrWhiteSpace(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrWhiteSpace(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            var pageSize = 20;

            //Lấy danh sách phân trang
            var gridModel = await _tblLogService.GetAllPagingByFirst(key, user, actions, fromdate, todate, page, pageSize, name);

            //Đổ lên grid
            

            ViewBag.keyValue = key;
            ViewBag.actionsValue = actions;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.AreaCodeValue = AreaCode;
            ViewBag.ActionDT = await GetAction_Multi("ddlactions", actions);

            ViewBag.UserDT = await GetUser_Multi("ddluser", user);
            ViewBag.UserId = user;
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Index", this.HttpContext);
            return View(gridModel);
        }
    }
}