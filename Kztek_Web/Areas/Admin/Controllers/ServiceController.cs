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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class ServiceController : Controller
    {
        private Itbl_EventService _tbl_EventService;
        private IGroupService _GroupService;
        private IServiceService _ServiceService;
        public ServiceController(Itbl_EventService _tbl_EventService, IGroupService _GroupService, IServiceService _ServiceService)
        {
            this._tbl_EventService = _tbl_EventService;
            this._ServiceService = _ServiceService;
            this._GroupService = _GroupService;
        }


        #region DDL

        private async Task<SelectListModel_Chosen> GetAllGroup(string selecteds, string id = "GroupID")
        {
            var data = await GetAllGroup();


            var cus = new List<SelectListModel>();
            var lst = data;
            if (lst != null && lst.Count > 0)
            {
                cus.Add(new SelectListModel()
                {
                    ItemText = "---- Lựa chọn ----",
                    ItemValue = ""
                });

                cus.AddRange(data.Select(n => new SelectListModel()
                {
                    ItemText = n.ItemText,
                    ItemValue = n.ItemValue
                }));
            }

            var model = new SelectListModel_Chosen
            {
                Data = cus,
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

        #region Danh sách
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(string StatusID = "", string ServiceId ="" ,string GroupId = "",string key = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1, string AreaCode = "")
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
           
            ViewBag.Eventype = await _tbl_EventService.GetEventypeService(selecteds: StatusID);

            ViewBag.LstService = await _ServiceService.SelectChoseService(selecteds: ServiceId);

            ViewBag.LstGrSelect = await _GroupService.GetaSelectModelChoseGroup(selecteds: GroupId);

          
            ViewBag.AreaCodeValue = AreaCode;

            return View();
        }

        public async Task<IActionResult> Partial_Service(string StatusID = "", string ServiceId = "", string GroupId = "", string key = "", string fromdate = "", string todate = "", int page = 1)
        {
          

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }
             
            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            var gridModel = await _tbl_EventService.GetPagingInOut(key, page, 20, StatusID, fromdate, todate, ServiceId, GroupId);

            ViewBag.Groups = await _GroupService.GetAll();

            ViewBag.lstService = await _ServiceService.GetAll();

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Service", this.HttpContext);

            ViewBag.keyValue = key;

            ViewBag.ServiceID = ServiceId;

            ViewBag.GroupID = GroupId;
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
        public async Task<IActionResult> Update(string id, int pageNumber = 1, string AreaCode = "")
        {
            var model = await _tbl_EventService.GetByCustomById(id);
            ViewBag.AreaCodeValue = AreaCode;
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
        public async Task<IActionResult> Update(tbl_Event_Cus model, int pageNumber = 1, string AreaCode = "")
        {
            ViewBag.AreaCodeValue = AreaCode;
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
            var s = Convert.ToDecimal(model.price);
            oldObj.PlateVN = model.plateVN;
            oldObj.PlateCN = model.plateCN;
            oldObj.ProductType = model.productType;
            oldObj.Weight = Convert.ToDecimal( model.weight);
            oldObj.VehicleType = model.vehicleType;
            oldObj.ServiceCode = model.serviceCode;
            oldObj.ProductGroup = model.productGroup;       
            oldObj.Price = Convert.ToDecimal( model.price);
          
            oldObj.SubPrice = Convert.ToDecimal(model.subPrice);
            oldObj.GroupId = model.GroupId != null ? model.GroupId : "";
            oldObj.Description = model.description;
            //oldObj.DivisionDate = model.DivisionDate != null ? model.DivisionDate : DateTime.MinValue;
            oldObj.CreatedDate = DateTime.Now;
            oldObj.ModifiedDate = DateTime.Now;
            //oldObj.TimeInVN =

            //Thực hiện cập nhậts
            var result = await _tbl_EventService.Update(oldObj);


            if (result.isSuccess)
            {

                await LogHelper.WriteLog(oldObj.Id.ToString(), ActionConfig.Update,"tbl_Event", JsonConvert.SerializeObject(oldObj), HttpContext);
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
                await LogHelper.WriteLog(id, ActionConfig.Delete,"tbl_Event", id, HttpContext);
            }

            return Json(result);
        }


        #endregion Xóa

        #region Phân tổ
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Assignment()
        {


            return View();
        }

        public async Task<IActionResult> Partial_Vehicle()
        {
            var list = await _tbl_EventService.GetListType2();
            ViewBag.lstService = await _ServiceService.GetAll();
            return PartialView(list);
        }

        public async Task<IActionResult> Partial_Group()
        {
            var list = await _tbl_EventService.GetCountServiceByGroup();

            ViewBag.Group = await GetAllGroup();

            return PartialView(list);
        }
        public async Task<IActionResult> Partial_GroupDetail(string id)
        {
            var list = await _tbl_EventService.GetListServiceByGroup(id);
         
            var lst = new List<tbl_Event_Cus>();
            foreach (var obj in list)
            {
                var objService = await _ServiceService.GetById(obj.Service);
                var model = new tbl_Event_Cus();
                model.Id = obj.Id;
                model.serviceCode = obj.ServiceCode;
                model.plateVN = obj.PlateVN;
                model.plateCN = obj.PlateCN;
                model.productType = obj.ProductType;
                model.weight = obj.Weight.ToString();
                model.vehicleType = obj.ProductType;
                model.productGroup = obj.ProductGroup;
                model.service = obj.Service;
                model.price = obj.Price.ToString("###,###.##");
                model.subPrice = obj.SubPrice.ToString("###,###.##");
                model.GroupId = obj.GroupId;
                model.description = obj.Description;
                model.serviceName = objService.Name;
                lst.Add(model);
            }
            ViewBag.Id = id;

            return PartialView(lst);
        }

        public async Task<IActionResult> Modal_Assign(string id)
        {
           
            var objService = await _tbl_EventService.GetByCustomById(id);
            ViewBag.Group = await GetAllGroup();

            return PartialView(objService);
        }

        public async Task<IActionResult> SaveAssign(string id,string groupid)
        {
            var result = new MessageReport(false,"Có lỗi xảy ra!");

            var objService = await _tbl_EventService.GetById(id);

            if(objService != null)
            {
                objService.GroupId = groupid;

                objService.EventType = 3; //đã phân tổ

                objService.DivisionDate = DateTime.Now;

                result = await _tbl_EventService.Update(objService);
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
