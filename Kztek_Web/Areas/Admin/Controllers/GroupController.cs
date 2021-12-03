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
    public class GroupController : Controller
    {
        private IGroupService _GroupService ;
        public GroupController(IGroupService _GroupService)
        {
            this._GroupService = _GroupService;
        }
        #region Danh sách
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {

            var gridmodel = await _GroupService.GetAllCustomPagingByFirst(key, pc, page, 20);
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Group", this.HttpContext);          
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
        public async Task<IActionResult> Create(Group model)
        {
            model = model == null ? new Group() : model;
         
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
        public async Task<IActionResult> Create(Group model, bool SaveAndCountinue = false)
        {
           

            if (!ModelState.IsValid)
            {
                return View(model);
            }

         
            //
            model.Id = Guid.NewGuid().ToString();
            model.CreatedDate = DateTime.Now;
            model.ModifiedDate = DateTime.Now;

            //Thực hiện thêm mới
            var result = await _GroupService.Create(model);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(model.Id.ToString(), ActionConfig.Create, JsonConvert.SerializeObject(model), HttpContext);
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
        public async Task<IActionResult> Update(string id, string AreaCode = "", int page = 1, string key = "")
        {
            var model = await _GroupService.GetById(id);
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
        public async Task<IActionResult> Update( Group obj,  string AreaCode = "", int page = 1, string key = "")
        {
            //
            ViewBag.keyValue = key;
            ViewBag.AreaCodeValue = AreaCode;
            ViewBag.PN = page;
            //Kiểm tra
            var oldObj = await _GroupService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = await LanguageHelper.GetLanguageText("MESSAGE:RECORD:NOTEXISTS");
                return View(obj);
            }
           
           
          
            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            oldObj.Id = obj.Id;
            oldObj.Code = obj.Code;
            oldObj.Name = obj.Name;
            oldObj.ModifiedDate = DateTime.Now;
          

            //Thực hiện cập nhậts
            var result = await _GroupService.Update(oldObj);


            if (result.isSuccess)
            {

                await LogHelper.WriteLog(oldObj.Id.ToString(), ActionConfig.Update, JsonConvert.SerializeObject(oldObj), HttpContext);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
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
            var result = await _GroupService.DeleteById(id);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(id, ActionConfig.Delete, id, HttpContext);
            }

            return Json(result);
        }


        #endregion Xóa
    }
}
