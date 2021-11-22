using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Library.Security;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class tblCameraController : Controller
    {
        private ItblCameraService _tblCameraService;
        private ItblPCService _tblPCService;

        public tblCameraController(ItblCameraService _tblCameraService, ItblPCService _tblPCService)
        {
            this._tblCameraService = _tblCameraService;
            this._tblPCService = _tblPCService;
        }

        #region ddl
        public async Task<SelectListModel_Chosen> GetSDK_Chosen(string selecteds)  //bind ServicePackageId to dropdownlist
        {

            var model = new SelectListModel_Chosen
            {
                Data = await StaticList.SDKs1(),
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = "SDK",
                isMultiSelect = false,
                Selecteds = selecteds
            };

            return model;
        }
        public async Task<SelectListModel_Chosen> GetStreamTypes_Chosen(string selecteds)  //bind ServicePackageId to dropdownlist
        {

            var model = new SelectListModel_Chosen
            {
                Data = await StaticList.StreamTypes1(),
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = "stream_Type",
                isMultiSelect = false,
                Selecteds = selecteds
            };

            return model;
        }
        public async Task<SelectListModel_Chosen> GetCameraType_Chosen(string selecteds)  //bind ServicePackageId to dropdownlist
        {

            var model = new SelectListModel_Chosen
            {
                Data = await StaticList.CameraTypes1(),
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = "camera_Type",
                isMultiSelect = false,
                Selecteds = selecteds
            };

            return model;
        }
        public async Task<SelectListModel_Chosen> GetListPC_Chosen(string selecteds,string id = "PCID")  //bind ServicePackageId to dropdownlist
        {
            var list = await GetPC();

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

        public async Task<List<SelectListModel>> GetPC()
        {
            var list = new List<SelectListModel> { };
            var lst = await _tblPCService.GetAllActive();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.id.ToString(), ItemText = item.pc_Name });
                }
            }
            return list;
        }
        #endregion

        #region Danh sách
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {

            var gridmodel = await _tblCameraService.GetAllCustomPagingByFirst(key, pc, page, 20);

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("tblCamera", this.HttpContext);
            ViewBag.PCs = await GetListPC_Chosen(pc, "pc");
            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;
            ViewBag.selectedIdValue = selectedId;

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
        public async Task<IActionResult> Create(tblCamera model)
        {
            model = model == null ? new tblCamera() : model;
            //model.id = Guid.NewGuid();
            //ViewBag.PCs = await GetListPC_Chosen(model.PCID);
            ViewBag.CameraType = await GetCameraType_Chosen(model.camera_Type);
            ViewBag.StreamType = await GetStreamTypes_Chosen(model.stream_Type);
            ViewBag.SDK = await GetSDK_Chosen(model.SDK);
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
        public async Task<IActionResult> Create(tblCamera model, bool SaveAndCountinue = false)
        {
            //ViewBag.PCs = await GetListPC_Chosen(model.PCID);
            ViewBag.CameraType = await GetCameraType_Chosen(model.camera_Type);
            ViewBag.StreamType = await GetStreamTypes_Chosen(model.stream_Type);
            ViewBag.SDK = await GetSDK_Chosen(model.SDK);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.camera_Name))
            {
                ModelState.AddModelError("CameraName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:enter_name"));
                return View(model);
            }
            if (!string.IsNullOrWhiteSpace(model.camera_Name))
            {
                var exist = await _tblCameraService.GetByName(model.camera_Name);
                if (exist != null)
                {
                    ModelState.AddModelError("CameraName","Camera đã tồn tại");
                    return View(model);
                }
            }
            if (string.IsNullOrWhiteSpace(model.video_Source))
            {
                ModelState.AddModelError("video_Source", "Bạn chưa nhập IP");
                return View(model);
            }

            //
            var existed = await _tblCameraService.GetByName(model.camera_Name);
            if (existed != null)
            {
                ModelState.AddModelError("CameraName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:exists_name"));
                return View(model);
            }

            //
            model.id = Guid.NewGuid();
           // model.camera_Type = FunctionHelper.GetCgiByCameraType(model.camera_Type, Convert.ToString(model.FrameRate), model.Resolution, model.SDK, model.UserName, model.Password);
              
            model.auth_Password = !string.IsNullOrWhiteSpace(model.auth_Password) ? model.auth_Password :"";

            //Thực hiện thêm mới
            var result = await _tblCameraService.Create(model);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(model.id.ToString(), ActionConfig.Create, JsonConvert.SerializeObject(model), HttpContext);
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
            var model = await _tblCameraService.GetById(id);

            //try
            //{
            //    model.Password = CryptoHelper.Decrypt(model.Password, true);
            //}
            //catch
            //{
            //    model.Password = "123456";
            //}

            //ViewBag.PCs = await GetListPC_Chosen(model.PCID);
            ViewBag.CameraType = await GetCameraType_Chosen(model.camera_Type);
            ViewBag.StreamType = await GetStreamTypes_Chosen(model.stream_Type);
            ViewBag.SDK = await GetSDK_Chosen(model.SDK);

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
        public async Task<IActionResult> Update(tblCamera model, int pageNumber = 1)
        {
           // ViewBag.PCs = await GetListPC_Chosen(model.PCID);
            ViewBag.CameraType = await GetCameraType_Chosen(model.camera_Type);
            ViewBag.StreamType = await GetStreamTypes_Chosen(model.stream_Type);
            ViewBag.SDK = await GetSDK_Chosen(model.SDK);
            //Kiểm tra
            var oldObj = await _tblCameraService.GetById(model.id.ToString());
            if (oldObj == null)
            {
                ViewBag.Error = await LanguageHelper.GetLanguageText("MESSAGE:RECORD:NOTEXISTS");
                return View(model);
            }
            if (string.IsNullOrWhiteSpace(model.video_Source))
            {
                ModelState.AddModelError("video_Source", "Bạn chưa nhập IP");
                return View(model);
            }
            if (!string.IsNullOrWhiteSpace(model.camera_Name))
            {
                var exist = await _tblCameraService.GetByName(model.camera_Name);
                if (exist != null)
                {
                    ModelState.AddModelError("CameraName", "Camera đã tồn tại");
                    return View(model);
                }
            }
            //
            if (string.IsNullOrWhiteSpace(model.camera_Name))
            {
                ModelState.AddModelError("CameraName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:enter_name"));
                return View(oldObj);
            }

            //
            var existed = await _tblCameraService.GetByName_Id(model.camera_Name, model.id.ToString());
            if (existed != null)
            {
                ModelState.AddModelError("CameraName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:exists_name"));
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            oldObj.camera_Code = model.camera_Code;
            oldObj.camera_Name = model.camera_Name;
            oldObj.camera_Type = model.camera_Type;
            oldObj.chanel = model.chanel;      
            oldObj.http_Port = model.http_Port;                  
            oldObj.auth_Password = "";   
            oldObj.resolution = model.resolution;       
            oldObj.SDK = model.SDK;
            //oldObj.SortOrder = obj.SortOrder;
            oldObj.stream_Type = model.stream_Type;

           // oldObj.auth_Password = !string.IsNullOrWhiteSpace(model.auth_Password) ? CryptoHelper.Encrypt(model.auth_Password, true) : CryptoHelper.Encrypt("123456", true);
            oldObj.auth_Password = !string.IsNullOrWhiteSpace(model.auth_Password) ? model.auth_Password : "";

            //Thực hiện cập nhậts
            var result = await _tblCameraService.Update(oldObj);


            if (result.isSuccess)
            {

                await LogHelper.WriteLog(oldObj.id.ToString(), ActionConfig.Update, JsonConvert.SerializeObject(oldObj), HttpContext);
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
            var result = await _tblCameraService.DeleteById(id);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(id, ActionConfig.Delete, id, HttpContext);
            }

            return Json(result);
        }


        #endregion Xóa
    }
}