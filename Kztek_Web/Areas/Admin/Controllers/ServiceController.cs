using Kztek_Core.Models;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private IColumTableService _ColumTableService;
        private ItblGroupServiceService _tblGroupServiceService;
        public ServiceController(ItblGroupServiceService _tblGroupServiceService, Itbl_EventService _tbl_EventService, IGroupService _GroupService, IServiceService _ServiceService, IColumTableService _ColumTableService)
        {
            this._tbl_EventService = _tbl_EventService;
            this._tblGroupServiceService = _tblGroupServiceService;
            this._ColumTableService = _ColumTableService;
            this._ServiceService = _ServiceService;
            this._GroupService = _GroupService;
        }


        #region DDL
        public async Task<List<SelectListModel>> GetAllGroupByService(string service)

        {
            var lstIdGr = await _tblGroupServiceService.GetAllByService(service);

            var lstGr = new List<Kztek_Model.Models.Group>();
            foreach (var item in lstIdGr)
            {
                var obj = new Kztek_Model.Models.Group();
                var m = await _GroupService.GetById(item.GroupId);
                if (m != null)
                {
                    obj.Id = m.Id;
                    obj.Code = m.Code;
                    obj.CreatedDate = m.CreatedDate;
                    obj.Description = m.Description;
                    obj.ModifiedDate = m.ModifiedDate;
                    obj.Name = m.Name;
                   
                    lstGr.Add(obj);
                }

            }
            List<Kztek_Model.Models.Group> lsGr = lstGr.OrderBy(group => group.Name).ToList();

            var list = new List<SelectListModel> { };
        
            if (lsGr.Any())
            {
                foreach (var item in lsGr)
                {
                    list.Add(new SelectListModel { ItemValue = item.Id, ItemText = item.Name });
                }
            }
            return /*(List<SelectListModel>)*/list;
        }
        private async Task<SelectListModel_Chosen> GetSelectModelGroupByService( string service ,string selecteds, string id = "GroupID" )
        {
            var data = await GetAllGroupByService(service);


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
        private async Task<SelectListModel_Chosen> GetAllGroup( string selecteds, string id = "GroupID")
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

        private async Task<dynamic> GetAllParkingPostion(string selecteds, string id = "ParkingPosition")
        {
            var data = await GetAllPosittion();


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
        public async Task<List<SelectListModel>> GetAllPosittion()
        {
            var list = new List<SelectListModel> { };
            var lst = await StaticList.ParkingPosittions();
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.ItemValue, ItemText = item.ItemText });
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
            var controller = "Service";
            var action = "Index";
            ViewBag.Eventype = await _tbl_EventService.GetEventypeService(selecteds: StatusID);

            ViewBag.LstService = await _ServiceService.SelectChoseService(selecteds: ServiceId);

            ViewBag.listSlectColumn = await _ServiceService.SelectColumn(controller,action);
           
            ViewBag.LstGrSelect = await _GroupService.GetaSelectModelChoseGroup(selecteds: GroupId);

            //ViewBag.showColumn = await _ColumTableService.GetDetailByController("Service", "Index");
            var obj = await _ColumTableService.GetDetailByController("Service", "Index");

            //ViewBag.StrShows = obj.ColumShows;

            ViewBag.fromdateValue = fromdate;

            ViewBag.todateValue = todate;

            ViewBag.AreaCodeValue = AreaCode;

            return View();
        }

        public async Task<IActionResult> Partial_Service(string StatusID = "", string ServiceId = "", string GroupId = "", string key = "", string fromdate = "", string todate = "", int page = 1,string Checkid = "")
        {
          

         
            var gridModel = await _tbl_EventService.GetPagingInOut(key, page, 20, StatusID, fromdate, todate, ServiceId, GroupId);

            ViewBag.Groups = await _GroupService.GetAll();

            ViewBag.lstService = await _ServiceService.GetAll();

            ViewBag.AuthValue = await AuthHelper.CheckAuthAction("Service", this.HttpContext);

            ViewBag.showColumn = await _ColumTableService.GetDetailByController("Service", "Index");

            ViewBag.keyValue = key;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.ServiceID = ServiceId;
            ViewBag.Check = Checkid;
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
        public async Task<IActionResult> Update(string id, int pageNumber = 1, string AreaCode = "" )
        {
            var model = await _tbl_EventService.GetByCustomById(id, this.HttpContext);

            ViewBag.AreaCodeValue = AreaCode;

            ViewBag.AllGroup = await GetAllGroup(model.GroupId);

            ViewBag.GetCurrentUser = await SessionCookieHelper.CurrentUser(this.HttpContext); 

            ViewBag.GetParkingPositon = await GetAllParkingPostion(model.ParkingPosition);
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
            ViewBag.GetParkingPositon = await GetAllParkingPostion(model.ParkingPosition);
            //Kiểm tra
            var oldObj = await _tbl_EventService.GetById(model.Id.ToString());
            if (oldObj == null)
            {
                ViewBag.Error = await LanguageHelper.GetLanguageText("MESSAGE:RECORD:NOTEXISTS");
                return View(model);
            }
            var oldObj1 = await _tbl_EventService.GetByCustomById(model.Id.ToString(), this.HttpContext);

            var user = await SessionCookieHelper.CurrentUser(HttpContext);
            if (user.isAdmin )
            {
                if (!ModelState.IsValid)
                {
                    return View(oldObj);
                }
                var s = Convert.ToDecimal(model.price);
                oldObj.PlateVN = model.plateVN;
                oldObj.PlateCN = model.plateCN;
                oldObj.ProductType = model.productType;
                oldObj.Weight = Convert.ToDecimal(model.weight);
                oldObj.VehicleType = model.vehicleType;
                oldObj.ServiceCode = model.serviceCode;
                oldObj.ProductGroup = model.productGroup;
                oldObj.Price = Convert.ToDecimal(model.price);
                //oldObj.EventType = 2;
                oldObj.SubPrice = Convert.ToDecimal(model.subPrice);
                oldObj.GroupId = model.GroupId != null ? model.GroupId : "";
                oldObj.Description = model.description;
                //oldObj.DivisionDate = model.DivisionDate != null ? model.DivisionDate : DateTime.MinValue;
                oldObj.ParkingPosition = model.ParkingPosition;
                oldObj.CreatedDate = DateTime.Now;
                oldObj.ModifiedDate = DateTime.Now;
            }
            else
            {
                if (user.TypeNotifi == "1")
                {
                    oldObj.ParkingPosition = model.ParkingPosition;
                    oldObj.Description = model.description;

                }
                else if(user.TypeNotifi == "2" )
                {
                    oldObj.Description = model.description;
                }
                else if (user.TypeNotifi == "3")
                {
                    oldObj.GroupId = model.GroupId != null ? model.GroupId : "";
                }
            }
            //oldObj.TimeInVN =
        
            //Thực hiện cập nhậts
            var result = await _tbl_EventService.Update(oldObj);


            if (result.isSuccess)
            {
                //thông tin cũ
            
                var oldModel = new tbl_Event_Cus();
                oldModel.plateVN = oldObj1.plateVN;
                oldModel.plateCN = oldObj1.plateCN;
                oldModel.productType = oldObj1.productType;
                oldModel.weight = oldObj1.weight.ToString();
                oldModel.vehicleType = oldObj1.vehicleType;
                oldModel.serviceCode = oldObj1.serviceCode;
                oldModel.productGroup = oldObj1.productGroup;
                oldModel.price = oldObj1.price;
                oldModel.EventType = oldObj1.EventType ;
                oldModel.ParkingPosition = oldObj1.ParkingPosition;
                oldModel.subPrice = (oldObj1.subPrice);
                oldModel.GroupId = oldObj1.GroupId != null ? oldObj.GroupId : "";
                oldModel.description = oldObj1.description;
                oldModel.PackageNumber = oldObj1.PackageNumber;
                var jsStrOld = Newtonsoft.Json.JsonConvert.SerializeObject(oldModel);

                //thông tin mới

                var NewModel = new tbl_Event_Cus();
                NewModel.plateVN = model.plateVN;
                NewModel.plateCN = model.plateCN;
                NewModel.productType = model.productType;
                NewModel.weight = model.weight != null ? model.weight : "" ;
                NewModel.vehicleType = model.vehicleType;
                NewModel.serviceCode = model.serviceCode;
                NewModel.productGroup = model.productGroup;
                NewModel.price = model.price;
                NewModel.EventType = model.EventType;
                NewModel.ParkingPosition = model.ParkingPosition;
                NewModel.subPrice = (model.subPrice);
                NewModel.GroupId = model.GroupId != null ? model.GroupId : "";
                NewModel.description = model.description;
                oldModel.PackageNumber = model.PackageNumber;
                var jsStrNew = Newtonsoft.Json.JsonConvert.SerializeObject(NewModel);
               await LogHelper.WriteLogupdateService(oldObj.Id.ToString(), ActionConfig.Update, "tbl_Event", JsonConvert.SerializeObject(oldObj), HttpContext, jsStrOld, jsStrNew);

                return RedirectToAction("Index","Service");
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
            var result = await _tbl_EventService.DeleteById(id , HttpContext);
            if (result.isSuccess)
            {
                await LogHelper.WriteLog(id, ActionConfig.Delete,"tbl_Event", id, HttpContext);
            }

            return Json(result);
        }


        #endregion Xóa

        #region Phân tổ
        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Assignment(string  ServiceId = "" ,string fromdate = "", string key = "", string ParkingPosittion = "")
        {
            ViewBag.LstService = await _ServiceService.SelectChoseService(selecteds: ServiceId);

            ViewBag.LstParkingPosit = await _ServiceService.SelectChoseParkingPosittion(selecteds: ParkingPosittion);

            ViewBag.Fromdate = fromdate;

            ViewBag.listSlectColumn = await _ServiceService.SelectColumnAssignment("Service", "Assignment");

        //    ViewBag.showColumn1 = await _ColumTableService.GetDetailByController("Service", "Assignment");

            return View();
        }

        public async Task<IActionResult> Partial_Vehicle(string key = "",  string ServiceId = "", string fromdate = "",  string ParkingPosittion = "" )
        {
            var list = await _tbl_EventService.GetListType2(key, ServiceId ,fromdate, ParkingPosittion);

            ViewBag.LstService = await _ServiceService.SelectChoseService(selecteds: ServiceId);
       
            ViewBag.lstService = await _ServiceService.GetAll();

            ViewBag.showColumn = await _ColumTableService.GetDetailByController("Service", "Assignment");

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
            foreach (var obj in list.OrderByDescending(n => n.StartDate))
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
                model.StartDate = obj.StartDate.Date != DateTime.MaxValue.Date ? obj.StartDate.ToString("dd/MM/yyyy HH:mm:ss") : "";
                model.EventTypeName = obj.EventType == 3 ? "<span class='label label-yellow'>Chưa thực hiện</span>" : "<span class='label' style='background-color: #385822'>Đang thực hiện</span>";
                model.EventType = obj.EventType;
                lst.Add(model);
            }
            ViewBag.Id = id;

            return PartialView(lst);
        }

        public async Task<IActionResult> Modal_Assign(string id)
        {          
            var objService = await _tbl_EventService.GetByCustomById(id,this.HttpContext);

            ViewBag.Group = await GetAllGroupByService(objService.service);
            ViewBag.TimeIntends = objService.TimeIntend;
            return PartialView(objService);
        }

      
        public async Task<IActionResult> SaveAssign(string id,string groupid , string TimeIntend)
        {
            var result = new MessageReport(false,"Có lỗi xảy ra!");

            var objService = await _tbl_EventService.GetById(id);

            if(objService != null)
            {
                objService.GroupId = groupid;

                objService.EventType = 3; //đã phân tổ

                objService.DivisionDate = DateTime.Now;

                objService.TimeIntend = Convert.ToDateTime(TimeIntend);

                result = await _tbl_EventService.Update(objService);

                //nếu phân tổ thành công thì load lại thông báo cho dg viên
                if (result.isSuccess)
                {
                    await SignalrHelper.SqlHub.Clients.All.SendAsync("Notifi");
                }
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return Json(result);
        }

        public async Task<IActionResult> Modal_UpdateGroup(string id)
        {
            var objService = await _tbl_EventService.GetByCustomById(id, this.HttpContext);

            ViewBag.Group = await GetAllGroupByService(objService.service);

            return PartialView(objService);
        }

        public async Task<IActionResult> UpdateGroup(string id, string groupid)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra!");

            var objService = await _tbl_EventService.GetById(id);

            if (objService != null)
            {
                objService.GroupId = !string.IsNullOrEmpty(groupid) ? groupid : "";

                objService.EventType = !string.IsNullOrEmpty(groupid) ? 3 : 2; 

                objService.DivisionDate = !string.IsNullOrEmpty(groupid) ? DateTime.Now : DateTime.MaxValue;

                objService.ModifiedDate = DateTime.Now;

                result = await _tbl_EventService.Update(objService);
            }
            else
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
            }

            return Json(result);
        }
        #endregion

        #region Hiện thị cột 

        public async Task<IActionResult> AddChooseSelect(string str,string controller, string action)
        {
            var obj = StaticList.ListDisplay_Display();
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
                objColum.ColumShows = str ;
                result = await _ColumTableService.Update(objColum); 
            }
            //var obj1 = await _ColumTableService.GetDetailByController(controller, action);
            //var shows = "";
            //if (result.isSuccess)
            //{
            //    shows = obj1.ColumShows;
            //}
            //result.Message = shows;
            return Json(result);
        }

        public async Task<IActionResult> AddChooseSelectAssignment(string str, string controller, string action)
        {
            var obj = StaticList.ListDisplay_DisplayAssignment();
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
            //var obj1 = await _ColumTableService.GetDetailByController(controller, action);
            //var shows = "";
            //if (result.isSuccess)
            //{
            //    shows = obj1.ColumShows;
            //}
            //result.Message = shows;
            return Json(result);
        }

        #endregion

        #region Thông báo
        public async Task<IActionResult> Partial_Notifi()
        {
            var user = await SessionCookieHelper.CurrentUser(HttpContext);

            var listData = new List<NotifiCustom>();

            if (user != null && user.TypeNotifi == "1" )
            {
                listData = await _tbl_EventService.NotifiSession1(HttpContext);
            }
            else if (user != null && user.TypeNotifi == "2")
            {
                listData = await _tbl_EventService.NotifiSession2(HttpContext);
            }
          
            return PartialView(listData);
        }
        #endregion
    }
}
