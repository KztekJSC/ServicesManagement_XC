using Autofac.Core;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
         private IGroupService _GroupService;
         private ItblGroupServiceService _tblGroupServiceService;

        public GroupServiceController(IGroupServieService _GroupServieService, IGroupService _GroupService , ItblGroupServiceService _tblGroupServiceService)
        {
            this._tblGroupServiceService = _tblGroupServiceService;
            this._GroupServieService = _GroupServieService;
            this._GroupService = _GroupService;
        }
        #region Danh sách


        public async Task<IActionResult> Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {

            var gridmodel = await _GroupServieService.GetAllCustomPagingByFirst(key, pc, page, 20);
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("GroupService", this.HttpContext);
            ViewBag.keyValue = key;
            return View(gridmodel);
        }
        #endregion
        #region Thêm mới

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
        public async Task<IActionResult> Create(Service_Submit model, string AreaCode = "")
        {
            model = model == null ? new Service_Submit() : model;
            model.Data_Group = await _GroupService.GetAll();
            ViewBag.AreaCodeValue = AreaCode;
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
        public async Task<IActionResult> Create(Service_Submit model, bool SaveAndCountinue = false, string AreaCode = "" , string Groupids = "")
        {


            if (!ModelState.IsValid)
            {
                return View(model);
            }
            model.Data_Group = await _GroupService.GetAll();
            ViewBag.AreaCodeValue = AreaCode;    
            ViewBag.Groupids  = Groupids;
            var obj = new Kztek_Model.Models.Service();
            //
            obj.Id = Guid.NewGuid().ToString();
            obj.Description = model.Description;         
            obj.Name = model.Name;
            obj.Code = model.Code;
            
            obj.CreatedDate = DateTime.Now;
            obj.ModifiedDate = DateTime.Now;

            //Thực hiện thêm mới
            if (!string.IsNullOrWhiteSpace(Groupids))
            {
                var ks = Groupids.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                model.Groups = new List<string>();
                foreach (var item in ks)
                {
                    model.Groups.Add(item);
                }

                foreach (var item in model.Groups)
                {
                    var t = new tblGroupService()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ServiceId = obj.Id,
                        GroupId = item
                    };

                    await _tblGroupServiceService.CreateMap(t);
                }
            }

            var result = await _GroupServieService.Create(obj);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(obj.Id.ToString(), ActionConfig.Create, JsonConvert.SerializeObject(obj), HttpContext);
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
                return View(obj);
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
        public async Task<IActionResult> Update(string id, string AreaCode = "", int page = 1, string key = "" , string Groupids = "")
        {
            var model = await _GroupServieService.GetCustomeById(id);
            model.Data_Group = await _GroupService.GetAll();
            ViewBag.Groupids = Groupids;
            ViewBag.PN = page;
            ViewBag.AreaCodeValue = AreaCode;
            ViewBag.keyValue = key;

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
        public async Task<IActionResult> Update(Service_Submit model, string AreaCode = "", int page = 1, string key = "", string Groupids = "")
        {
            //
            ViewBag.keyValue = key;
            ViewBag.Groupids = Groupids;
            ViewBag.AreaCodeValue = AreaCode;
            ViewBag.PN = page;
            //Kiểm tra
            var oldObj = await _GroupServieService.GetById(model.Id);
            model.Data_Group = await _GroupService.GetAll();
            if (oldObj == null)
            {
                ViewBag.Error = await LanguageHelper.GetLanguageText("MESSAGE:RECORD:NOTEXISTS");
                return View(model);
            }



            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            oldObj.Id = model.Id;
            oldObj.Code = model.Code;
            oldObj.Name = model.Name;
            oldObj.ModifiedDate = DateTime.Now;
            await _tblGroupServiceService.DeleteMap(oldObj.Id);
            if (!string.IsNullOrWhiteSpace(Groupids))
            {
                var ks = Groupids.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                model.Groups = new List<string>();
                foreach (var item in ks)
                {
                    model.Groups.Add(item);
                }

                foreach (var item in model.Groups)
                {
                    var t = new tblGroupService()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ServiceId = model.Id,
                        GroupId = item
                    };

                    await _tblGroupServiceService.CreateMap(t);
                }
            }

            //Thực hiện cập nhậts
            var result = await _GroupServieService.Update(oldObj);


            if (result.isSuccess)
            {
                await LogHelper.WriteLog(oldObj.Id.ToString(), ActionConfig.Update, JsonConvert.SerializeObject(oldObj), HttpContext);
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
            var result = await _GroupServieService.DeleteById(id);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(id, ActionConfig.Delete, id, HttpContext);
            }

            return Json(result);
        }


        #endregion Xóa
    }
}
