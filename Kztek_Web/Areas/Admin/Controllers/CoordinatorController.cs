using Kztek_Core.Models;
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
        private IServiceService _ServiceService;
        private IColumTableService _ColumTableService;
        public CoordinatorController(Itbl_EventService _tbl_EventService, IGroupService _GroupService , IServiceService _ServiceService, IColumTableService _ColumTableService)
        {
            this._tbl_EventService = _tbl_EventService;
            this._ColumTableService = _ColumTableService;
            this._GroupService = _GroupService;
            this._ServiceService = _ServiceService;
        }

        #region Danh sách
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
         
            var controller = "Coordinator";
            var action = "Index";
            ViewBag.Eventype = await _tbl_EventService.GetEventypeCoordination(selecteds: StatusID);

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Coordinator", this.HttpContext);

            ViewBag.listSlectColumn = await _ServiceService.SelectColumnCoor(controller, action);

            ViewBag.StatusID = StatusID;
           
            ViewBag.keyValue = key;

            ViewBag.AreaCodeValue = AreaCode;

            return View();
           
        }

        public async Task<IActionResult> Partial_Coordinator(string StatusID = "", string key = "", int page = 1)
        {

            var gridModel = await _tbl_EventService.GetPagingCoordinatort(key, page, 20, StatusID, "", "");

            ViewBag.showColumn = await _ColumTableService.GetDetailByController("Coordinator", "Index");

            ViewBag.lstService = await _ServiceService.GetAll();


            ViewBag.Groups = await _GroupService.GetAll();

            return PartialView(gridModel);

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
            var model = await _tbl_EventService.GetById(id.ToString());
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
  
            oldObj.EventType = 5;
            oldObj.ModifiedDate = DateTime.Now;

            //Thực hiện cập nhậts
            var result = await _tbl_EventService.Update(oldObj);


            if (result.isSuccess)
            {

                await LogHelper.WriteLog(oldObj.Id.ToString(), ActionConfig.Update, "tbl_Event", JsonConvert.SerializeObject(oldObj), HttpContext);
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

        #region DDL
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

        #endregion

        #region Cập nhật sự kiện

        /// <summary>
        /// Chuyển từ chờ duyệt -> hoàn thành
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateService(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = await _tbl_EventService.GetById(id);

            if (obj != null)
            {
                obj.EventType = 6; //Hoàn thành
                obj.ModifiedDate = DateTime.Now;

                result = await _tbl_EventService.Update(obj);
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return Json(result);
        }
        #endregion

        #region Modal xác nhận hoàn thành dịch vụ
        public async Task<IActionResult> Modal_Info(string id)
        {
            var objService = await _tbl_EventService.GetById(id);

            return PartialView(objService);
        }

        public async Task<IActionResult> SaveService(tbl_Event model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = await _tbl_EventService.GetById(model.Id);

            if (obj != null)
            {
                obj.EventType = 6; //Hoàn thành
                obj.EndDate = DateTime.Now;
                obj.ModifiedDate = DateTime.Now;

                obj.ServiceCode = model.ServiceCode;
                obj.VehicleType = model.VehicleType;
                obj.Weight = model.Weight;
                obj.PackageNumber = model.PackageNumber;

                result = await _tbl_EventService.Update(obj);
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return Json(result);
        }
        #endregion


        #region Hiện thị cột 

        public async Task<IActionResult> AddChooseSelect(string str, string controller, string action)
        {
            var obj = StaticList.ListCoordinator_Display();
            var result = new MessageReport(false, "Có lỗi xảy ra");
            var str1 = "";
            foreach (var item in obj)
            {
                str1 += (item.ItemValue + "-" + item.ItemText) + ",";
            }
            var objColum = await _ColumTableService.GetDetailByController(controller, action);
            if (objColum == null)
            {
                var model = new ColumTable();
                model.Controller = controller;
                model.Action = action;
                model.ColumShows = str;
                model.Columns = str1;
                model.Id = Guid.NewGuid().ToString();
                result = await _ColumTableService.Create(model);
            }
            else
            {
                objColum.ColumShows = str;
                result = await _ColumTableService.Update(objColum);
            }

            return Json(result);
        }


        #endregion
    }
}
