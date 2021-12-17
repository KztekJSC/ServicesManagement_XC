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
        public async Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int pageSize, string groupid, string fromdate, string todate)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) as RowNumber,a.*", "StartDate"));
            sb.AppendLine("FROM(");
            sb.AppendLine("  select * from [tbl_Event]");
            sb.AppendLine("WHere 1 =1 and ( EventType = 3 OR EventType = 4) and  IsDeleted = 0");
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("and (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' or  ProductType LIKE '%{0}%' )", key));
            }


            //event Code
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
            sb.AppendLine(")as a");
            sb.AppendLine(") as C1");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", page, pageSize));
            var listData = DatabaseHelper.ExcuteCommandToList<tbl_Event>(sb.ToString());


            // Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");
            sb.AppendLine("FROM [tbl_Event] where 1 = 1  and ( EventType = 3 OR EventType = 4)");

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("and (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' OR  ProductType LIKE '%{0}%' )", key));
            }

            //event Code
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
            var _total = DatabaseHelper.ExcuteCommandToModel<TotalPagingModel>(sb.ToString());

            var model = GridModelHelper<tbl_Event>.GetPage(listData, page, pageSize, _total.TotalCount);

            return await Task.FromResult(model);
        }
    }
}
