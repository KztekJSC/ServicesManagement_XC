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
    public class tblPCService : ItblPCService
    {
        private ItblPCRepository _tblPCRepository;

        public tblPCService(ItblPCRepository _tblPCRepository)
        {
            this._tblPCRepository = _tblPCRepository;
        }

        public async Task<IEnumerable<tblPC>> GetAllByFirst(string key, string gateid)
        {
            var query = from n in _tblPCRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLower();
                query = query.Where(n => n.pc_Name.ToLower().Contains(key));
            }



            return await Task.FromResult(query);
        }

        public async Task<tblPC> GetById(string id)
        {
            return await _tblPCRepository.GetOneById(id);
        }

        public async Task<MessageReport> Create(tblPC_POST model)
        {
            var obj = new tblPC()
            {
                pc_Code = model.pc_Code,
                pc_Name = model.pc_Name,
                description = model.description,
                ip_Address = model.ip_Address,
                id = Guid.NewGuid()
            };

            return await _tblPCRepository.Add(obj);
        }

        public async Task<MessageReport> Update(tblPC_POST model)
        {
            //Exited? code
            var result = new MessageReport(false, "Có lỗi xảy ra");

            //
            var obj = GetById(model.id).Result;
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return await Task.FromResult(result);
            }

            obj.pc_Code = model.pc_Code;
            obj.pc_Name = model.pc_Name;
            obj.description = model.description;
            obj.ip_Address = model.ip_Address;
            obj.id = Guid.Parse( model.id);

            return await _tblPCRepository.Update(obj);
        }

        public async Task<MessageReport> Remove(string id)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var obj = GetById(id).Result;
            if (obj == null)
            {
                result = new MessageReport(false, "Bản ghi không tồn tại");
                return await Task.FromResult(result);
            }

            return await _tblPCRepository.Remove(obj);
        }
    }
}
