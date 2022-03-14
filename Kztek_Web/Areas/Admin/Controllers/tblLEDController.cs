using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin.Database;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class tblLEDController : Controller
    {
        private ItblLedService _tblLedService;
        public tblLEDController(ItblLedService _tblLedService)
        {
            this._tblLedService = _tblLedService;
        }

        #region Danh sách
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {

            var gridmodel = await _tblLedService.GetAllCustomPagingByFirst(key, pc, page, 20);

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("tblLED", this.HttpContext);
           
            ViewBag.keyValue = key;
            return View(gridmodel);
        }
        #endregion

        #region Thêm mới
        private async Task<SelectListModel_Chosen> GetListColumn(string selecteds, string id = "column")
        {
            var list = await GetListLed_Column();

            var newobj = new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT") };

            list.Insert(0, newobj);

            var model = new SelectListModel_Chosen
            {
                Data = list,
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = id,
                isMultiSelect = false,
                Selecteds = selecteds
            };

            return model;
        }

        private async Task<List<SelectListModel>> GetListLed_Column()
        {
            var list = new List<SelectListModel> { };
            var lst = await StaticList.GetListLed_Column();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.ItemValue, ItemText = item.ItemText });
                }
            }
            return list;
        }
        private async Task<SelectListModel_Chosen> GetListRow(string selecteds, string id = "row")
        {
            var list = await GetListLed_Row();

            var newobj = new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT") };

            list.Insert(0, newobj);

            var model = new SelectListModel_Chosen
            {
                Data = list,
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = id,
                isMultiSelect = false,
                Selecteds = selecteds
            };

            return model;
        }

        private async Task<List<SelectListModel>> GetListLed_Row()
        {
            var list = new List<SelectListModel> { };
            var lst = await StaticList.GetListLed_Row();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.ItemValue, ItemText = item.ItemText });
                }
            }
            return list;
        }
    

        private async Task<SelectListModel_Chosen> GetListLed_Function(string selecteds, string id = "FunctionLed")
        {
            var list = await GetListLed_Function();

            var newobj = new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT") };

            list.Insert(0, newobj);

            var model = new SelectListModel_Chosen
            {
                Data = list,
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = id,
                isMultiSelect = false,
                Selecteds = selecteds
            };

            return model;
        }

        private async Task<List<SelectListModel>> GetListLed_Function()
        {
            var list = new List<SelectListModel> { };
            var lst = await StaticList.GetListLed_Function();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.ItemValue, ItemText = item.ItemText });
                }
            }
            return list;
        }
        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>     
        [CheckSessionCookie(AreaConfig.Admin)]
        [HttpGet]
        public async Task<IActionResult> Create(tblLED_Submit model )
        {
            model = model == null ? new tblLED_Submit() : model;
            model.row = 32;
            model.column_Led = 48;
            ViewBag.LedFunction = await GetListLed_Function(model.FunctionLed);
            ViewBag.RowSelect = await GetListRow(model.row.ToString());
            ViewBag.ColumSelect = await GetListColumn(model.column_Led.ToString());
            return await Task.FromResult(View(model));
        }

    


        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="SaveAndCountinue">Tiếp tục thêm</param>
        /// <returns></returns>    
        [CheckSessionCookie(AreaConfig.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(tblLED_Submit model, string function_LED, string row_led, string column_led, bool SaveAndCountinue = false )
        {
            model.FunctionLed = function_LED;
            ViewBag.LedFunction = await GetListLed_Function(model.FunctionLed);
            ViewBag.RowSelect = await GetListRow(model.row.ToString());
            ViewBag.ColumSelect = await GetListColumn(model.column_Led.ToString());
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.led_Name))
            {
                ModelState.AddModelError("led_Name", "Nhập tên LED");
                return View(model);
            }

            var existed = await _tblLedService.GetByName(model.led_Name);
            if (existed != null)
            {
                ModelState.AddModelError("led_Name", "LED đã tồn tại");
                return View(model);
            }
            var obj = new tblLED();
            obj.id = Guid.NewGuid();
            obj.led_Code = model.led_Code;
            obj.led_Name = model.led_Name;
            obj.row = Convert.ToInt32(row_led);

            obj.column_Led = Convert.ToInt32(column_led);
            obj.ip_Address = model.ip_Address;
            obj.led_Function = Convert.ToInt32( function_LED);
            obj.description = model.description;
            obj.port = model.port;
            obj.controller_Type = model.controller_Type;
            
          
            //Thực hiện thêm mới
            var result = await _tblLedService.Create(obj);
            if (result.isSuccess)
            {
                //await LogHelper.WriteLog(model.id.ToString(), ActionConfig.Create, JsonConvert.SerializeObject(model), HttpContext);
                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }
        }





        #endregion

        #region Cập nhật

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionCookie(AreaConfig.Admin)]
        [HttpGet]
        public async Task<IActionResult> Update(string id, int pageNumber = 1)
        {
            var model = await _tblLedService.GetByCustomId(id);
            ViewBag.LedFunction = await GetListLed_Function(model.FunctionLed);
            ViewBag.RowSelect = await GetListRow(model.row.ToString());
            ViewBag.ColumSelect = await GetListColumn(model.column_Led.ToString());
            return View(model);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionCookie(AreaConfig.Admin)]
        [HttpPost]
        public async Task<IActionResult> Update(tblLED_Submit model,string function_LED, string row_led, string column_led, int pageNumber = 1)
        {
            model.FunctionLed = function_LED;
            ViewBag.LedFunction = await GetListLed_Function(model.FunctionLed);
            ViewBag.RowSelect = await GetListRow(model.row.ToString());
            ViewBag.ColumSelect = await GetListColumn(model.column_Led.ToString());
            var oldObj = await _tblLedService.GetByID(model.id);
            if (oldObj == null)
            {
                ViewBag.Error = await LanguageHelper.GetLanguageText("MESSAGE:RECORD:NOTEXISTS");
                return View(model);
            }

            //
            if (string.IsNullOrWhiteSpace(model.led_Name))
            {
                ModelState.AddModelError("led_Name", "Tên đã tồn tại");
                return View(oldObj);
            }

            //
            var existed = await _tblLedService.GetByName_Id(model.led_Name, model.id);
            if (existed != null)
            {
                ModelState.AddModelError("led_Name", "Tên đã tồn tại");
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị

            oldObj.id = Guid.Parse(model.id); ;
            oldObj.led_Code = model.led_Code;
            oldObj.led_Name = model.led_Name;
            oldObj.ip_Address = model.ip_Address;
            oldObj.row = Convert.ToInt32(row_led);
            oldObj.column_Led = Convert.ToInt32(column_led);
            oldObj.led_Function = Convert.ToInt32( function_LED);
            oldObj.description = model.description;
            oldObj.port = model.port;
            oldObj.controller_Type = model.controller_Type;


            //Thực hiện cập nhật
            var result = await _tblLedService.Update(oldObj);


            if (result.isSuccess)
            {

                //await LogHelper.WriteLog(oldObj.id.ToString(), ActionConfig.Update, JsonConvert.SerializeObject(oldObj), HttpContext);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

           
        }

        #endregion Cập nhật

        #region Xóa

        /// <summary>
        /// Xóa
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>

        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _tblLedService.DeleteById(id);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(id, ActionConfig.Delete, id, HttpContext);
            }

            return Json(result);
          
        }


        #endregion Xóa
    }
}
