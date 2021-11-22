using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
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
    public class tblLaneService : ItblLaneService
    {
        private ItblLaneRepository _tblLaneRepository;
        private ItblPCRepository _tblPCRepository;


        public tblLaneService(ItblLaneRepository _tblLaneRepository, ItblPCRepository _tblPCRepository)
        {
            this._tblLaneRepository = _tblLaneRepository;
            this._tblPCRepository = _tblPCRepository;

        }

        public async Task<IEnumerable<tblLane>> GetAll()
        {
            var query = from n in _tblLaneRepository.Table
                        select n;

            return await Task.FromResult(query.ToList());
        }

        public async Task<IEnumerable<tblLane>> GetAllActive()
        {
            var query = from n in _tblLaneRepository.Table
                            //where n.Inactive == false
                        select n;

            return await Task.FromResult(query.ToList());
        }

        public async Task<tblLane> GetActiveLoop()
        {
            //var query = from n in _tblLaneRepository.Table
            //            where n.Inactive == false && n.IsLoop == true
            //            select n;

            //return await Task.FromResult(query.FirstOrDefault());
            return null;
        }

        public async Task<IEnumerable<tblLane>> GetAllActiveByListId(string ids)
        {
            //var query = from n in _tblLaneRepository.Table
            //            where n.Inactive == false && ids.Contains(n.LaneID.ToString())
            //            select n;

            //return await Task.FromResult(query.ToList());
            return null;
        }

        public async Task<GridModel<tblLane>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in _tblLaneRepository.Table
                        select n;
            //var query = (from n in _tblLaneRepository.Table
            //             join m in _tblPCRepository.Table on n.PCID equals m.PCID.ToString() into n_m
            //             from m in n_m.DefaultIfEmpty()

            //             select new tblLaneCustomViewModel()
            //             {
            //                 Inactive = n.Inactive,
            //                 LaneID = n.LaneID.ToString(),
            //                 LaneName = n.LaneName,
            //                 LaneType = n.LaneType.Value,
            //                 PCID = n.PCID,
            //                 PCName = m.ComputerName,
            //                 SortOrder = n.SortOrder,
            //             });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.lane_Name.Contains(key));
            }



            var pageList = query.ToPagedList(pageNumber, pageSize);

            var model = GridModelHelper<tblLane>.GetPage(pageList.OrderByDescending(n => n.lane_Name).ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }

        public async Task<MessageReport> Create(tblLane model)
        {
            return await _tblLaneRepository.Add(model);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await _tblLaneRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }

        public async Task<tblLane> GetById(string id)
        {
            return await _tblLaneRepository.GetOneById(Guid.Parse(id));
        }

        public async Task<MessageReport> Update(tblLane model)
        {
            return await _tblLaneRepository.Update(model);
        }

        public async Task<tblLane> GetByName(string name)
        {
            var query = from n in _tblLaneRepository.Table
                        where n.lane_Name == name
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<tblLane> GetByName_Id(string name, string id)
        {
            var query = from n in _tblLaneRepository.Table
                        where n.lane_Name == name && n.id.ToString() != id
                        select n;

            return await Task.FromResult(query.FirstOrDefault());
        }

        public async Task<string> GetTitle(string landids)
        {
            var str = "";

            var query = from n in _tblLaneRepository.Table
                        select n;

            var listdata = query.ToList();

            if (!string.IsNullOrEmpty(landids))
            {
                var obj = listdata.Where(n => landids.Contains(n.id.ToString())).FirstOrDefault();

                var listid = landids.Split(',');

                if (listdata.Count() > 0 && obj != null)
                {
                    str = ((listid.Length - 1) < listdata.Count()) ? obj.lane_Code : "";
                }


            }

            return await Task.FromResult(str);
        }

        public async Task<tblLane_Submit> GetByCustomId(string id)
        {
            var model = await GetById(id);
            var obj = new tblLane_Submit()
            {
                id = id.ToString(),
                lane_Code = model.lane_Code,
                lane_Name = model.lane_Name,
                vehicle_Types = model.vehicle_Types,
                reversal = model.reversal,
                description = model.description,
                camera_LPR_1_id = model.camera_LPR_1,
                camera_LPR_2_id = model.camera_LPR_2,
                camera_Panorama_1_id = model.camera_Panorama_1,
                camera_Panorama_2_id = model.camera_Panorama_2,
                card_Types = model.card_Types,
                auto_Mode = model.auto_Mode

            };
            return obj;
    }
    }
}
