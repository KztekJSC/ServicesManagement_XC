using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Model.Models;
using Kztek_Service.Api;

namespace Kztek_Service.Api.Database.SQLSERVER
{
    public class tblLaneService : ItblLaneService
    {
        private ItblLaneRepository _tblLaneRepository;

        public tblLaneService(ItblLaneRepository _tblLaneRepository)
        {
            this._tblLaneRepository = _tblLaneRepository;
        }

        public Task<IEnumerable<tblLane>> GetAllByFirst(string key, string pcid)
        {
            var query = from n in _tblLaneRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLower();
                query = query.Where(n => n.lane_Name.ToLower().Contains(key));
            }

            //if (!string.IsNullOrWhiteSpace(pcid))
            //{
            //    query = query.Where(n => n.PCID == pcid);
            //}

            return Task.FromResult(query);
        }

        public Task<tblLane> GetById(string id)
        {
            return _tblLaneRepository.GetOneById(id);
        }

        public Task<MessageReport> Create(tblLane_Submit model)
        {
            var obj = new tblLane()
            {

                lane_Name = model.lane_Name,
                lane_Code = model.lane_Code,
                auto_Mode = model.auto_Mode,
                camera_LPR_1 = model.camera_LPR_1_id,
                camera_LPR_2 = model.camera_LPR_2_id,
                card_Types = model.card_Types,
                description = model.description,
                reversal = model.reversal,
                vehicle_Types = model.vehicle_Types,
                camera_Panorama_1 = model.camera_Panorama_1_id,
                camera_Panorama_2 = model.camera_Panorama_2_id,
                direction = model.direction,
                id = Guid.NewGuid()
            };

            return _tblLaneRepository.Add(obj);
        }

        public Task<MessageReport> Update(tblLane_Submit model)
        {
            //Exited? code
            var result = new MessageReport(false, "Có lỗi xảy ra");

            //
            var obj = GetById(model.id).Result;
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return Task.FromResult(result);
            }

            obj.lane_Name = model.lane_Name;
            obj.lane_Code = model.lane_Code;
            obj.auto_Mode = model.auto_Mode;
            obj.camera_LPR_1 = model.camera_LPR_1_id;
            obj.camera_LPR_2 = model.camera_LPR_2_id;
            obj.card_Types = model.card_Types;
            obj.description = model.description;
            obj.reversal = model.reversal;
            obj.vehicle_Types = model.vehicle_Types;
            obj.camera_Panorama_1 = model.camera_Panorama_1_id;
            obj.camera_Panorama_2 = model.camera_Panorama_2_id;
            obj.direction = model.direction;
            obj.id = Guid.Parse(model.id);

            return _tblLaneRepository.Update(obj);
        }

        public Task<MessageReport> Remove(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = GetById(id).Result;
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return Task.FromResult(result);
            }

            return _tblLaneRepository.Remove(obj);
        }
    }
}
