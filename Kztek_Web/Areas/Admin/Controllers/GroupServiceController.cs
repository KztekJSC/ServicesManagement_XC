using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Service.Admin;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class GroupServiceController : Controller
    {
        private IGroupServieService _GroupServieService;
        public GroupServiceController(IGroupServieService _GroupServieService)
        {
            this._GroupServieService = _GroupServieService;
        }
        public async Task<IActionResult> Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {

            var gridmodel = await _GroupServieService.GetAllCustomPagingByFirst(key, pc, page, 20);
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("GroupService", this.HttpContext);
            ViewBag.keyValue = key;
            return View(gridmodel);
        }


    }
}
