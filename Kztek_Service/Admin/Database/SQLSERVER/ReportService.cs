using Kztek_Core.Models;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class ReportService : IReportService
    {
        private IServiceService _ServiceService;
        private IGroupService _GroupService;

        public ReportService(IServiceService _ServiceService, IGroupService _GroupService)
        {
            this._ServiceService = _ServiceService;
            this._GroupService = _GroupService;
        }
        #region Theo tổ
        public async Task<List<GroupCustom>> GetByGroup(string key, int page, int v, string statusID, string fromdate, string todate, string isCheckByTime, string Groupid)
        {

            var objName = await _GroupService.GetByName(key.Trim());
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");

            sb.AppendLine("SELECT ROW_NUMBER () OVER ( ORDER BY GroupId desc) AS RowNumber,C.* FROM (");

            sb.AppendLine(string.Format("SELECT   Count(GroupId) AS 'CountGroup' ,CAST( CASE WHEN GroupId <> '' THEN GroupId ELSE 'Không có tên' END AS nvarchar(50)) as GroupId , Sum(Price) AS 'SumPrice', Sum(SubPrice) AS 'SumSub'  FROM  [tbl_Event]"));

            sb.AppendLine("WHERE 1 =1 AND  IsDeleted = 0");

            if (!string.IsNullOrEmpty(Groupid))
            {
                if (Groupid != "00")
                {
                    sb.AppendLine(string.Format("AND  GroupId IN ('{0}')    ", Groupid));
                }



            }
            switch (isCheckByTime)
            {

                case "0"://tg bắt đầu
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate < '{0}'", tdate));
                    }
                    break;

                case "1"://phân tổ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND DivisionDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND [DivisionDate] < '{0}'", tdate));
                    }
                    break;
                case "2"://kết thúc
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] < '{0}'", tdate));
                    }
                    break;
                //case "4"://Không check thời gian

                //    break;

                default:
                    break;
            }
            sb.AppendLine("GROUP BY [GroupId]  ");

            sb.AppendLine(") AS C");

            sb.AppendLine(") AS B");

            var listData = DatabaseHelper.ExcuteCommandToList<GroupCustom>(sb.ToString());

            return listData;
        }

        #endregion

        #region Theo dịch vụ
        public async Task<List<ServiceCustom>> GetByService(string key, int page, int v, string statusID, string fromdate, string todate, string isCheckByTime,string ServiceId)
        {
            var objName = await _ServiceService.GetByName(key);


            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");

            sb.AppendLine("SELECT ROW_NUMBER () OVER ( ORDER BY [ServiceId] desc) AS RowNumber,C.* FROM (");

            sb.AppendLine(string.Format("SELECT   Count(Service) AS 'CountService' ,[Service] as 'ServiceId', Sum(Price) AS 'SumPrice', Sum(SubPrice) AS 'SumSub'  FROM  [tbl_Event]"));

            sb.AppendLine("WHERE 1 =1 AND  IsDeleted = 0");
            if (!string.IsNullOrEmpty(ServiceId))
            {
                if (ServiceId != "00")
                {
                    sb.AppendLine(string.Format("AND  Service IN ('{0}')    ", ServiceId));
                }
            }
            switch (isCheckByTime)
            {

                case "0"://tg bắt đầu
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate < '{0}'", tdate));
                    }
                    break;

                case "1"://phân tổ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND DivisionDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND [DivisionDate] < '{0}'", tdate));
                    }
                    break;
                case "2"://kết thúc
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] < '{0}'", tdate));
                    }
                    break;
              

                default:
                    break;
            }

            sb.AppendLine("GROUP BY [Service]  ");

            sb.AppendLine(") AS C");

            sb.AppendLine(") AS B");
            var listData = DatabaseHelper.ExcuteCommandToList<ServiceCustom>(sb.ToString());



            return listData;
        }

        public async Task<DataTable> GetByTime(string key, int page, int v, string statusID, string _fromdate, string _todate, string isCheckByTime, string groupId)
        {
            var connect = await AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:DefaultConnection");
            if (!string.IsNullOrEmpty(_fromdate))
            {
                _fromdate = Convert.ToDateTime(_fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(_todate))
            {
                _todate = Convert.ToDateTime(_todate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            var query = new StringBuilder();
            query.AppendLine("DECLARE @list AS NVARCHAR(MAX),@query  AS NVARCHAR(MAX)");
            query.AppendLine("SELECT @list =");
            query.AppendLine("COALESCE(@list + ', ','') + QUOTENAME(date)");
            query.AppendLine("FROM (");
            query.AppendLine("Select (CONVERT(nvarchar(50), mon) + '/' + CONVERT(nvarchar(50), yea)) as date");
            query.AppendLine("FROM (");
            query.AppendLine("Select distinct MONTH(CreatedDate) as mon,Year(CreatedDate) as yea FROM tbl_Event ce WITH (NOLOCK) where  ");
            query.AppendLine(string.Format(" ce.[CreatedDate] >= '{0}' AND ce.[CreatedDate] <= '{1}'", _fromdate, _todate));
            query.AppendLine("Order by yea asc,mon asc OFFSET 0 ROWS");
            query.AppendLine(") AS C ");
            query.AppendLine(") as k");
            query.AppendLine("set @query ='select * from(");
            query.AppendLine("Select TotalMoney,Service,(CONVERT(nvarchar(50), mon) + ''' + '/' + ''' + CONVERT(nvarchar(50), yea)) as date   FROM (");
            query.AppendLine("Select SUM(Price) as TotalMoney,mon,yea ,Service FROM (");
            query.AppendLine("Select Price,Service,MONTH(CreatedDate) as mon,Year(CreatedDate) as yea FROM tbl_Event ce WITH (NOLOCK)  ");
            query.AppendLine(string.Format(" where  ce.[CreatedDate] >= ''' + cast('{0}' as VARCHAR(50)) + '''AND ce.[CreatedDate] <= ''' + cast('{1}' as VARCHAR(50)) + '''", _fromdate, _todate));
            query.AppendLine("Order by CreatedDate desc OFFSET 0 ROWS");
            query.AppendLine(") as A");
            query.AppendLine("Group by mon,yea,Service");
            query.AppendLine("Order by yea asc,mon asc OFFSET 0 ROWS");
            query.AppendLine(") AS C ");
            query.AppendLine(") as s PIVOT");
            query.AppendLine("(");
            query.AppendLine("SUM(TotalMoney)");
            query.AppendLine("  FOR date IN ('+@list+')");
            query.AppendLine(")AS pvt'");
            query.AppendLine("execute(@query)");
            var list = SqlHelper.ExcuteCommandToDataTable(connect, query.ToString());
            
            return list;
        }

      

        #endregion


        #region danh sách sự kiện
        public async Task<List<tbl_Event_Custom>> GetPagingEvent_Excel(string key, int page, int pageSize, string statusID, string isCheckByTime, string fromdate, string todate)
        {
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;

            var sb = new StringBuilder();

            sb.AppendLine("SELECT * FROM (");

            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) AS RowNumber,a.*", "CreatedDate"));

            sb.AppendLine("FROM(");

            sb.AppendLine("SELECT * FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 =1 AND  IsDeleted = 0");

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
            }
            switch (isCheckByTime)
            {

                case "0"://tg bắt đầu
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate < '{0}'", tdate));
                    }
                    break;

                case "1"://phân tổ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND DivisionDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND [DivisionDate] < '{0}'", tdate));
                    }
                    break;
                case "2"://kết thúc
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] < '{0}'", tdate));
                    }
                    break;
                //case "4"://Không check thời gian

                //    break;

                default:
                    break;
            }

            //event Code
            if (!string.IsNullOrWhiteSpace(statusID) && statusID != "00")
            {
                var t = statusID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([EventType] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }

            sb.AppendLine(") AS a");

            sb.AppendLine(") AS C1");

            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", page, pageSize));

            var listData = DatabaseHelper.ExcuteCommandToList<tbl_EventExcel>(sb.ToString());
            var lst = new List<tbl_Event_Custom>();
            if (listData.Count > 0)
            {
                foreach (var item in listData)
                {
                    var objService = await _ServiceService.GetById(item.Service);
                    var obj = new tbl_Event_Custom();
                    obj.RowNumber = item.RowNumber;
                    obj.serviceCode = item.ServiceCode;
                    obj.plateVN = item.PlateVN;
                    obj.plateCN = item.PlateVN;
                    obj.productType = item.ProductType;
                    obj.weight = item.Weight.ToString();
                    obj.vehicleType = item.VehicleType;
                    obj.productGroup = item.ProductGroup;
                    obj.serviceName = objService.Name;
                    obj.price = item.Price.ToString("###,###.##");
                    obj.subPrice = item.SubPrice.ToString("###,###.##");
                    obj.description = item.Description;
                    if (item.StartDate.ToString("dd/MM/yyyy") == "31/12/9999")
                    {
                        obj.StartDate = "";
                    }
                    else
                    {
                        obj.StartDate = item.StartDate.ToString();
                    }
                    if (item.EndDate.ToString("dd/MM/yyyy") == "31/12/9999")
                    {
                        obj.EndDate = "";
                    }
                    else
                    {
                        obj.EndDate = item.EndDate.ToString();
                    }

                    if (item.DivisionDate.ToString("dd/MM/yyyy") == "31/12/9999")
                    {
                        obj.DivisionDate = "";
                    }
                    else
                    {
                        obj.DivisionDate = item.DivisionDate.ToString();
                    }

                    switch (item.EventType)
                    {
                        case 1:
                            obj.eventTypeName = "Chờ xác nhận";
                            break;
                        case 2:
                            obj.eventTypeName = "Chưa phân tổ";
                            break;
                        case 3:
                            obj.eventTypeName = "Đã phân tổ";
                            break;
                        case 4:
                            obj.eventTypeName = "Đang thực hiện";
                            break;
                        case 5:
                            obj.eventTypeName = "Chờ duyệt";
                            break;
                        case 6:
                            obj.eventTypeName = "Hoàn thành";
                            break;
                        default:
                            break;
                    }
                    lst.Add(obj);
                }
            }



            return await Task.FromResult(lst);
        }

        public async Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int pageSize, string statusID, string fromdate, string todate, string isCheckByTime)
        {
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;

            var sb = new StringBuilder();

            sb.AppendLine("SELECT * FROM (");

            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) AS RowNumber,a.*", "CreatedDate"));

            sb.AppendLine("FROM(");

            sb.AppendLine("SELECT * FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 =1 AND  IsDeleted = 0");

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
            }

            switch (isCheckByTime)
            {

                case "0"://tg bắt đầu
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate < '{0}'", tdate));
                    }
                    break;

                case "1"://phân tổ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND DivisionDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND [DivisionDate] < '{0}'", tdate));
                    }
                    break;
                case "2"://kết thúc
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] < '{0}'", tdate));
                    }
                    break;
                //case "4"://Không check thời gian

                //    break;

                default:
                    break;
            }
            //event Code
            if (!string.IsNullOrWhiteSpace(statusID) && statusID != "00")
            {
                var t = statusID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([EventType] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }

            sb.AppendLine(") AS a");

            sb.AppendLine(") AS C1");

            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", page, pageSize));

            var listData = DatabaseHelper.ExcuteCommandToList<tbl_Event>(sb.ToString());


            // Tính tổng
            sb.Clear();

            sb.AppendLine("SELECT COUNT(*) TotalCount");

            sb.AppendLine("FROM [tbl_Event] where 1 = 1   AND  IsDeleted = 0");

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%') ", key));
            }
            switch (isCheckByTime)
            {

                case "0"://tg bắt đầu
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND StartDate < '{0}'", tdate));
                    }
                    break;

                case "1"://phân tổ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND DivisionDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND [DivisionDate] < '{0}'", tdate));
                    }
                    break;
                case "2"://kết thúc
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND [EndDate] < '{0}'", tdate));
                    }
                    break;
                //case "4"://Không check thời gian

                //    break;

                default:
                    break;
            }
            //event Code
            if (!string.IsNullOrWhiteSpace(statusID) && statusID != "00")
            {
                var t = statusID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND ([EventType] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }
            var _total = DatabaseHelper.ExcuteCommandToModel<TotalPagingModel>(sb.ToString());

            var model = GridModelHelper<tbl_Event>.GetPage(listData, page, pageSize, _total.TotalCount);

            return await Task.FromResult(model);
        }
        #endregion

    }
}
