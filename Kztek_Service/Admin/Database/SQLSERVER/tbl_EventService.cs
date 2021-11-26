using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class tbl_EventService : Itbl_EventService
    {
        private Itbl_EventRepository _tbl_EventRepository;
        public tbl_EventService(Itbl_EventRepository _tbl_EventRepository)
        {
            this._tbl_EventRepository = _tbl_EventRepository;
        }

        public async Task<MessageReport> Create(tbl_Event obj)
        {
            return await _tbl_EventRepository.Add(obj);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = await GetById(id);
            if (obj != null)
            {
                return await _tbl_EventRepository.Remove(obj);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

    

        public async Task<tbl_Event> GetById(string id)
        {
            return await _tbl_EventRepository.GetOneById(id);
        }

        public async Task<SelectListModel_Chosen> GetEventype(string id = "", string placeholder = "", string selecteds = "")
        {
            var data = StaticList.ListStatus();
            var cus = new List<SelectListModel>();
            var lst = data;
            if (lst != null && lst.Count > 0)
            {
                cus.Add(new SelectListModel()
                {
                    ItemText = "---- Lựa chọn ----",
                    ItemValue = "00"
                });

                cus.AddRange(data.Select(n => new SelectListModel()
                {
                    ItemText = n.ItemText,
                    ItemValue = n.ItemValue
                }));
            }

            var model = new SelectListModel_Chosen()
            {
                IdSelectList = "StatusID",
                Selecteds = selecteds,
                Placeholder = placeholder,
                Data = cus.ToList(),
                isMultiSelect = false
            };
            return model;
        }

        public async Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int pageSize, string statusID, string fromdate, string todate)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) as RowNumber,a.*", "StartDate"));
            sb.AppendLine("FROM(");
            sb.AppendLine("  select * from [tbl_Event]");
            sb.AppendLine("WHere 1 =1 and EventType = 0 OR EventType = 1");
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("and ([ServiceCode] LIKE '%{0}%' OR [PlateVN] LIKE '%{0}%' OR [PlateCN] LIKE '%{0}%')", key));
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
            sb.AppendLine(")as a");
            sb.AppendLine(") as C1");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", page, pageSize));
            var listData = DatabaseHelper.ExcuteCommandToList<tbl_Event>(sb.ToString());


            // Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");
            sb.AppendLine("FROM [tbl_Event] where 1 = 1  and EventType = 0 OR EventType = 1");
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("and ([ServiceCode] LIKE '%{0}%' OR [PlateVN] LIKE '%{0}%' OR [PlateCN] LIKE '%{0}%')", key));
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
            var _total = DatabaseHelper.ExcuteCommandToModel<TotalPagingModel>(sb.ToString());

            var model = GridModelHelper<tbl_Event>.GetPage(listData, page, pageSize, _total.TotalCount);

            return await Task.FromResult(model);
        }

        public async Task<MessageReport> Update(tbl_Event oldObj)
        {
            return await _tbl_EventRepository.Update(oldObj);
        }
    }
}
