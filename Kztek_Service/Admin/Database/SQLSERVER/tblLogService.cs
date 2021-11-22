using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class tblLogService : ItblLogService
    {
        private ItblLogRepository _tblLogRepository;

        public tblLogService(ItblLogRepository _tblLogRepository)
        {
            this._tblLogRepository = _tblLogRepository;
        }

        public async Task<GridModel<tblLog>> GetAllPagingByFirst(string key, string users, string actions, string fromdate, string todate, int pageNumber, int pageSize, string appcode = "")
        {
            var query = from n in _tblLogRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(appcode))
            {
                query = query.Where(n => n.AppCode == appcode);
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLower();

                query = query.Where(n => n.ComputerName.ToLower().Contains(key) || n.Actions.ToLower().Contains(key) || n.ComputerName.ToLower().Contains(key) || n.Description.Contains(key) || n.ObjectName.ToLower().Contains(key) || n.SubSystemCode.ToLower().Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(users))
            {
                query = query.Where(n => users.Contains(n.UserName));
            }

            if (!string.IsNullOrWhiteSpace(actions))
            {
                query = query.Where(n => actions.Contains(n.Actions));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) || !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.Date.Value >= fdate && n.Date < tdate);
            }
            else
            {
                var fdate = DateTime.Now;
                var tdate = fdate.AddDays(1);

                query = query.Where(n => n.Date.Value >= fdate && n.Date < tdate);
            }

            var pageList = query.ToPagedList(pageNumber, pageSize);

            var model = GridModelHelper<tblLog>.GetPage(pageList.OrderByDescending(n => n.Date).ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }
    }
}
