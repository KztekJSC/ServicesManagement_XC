using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
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
    public class CoordinatorController : Controller
    {
        private Itbl_EventService _tbl_EventService;
        private IGroupService _GroupService;
        public CoordinatorController(Itbl_EventService _tbl_EventService, IGroupService _GroupService)
        {
            this._tbl_EventService = _tbl_EventService;
            this._GroupService = _GroupService;
        }
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string StatusID = "", string key = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1, string AreaCode = ""
)
        {
            var datefrompicker = "";

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            //if (chkExport.Equals("1"))
            //{
            //    await ExportFile(key, sort, page, 20, StatusID, isCheckByTime, fromdate, todate, this.HttpContext);

            //    //return View(gridmodel);
            //}


            #region Giao diện

            var gridModel = await _tbl_EventService.GetPagingCoordinatort(key, page, 20, StatusID, fromdate, todate);
            ViewBag.Eventype = await _tbl_EventService.GetEventype(selecteds: StatusID);
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Coordinator", this.HttpContext);
            ViewBag.StatusID = StatusID;
            ViewBag.Groups = await _GroupService.GetAll();
            ViewBag.keyValue = key;
            ViewBag.AreaCodeValue = AreaCode;
            return View(gridModel);
            #endregion
        }

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
            var model = await _tbl_EventService.GetById(id);
            ViewBag.AllGroup = await GetAllGroup(model.GroupId);
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
        public async Task<IActionResult> Update(tbl_Event model, int pageNumber = 1)
        {

            ViewBag.AllGroup = await GetAllGroup(model.GroupId);
            //Kiểm tra
            var oldObj = await _tbl_EventService.GetById(model.Id.ToString());
            if (oldObj == null)
            {
                ViewBag.Error = await LanguageHelper.GetLanguageText("MESSAGE:RECORD:NOTEXISTS");
                return View(model);
            }



            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

         
            oldObj.PlateVN = model.PlateVN;
            oldObj.PlateCN = model.PlateCN;         
            oldObj.Weight = model.Weight;
            oldObj.PackageNumber = model.PackageNumber;
            oldObj.VehicleType = model.VehicleType;
            oldObj.ServiceCode = model.ServiceCode;
            oldObj.EventType = 5;
            oldObj.ModifiedDate = DateTime.Now;

            //Thực hiện cập nhậts
            var result = await _tbl_EventService.Update(oldObj);


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
            var result = await _tbl_EventService.DeleteById(id);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(id, ActionConfig.Delete, id, HttpContext);
            }

            return Json(result);
        }


        #endregion Xóa

        private async Task<SelectListModel_Chosen> GetAllGroup(string selecteds, string id = "GroupID")
        {
            var list = await GetAllGroup();



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
        public async Task<List<SelectListModel>> GetAllGroup()
        {
            var list = new List<SelectListModel> { };
            var lst = await _GroupService.GetAll();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.Id, ItemText = item.Name });
                }
            }
            return list;
        }
    }
}
