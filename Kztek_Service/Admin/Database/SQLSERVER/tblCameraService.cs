using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Library.Models;
using Kztek_Model.Models;
using Kztek_Service.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Kztek_Service.Admin.Database.SQLSERVER
{
    public class tblCameraService : ItblCameraService
    {
        private ItblCameraRepository _tblCameraRepository;
        private ItblPCRepository _tblPCRepository;
      
        public tblCameraService(ItblCameraRepository _tblCameraRepository, ItblPCRepository _tblPCRepository)
        {
            this._tblCameraRepository = _tblCameraRepository;
            this._tblPCRepository = _tblPCRepository;
           
        }

        public async Task<MessageReport> Create(tblCamera model)
        {
            return await _tblCameraRepository.Add(model);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _tblCameraRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

        public async Task<tblCamera> GetById(string id)
        {
            return await _tblCameraRepository.GetOneById(Guid.Parse( id));
        }

        public async Task<MessageReport> Update(tblCamera model)
        {
            return await _tblCameraRepository.Update(model);
        }
        public async Task<List<tblCamera>> GetAllActive()
        {
            var query = from n in _tblCameraRepository.Table                       
                        select n;

            return await Task.FromResult(query.ToList());
        }
        public async Task<IEnumerable<tblCamera>> GetAllActiveByPC(string id)
        {
            //var query = from n in _tblCameraRepository.Table
            //            where n.Inactive == false && n.PCID == id
            //            select n;

            //return await Task.FromResult(query.ToList());
            return null;
        }

        public async Task<GridModel<tblCamera>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            //var query = (from n in _tblCameraRepository.Table
            //             //join m in _tblPCRepository.Table on n.PCID equals m.PCID.ToString() into n_m
            //             from m in n_m.DefaultIfEmpty()

            //             select new tblCameraCustomViewModel()
            //             {
            //                 CameraID = n.CameraID.ToString(),
            //                 CameraName = n.CameraName,
            //                 PCID = n.PCID,
            //                 PCName = m != null ? m.ComputerName : "",
            //                 HttpUrl = n.HttpURL,
            //                 Inactive = n.Inactive,
            //                 SortOrder = n.SortOrder
            //             });
            var query = from n in _tblCameraRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.camera_Name.Contains(key) );
            }

            //if (!string.IsNullOrWhiteSpace(pc))
            //{
            //    query = query.Where(n => n.PCID == pc);
            //}
        

            var pageList = query.ToPagedList(pageNumber, pageSize);

            var model = GridModelHelper<tblCamera>.GetPage(pageList.OrderByDescending(n => n.chanel).ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }

        public async Task<tblCamera> GetByName(string name)
        {
            var query = from n in _tblCameraRepository.Table
                        where n.camera_Name == name
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<tblCamera> GetByName_Id(string name, string id)
        {
            var query = from n in _tblCameraRepository.Table
                        where n.camera_Name == name && n.id.ToString() != id
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }
    }
}
