using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface IReportService
    {
        Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int v, string statusID, string fromdate, string todate ,string isCheckByTime);
         Task<List<tbl_Event_Custom>> GetPagingEvent_Excel(string key, int page, int v, string status, string isCheckByTime, string fromdate, string todate);
        Task<List<ServiceCustom>> GetByService (string key, int page, int v, string statusID, string fromdate, string todate, string isCheckByTime  , string ServiceId);
        Task<List<GroupCustom>> GetByGroup(string key, int page, int v, string statusID, string fromdate, string todate, string isCheckByTime,string GroupId);
        Task<List<GroupCustom>> GetByTime(string key, int page, int v, string statusID, string fromdate, string todate, string isCheckByTime, string groupId);
        //Task<List<ServiceCustom>> GetByService_Excel(string key, string fromdate, string todate);
    }
}
