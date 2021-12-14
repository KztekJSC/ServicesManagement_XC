using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Model.Models;
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
        Task<MessageReport> DeleteById(string id);
        Task<tbl_Event> GetById(string id);
        Task<MessageReport> Update(tbl_Event oldObj);
         Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int v, string statusID, string fromdate, string todate);
        Task<SelectListModel_Chosen> GetEventype(string id = "", string placeholder = "", string selecteds = "");
        Task<GridModel<tbl_Event>> GetPagingCoordinatort(string key, int page, int v, string statusID, string fromdate, string todate);
        Task<GridModel<tbl_Event>> GetPagingConfirmGroup(string key, int page, int v, string statusID, string fromdate, string todate);
        Task<SelectListModel_Chosen> GetEventypeService(string id = "", string placeholder = "", string selecteds = "");
        Task<tbl_Event_Cus> GetByCustomById(string id);
        Task<SelectListModel_Chosen> GetEventypeCoordination(string id = "", string placeholder = "", string selecteds = "");

        /// <summary>
        /// Danh sách dịch vụ đã xác nhận
        /// Dùng cho giao diện phân tổ
        /// </summary>
        /// <returns></returns>
        Task<List<tbl_Event>> GetListType2();

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
    }
}
