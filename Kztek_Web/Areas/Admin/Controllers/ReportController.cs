

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
        public ReportController(IReportService _ReportService, Itbl_EventService _tbl_EventService , IServiceService _ServiceService)
        {
            this._tbl_EventService = _tbl_EventService;
            this._ServiceService = _ServiceService;
            this._ReportService = _ReportService;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }
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

            var gridModel = await _ReportService.GetPagingInOut(key, page, 10, StatusID, fromdate, todate , isCheckByTime);
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
            var lstdata = await _ReportService.GetPagingEvent_Excel(key,  page, 10, status, isCheckByTime, fromdate, todate);

            return await PrintHelper.Excel_Write<tbl_Event_Custom>(context, lstdata, "Event_" + DateTime.Now.ToString("ddMMyyyyHHmmss"), printConfig);
        }
    }


}
