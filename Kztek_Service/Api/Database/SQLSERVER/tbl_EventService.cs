using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Api.Database.SQLSERVER
{
    public class tbl_EventService : Itbl_EventService
    {
        private Itbl_EventRepository _tbl_EventRepository;

        public tbl_EventService(Itbl_EventRepository _tbl_EventRepository)
        {
            this._tbl_EventRepository = _tbl_EventRepository;
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
            query.AppendLine(string.Format("Set VehicleType = '{0}',", obj.vehicleType));
            query.AppendLine(string.Format("{0} = 1,", obj.type == "VN" ? "VehicleStatusVN" : "VehicleStatusCN")); //xe vào
            query.AppendLine(string.Format("{0} = '{1}',", obj.type == "VN" ? "ImageVN" : "ImageCN",obj.image)); //ảnh xe
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
            query.AppendLine(string.Format("Set VehicleType = '{0}',", obj.vehicleType));
            query.AppendLine(string.Format("{0} = 2,", obj.type == "VN" ? "VehicleStatusVN" : "VehicleStatusCN")); //xe ra
            query.AppendLine(string.Format("{0} = '{1}',", obj.type == "VN" ? "ImageVN" : "ImageCN", obj.image)); //ảnh
            query.AppendLine(string.Format("{0} = '{1}'", obj.type == "VN" ? "TimeOutVN" : "TimeOutCN", Convert.ToDateTime(obj.time).ToString("MM/dd/yyyy HH:mm:ss"))); //thời gian ra
            query.AppendLine("where IsDeleted = 0 and EventType = 5");
            query.AppendLine(string.Format("and {0} = 1", obj.type == "VN" ? "VehicleStatusVN" : "VehicleStatusCN")); //trạng thái xe đã vào
            query.AppendLine(string.Format("and REPLACE(REPLACE({0}, '-', ''), '.', '') LIKE '%{1}%'", obj.type == "VN" ? "PlateVN" : "PlateCN", obj.plate.Replace("-", "").Replace(".", "")));

            var connectionString = AppSettingHelper.GetStringFromFileJson("connectstring", "ConnectionStrings:DefaultConnection").Result;

            var check = SqlHelper.ExcuteCommandToBool(connectionString, query.ToString());

            return await Task.FromResult(check);
        }

        public async Task<MessageReport> Create(tbl_Event_POST model)
        {
            var obj = new tbl_Event()
            {
                Id = Guid.NewGuid().ToString(),
                Code = model.code,
                PlateVN = model.plateVN,
                ImageVN = model.imageVN,
                TimeInVN = !string.IsNullOrEmpty(model.timeInVN) ? Convert.ToDateTime(model.timeInVN) : DateTime.MaxValue,
                TimeOutVN = DateTime.MaxValue,
                VehicleStatusVN = !string.IsNullOrEmpty(model.timeInVN) ? 1 : 0,

                PlateCN = model.plateCN,
                ImageCN = model.imageCN,
                TimeInCN = !string.IsNullOrEmpty(model.timeInCN) ? Convert.ToDateTime(model.timeInCN) : DateTime.MaxValue,
                TimeOutCN = DateTime.MaxValue,
                VehicleStatusCN = !string.IsNullOrEmpty(model.timeInCN) ? 1 : 0,

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
                ParkingPosition = "",
                GroupId = ""           
            };

            var result = await _tbl_EventRepository.Add(obj);

            if (result.isSuccess)
            {
                result = new MessageReport(true, "Thành công");
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
            obj.ServiceCode = model.serviceCode;
            obj.PaymentStatus = model.paymentStatus;
            obj.ModifiedDate = DateTime.Now;

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
            }

            return result;
        }

        public async Task<MessageReport> VehicleStatusIn(API_VehicleStatus model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                var check = await UpdateVehicleIn(model);

                if (check)
                {
                    result = new MessageReport(true, "Thành công");
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
            }
            catch (Exception ex)
            {

                result = new MessageReport(false, ex.Message);
            }

            return result;
        }
    }
}
