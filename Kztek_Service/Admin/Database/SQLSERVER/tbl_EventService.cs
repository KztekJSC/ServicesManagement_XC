using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Configs;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
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

        /// <summary>
        /// Thông báo tài khoản giao dịch viên
        /// </summary>
        /// <returns></returns>
        public async Task<List<NotifiCustom>> NotifiSession1(HttpContext HttpContext)
        {
            var listData = new List<NotifiCustom>();

            #region Tạm thời ẩn session, nếu count chậm có thể áp dụng phương án session
            //lấy data từ session
          //  var sessionValue = HttpContext.Session.GetString(SessionConfig.NotifiType1Session);

            ////nếu có dữ liệu
            //if (!string.IsNullOrWhiteSpace(sessionValue))
            //{
            //    listData = JsonConvert.DeserializeObject<List<NotifiCustom>>(sessionValue);
            //}
            //else
            //{
            //    //nếu chưa có thì lấy từ db
            //    listData = await GetCountServiceByType12();

            //    //add vào session
            //    HttpContext.Session.SetString(SessionConfig.NotifiType1Session, JsonConvert.SerializeObject(listData));
            //}
            #endregion

            listData = await GetCountServiceByType12();

            return listData;
        }

        /// <summary>
        /// Đếm số lượng chưa xác nhận, đã xác nhận
        /// </summary>
        /// <returns></returns>
        public async Task<List<NotifiCustom>> GetCountServiceByType12()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT EventType, (CASE WHEN EventType = 1 THEN N'Dịch vụ chưa xác nhận' ELSE N'Dịch vụ đã xác nhận' END) as TypeName, Count(Id) as Quantity FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 =1 AND EventType IN (1,2) AND  IsDeleted = 0");

            sb.AppendLine("GROUP BY EventType");

            var listData = DatabaseHelper.ExcuteCommandToList<NotifiCustom>(sb.ToString());

            return await Task.FromResult(listData);
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
            model.ParkingPosition = obj.ParkingPosition;
            model.price = obj.Price.ToString("###,###.##");
            model.subPrice = obj.SubPrice.ToString("###,###.##");
            model.GroupId = obj.GroupId;
            model.description = obj.Description;
            model.serviceName = objService.Name;
            return await Task.FromResult(model);
        }

        public async Task<tbl_Event> GetById(string id)
        {

            return await _tbl_EventRepository.GetOneById(id.ToLower());
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



        public async Task<GridModel<tbl_Event>> GetPagingConfirmGroup(HttpContext httpContext, string key, int page, int pageSize, string statusID, string fromdate, string todate)
        {
            var currentUser = SessionCookieHelper.CurrentUser(httpContext).Result;
            var tdate = Convert.ToDateTime(fromdate);
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} asc) as RowNumber,a.*", "EventType"));
            sb.AppendLine("FROM(");
            sb.AppendLine("  SELECT * FROM [tbl_Event]");
            sb.AppendLine("WHERE 1 =1 AND EventType IN(3,4,5,6)  AND  IsDeleted = 0");
            sb.AppendLine(string.Format("AND  FORMAT(DivisionDate,'yyyyMMdd') = '{0}' ", tdate.ToString("yyyyMMdd")));
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
            }

            //nếu tk đăng nhập không phải admin thì lấy dữ liệu theo tổ được phân quyền
            if (currentUser != null && !currentUser.isAdmin)
            {
                if (!string.IsNullOrEmpty(currentUser.GroupIds))
                {
                    var t = currentUser.GroupIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t.Any())
                    {
                        var count = 0;

                        sb.AppendLine("AND ([GroupId] IN ( ");

                        foreach (var item in t)
                        {
                            count++;

                            sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                        }

                        sb.AppendLine(" )) ");


                    }
                }
                else
                {
                    sb.AppendLine("AND GroupId = 'Null99'");
                }
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
            sb.AppendLine(")as a");
            sb.AppendLine(") as C1");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", page, pageSize));
            var listData = DatabaseHelper.ExcuteCommandToList<tbl_Event>(sb.ToString());


            // Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");
            sb.AppendLine("FROM [tbl_Event] where 1 = 1 AND EventType IN(3,4,5,6)  AND  IsDeleted = 0");
            sb.AppendLine(string.Format("AND  FORMAT(DivisionDate,'yyyyMMdd') = '{0}' ", tdate.ToString("yyyyMMdd")));
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
            }

            //nếu tk đăng nhập không phải admin thì lấy dữ liệu theo tổ được phân quyền
            if (currentUser != null && !currentUser.isAdmin)
            {
                if (!string.IsNullOrEmpty(currentUser.GroupIds))
                {
                    var t = currentUser.GroupIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t.Any())
                    {
                        var count = 0;

                        sb.AppendLine("AND ([GroupId] IN ( ");

                        foreach (var item in t)
                        {
                            count++;

                            sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                        }

                        sb.AppendLine(" )) ");


                    }
                }
                else
                {
                    sb.AppendLine("AND GroupId = 'Null99'");
                }
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

        public async Task<List<CountEventByType>> CountEventByType(HttpContext httpContext,string fromdate)
        {
            var currentUser = SessionCookieHelper.CurrentUser(httpContext).Result;
            var tdate = Convert.ToDateTime(fromdate);
            var sb = new StringBuilder();
            sb.AppendLine("SELECT EventType,Count(*) as Number from [tbl_Event]");
            sb.AppendLine("WHERE EventType IN(3,4,5,6)  and  IsDeleted = 0");
            sb.AppendLine(string.Format("AND  FORMAT(DivisionDate,'yyyyMMdd') = '{0}' ", tdate.ToString("yyyyMMdd")));
            //nếu tk đăng nhập không phải admin thì lấy dữ liệu theo tổ được phân quyền
            if (currentUser != null && !currentUser.isAdmin)
            {
                if (!string.IsNullOrEmpty(currentUser.GroupIds))
                {
                    var t = currentUser.GroupIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t.Any())
                    {
                        var count = 0;

                        sb.AppendLine("AND ([GroupId] IN ( ");

                        foreach (var item in t)
                        {
                            count++;

                            sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                        }

                        sb.AppendLine(" )) ");


                    }
                }
                else
                {
                    sb.AppendLine("AND GroupId = 'Null99'");
                }
            }

            sb.AppendLine("GROUP BY EventType");

            var list = DatabaseHelper.ExcuteCommandToList<CountEventByType>(sb.ToString());

            var listData = new List<CountEventByType>();

            listData.Add(new CountEventByType { EventType = 3, Number = 0, ColorClass = "infobox-pink", Icon = "fa-gavel", Title = "Đã phân tổ" });

            listData.Add(new CountEventByType { EventType = 4, Number = 0, ColorClass = "infobox-red", Icon = "fa-briefcase", Title = "Đang thực hiện" });

            listData.Add(new CountEventByType { EventType = 6, Number = 0, ColorClass = "infobox-orange2", Icon = "fa-check", Title = "Đã hoàn thành" });

            listData.Add(new CountEventByType { EventType = 5, Number = 0, ColorClass = "infobox-orange2", Icon = "fa-spinner", Title = "Đang chờ duyệt" });

            listData.Add(new CountEventByType { EventType = 99, Number = 0, ColorClass = "infobox-orange2", Icon = "fa-truck", Title = "Việc trong ngày" });

            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    var objCount =  listData.FirstOrDefault(n => n.EventType == item.EventType);

                    if(objCount != null)
                    {
                        objCount.Number = item.Number;
                    }
                }

                var objTotal = listData.FirstOrDefault(n => n.EventType == 99 );
                //foreach (var item1 in listData)
                //{
                //    if (item1.EventType == 3 || item1.EventType == 4 || item1.EventType == 5)
                //    {
                //        objTotal.Number +=  item1.Number;
                //    }
                //}
                objTotal.Number = listData.Sum(n => n.Number);
            }

            return await Task.FromResult(listData);
        }

        public async Task<GridModel<tbl_Event>> GetPagingCoordinatort(string key, int page, int pageSize, string statusID, string fromdate, string todate, string ServiceId)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} asc) as RowNumber,a.*", "EventType"));
            sb.AppendLine("FROM(");
            sb.AppendLine("  SELECT * FROM [tbl_Event]");
            sb.AppendLine("WHERE 1 =1 AND  EventType != 1 AND IsDeleted = 0");
            sb.AppendLine(string.Format("AND CreatedDate >= '{0}'  AND CreatedDate <= '{1}'",fromdate,todate));
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
            }

            //Service
            if (!string.IsNullOrWhiteSpace(ServiceId) && ServiceId != "00")
            {
                var t = ServiceId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([Service] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
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
            sb.AppendLine("FROM [tbl_Event] WHERE 1 = 1 AND EventType != 1 AND IsDeleted = 0");
            sb.AppendLine(string.Format("AND CreatedDate >= '{0}'  AND CreatedDate <= '{1}'", fromdate, todate));
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
            }
            //Service
            if (!string.IsNullOrWhiteSpace(ServiceId) && ServiceId != "00")
            {
                var t = ServiceId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([Service] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
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

        public async Task<GridModel<tbl_Event>> GetPagingInOut(string key, int page, int pageSize, string statusID, string fromdate, string todate, string ServiceId , string GroupId )
        {
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;

            var sb = new StringBuilder();

            sb.AppendLine("SELECT * FROM (");

            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) AS RowNumber,a.*", "CreatedDate"));

            sb.AppendLine("FROM(");

            sb.AppendLine("SELECT * FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 =1 AND EventType IN (1,2) AND  IsDeleted = 0");

            sb.AppendLine(string.Format("AND CreatedDate >= '{0}'  AND CreatedDate <= '{1}'", fromdate, todate));
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE (REPLACE(REPLACE([PlateVN], '-', ''), '.', ''),' ','' ) LIKE '%{0}%' OR REPLACE (REPLACE(REPLACE([PlateCN], '-', ''), '.', ''),' ','' ) LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE N'%{0}%' OR  ProductType LIKE N'%{0}%' OR Weight LIKE N'%{0}%'  OR VehicleType LIKE N'%{0}%' OR ProductGroup LIKE N'%{0}%') ", key));
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
            //Service
            if (!string.IsNullOrWhiteSpace(ServiceId) && ServiceId != "00")
            {
                var t = ServiceId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([Service] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }
            //Group
            if (!string.IsNullOrWhiteSpace(GroupId) && GroupId != "00")
            {
                var t = GroupId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
            sb.AppendLine(") AS a");

            sb.AppendLine(") AS C1");

            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", page, pageSize));

            var listData = DatabaseHelper.ExcuteCommandToList<tbl_Event>(sb.ToString());


            // Tính tổng
            sb.Clear();

            sb.AppendLine("SELECT COUNT(*) TotalCount");

            sb.AppendLine("FROM [tbl_Event] where 1 = 1  AND EventType IN (1,2) AND  IsDeleted = 0");

            sb.AppendLine(string.Format("AND CreatedDate >= '{0}'  AND CreatedDate <= '{1}'", fromdate, todate));

            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE (REPLACE(REPLACE([PlateVN], '-', ''), '.', ''),' ','' ) LIKE '%{0}%' OR REPLACE (REPLACE(REPLACE([PlateCN], '-', ''), '.', ''),' ','' ) LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE N'%{0}%' OR  ProductType LIKE N'%{0}%' OR Weight LIKE N'%{0}%'  OR VehicleType LIKE N'%{0}%' OR ProductGroup LIKE N'%{0}%') ", key));
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
            //Service
            if (!string.IsNullOrWhiteSpace(ServiceId) && ServiceId != "00")
            {
                var t = ServiceId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([Service] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }
            //Group
            if (!string.IsNullOrWhiteSpace(GroupId) && GroupId != "00")
            {
                var t = GroupId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

        /// <summary>
        /// Danh sách dịch vụ đã xác nhận
        /// Dùng cho giao diện phân tổ
        /// </summary>
        /// <returns></returns>
        public async Task<List<tbl_Event>> GetListType2(string key , string ServiceId , string fromdate , string ParkingPosittion )
        {
            var tdate = Convert.ToDateTime(fromdate);

            var sb = new StringBuilder();

            sb.AppendLine(string.Format("SELECT ROW_NUMBER () OVER ( ORDER BY {0} desc) AS RowNumber,a.*", "CreatedDate"));

            sb.AppendLine("FROM(");

            sb.AppendLine("SELECT * FROM [tbl_Event]");

            sb.AppendLine("WHERE 1 = 1 AND EventType = 2 AND  IsDeleted = 0");

            sb.AppendLine(string.Format("AND  FORMAT(CreatedDate,'yyyyMMdd') = '{0}' ", tdate.ToString("yyyyMMdd")));
            var keyReplace = !String.IsNullOrEmpty(key) ? key.Replace(".", "").Replace("-", "").Replace(" ", "") : String.Empty;
            if (!string.IsNullOrEmpty(keyReplace))
            {
                sb.AppendLine(string.Format("AND (  REPLACE(REPLACE([PlateVN], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE([PlateCN], '-', ''), '.', '') LIKE '%{0}%'", keyReplace));
            }
            if (!string.IsNullOrEmpty(key))
            {
                sb.AppendLine(string.Format("OR  ServiceCode LIKE '%{0}%' )", key));
            }
            //Service
            if (!string.IsNullOrWhiteSpace(ServiceId) && ServiceId != "00")
            {
                var t = ServiceId.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([Service] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }
            //Service
            if (!string.IsNullOrWhiteSpace(ParkingPosittion) && ParkingPosittion != "00")
            {
                var t = ParkingPosittion.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ([ParkingPosition] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )) ");


                }
            }
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

        public async Task<SelectListModel_Chosen> GetEventypeReport(string id = "", string placeholder = "", string selecteds = "")
        {
            var data = StaticList.ListStatusReport();
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

        public async Task<tbl_Event> GetByService(string id)
        {
            var query = from n in _tbl_EventRepository.Table
                        where n.Service == id
                        select n;
            return await Task.FromResult(query.FirstOrDefault());
        }
    }
}
