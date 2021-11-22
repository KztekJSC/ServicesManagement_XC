using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class tblLaneController : Controller
    {
        private ItblLaneService _tblLaneService;
        private ItblPCService _tblPCService;
        private ItblCameraService _tblCameraService;

        public tblLaneController(ItblLaneService _tblLaneService, ItblPCService _tblPCService, ItblCameraService _tblCameraService)
        {
            this._tblLaneService = _tblLaneService;
            this._tblPCService = _tblPCService;
            this._tblCameraService = _tblCameraService;
        }

        #region ddl      
        public async Task<SelectListModel_Chosen> GetLaneType_Chosen(string selecteds)  //bind ServicePackageId to dropdownlist
        {

            var model = new SelectListModel_Chosen
            {
                Data = await StaticList.LaneTypes1(),
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = "LaneType",
                isMultiSelect = false,
                Selecteds = selecteds
            };

            return model;
        }

        public async Task<SelectListModel_Chosen> GetListPC_Chosen(string selecteds, string id = "PCID")  //bind ServicePackageId to dropdownlist
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

            var gridmodel = await _tblLaneService.GetAllCustomPagingByFirst(key, pc, page, 20);

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("tblLane", this.HttpContext);
            ViewBag.PCs = await GetListPC_Chosen(pc, "pc");
            ViewBag.LaneTypes = await GetLaneType_Chosen("");
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
        public async Task<IActionResult> Create(tblLane_Submit model, string c)
        {
            model = model == null ? new tblLane_Submit() : model;
            ViewBag.camera_LPR_1 = await GetListCameraLPR_1_Chosen(model.camera_LPR_1_id);
            ViewBag.camera_LPR_2 = await GetCameraLPR_2_Chosen(model.camera_LPR_2_id);
            ViewBag.camera_Panorama_1 = await GetCameraPanorama1_Chosen(model.camera_Panorama_1_id);
            ViewBag.camera_Panorama_2 = await GetCameraPanorama2_Chosen(model.camera_Panorama_2_id);
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
        public async Task<IActionResult> Create(tblLane_Submit model, bool SaveAndCountinue = false)
        {
            ViewBag.camera_LPR_1 = await GetListCameraLPR_1_Chosen(model.camera_LPR_1_id);
            ViewBag.camera_LPR_2 = await GetCameraLPR_2_Chosen(model.camera_LPR_2_id);
            ViewBag.camera_Panorama_1 = await GetCameraPanorama1_Chosen(model.camera_Panorama_1_id);
            ViewBag.camera_Panorama_2 = await GetCameraPanorama2_Chosen(model.camera_Panorama_2_id);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(model.lane_Name))
            {
                ModelState.AddModelError("LaneName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:enter_name"));
                return View(model);
            }

            var existed = await _tblLaneService.GetByName(model.lane_Name);
            if (existed != null)
            {
                ModelState.AddModelError("LaneName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:exists_name"));
                return View(model);
            }
            var obj = new tblLane();
            obj.id = Guid.NewGuid();
            obj.lane_Code = model.lane_Code;
            obj.lane_Name = model.lane_Name;
            obj.vehicle_Types = model.vehicle_Types;
            obj.reversal = model.reversal;
            obj.description = model.description;
            obj.camera_LPR_1 = model.camera_LPR_1_id;
            obj.camera_LPR_2 = model.camera_LPR_2_id;
            obj.camera_Panorama_1 = model.camera_Panorama_1_id;
            obj.camera_Panorama_2 = model.camera_Panorama_2_id;
            obj.card_Types = model.card_Types;
            obj.auto_Mode = model.auto_Mode;

            //Thực hiện thêm mới
            var result = await _tblLaneService.Create(obj);
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
        private async Task<SelectListModel_Chosen> GetCameraPanorama2_Chosen(string selecteds, string id = "camera_Panorama_2_id")
        {
            var list = await GetAllCamera();

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

        private async Task<SelectListModel_Chosen> GetCameraPanorama1_Chosen(string selecteds, string id = "camera_Panorama_1_id")
        {
            var list = await GetAllCamera();

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

        private async Task<SelectListModel_Chosen> GetCameraLPR_2_Chosen(string selecteds, string id = "camera_LPR_2_id")
        {
            var list = await GetAllCamera();

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

        private async Task<SelectListModel_Chosen> GetListCameraLPR_1_Chosen(string selecteds, string id = "camera_LPR_1_id")
        {
            var list = await GetAllCamera();

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
        public async Task<List<SelectListModel>> GetAllCamera()
        {
            var list = new List<SelectListModel> { };
            var lst = await _tblCameraService.GetAllActive();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.id.ToString(), ItemText = item.camera_Name });
                }
            }
            return list;
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
            var model = await _tblLaneService.GetByCustomId(id);
            ViewBag.camera_LPR_1 = await GetListCameraLPR_1_Chosen(model.camera_LPR_1_id);
            ViewBag.camera_LPR_2 = await GetCameraLPR_2_Chosen(model.camera_LPR_2_id);
            ViewBag.camera_Panorama_1 = await GetCameraPanorama1_Chosen(model.camera_Panorama_1_id);
            ViewBag.camera_Panorama_2 = await GetCameraPanorama2_Chosen(model.camera_Panorama_2_id);
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
        public async Task<IActionResult> Update(tblLane_Submit model, int pageNumber = 1)
        {
            ViewBag.camera_LPR_1 = await GetListCameraLPR_1_Chosen(model.camera_LPR_1_id);
            ViewBag.camera_LPR_2 = await GetCameraLPR_2_Chosen(model.camera_LPR_2_id);
            ViewBag.camera_Panorama_1 = await GetCameraPanorama1_Chosen(model.camera_Panorama_1_id);
            ViewBag.camera_Panorama_2 = await GetCameraPanorama2_Chosen(model.camera_Panorama_2_id);
            var oldObj = await _tblLaneService.GetById(model.id);
            if (oldObj == null)
            {
                ViewBag.Error = await LanguageHelper.GetLanguageText("MESSAGE:RECORD:NOTEXISTS");
                return View(model);
            }

            //
            if (string.IsNullOrWhiteSpace(model.lane_Name))
            {
                ModelState.AddModelError("LaneName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:enter_name"));
                return View(oldObj);
            }

            //
            var existed = await _tblLaneService.GetByName_Id(model.lane_Name, model.id);
            if (existed != null)
            {
                ModelState.AddModelError("LaneName", await LanguageHelper.GetLanguageText("MESSAGE:FEE:exists_name"));
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị

            oldObj.id = Guid.Parse(model.id);
            oldObj.lane_Code = model.lane_Code;
            oldObj.lane_Name = model.lane_Name;
            oldObj.vehicle_Types = model.vehicle_Types;
            oldObj.reversal = model.reversal;
            oldObj.description = model.description;
            oldObj.camera_LPR_1 = model.camera_LPR_1_id;
            oldObj.camera_LPR_2 = model.camera_LPR_2_id;
            oldObj.camera_Panorama_1 = model.camera_Panorama_1_id;
            oldObj.camera_Panorama_2 = model.camera_Panorama_2_id;
            oldObj.card_Types = model.card_Types;
            oldObj.auto_Mode = model.auto_Mode;

            //Thực hiện cập nhật
            var result = await _tblLaneService.Update(oldObj);


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
            var result = await _tblLaneService.DeleteById(id);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(id, ActionConfig.Delete, id, HttpContext);
            }

            return Json(result);
        }


        #endregion Xóa

        //public async Task<IActionResult> ListCameraByPC(string pcID, string cameraNumber, string selected)
        //{
        //    List<tblCamera> newList = new List<tblCamera>();

        //    var list = await _tblCameraService.GetAllActiveByPC(pcID);
        //    if (list.Any())
        //    {
        //        foreach (var item in list.ToList())
        //        {
        //            newList.Add(new tblCamera { id = item.id, camera_Name = string.Format("{0} ({1})", item.camera_Name, item.HttpURL) });
        //        }
        //    }

        //    ViewBag.Number = cameraNumber;
        //    ViewBag.selectedCamera = selected;
        //    ViewBag.Choose = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT");
        //    return PartialView(newList);
        //}
    }
}