

using Kztek_Core.Models;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Kztek_Web.Attributes;
using Kztek_Web.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base;
using TableDependency.SqlClient.Base.EventArgs;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class ReportController : Controller
    {
        #region Service
        private IReportService _ReportService;
        private Itbl_EventService _tbl_EventService;
        private IServiceService _ServiceService;
        private IGroupService _GroupService;
        public ReportController(IReportService _ReportService, Itbl_EventService _tbl_EventService, IServiceService _ServiceService, IGroupService _GroupService)
        {
            this._tbl_EventService = _tbl_EventService;
            this._ServiceService = _ServiceService;
            this._ReportService = _ReportService;
            this._GroupService = _GroupService;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }
        #region Danh sách sự kiện
        /// <summary>
        /// Danh sách sự kiện
        /// </summary>
        /// <param name="StatusID"></param>
        /// <param name="key"></param>
        /// <param name="isCheckByTime"></param>
        /// <param name="chkExport"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="page"></param>
        /// <param name="AreaCode"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReportEvent(string StatusID = "", string key = "", string isCheckByTime = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1, string AreaCode = "")
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
            if (chkExport.Equals("1"))
            {
                await ExportFile(key, page, 10, StatusID, isCheckByTime, fromdate, todate, this.HttpContext);

                //return View(gridmodel);
            }

            var gridModel = await _ReportService.GetPagingInOut(key, page, 10, StatusID, fromdate, todate, isCheckByTime);
            ViewBag.Eventype = await _tbl_EventService.GetEventypeReport(selecteds: StatusID);
            ViewBag.isFilterByTimeIn = isCheckByTime;
            ViewBag.keyValue = key;
            ViewBag.Status = StatusID;
            ViewBag.todateValue = string.IsNullOrWhiteSpace(todate) ? DateTime.Now.ToString("đd/MM/yyyy 23:59") : todate;
            ViewBag.AreaCodeValue = AreaCode;
            ViewBag.fromdateValue = fromdate;
            ViewBag.lstService = await _ServiceService.GetAll();

            return View(gridModel);
        }


        private async Task<bool> ExportFile(string key, int page, int pageSize, string status, string isCheckByTime, string fromdate, string todate, HttpContext context)
        {

            //column header
            var Data_ColumnHeader = new List<SelectListModel_Print_Column_Header>();
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "STT" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Số trang" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Trạng thái" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Xe VN" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Xe CN" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Loại hàng" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Khối lượng" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Loại xe" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Nhóm hàng" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Dịch vụ" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Giá dịch vụ" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Phụ thu" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "T/G bắt đầu" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "T/G phân tổ" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "T/G hoàn thành" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Ghi chú" });
            //
            var printConfig = PrintHelper.Template_Excel_V1(PrintConfig.HeaderType.TwoColumns, "Danh sách sự kiện", DateTime.Now, SessionCookieHelper.CurrentUser(this.HttpContext).Result, "Kztek", Data_ColumnHeader, 4, 5, 5);

            //
            var lstdata = await _ReportService.GetPagingEvent_Excel(key, page, 10, status, isCheckByTime, fromdate, todate);

            return await PrintHelper.Excel_Write<tbl_Event_Custom>(context, lstdata, "Event_" + DateTime.Now.ToString("ddMMyyyyHHmmss"), printConfig);
        }
        #endregion

        #region Thống kê
        #region Theo loại dịch vụ
        /// <summary>
        /// Theo loại dịch vụ
        /// </summary>
        /// <param name="StatusID"></param>
        /// <param name="key"></param>
        /// <param name="isCheckByTime"></param>
        /// <param name="chkExport"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="page"></param>
        /// <param name="AreaCode"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReportByService(string StatusID = "", string key = "", string isCheckByTime = "",string ServiceId ="" , string chkExport = "0", string fromdate = "", string todate = "", int page = 1, string AreaCode = "")
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
            if (chkExport.Equals("1"))
            {
                await ExportFileByService(key, page, 10, StatusID, fromdate, todate, isCheckByTime , this.HttpContext , ServiceId);

                //return View(gridmodel);
            }

            var gridModel = await _ReportService.GetByService(key, page, 10, StatusID, fromdate, todate, isCheckByTime, ServiceId);
            ViewBag.isFilterByTimeIn = isCheckByTime;
            ViewBag.fromdateValue = fromdate;
            ViewBag.Service = await _ServiceService.GetAll();
            ViewBag.LstService = await _ServiceService.SelectChoseService(selecteds: ServiceId);
            return View(gridModel);
        }

        private async Task<bool> ExportFileByService(string key, int page,int pagesize,string status , string fromdate, string todate, string isCheckByTime,HttpContext httpContext ,string ServiceId)
        {
            //column header
            var Data_ColumnHeader = new List<SelectListModel_Print_Column_Header>();
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "STT" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Dịch vụ" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Số lượng dịch vụ" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Số tiền(VNĐ)" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Phụ thu(VNĐ)" });
         
            //
            var printConfig = PrintHelper.Template_Excel_V1(PrintConfig.HeaderType.TwoColumns, "Theo loại dich vụ", DateTime.Now, SessionCookieHelper.CurrentUser(this.HttpContext).Result, "Kztek", Data_ColumnHeader, 4, 5, 5);
        
            //
            var lstdata = await _ReportService.GetByService(key, page, 10, status, fromdate, todate, isCheckByTime, ServiceId);
            var lst = new List<ServiceCustom>();
            foreach (var item in lstdata)
            {
                var obj1 = await _ServiceService.GetById(item.ServiceId);
                var objService = new ServiceCustom();
                objService.RowNumber = item.RowNumber;
                objService.ServiceName = obj1.Name;
                objService.CountService = item.CountService;
                objService.SumPrice = item.SumPrice;
                objService.SumSub = item.SumSub;
                lst.Add(objService);

            }

            return await PrintHelper.Excel_Write<ServiceCustom>(httpContext, lst, "Event_" + DateTime.Now.ToString("ddMMyyyyHHmmss"), printConfig);
        }

        #endregion

        #region Theo tổ thực hiện
        /// <summary>
        /// Theo loại dịch vụ
        /// </summary>
        /// <param name="StatusID"></param>
        /// <param name="key"></param>
        /// <param name="isCheckByTime"></param>
        /// <param name="chkExport"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="page"></param>
        /// <param name="AreaCode"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReportByGroup(string StatusID = "",string GroupId = "", string key = "", string isCheckByTime = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1, string AreaCode = "")
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
            if (chkExport.Equals("1"))
            {
                await ExportFileByGroup(key, page, 10, StatusID, fromdate, todate, isCheckByTime, this.HttpContext,GroupId);

                //return View(gridmodel);
            }

            var gridModel = await _ReportService.GetByGroup(key, page, 10, StatusID, fromdate, todate, isCheckByTime,GroupId);
            ViewBag.lstGroup = await _GroupService.GetAll();
            ViewBag.isFilterByTimeIn = isCheckByTime;
            ViewBag.fromdateValue = fromdate;
          
            return View(gridModel);
        }

        private async Task<bool> ExportFileByGroup(string key, int page, int pagesize, string status, string fromdate, string todate, string isCheckByTime, HttpContext httpContext, string GroupId)
        {
            //column header
            var Data_ColumnHeader = new List<SelectListModel_Print_Column_Header>();
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "STT" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Tổ" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Số lượng" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Số tiền(VNĐ)" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Phụ thu(VNĐ)" });

            //
            var printConfig = PrintHelper.Template_Excel_V1(PrintConfig.HeaderType.TwoColumns, "Tổ thực hiện", DateTime.Now, SessionCookieHelper.CurrentUser(this.HttpContext).Result, "Kztek", Data_ColumnHeader, 4, 5, 5);

            //
            var lstdata = await _ReportService.GetByGroup(key, page, 10, status, fromdate, todate, isCheckByTime, GroupId);
            var lst = new List<GroupCustomExcel>();
            foreach (var item in lstdata)
            {
                var obj1 = await _GroupService.GetById(item.GroupId);
               
                var objGr = new GroupCustomExcel();
                objGr.RowNumber = item.RowNumber;
                if (obj1 != null)
                {
                    objGr.GroupName = obj1.Name;
                }
                else
                {
                    objGr.GroupName = item.GroupId;
                }
                objGr.CountGroup = item.CountGroup;
                objGr.SumPrice = item.SumPrice.ToString("###,###.##");
                objGr.SumSub = item.SumSub.ToString("###,###.##");
                lst.Add(objGr);

            }

            return await PrintHelper.Excel_Write<GroupCustomExcel>(httpContext, lst, "Event_" + DateTime.Now.ToString("ddMMyyyyHHmmss"), printConfig);
        }

        #endregion

        #region Theo thời gian
        /// <summary>
        /// Theo loại dịch vụ
        /// </summary>
        /// <param name="StatusID"></param>
        /// <param name="key"></param>
        /// <param name="isCheckByTime"></param>
        /// <param name="chkExport"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="page"></param>
        /// <param name="AreaCode"></param>
        /// <returns></returns>
        public async Task<IActionResult> ReportByTime(string StatusID = "", string GroupId = "", string key = "", string isCheckByTime = "", string chkExport = "0", string fromdate = "", string todate = "", int page = 1, string AreaCode = "")
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
            if (chkExport.Equals("1"))
            {
                await ExportFileByTime(key, page, 10, StatusID, fromdate, todate, isCheckByTime, this.HttpContext, GroupId);

                //return View(gridmodel);
            }

            var gridModel = await _ReportService.GetByTime(key, page, 10, StatusID, fromdate, todate, isCheckByTime, GroupId);
            ViewBag.lstGroup = await _GroupService.GetAll();
            ViewBag.isFilterByTimeIn = isCheckByTime;
            ViewBag.fromdateValue = fromdate;

            return View(gridModel);
        }

        private async Task<bool> ExportFileByTime(string key, int page, int pagesize, string status, string fromdate, string todate, string isCheckByTime, HttpContext httpContext, string GroupId)
        {
            //column header
            var Data_ColumnHeader = new List<SelectListModel_Print_Column_Header>();
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "STT" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Tổ" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Số lượng" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Số tiền(VNĐ)" });
            Data_ColumnHeader.Add(new SelectListModel_Print_Column_Header { ItemText = "Phụ thu(VNĐ)" });

            //
            var printConfig = PrintHelper.Template_Excel_V1(PrintConfig.HeaderType.TwoColumns, "Tổ thực hiện", DateTime.Now, SessionCookieHelper.CurrentUser(this.HttpContext).Result, "Kztek", Data_ColumnHeader, 4, 5, 5);

            //
            var lstdata = await _ReportService.GetByGroup(key, page, 10, status, fromdate, todate, isCheckByTime, GroupId);
            var lst = new List<GroupCustomExcel>();
            foreach (var item in lstdata)
            {
                var obj1 = await _GroupService.GetById(item.GroupId);

                var objGr = new GroupCustomExcel();
                objGr.RowNumber = item.RowNumber;
                if (obj1 != null)
                {
                    objGr.GroupName = obj1.Name;
                }
                else
                {
                    objGr.GroupName = item.GroupId;
                }
                objGr.CountGroup = item.CountGroup;
                objGr.SumPrice = item.SumPrice.ToString("###,###.##");
                objGr.SumSub = item.SumSub.ToString("###,###.##");
                lst.Add(objGr);

            }

            return await PrintHelper.Excel_Write<GroupCustomExcel>(httpContext, lst, "Event_" + DateTime.Now.ToString("ddMMyyyyHHmmss"), printConfig);
        }

        #endregion
        #endregion
    }


}
