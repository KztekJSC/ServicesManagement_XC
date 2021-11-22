using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kztek_Web.Models;
using Kztek_Service.Admin;
using Kztek_Library.Models;
using Kztek_Web.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Microsoft.AspNetCore.SignalR;
using Kztek_Web.Hubs;
using System.Data.SqlClient;
using Kztek_Model.Models;
using Newtonsoft.Json;
using System.Data;

namespace Kztek_Web.Areas.Admin.Controllers
{
    [Area(AreaConfig.Admin)]
    public class HomeController : Controller
    {
       
        private readonly IHubContext<SignalServer> _context;
        private Itbl_EventService _tbl_EventService;

        public HomeController(IHubContext<SignalServer> context, Itbl_EventService _tbl_EventService)
        {
            _context = context;
            this._tbl_EventService = _tbl_EventService;
        }

        [CheckSessionCookie(AreaConfig.Admin)]
        public async Task<IActionResult> Index(int page = 1)
        {
            //kiểm tra session xem có lưu ngôn ngữ không nếu có thì lấy không mặc định là "vi"
            string sessionValue = HttpContext.Session.GetString(SessionConfig.Kz_Language);
            if (string.IsNullOrWhiteSpace(sessionValue))
                sessionValue = HttpContext.Request.Cookies[CookieConfig.Kz_LanguageCookie];
            sessionValue = String.IsNullOrEmpty(sessionValue) ? "vi" : sessionValue;
            LanguageHelper.GetLang(sessionValue);

          

            return View();
        }
        public async Task<IActionResult> Partial_TopService()
        {
          

            return PartialView();
        }


        public async Task<IActionResult> DashboardPartial(SelectListModel_Date model)
        {
          

            return PartialView();
        }

        public async Task<IActionResult> GetId()//Count(*) as SOLUONG 
        {
            string id = "";

            var connectionString = AppSettingHelper.GetStringFromFileJson("connectstring", "ConnectionStrings:DefaultConnection").Result;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
               
                SqlDependency.Start(connectionString);

                string commandText = string.Format("select Id from dbo.VehicleEvent where IsDelete = 0 order by ModifiedDate desc"); 

                SqlCommand cmd = new SqlCommand(commandText, conn);

                SqlDependency dependency = new SqlDependency(cmd);

                dependency.OnChange += new OnChangeEventHandler(dbChangeNotification);

                var reader = cmd.ExecuteReader();

                int count = 0;

                while (reader.Read())
                {
                    count++;

                    if(count == 1)
                    {
                        id = reader["Id"].ToString();

                        break;
                    }

                }             
            }

            return Json(id);
        }


        private void dbChangeNotification(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info == SqlNotificationInfo.Insert)
            {
                _context.Clients.All.SendAsync("refreshEmployees");
            }
           
        }
    }
}
