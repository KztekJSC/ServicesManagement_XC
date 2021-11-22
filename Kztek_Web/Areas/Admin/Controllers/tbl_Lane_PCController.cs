using Kztek_Core.Models;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Service.Admin.Database;
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
    public class tbl_Lane_PCController : Controller
    {
        private Itbl_Lane_PCService _tbl_Lane_PCService;
        private ItblPCService _tblPCService;
        private ItblLaneService _tblLaneService;
        public tbl_Lane_PCController(Itbl_Lane_PCService _tbl_Lane_PCService, ItblPCService _tblPCService, ItblLaneService _tblLaneService)
        {
            this._tblPCService = _tblPCService;
            this._tblLaneService = _tblLaneService;
            this._tbl_Lane_PCService = _tbl_Lane_PCService;
        }
        #region Danh sách
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {

            var gridmodel = await _tbl_Lane_PCService.GetAllCustomPagingByFirst(key, pc, page, 20);
            ViewBag.getPC = await _tblPCService.GetAllActive();
            ViewBag.getLane = await _tblLaneService.GetAllActive();
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("tbl_Lane_PC", this.HttpContext);
            //ViewBag.PCs = await GetListPC_Chosen(pc, "pc");
            //ViewBag.LaneTypes = await GetLaneType_Chosen("");
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
        public async Task<IActionResult> Create(tbl_Lane_PC_Custom model, string c)
        {
            model = model == null ? new tbl_Lane_PC_Custom() : model;
            ViewBag.LstLane = await GetListLane_Chosen(model.lane_ID);
            ViewBag.LstPC = await GetListPC_Chosen(model.pc_ID);
            return await Task.FromResult(View(model));
        }

        private async Task<SelectListModel_Chosen> GetListPC_Chosen(string selecteds, string id = "pc_ID")
        {
            var list = await GetAllPC();

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

        private async Task<List<SelectListModel>> GetAllPC()
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

        private async Task<SelectListModel_Chosen> GetListLane_Chosen(string selecteds, string id = "lane_ID")
        {
            var list = await GetAllLane();

            //var newobj = new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT") };

            //list.Insert(0, newobj);

            var model = new SelectListModel_Chosen
            {
                Data = list,
                Placeholder = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT"),
                IdSelectList = id,
                isMultiSelect = true,
                Selecteds = selecteds
            };

            return model;
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
        public async Task<IActionResult> Create(tbl_Lane_PC_Custom model, string lanes, bool SaveAndCountinue = false)
        {
            ViewBag.LstPC = await GetListPC_Chosen(model.pc_ID);
            ViewBag.LstLane = await GetListLane_Chosen(model.lane_ID);
            var result = new MessageReport(false, "error");
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var obj = new tbl_Lane_PC();
          
            if (!string.IsNullOrWhiteSpace(model.pc_ID))
            {
                var value = await _tbl_Lane_PCService.getByPc_ID(model.pc_ID);
                if (value == null)
                {
                    obj.id = Guid.NewGuid();
                    obj.pc_ID = model.pc_ID;
                    obj.lane_ID = lanes;
                }
                else
                {
                    ModelState.AddModelError("pc_ID", "Máy tính này đã tồn tại");
                    return View(model);
                }
              
            }

            result = await _tbl_Lane_PCService.Create(obj);
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


        public async Task<List<SelectListModel>> GetAllLane()
        {
            var list = new List<SelectListModel> { };
            var lst = await _tblLaneService.GetAllActive();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.id.ToString(), ItemText = item.lane_Name });
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
        public async Task<IActionResult> Update(string id, string lanes, int pageNumber = 1)
        {
            var model = await _tbl_Lane_PCService.GetByCustomId(id);          
            ViewBag.LstLane = await GetListLane_Chosen(model.lane_ID);
            ViewBag.LstPC = await GetListPC_Chosen(model.pc_ID);
          

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
        public async Task<IActionResult> Update(tbl_Lane_PC_Custom model, string pcid,string laneid, int pageNumber = 1)
        {
            model.lane_ID = laneid;
            model.pc_ID = pcid;
            ViewBag.LstLane = await GetListLane_Chosen(model.lane_ID);
            ViewBag.LstPC = await GetListPC_Chosen(model.pc_ID);

            var result = new MessageReport(false, "Error");
            var oldObj = await _tbl_Lane_PCService.GetById(model.id.ToString());
            if (oldObj == null)
            {
                ViewBag.Error = await LanguageHelper.GetLanguageText("MESSAGE:RECORD:NOTEXISTS");
                return View(model);
            }

          

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị
           
           
            if (!string.IsNullOrWhiteSpace(model.pc_ID))
            {
                var value = await _tbl_Lane_PCService.getByPc_ID(model.pc_ID);
                
                if (value == null)
                {
                    oldObj.id = Guid.Parse(model.id);
                    oldObj.pc_ID = model.pc_ID;
                    oldObj.lane_ID = laneid;
                }
                else
                {
                    if (oldObj.pc_ID == model.pc_ID)
                    {
                        oldObj.id = Guid.Parse(model.id);
                        oldObj.pc_ID = model.pc_ID;
                        oldObj.lane_ID = laneid;
                      
                    }
                    else
                    {
                        ModelState.AddModelError("pc_ID", "Máy tính này đã tồn tại");
                        return View(model);
                    }

                   
                }

            }
            result = await _tbl_Lane_PCService.Update(oldObj);

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
            var result = await _tbl_Lane_PCService.DeleteById(id);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(id, ActionConfig.Delete, id, HttpContext);
            }

            return Json(result);
        }


        #endregion Xóa

    }
}
