using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
namespace Kztek_Service.Api.Database.SQLSERVER
{
    public class tbl_EventService : Itbl_EventService
    {
        private Itbl_EventRepository _tbl_EventRepository;
        private IServiceRepository _ServiceRepository;
        private IEventInRepository _EventInRepository;
        public HttpContext httpContext { get; set; }
        public tbl_EventService(Itbl_EventRepository _tbl_EventRepository, IServiceRepository _ServiceRepository, IEventInRepository _EventInRepository)
        {
            this._tbl_EventRepository = _tbl_EventRepository;
            this._ServiceRepository = _ServiceRepository;
            this._EventInRepository = _EventInRepository;
        }
        public async Task<tbl_Event> GetById(string id)
        {
            return await _tbl_EventRepository.GetOneById(id);
        }
        public async Task<tbl_Event> GetOneByBBInfo(string tableName,string id)
        {
            var query = from n in _tbl_EventRepository.Table
                        where !n.IsDeleted && n.BB_Table == tableName && n.BB_Id == id
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<bool> UpdateVehicleIn(API_VehicleStatus obj)
        {
            var query = new StringBuilder();
            query.AppendLine("Update tbl_Event");
            query.AppendLine(string.Format("Set VehicleType = N'{0}',", obj.vehicleType));
            query.AppendLine(string.Format("{0} = 1,", obj.type == "VN" ? "VehicleStatusVN" : "VehicleStatusCN")); //xe vào
            query.AppendLine(string.Format("{0} = '{1}',", obj.type == "VN" ? "ImageVN" : "ImageCN",obj.image)); //ảnh xe

            //đổi sang trạng thái đã xác nhận
            //nếu type == VN thì kiểm tra giờ vào xe CN và ngược lại
            //nếu thời gian vào loại xe CN đã có thì chuyển trang thái sang đã xác nhận ko thì giữ nguyên
            query.AppendLine(string.Format("EventType = (CASE WHEN FORMAT ({0}, 'yyyy-MM-dd') != '9999-12-31' THEN 2 ELSE 1 END),", obj.type == "VN" ? "TimeInCN" : "TimeInVN"));

            query.AppendLine(string.Format("{0} = '{1}'", obj.type == "VN" ? "TimeInVN" : "TimeInCN",Convert.ToDateTime(obj.time).ToString("MM/dd/yyyy HH:mm:ss") )); //thời gian vào
            query.AppendLine("where IsDeleted = 0 and EventType = 1");
            query.AppendLine(string.Format("and {0} = 0", obj.type == "VN" ? "VehicleStatusVN" : "VehicleStatusCN")); //trạng thái xe chưa vào
            query.AppendLine(string.Format("and REPLACE(REPLACE({0}, '-', ''), '.', '') LIKE '%{1}%'", obj.type == "VN" ? "PlateVN" : "PlateCN", obj.plate.Replace("-", "").Replace(".", "")));

            var connectionString = AppSettingHelper.GetStringFromFileJson("connectstring", "ConnectionStrings:DefaultConnection").Result;

            var check = SqlHelper.ExcuteCommandToBool(connectionString, query.ToString());

            return await Task.FromResult(check);
        }

        public async Task<bool> UpdateVehicleOut(API_VehicleStatus obj)
        {
            var query = new StringBuilder();
            query.AppendLine("Update tbl_Event");
            query.AppendLine(string.Format("Set VehicleType = N'{0}',", obj.vehicleType));
            query.AppendLine(string.Format("{0} = 2,", obj.type == "VN" ? "VehicleStatusVN" : "VehicleStatusCN")); //xe ra
            query.AppendLine(string.Format("{0} = '{1}',", obj.type == "VN" ? "ImageVN" : "ImageCN", obj.image)); //ảnh
            query.AppendLine(string.Format("{0} = '{1}'", obj.type == "VN" ? "TimeOutVN" : "TimeOutCN", Convert.ToDateTime(obj.time).ToString("MM/dd/yyyy HH:mm:ss"))); //thời gian ra
            query.AppendLine("where IsDeleted = 0");
            query.AppendLine(string.Format("and {0} = 1", obj.type == "VN" ? "VehicleStatusVN" : "VehicleStatusCN")); //trạng thái xe đã vào
            query.AppendLine(string.Format("and REPLACE(REPLACE({0}, '-', ''), '.', '') LIKE '%{1}%'", obj.type == "VN" ? "PlateVN" : "PlateCN", obj.plate.Replace("-", "").Replace(".", "")));

            var connectionString = AppSettingHelper.GetStringFromFileJson("connectstring", "ConnectionStrings:DefaultConnection").Result;

            var check = SqlHelper.ExcuteCommandToBool(connectionString, query.ToString());

            return await Task.FromResult(check);
        }

        public async Task<MessageReport> Create(tbl_Event_POST model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var listEventIn = new List<EventIn>();

            try
            {
                //Lấy id dịch vụ từ db
                var idService = await GetIdService(model.service, model.serviceCode);

                //tách ra từng biển số VN
                var arrPlateVN = model.plateVN.Split(",");

                //tách ra từng biển số CN
                var arrPlateCN = model.plateCN.Split(",");

                //Nếu không có thời gian vào VN hoặc không có thời gian vào CN thì lấy danh sách sự kiện vào để kiểm tra
                if (string.IsNullOrEmpty(model.timeInVN) || string.IsNullOrEmpty(model.timeInCN))
                {
                    //gộp 2 chuỗi biển số
                    var arr = arrPlateVN.Concat(arrPlateCN);

                    //danh sách xe vào bãi
                    listEventIn = await GetEventInByPlates(arr.ToList());
                }

               

               

                //lặp từng biển số VN
                for (int i = 0; i < arrPlateVN.Length; i++)
                {
                    if (!string.IsNullOrEmpty(arrPlateVN[i]))
                    {
                        var plateVN = arrPlateVN[i].Trim();

                        string timeInVN = model.timeInVN;

                        //Nếu không có thời gian vào thì kiểm tra sự kiện xe vào để lấy thời gian
                        if (string.IsNullOrEmpty(model.timeInVN))
                        {
                            var plateUnsignVN = await FunctionHelper.RemoveSpecialCharactersVn(plateVN);

                            var objEventIn = listEventIn.FirstOrDefault(n => n.PlateUnsign == plateUnsignVN);

                            if (objEventIn != null)
                            {
                                timeInVN = objEventIn.TimeIn.ToString("dd/MM/yyyy HH:mm:ss");
                            }
                        }

                        //lặp từng biển số CN
                        for (int j = 0; j < arrPlateCN.Length; j++)
                        {
                            if (!string.IsNullOrEmpty(arrPlateCN[j]))
                            {
                                var plateCN = arrPlateCN[j].Trim();

                                string timeInCN = model.timeInCN;

                                //Nếu không có thời gian vào thì kiểm tra sự kiện xe vào để lấy thời gian
                                if (string.IsNullOrEmpty(model.timeInCN))
                                {
                                    var plateUnsignCN = await FunctionHelper.RemoveSpecialCharactersVn(plateCN);

                                    var objEventIn = listEventIn.FirstOrDefault(n => n.PlateUnsign == plateUnsignCN);

                                    if (objEventIn != null)
                                    {
                                        timeInCN = objEventIn.TimeIn.ToString("dd/MM/yyyy HH:mm:ss");
                                    }
                                }

                                var obj = new tbl_Event()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    Code = model.code,

                                    PlateVN = plateVN,
                                    ImageVN = model.imageVN,
                                    TimeInVN = !string.IsNullOrEmpty(timeInVN) ? Convert.ToDateTime(timeInVN) : DateTime.MaxValue,
                                    TimeOutVN = DateTime.MaxValue,
                                    VehicleStatusVN = !string.IsNullOrEmpty(timeInVN) ? 1 : 0,

                                    PlateCN = plateCN,
                                    ImageCN = model.imageCN,
                                    TimeInCN = !string.IsNullOrEmpty(timeInCN) ? Convert.ToDateTime(timeInCN) : DateTime.MaxValue,
                                    TimeOutCN = DateTime.MaxValue,
                                    VehicleStatusCN = !string.IsNullOrEmpty(timeInCN) ? 1 : 0,

                                    Description = model.description,

                                    Price = model.price,
                                    SubPrice = model.subPrice,
                                    ServiceCode = model.serviceCode,
                                    Service = model.service,

                                    VehicleType = model.vehicleType,
                                    Weight = model.weight,
                                    ProductType = model.productType,
                                    ProductGroup = model.productGroup,
                                    EventType = 1, //chờ xác nhận
                                    PaymentStatus = model.paymentStatus,

                                    BB_Table = model.bb_Table,
                                    BB_Id = model.bb_Id,

                                    Cost = 0,
                                    IsDeleted = false,
                                    CreatedDate = DateTime.Now,
                                    ModifiedDate = DateTime.Now,
                                    StartDate = DateTime.MaxValue,
                                    EndDate = DateTime.MaxValue,
                                    ConfirmDate = DateTime.MaxValue,
                                    DivisionDate = DateTime.MaxValue,
                                    ParkingPosition = "Chưa có vị trí đỗ",
                                    GroupId = ""
                                };

                                if (obj.VehicleStatusVN == 1 && obj.VehicleStatusCN == 1)
                                {
                                    obj.EventType = 2; //Đã xác nhận nếu 2 xe đều vào bãi
                                }

                                //gán lại id dịch vụ
                                obj.Service = idService;

                                result = await _tbl_EventRepository.Add(obj);
                                //Luu log

                                await LogHelper.WriteLogAPI(obj.Id.ToString(), "Thêm mới", "tbl_Event", JsonConvert.SerializeObject(obj).ToString());
                            }                              
                        }
                    }                 
                }

                result = new MessageReport(true, "Thành công");
                if (result.isSuccess)
                {
                    //loại lại danh sách
                    await SignalrHelper.SqlHub.Clients.All.SendAsync("Service");

                   
                    //load lại thông báo cho dg viên
                    await SignalrHelper.SqlHub.Clients.All.SendAsync("Notifi");
                }
             
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }
           
            return result;
        }

