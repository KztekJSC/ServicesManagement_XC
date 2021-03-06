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
    public class ConfirmedGroupController : Controller
    {
        private Itbl_EventService _tbl_EventService;
        private IGroupService _GroupService;
        private IServiceService _ServiceService;
        public ConfirmedGroupController(Itbl_EventService _tbl_EventService, IGroupService _GroupService , IServiceService _ServiceService)
        {
            this._tbl_EventService = _tbl_EventService;
            this._GroupService = _GroupService;
            this._ServiceService = _ServiceService;
        }
        #region Danh sách
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string StatusID = "", string key = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1, string AreaCode = "")
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

            ViewBag.Eventype = await _tbl_EventService.GetEventype(selecteds: StatusID);
            
            ViewBag.StatusID = StatusID;

            ViewBag.keyValue = key;

            ViewBag.AreaCodeValue = AreaCode;

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("ConfirmedGroup", this.HttpContext);

            return View();
        }

        public async Task<IActionResult> Partial_ConfirmedGroup(string StatusID = "", string key = "", int page = 1, string fromdate = "")
        {
       
            ViewBag.lstService = await _ServiceService.GetAll();

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("ConfirmedGroup", this.HttpContext);

            var gridModel = await _tbl_EventService.GetPagingConfirmGroup(this.HttpContext,key, page, 20, StatusID, fromdate, "");

            ViewBag.Groups = await _GroupService.GetAll();

            return PartialView(gridModel);

        }

        public async Task<IActionResult> Partial_CountEvent(string fromdate = "")
        {
            var list = await _tbl_EventService.CountEventByType(this.HttpContext, fromdate);

            return PartialView(list);
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

            oldObj.Id = model.Id;
            oldObj.GroupId = model.GroupId != null ? model.GroupId : "";

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

        #region DDL
        public async Task<IActionResult> UpdateEvent(string eventype, string id)
        {
            var result = new MessageReport(false, "error");
            //Kiểm tra
            var oldObj = await _tbl_EventService.GetById(id);
            oldObj.EventType = Convert.ToInt32( eventype);
            oldObj.StartDate = DateTime.Now;

            //Thực hiện cập nhậts
             result = await _tbl_EventService.Update(oldObj);

           
            return Json(result);
        }
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
        /// Chuyển Đã phân tổ -> Đang thực hiện
        /// Chuyển Đang thực hiện -> Chờ duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<IActionResult> UpdateService(string id, int type)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
            var oldObj = await _tbl_EventService.GetByCustomById(id, this.HttpContext);
            var obj = await _tbl_EventService.GetById(id);

            if(obj != null)
            {
                //TH: Đã phân tổ -> bắt đầu
                if(type == 3)
                {
                    obj.EventType = 4; //bắt đầu
                    obj.StartDate = DateTime.Now;
                    obj.ModifiedDate = DateTime.Now;

                }
                else
                {
                    obj.EventType = 5; //chờ duyệt
                    obj.EndDate = DateTime.Now;
                    obj.ModifiedDate = DateTime.Now;

                  
                }

                result = await _tbl_EventService.Update(obj);
                if (result.isSuccess)
                {

                    //thông tin cũ

                    var oldModel = new tbl_Event_Cus();
                    oldModel.PackageNumber = oldObj.PackageNumber;
                    oldModel.weight = oldObj.weight.ToString();
                    oldModel.vehicleType = oldObj.vehicleType;
                    oldModel.description = oldObj.description;
                    oldModel.EventType = oldObj.EventType;
                    var jsStrOld = Newtonsoft.Json.JsonConvert.SerializeObject(oldModel);

                    //thông tin mới

                    var NewModel = new tbl_Event_Cus();
                    NewModel.PackageNumber = obj.PackageNumber;
                    NewModel.weight = obj.Weight.ToString();
                    NewModel.vehicleType = obj.VehicleType;
                    NewModel.description = obj.Description;
                    NewModel.EventType = 4;
                    var jsStrNew = Newtonsoft.Json.JsonConvert.SerializeObject(NewModel);
                    await LogHelper.WriteLogupdateService(obj.Id.ToString(), "Bắt đầu", "tbl_Event", JsonConvert.SerializeObject(obj), HttpContext, jsStrOld, jsStrNew);
                }
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return Json(result);
        }
        #endregion

        #region Modal kết thúc dịch vụ
        public async Task<IActionResult> Modal_Info(string id)
        {
            var objService = await _tbl_EventService.GetById(id);

            return PartialView(objService);
        }

        public async Task<IActionResult> SaveService(tbl_Event model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");
            var oldObj = await _tbl_EventService.GetByCustomById(model.Id , this.HttpContext);
            var obj = await _tbl_EventService.GetById(model.Id);
          
            if (obj != null)
            {
                obj.EventType = 5; //chờ duyệt
                obj.EndDate = DateTime.Now;
                obj.ModifiedDate = DateTime.Now;
                obj.ServiceCode = model.ServiceCode;
                obj.VehicleType = model.VehicleType;
                obj.Weight = model.Weight;
                obj.PackageNumber = model.PackageNumber;

                result = await _tbl_EventService.Update(obj);
                if (result.isSuccess)
                {

                    var oldModel = new tbl_Event_Cus();
                    oldModel.plateVN = oldObj.plateVN;
                    oldModel.plateCN = oldObj.plateCN;
                    oldModel.productGroup = oldObj.productGroup;
                    oldModel.weight = Convert.ToDecimal(oldObj.weight).ToString();
                    oldModel.EventType = oldObj.EventType;
                    oldModel.vehicleType = oldObj.vehicleType;
                    oldModel.PackageNumber = oldObj.PackageNumber;
                    oldModel.serviceCode = oldObj.serviceCode;
                    var jsStrOld = Newtonsoft.Json.JsonConvert.SerializeObject(oldModel);

                    //thông tin mới

                    var NewModel = new tbl_Event_Cus();
                    NewModel.plateVN = model.PlateVN;
                    NewModel.plateCN = model.PlateCN;
                    NewModel.productGroup = model.ProductGroup;
                    NewModel.weight = Convert.ToDecimal(model.Weight).ToString();
                    NewModel.EventType = 5;
                    NewModel.vehicleType = model.VehicleType;
                    NewModel.PackageNumber = model.PackageNumber;
                    NewModel.serviceCode = model.ServiceCode;
                    var jsStrNew = Newtonsoft.Json.JsonConvert.SerializeObject(NewModel);
                    await LogHelper.WriteLogupdateService(obj.Id.ToString(), "Kết thúc", "tbl_Event", JsonConvert.SerializeObject(obj), HttpContext, jsStrOld, jsStrNew);
                }
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return Json(result);
        }
        #endregion
    }
}
