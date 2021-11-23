using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Api.Database.SQLSERVER
{
    public class tbl_EventService
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

        public async Task<MessageReport> Create(tbl_Event_POST model)
        {
            var obj = new tbl_Event()
            {
                Id = Guid.NewGuid().ToString(),
                Code = model.Code,
                ImageCN = model.ImageCN,
                ImageVN = model.ImageVN,
                PlateCN = model.PlateCN,
                PlateVN = model.PlateVN,
                Description = model.Description,
                GroupId = "",
                Price = model.Price,
                SubPrice = model.SubPrice,
                ServiceCode = model.ServiceCode,
                Service = model.Service,
                ParkingPosition = "",
                VehicleType = model.VehicleType,
                Weight = model.Weight,
                ProductType = model.ProductType,
                ProductGroup = model.ProductGroup,
                EventType = model.EventType,
                PaymentStatus = model.PaymentStatus,
                Cost = model.Cost,
                IsDeleted = false,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                StartDate = DateTime.MinValue,
                EndDate = DateTime.MinValue
            };

            var result = await _tbl_EventRepository.Add(obj);

            if (result.isSuccess)
            {
                result = new MessageReport(true, obj.Id);
            }

            return result;
        }

        public async Task<MessageReport> Update(tbl_Event_POST model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = await GetById(model.ID);
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return await Task.FromResult(result);
            }

            obj.Code = model.Code;
            obj.ImageCN = model.ImageCN;
            obj.ImageVN = model.ImageVN;
            obj.PlateCN = model.PlateCN;
            obj.PlateVN = model.PlateVN;
            obj.Description = model.Description;
            obj.GroupId = "";
            obj.Price = model.Price;
            obj.SubPrice = model.SubPrice;
            obj.ServiceCode = model.ServiceCode;
            obj.Service = model.Service;
            obj.ParkingPosition = "";
            obj.VehicleType = model.VehicleType;
            obj.Weight = model.Weight;
            obj.ProductType = model.ProductType;
            obj.ProductGroup = model.ProductGroup;
            obj.EventType = model.EventType;
            obj.ModifiedDate = DateTime.Now;

            result = await _tbl_EventRepository.Update(obj);

            if (result.isSuccess)
            {
                result = new MessageReport(true, obj.Id);
            }

            return result;
        }

        public async Task<MessageReport> Delete(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = await GetById(id);
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
    }
}
