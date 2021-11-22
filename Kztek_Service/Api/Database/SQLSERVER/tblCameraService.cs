using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Security;
using Kztek_Model.Models;
using Kztek_Service.Api;

namespace Kztek_Service.Api.Database.SQLSERVER
{
    public class tblCameraService : ItblCameraService
    {
        private ItblCameraRepository _tblCameraRepository;

        public tblCameraService(ItblCameraRepository _tblCameraRepository)
        {
            this._tblCameraRepository = _tblCameraRepository;
        }

        public Task<IEnumerable<tblCamera>> GetAllByFirst(string key, string pcid)
        {
            var query = from n in _tblCameraRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLower();
                query = query.Where(n => n.camera_Name.ToLower().Contains(key));
            }

          

            return Task.FromResult(query);
        }

        Task<List<tblCamera_View>> ItblCameraService.GetAllCustomByFirst(string key, string pcid)
        {
            var query = from n in _tblCameraRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLower();
                query = query.Where(n => n.camera_Name.ToLower().Contains(key));
            }

           

            var data = query.ToList();
            var ls = new List<tblCamera_View>();

            foreach (var item in data)
            {
                ls.Add(GetCustomByModel(item).Result);
            }
            return Task.FromResult(ls);
        }

        public Task<tblCamera> GetById(string id)
        {
            return _tblCameraRepository.GetOneById(id);
        }

        public Task<tblCamera_View> GetCustomById(string id)
        {
            var model = new tblCamera_View();

            var obj = GetById(id).Result;
            if (obj != null)
            {
                return GetCustomByModel(obj);
            }

            return Task.FromResult(model);
        }

        public Task<tblCamera_View> GetCustomByModel(tblCamera model)
        {
            var obj = new tblCamera_View()
            {
                CameraCode = model.camera_Code,
                CameraID = model.id.ToString(),
                CameraName = model.camera_Name,
                CameraType = model.camera_Type,      
                Channel = model.chanel,  
                HttpPort = model.http_Port.ToString(),
                Resolution = model.resolution,            
                SDK = model.SDK,
            
                StreamType = model.stream_Type,
                UserName = model.auth_Login,
                Password = CryptoHelper.Decrypt(model.auth_Password, true)
            };

            return Task.FromResult(obj);
        }

        public Task<MessageReport> Create(tblCamera_POST model)
        {
            var obj = new tblCamera()
            {
                camera_Code = model.camera_Code,
                id = Guid.NewGuid(),
                camera_Name = model.camera_Name,
                camera_Type = model.camera_Type,               
                chanel =Convert.ToInt32( model.chanel),
                http_Port = Convert.ToInt32( model.http_Port),             
                auth_Password = CryptoHelper.Encrypt(model.auth_Password, true),
                resolution = model.resolution,          
                SDK = model.SDK,
                stream_Type = model.stream_Type,
                auth_Login = model.auth_Login,
               
              
            };

            return _tblCameraRepository.Add(obj);
        }

        public Task<MessageReport> Update(tblCamera_PUT model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = GetById(model.Id).Result;
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return Task.FromResult(result);
            }

            //obj.CameraCode = model.CameraCode;
            //obj.CameraName = model.CameraName;
            //obj.CameraType = model.CameraType;
            //obj.Cgi = model.Cgi;
            //obj.Channel = model.Channel;
            //obj.EnableRecording = model.EnableRecording;
            //obj.FrameRate = model.FrameRate;
            //obj.HttpPort = model.HttpPort;
            //obj.HttpURL = model.HttpURL;
            //obj.Inactive = model.Inactive;
            //obj.PCID = model.PCID;
            //obj.Password = CryptoHelper.Encrypt(model.Password, true);
            //obj.PositionIndex = model.PositionIndex;
            //obj.Resolution = model.Resolution;
            //obj.RtspPort = model.RtspPort;
            //obj.SDK = model.SDK;
            //obj.StreamType = model.StreamType;
            //obj.UserName = model.UserName;
            //obj.MotionZone = model.MotionZone;
            //obj.Config = model.Config;

            return _tblCameraRepository.Update(obj);
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

            return _tblCameraRepository.Remove(obj);
        }
    }
}
