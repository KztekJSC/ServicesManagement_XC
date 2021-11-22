
using Kztek.LedController;
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
        public ReportController(IReportService _ReportService, Itbl_EventService _tbl_EventService)
        {
            this._tbl_EventService = _tbl_EventService;
            this._ReportService = _ReportService;
        }
        #endregion

        public IActionResult Index()
        {
            return View();
        }

    }


}
