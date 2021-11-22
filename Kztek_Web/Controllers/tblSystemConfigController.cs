using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Kztek_Web.Controllers
{
    public class tblSystemConfigController : Controller
    {
        private ItblSystemConfigService _tblSystemConfigService;

        public tblSystemConfigController(ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string AreaCode = "")
        {
            ViewBag.AreaCodeValue = AreaCode;

            var obj = await _tblSystemConfigService.GetDefault();

            if (obj == null)
            {
                obj = new tblSystemConfig();
                obj.SystemConfigID = Guid.NewGuid().ToString();
                obj.FeeName = "FUTECH";
                obj.SortOrder = 1;
                obj.DelayTime = 0;

                var result = await _tblSystemConfigService.Create(obj);
            }

            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Index(tblSystemConfig obj, string AreaCode = "")
        {
            ViewBag.AreaCodeValue = AreaCode;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = await _tblSystemConfigService.GetDefault();

            //Gán
            oldObj.Address = obj.Address;
            oldObj.Company = obj.Company;
            oldObj.DelayTime = obj.DelayTime;
            oldObj.EnableAlarmMessageBox = obj.EnableAlarmMessageBox;
            oldObj.EnableAlarmMessageBoxIn = obj.EnableAlarmMessageBoxIn;
            oldObj.EnableDeleteCardFailed = obj.EnableDeleteCardFailed;
            oldObj.EnableSoundAlarm = obj.EnableSoundAlarm;
            oldObj.Fax = obj.Fax;
            oldObj.FeeName = obj.FeeName;
            oldObj.KeyA = obj.KeyA;
            oldObj.KeyB = obj.KeyB;
            oldObj.Logo = !string.IsNullOrWhiteSpace(obj.Logo) ? obj.Logo : "";
            oldObj.Para1 = obj.Para1;
            oldObj.Para2 = obj.Para2;
            oldObj.SystemCode = obj.SystemCode;
            oldObj.Tax = !string.IsNullOrWhiteSpace(obj.Tax) ? obj.Tax : "";
            oldObj.Tel = !string.IsNullOrWhiteSpace(obj.Tel) ? obj.Tel : "";
            oldObj.ImagePath = obj.ImagePath;

            var result = await _tblSystemConfigService.Update(oldObj);
            if (result.isSuccess)
            {
                TempData["Success"] = result.Message;
                return View(obj);
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }
    }
}
