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
        private IServiceService _ServiceService;
        public tbl_EventService(Itbl_EventRepository _tbl_EventRepository, IServiceService _ServiceService)
        {
            this._tbl_EventRepository = _tbl_EventRepository;
            this._ServiceService = _ServiceService;
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
                if (obj.EventType == 5)
                {
                    result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERROR1"));
                    return result;
                }
                else
                {
                    obj.IsDeleted = true;
                    return await _tbl_EventRepository.Update(obj);
                }

            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

        public async Task<tbl_Event_Cus> GetByCustomById(string id)
        {
            var obj = await GetById(id);
            var objService = await _ServiceService.GetById(obj.Service);
            var model = new tbl_Event_Cus()
            ;
            model.Id = obj.Id;
            model.serviceCode = obj.ServiceCode;
            model.plateVN = obj.PlateVN;
            model.plateCN = obj.PlateCN;
            model.productType = obj.ProductType;
            model.weight = obj.Weight.ToString();
            model.vehicleType = obj.ProductType;
            model.productGroup = obj.ProductGroup;
            model.service = obj.Service;
            model.price = obj.Price.ToString("###,###.##");
            model.subPrice = obj.SubPrice.ToString("###,###.##");
            model.GroupId = obj.GroupId;
            model.description = obj.Description;
            model.serviceName = objService.Name;
            return await Task.FromResult(model);
        }

        public async Task<tbl_Event> GetById(string id)
        {

            return await _tbl_EventRepository.GetOneById(id);
        }

        public async Task<SelectListModel_Chosen> GetEventype(string id = "", string placeholder = "", string selecteds = "")
        {
            var data = StaticList.ListStatusConfirmGroup();
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

        public async Task<SelectListModel_Chosen> GetEventypeCoordination(string id = "", string placeholder = "", string selecteds = "")
        {
            var data = StaticList.ListStatusCoordinator();
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

        public async Task<SelectListModel_Chosen> GetEventypeService(string id = "", string placeholder = "", string selecteds = "")
        {
            var data = StaticList.ListStatusService();
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



        public async Task<GridModel<tbl_Event>> GetPagingConfirmGroup(string key, int page, int pageSize, string statusID, string fromdate, string todate)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} asc) as RowNumber,a.*", "EventType"));
            sb.AppendLine("FROM(");
            sb.AppendLine("  select * from [tbl_Event]");
            sb.AppendLine("WHere 1 =1 and EventType IN(3,4,5,6)  and  IsDeleted = 0");
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
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
            sb.AppendLine("FROM [tbl_Event] where 1 = 1 AND EventType IN(3,4,5)  AND  IsDeleted = 0");
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
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

        public async Task<GridModel<tbl_Event>> GetPagingCoordinatort(string key, int page, int pageSize, string statusID, string fromdate, string todate)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} asc) as RowNumber,a.*", "EventType"));
            sb.AppendLine("FROM(");
            sb.AppendLine("  SELECT * FROM [tbl_Event]");
            sb.AppendLine("WHERE 1 =1 and  EventType IN (2,3,4,5,6) AND IsDeleted = 0");
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
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
            sb.AppendLine("FROM [tbl_Event] WHERE 1 = 1 AND EventType IN (2,3,4,5) AND IsDeleted = 0");

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
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

        public async Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int pageSize, string statusID, string fromdate, string todate)
        {
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;

            var sb = new StringBuilder();

            sb.AppendLine("SELECT * FROM (");

            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) AS RowNumber,a.*", "CreatedDate"));

            sb.AppendLine("FROM(");

            sb.AppendLine("SELECT * FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 =1 AND EventType IN (1,2) AND  IsDeleted = 0");

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%')", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' ", key));
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

            sb.AppendLine("FROM [tbl_Event] where 1 = 1  AND EventType IN (1,2) AND  IsDeleted = 0");

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%')", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' ", key));
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

        /// <summary>
        /// Danh sách dịch vụ đã xác nhận
        /// Dùng cho giao diện phân tổ
        /// </summary>
        /// <returns></returns>
        public async Task<List<tbl_Event>> GetListType2()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) AS RowNumber,a.*", "CreatedDate"));

            sb.AppendLine("FROM(");

            sb.AppendLine("SELECT * FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 = 1 AND EventType = 2 AND  IsDeleted = 0");

            sb.AppendLine(") AS a");


            var listData = DatabaseHelper.ExcuteCommandToList<tbl_Event>(sb.ToString());

            return await Task.FromResult(listData);
        }

        public async Task<List<Model_ServiceByGroup>> GetCountServiceByGroup()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT GroupId, Count(Id) as Number FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 = 1 AND EventType IN(3,4) AND  IsDeleted = 0");

            sb.AppendLine("Group By GroupId");

            var listData = DatabaseHelper.ExcuteCommandToList<Model_ServiceByGroup>(sb.ToString());

            return await Task.FromResult(listData);
        }

        public async Task<List<tbl_Event>> GetListServiceByGroup(string groupid)
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT * FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 = 1 AND EventType IN(3,4) AND  IsDeleted = 0");

            sb.AppendLine(string.Format("AND GroupId = '{0}'", groupid));

            var listData = DatabaseHelper.ExcuteCommandToList<tbl_Event>(sb.ToString());

            return await Task.FromResult(listData);
        }

        public async Task<MessageReport> Update(tbl_Event oldObj)
        {
            return await _tbl_EventRepository.Update(oldObj);
        }

     
    }
}
