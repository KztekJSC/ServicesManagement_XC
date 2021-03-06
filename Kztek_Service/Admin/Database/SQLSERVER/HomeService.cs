using Kztek_Core.Models;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class HomeService : IHomeService
    {
        public async Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int pageSize, string groupid, string fromdate, string todate, string StatusID)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) as RowNumber,a.*", "StartDate"));
            sb.AppendLine("FROM(");
            sb.AppendLine("  select * from [tbl_Event]");
            sb.AppendLine("WHERE 1 =1 AND EventType IN (2 , 3 ,4 ,5,6 ) AND  IsDeleted = 0");

            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                sb.AppendLine(string.Format("AND CreatedDate >= '{0}'  AND CreatedDate <= '{1}'", Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss"), Convert.ToDateTime(todate).ToString("yyyy/MM/dd HH:mm:ss")));
            }

            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE (REPLACE(REPLACE([PlateVN], '-', ''), '.', ''),' ','' ) LIKE '%{0}%' OR REPLACE (REPLACE(REPLACE([PlateCN], '-', ''), '.', ''),' ','' ) LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE N'%{0}%' OR  ProductType LIKE N'%{0}%' OR Weight LIKE N'%{0}%'  OR VehicleType LIKE N'%{0}%' OR ProductGroup LIKE N'%{0}%') ", key));
            }
          

            //group
            if (!string.IsNullOrWhiteSpace(groupid) && groupid != "00")
            {
                var t = groupid.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([GroupId] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }
            //event Code
            if (!string.IsNullOrWhiteSpace(StatusID) && StatusID != "00")
            {
                var t = StatusID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
            sb.AppendLine(")as a");
            sb.AppendLine(") as C1");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", page, pageSize));
            var listData = DatabaseHelper.ExcuteCommandToList<tbl_Event>(sb.ToString());


            // Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");
            sb.AppendLine("FROM [tbl_Event] WHERE 1 = 1  AND EventType IN (2 , 3 ,4 ,5 ,6) AND  IsDeleted = 0");

            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                sb.AppendLine(string.Format("AND CreatedDate >= '{0}'  AND CreatedDate <= '{1}'", Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss"), Convert.ToDateTime(todate).ToString("yyyy/MM/dd HH:mm:ss")));
            }

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE (REPLACE(REPLACE([PlateVN], '-', ''), '.', ''),' ','' ) LIKE '%{0}%' OR REPLACE (REPLACE(REPLACE([PlateCN], '-', ''), '.', ''),' ','' ) LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE N'%{0}%' OR  ProductType LIKE N'%{0}%' OR Weight LIKE N'%{0}%'  OR VehicleType LIKE N'%{0}%' OR ProductGroup LIKE N'%{0}%') ", key));
            }
            //group
            if (!string.IsNullOrWhiteSpace(groupid) && groupid != "00")
            {
                var t = groupid.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([GroupId] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }

            //event Code
            if (!string.IsNullOrWhiteSpace(StatusID) && StatusID != "00")
            {
                var t = StatusID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
            var _total = DatabaseHelper.ExcuteCommandToModel<TotalPagingModel>(sb.ToString());

            var model = GridModelHelper<tbl_Event>.GetPage(listData, page, pageSize, _total.TotalCount);

            return await Task.FromResult(model);
        }
    }
}