        public async Task<MessageReport> Update(tbl_Event_POST model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = await GetOneByBBInfo(model.bb_Table,model.bb_Id);
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return result;
            }

            obj.Service = model.service;
            obj.ServiceCode = model.serviceCode;
            obj.Code = model.code;

            obj.PlateVN = model.plateVN;
            obj.ImageVN = model.imageVN;

            if (!string.IsNullOrEmpty(model.timeInVN))
            {
                obj.TimeInVN = Convert.ToDateTime(model.timeInVN);
            }

            obj.PlateCN = model.plateCN;
            obj.ImageCN = model.imageCN;

            if (!string.IsNullOrEmpty(model.timeInCN))
            {
                obj.TimeInCN = Convert.ToDateTime(model.timeInCN);
            }

            obj.ProductType = model.productType;
            obj.Weight = model.weight;
            obj.VehicleType = model.vehicleType;
            obj.ProductGroup = model.productGroup;
            obj.Price = model.price;
            obj.SubPrice = model.subPrice;           
            obj.PaymentStatus = model.paymentStatus;
            obj.ModifiedDate = DateTime.Now;

            //Lấy id dịch vụ từ db
            var idService = await GetIdService(obj.Service, obj.ServiceCode);

            //gán lại id dịch vụ
            obj.Service = idService;

