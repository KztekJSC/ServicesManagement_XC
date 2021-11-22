using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Model.Models;

namespace Kztek_Service.Api.Database.SQLSERVER
{
    public class tblSystemConfigService : ItblSystemConfigService
    {
        private ItblSystemConfigRepository _tblSystemConfigRepository;

        public tblSystemConfigService(ItblSystemConfigRepository _tblSystemConfigRepository)
        {
            this._tblSystemConfigRepository = _tblSystemConfigRepository;
        }

        public async Task<MessageReport> Create(tblSystemConfig_POST model)
        {
            var obj = new tblSystemConfig()
            {
                SystemConfigID = Guid.NewGuid().ToString(),
                Company = model.Company,
                Address = model.Address,
                DelayTime = model.DelayTime,
                EnableAlarmMessageBox = model.EnableAlarmMessageBox,
                EnableAlarmMessageBoxIn = model.EnableAlarmMessageBoxIn,
                EnableDeleteCardFailed = model.EnableDeleteCardFailed,
                EnableSoundAlarm = model.EnableSoundAlarm,
                Fax = model.Fax,
                FeeName = model.FeeName,
                KeyA = model.KeyA,
                KeyB = model.KeyB,
                Logo = model.Logo,
                Para1 = model.Para1,
                Para2 = model.Para2,
                SystemCode = model.SystemCode,
                Tax = model.Tax,
                Tel = model.Tel
            };

            return await _tblSystemConfigRepository.Add(obj);
        }

        public async Task<List<tblSystemConfig>> GetDefault()
        {
            var model = _tblSystemConfigRepository.Table;
            return await Task.FromResult(model.ToList());
        }

        public async Task<MessageReport> Update(tblSystemConfig_PUT model)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = _tblSystemConfigRepository.GetOneById(Guid.Parse(model.Id)).Result;
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return await Task.FromResult(result);
            }

            obj.Address = model.Address;
            obj.Company = model.Company;
            obj.DelayTime = model.DelayTime;
            obj.EnableAlarmMessageBox = model.EnableAlarmMessageBox;
            obj.EnableAlarmMessageBoxIn = model.EnableAlarmMessageBoxIn;
            obj.EnableDeleteCardFailed = model.EnableDeleteCardFailed;
            obj.EnableSoundAlarm = model.EnableSoundAlarm;
            obj.Fax = model.Fax;
            obj.FeeName = model.FeeName;
            obj.KeyA = model.KeyA;
            obj.KeyB = model.KeyB;
            obj.Logo = model.Logo;
            obj.Para1 = model.Para1;
            obj.Para2 = model.Para2;
            obj.SystemCode = model.SystemCode;
            obj.Tax = model.Tax;
            obj.Tel = model.Tel;
            obj.isAuthInView = model.isAuthInView;

            return await _tblSystemConfigRepository.Update(obj);
        }

        public async Task<MessageReport> Remove(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = _tblSystemConfigRepository.GetOneById(Guid.Parse(id)).Result;
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return await Task.FromResult(result);
            }

            return await _tblSystemConfigRepository.Remove(obj);
        }
    }
}
