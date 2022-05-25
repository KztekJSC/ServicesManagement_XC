using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface Itbl_EventService
    {
        Task<MessageReport> Create(tbl_Event obj);
        Task<MessageReport> DeleteById(string id , HttpContext HttpContext);
        Task<tbl_Event> GetById(string id);
        Task<MessageReport> Update(tbl_Event oldObj);
         Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int v, string statusID, string fromdate, string todate, string ServiceId = "", string GroupId = "");
        Task<SelectListModel_Chosen> GetEventype(string id = "", string placeholder = "", string selecteds = "");
        Task<GridModel<tbl_Event>> GetPagingCoordinatort(string key, int page, int v, string statusID, string fromdate, string todate, string ServiceId);
        Task<GridModel<tbl_Event>> GetPagingConfirmGroup(HttpContext httpContext, string key, int page, int v, string statusID, string fromdate, string todate);
        Task<tbl_Event> GetByService(string id);
        Task<SelectListModel_Chosen> GetEventypeService(string id = "", string placeholder = "", string selecteds = "");
        Task<tbl_Event_Cus> GetByCustomById(string id,HttpContext context);
        Task<SelectListModel_Chosen> GetEventypeCoordination(string id = "", string placeholder = "", string selecteds = "");

        /// <summary>
        /// Danh sách dịch vụ đã xác nhận
        /// Dùng cho giao diện phân tổ
        /// </summary>
        /// <returns></returns>
        Task<List<tbl_Event>> GetListType2(string key = "", string ServiceId = "", string fromdate = "", string ParkingPosittion = "" );

        /// <summary>
        /// Đếm số lượng dịch vụ đã giao của từng tổ
        /// </summary>
        /// <returns></returns>
        Task<List<Model_ServiceByGroup>> GetCountServiceByGroup();

        /// <summary>
        /// Danh sách chi tiết dịch vụ được giao của tổ
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        Task<List<tbl_Event>> GetListServiceByGroup(string groupid);
        Task<SelectListModel_Chosen>  GetEventypeReport(string id = "", string placeholder = "", string selecteds = "");


        /// <summary>
        /// Thống kê công việc cần làm của tổ
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        Task<List<CountEventByType>> CountEventByType(HttpContext httpContext,string fromdate);

        /// <summary>
        /// Thông báo cho giao dịch viên
        /// </summary>
        /// <param name="HttpContext"></param>
        /// <returns></returns>
        Task<List<NotifiCustom>> NotifiSession1(HttpContext HttpContext);
        Task<List<NotifiCustom>> NotifiSession2(HttpContext httpContext);
    }
}