            result = await _tbl_EventRepository.Update(obj);

            if (result.isSuccess)
            {
                result = new MessageReport(true, "Thành công");
            }

            return result;
        }

        public async Task<MessageReport> Delete(tbl_Event_POST model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = await GetOneByBBInfo(model.bb_Table, model.bb_Id);
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return result;
            }

            obj.IsDeleted = true;

            result = await _tbl_EventRepository.Update(obj);

            if (result.isSuccess)
            {
                result = new MessageReport(true, "Thành công");

                //nếu xóa sự kiện 2 xe đã vào bãi thì loại lại danh sách xác nhận
                if (obj.VehicleStatusVN == 1 && obj.VehicleStatusCN == 1)
                {
                    await SignalrHelper.SqlHub.Clients.All.SendAsync("Service");
                }
            }

            return result;
        }

        public async Task<MessageReport> VehicleStatusIn(API_VehicleStatus model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                //cập nhật giờ vào cho xe dịch vụ
                var check = await UpdateVehicleIn(model);

                if (check)
                {
                    result = new MessageReport(true, "Thành công");

                    await SignalrHelper.SqlHub.Clients.All.SendAsync("Service");

                    //load lại thông báo cho dg viên
                    await SignalrHelper.SqlHub.Clients.All.SendAsync("Notifi");
                }

                //tạo bản ghi xe vào bãi nếu có dữ liệu biển số và thời gian
                if(!string.IsNullOrEmpty(model.plate) && !string.IsNullOrEmpty(model.time))
                {
                    var eventIn = new EventIn
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        Plate = model.plate,
                        PlateUnsign = await FunctionHelper.RemoveSpecialCharactersVn(model.plate),
                        TimeIn = !string.IsNullOrEmpty(model.time) ? Convert.ToDateTime(model.time) : DateTime.MaxValue
                    };

                    await CreateEventIn(eventIn);
                }
               
            }
            catch (Exception ex)
            {

                result = new MessageReport(false, ex.Message);
            }

            return result;
        }

        public async Task<MessageReport> VehicleStatusOut(API_VehicleStatus model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                var check = await UpdateVehicleOut(model);

                if (check)
                {
                    result = new MessageReport(true, "Thành công");
                }

                //xóa sự kiện xe vào bãi
                await DeleteEventInByPlate(model);
            }
            catch (Exception ex)
            {

                result = new MessageReport(false, ex.Message);
            }

            return result;
        }

        #region Service
        public async Task<MessageReport> Create(Service model)
        {
            return await _ServiceRepository.Add(model);
        }

        public async Task<string> GetIdService(string name, string code)
        {
            string id = "";

            if (!string.IsNullOrEmpty(name))
            {
                name = name.Trim();

                code = code.Trim();

                //lấy service theo tên
                var query = from n in _ServiceRepository.Table
                            where n.Name == name
                            select n;

                var objService = query.FirstOrDefault();

                //null thì tạo mới
                if (objService == null)
                {
                    objService = new Service
                    {
                        Id = Guid.NewGuid().ToString(),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Code = code,
                        Name = name,
                    };

                    var check = await _ServiceRepository.Add(objService);

                    if (check.isSuccess)
                    {
                        id = objService.Id;
                    }
                }
                else
                {
                    id = objService.Id;
                }
            }

            return await Task.FromResult(id);
        }

        public async Task<Service> GetServiceById(string id)
        {
            return await _ServiceRepository.GetOneById(id);
        }
        #endregion

        #region Xe vào bãi
        public async Task<MessageReport> UpdateEventIn(EventIn oldObj)
        {
            return await _EventInRepository.Update(oldObj);
        }
        public async Task<MessageReport> CreateEventIn(EventIn model)
        {
            var result = new MessageReport(false,"Có lỗi xảy ra"); 

            //kiểm tra xe đã có chưa
            var obj = await GetEventInByPlate(model.PlateUnsign);

            //đã có thì cập nhật thời gian vào
            if(obj != null)
            {
                obj.TimeIn = model.TimeIn;

                result = await UpdateEventIn(obj);
            }
            else
            {
                result = await _EventInRepository.Add(model);
            }

            return result;
        }
        public async Task<bool> DeleteEventInByPlate(API_VehicleStatus model)
        {
            var plate = await FunctionHelper.RemoveSpecialCharactersVn(model.plate);

            var query = new StringBuilder();

            query.AppendLine("Delete EventIn");

            query.AppendLine(string.Format("where PlateUnsign = '{0}'", plate));

            var connectionString = AppSettingHelper.GetStringFromFileJson("connectstring", "ConnectionStrings:DefaultConnection").Result;

            var check = SqlHelper.ExcuteCommandToBool(connectionString, query.ToString());

            return await Task.FromResult(check);
        }

        public async Task<EventIn> GetEventInById(string id)
        {
            return await _EventInRepository.GetOneById(id);
        }
        public async Task<EventIn> GetEventInByPlate(string plate)
        {
            var query = from n in _EventInRepository.Table
                        where n.PlateUnsign == plate || n.Plate == plate
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<List<EventIn>> GetEventInByPlates(List<string> plates)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT * FROM EventIn");

            if (plates != null && plates.Count > 0)
            {
                var count = 0;

                query.AppendLine("where PlateUnsign IN ( ");

                foreach (var item in plates)
                {
                    count++;

                    query.AppendLine(string.Format("'{0}'{1}", item, count == plates.Count ? "" : ","));
                }

                query.AppendLine(" )");
            }

            var connectionString = AppSettingHelper.GetStringFromFileJson("connectstring", "ConnectionStrings:DefaultConnection").Result;

            var dataSet = SqlHelper.GetDataSet(connectionString,query.ToString());

            var list = SqlHelper.ConvertTo<EventIn>(dataSet.Tables[0]);

            return await Task.FromResult(list);
        }
        #endregion
    }
}
