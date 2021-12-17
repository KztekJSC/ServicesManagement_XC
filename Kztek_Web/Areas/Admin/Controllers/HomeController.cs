using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kztek_Web.Models;
using Kztek_Service.Admin;
using Kztek_Library.Models;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Microsoft.AspNetCore.SignalR;
using Kztek_Web.Hubs;
using System.Data.SqlClient;
using Kztek_Model.Models;
using Newtonsoft.Json;
using System.Data;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class HomeController : Controller
    {
        private Itbl_EventService _tbl_EventService;
        private IGroupService _GroupService;
        private IServiceService _ServiceService;
        private IHomeService _HomeService;

        public HomeController(Itbl_EventService _tbl_EventService, IGroupService _GroupService, IHomeService _HomeService, IServiceService _ServiceService)
        {
            this._HomeService = _HomeService;
            this._tbl_EventService = _tbl_EventService;
            this._GroupService = _GroupService;
            this._ServiceService = _ServiceService;
        }

        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string Groupid = "", string key = "", int page = 1, string AreaCode = "")
        {
            ////kiểm tra session xem có lưu ngôn ngữ không nếu có thì lấy không mặc định là "vi"
            //string sessionValue = HttpContext.Session.GetString(SessionConfig.Kz_Language);
            //if (string.IsNullOrWhiteSpace(sessionValue))
            //    sessionValue = HttpContext.Request.Cookies[CookieConfig.Kz_LanguageCookie];
            //sessionValue = String.IsNullOrEmpty(sessionValue) ? "vi" : sessionValue;
            //LanguageHelper.GetLang(sessionValue);

            ViewBag.Groups = await _GroupService.GetaSelectModelChoseGroup(selecteds: Groupid);

            ViewBag.keyValue = key; 

            ViewBag.AreaCodeValue = AreaCode;

            return View();
        }
        public async Task<IActionResult> Partial_Data(string Groupid = "", string key = "", string fromdate = "", string todate = "", int page = 1)
        {


            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            var gridModel = await _HomeService.GetPagingInOut(key, page, 20, Groupid, fromdate, todate);                         

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Home", this.HttpContext);

            ViewBag.Groupid = Groupid;

            ViewBag.Groups = await _GroupService.GetAll();

            ViewBag.Service = await _ServiceService.GetAll();

            return PartialView(gridModel);
          
        }
        public async Task<IActionResult> Partial_TopService()
        {
          

            return PartialView();
        }


        public async Task<IActionResult> DashboardPartial(SelectListModel_Date model)
        {
          

            return PartialView();
        }
    }
}
