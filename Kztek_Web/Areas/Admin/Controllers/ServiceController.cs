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
        public ServiceController(Itbl_EventService _tbl_EventService, IGroupService _GroupService)
        {
            this._tbl_EventService = _tbl_EventService;
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
           
            ViewBag.Eventype = await _tbl_EventService.GetEventypeService(selecteds: StatusID);

            ViewBag.keyValue = key;  
            
            ViewBag.AreaCodeValue = AreaCode;

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Service", this.HttpContext);

            return View();
        }

        public async Task<IActionResult> Partial_Service(string StatusID = "", string key = "", string fromdate = "", string todate = "", int page = 1)
        {
          

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }
             
            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            var gridModel = await _tbl_EventService.GetPagingInOut(key, page, 20, StatusID, fromdate, todate);

            ViewBag.Groups = await _GroupService.GetAll();
            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Service", this.HttpContext);
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
            oldObj.Service = model.service;
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
        public async Task<IActionResult> Assignment(string StatusID = "", string key = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1, string AreaCode = "")
        {


            return View();
        }

        public async Task<IActionResult> Partial_Vehicle()
        {
            var list = await _tbl_EventService.GetListType2();

            return PartialView(list);
        }

        #endregion
    }
}
