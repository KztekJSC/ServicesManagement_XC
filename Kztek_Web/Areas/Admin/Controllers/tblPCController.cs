using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Configs;
using Kztek_Library.Extensions;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class tblPCController : Controller
    {
        private ItblPCService _tblPCService;
    
        private ItblCameraService _tblCameraService;
        public tblPCController(ItblPCService _tblPCService, ItblCameraService _tblCameraService)
        {
            this._tblPCService = _tblPCService;

            this._tblCameraService = _tblCameraService;
        }

        #region ddl      
        public async Task<SelectListModel_Chosen> GetCam_Chosen(string selecteds, string id = "CameraIn")  //bind ServicePackageId to dropdownlist
        {
            //var list = await GetCam();

            //var newobj = new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT") };

            //list.Insert(0, newobj);

            //var model = new SelectListModel_Chosen
            //{
            //    Data = list,
            //    Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
            //    IdSelectList = id,
            //    isMultiSelect = false,
            //    Selecteds = selecteds
            //};

            //return model;
            return null;
        }

        //public async Task<List<SelectListModel>> GetCam()
        //{
        //    var list = new List<SelectListModel> { };
        //    var lst = await _tblCameraService.GetAllActive();
        //    if (lst.Any())
        //    {
        //        foreach (var item in lst)
        //        {
        //            list.Add(new SelectListModel { ItemValue = item.CameraID, ItemText = item.CameraName });
        //        }
        //    }
        //    return list;
        //}

        #endregion
      
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string key = "", string gate = "", int page = 1, string AreaCode = "", string selectedId = "")
        {
            var gridmodel = await _tblPCService.GetAllCustomPagingByFirst(key, gate, page, 20);



            ViewBag.keyValue = key;
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("tblPC", this.HttpContext);
            ViewBag.AreaCodeValue = AreaCode;
            ViewBag.SelectedIdValue = selectedId;
          
            ViewBag.gateValue = gate;

            return View(gridmodel);
        }

        #region Thêm mới
        [CheckSessionCookie(AreaConfig.Admin)]
        [HttpGet]
        public async Task<IActionResult> Create(tblPC model, string AreaCode = "", string key = "")
        {
           

            model = model == null ? new tblPC() : model;           
            ViewBag.keyValue = key;
            ViewBag.AreaCodeValue = AreaCode;
           

            return await Task.FromResult(View(model));
        }
     
        [CheckSessionCookie(AreaConfig.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(tblPC model, string key = "", bool SaveAndCountinue = false, string AreaCode = "", string gates = "")
        {
           

            ViewBag.keyValue = key;
            ViewBag.AreaCodeValue = AreaCode;   
            //
            if (string.IsNullOrWhiteSpace(model.pc_Name))
            {
                ModelState.AddModelError("ComputerName", "Vui lòng nhập tên máy tính");
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.ip_Address))
            {
                ModelState.AddModelError("IPAddress", "Vui lòng nhập địa chỉ Ip");
                return View(model);
            }

            //
            var existedName = await _tblPCService.GetByName(model.ip_Address);
            if (existedName != null)
            {
                ModelState.AddModelError("IPAddress", "Địa chỉ ip đã tồn tại");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.id = Guid.NewGuid();
       

            //Thực hiện thêm mới
            var result = await _tblPCService.Create(model);
            if (result.isSuccess)
            {              
                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { AreaCode = AreaCode, key = key, selectedId = model.id });
                }

                return RedirectToAction("Index", new { AreaCode = AreaCode, key = key, selectedId = model.id });
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

            var model = await _tblPCService.GetById(id);
            ViewBag.PN = page;
            ViewBag.AreaCodeValue = AreaCode;
            ViewBag.keyValue = key;

            //model.Password = "";
     
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
        public async Task<IActionResult> Update(tblPC model, string AreaCode = "", int page = 1, string key = "", string gates = "")
        {
           

            //
            ViewBag.keyValue = key;
            ViewBag.AreaCodeValue = AreaCode;
            ViewBag.PN = page;

            //Kiểm tra
         
            var oldObj = await _tblPCService.GetById(model.id.ToString());
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(model);
            }

            //
            if (string.IsNullOrWhiteSpace(model.pc_Name))
            {
                ModelState.AddModelError("ComputerName", "Vui lòng nhập tên máy tính");
                return View(oldObj);
            }

            if (string.IsNullOrWhiteSpace(model.ip_Address))
            {
                ModelState.AddModelError("IPAddress", "Vui lòng nhập địa chỉ Ip");
                return View(oldObj);
            }

            //
            var existedName = await _tblPCService.GetByName_Id(model.ip_Address, model.pc_Name);
            if (existedName != null)
            {
                ModelState.AddModelError("IPAddress", "Địa chỉ ip đã tồn tại");
                return View(oldObj);
            }

            //
            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.pc_Name = model.pc_Name;
            oldObj.description = model.description;
            oldObj.ip_Address = model.ip_Address;
            

            ////Kiểm tra mật khẩu mới
            //if (!string.IsNullOrWhiteSpace(model.Password))
            //{
            //    //Sinh mã salat mới
            //    oldObj.PasswordSalat = Guid.NewGuid().ToString();

            //    //
            //    //oldObj.Password = CryptoHelper.EncryptPass_User(model.Password, oldObj.PasswordSalat);
            //    oldObj.Password = model.Password/*.PasswordHashed(oldObj.PasswordSalat)*/;
            //}

            var result = await _tblPCService.Update(oldObj);
            if (result.isSuccess)
            {

                return RedirectToAction("Index", new { AreaCode = AreaCode, key = key, page = page, selectedId = model.id });
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
            var result = await _tblPCService.DeleteById(id);

            return Json(result);
        }

        #endregion Xóa
    }
}