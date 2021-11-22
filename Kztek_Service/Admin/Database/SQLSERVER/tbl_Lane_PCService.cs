using Kztek_Core.Models;
using Kztek_Data.Repository;
using Kztek_Library.Helpers;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Kztek_Service.Admin.Database.SQLSERVER
{

    public class tbl_Lane_PCService : Itbl_Lane_PCService
    {
        private Itbl_Lane_PCRepository tbl_Lane_PCRepository;
        public tbl_Lane_PCService(Itbl_Lane_PCRepository tbl_Lane_PCRepository)
        {
            this.tbl_Lane_PCRepository = tbl_Lane_PCRepository;
        }

        public async Task<MessageReport> Create(tbl_Lane_PC obj)
        {
            return await tbl_Lane_PCRepository.Add(obj);
        }

        public async Task<MessageReport> DeleteById(string id)
        {
            var result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:ERR"));

            var obj = GetById(id);
            if (obj.Result != null)
            {
                return await tbl_Lane_PCRepository.Remove(obj.Result);
            }
            else
            {
                result = new MessageReport(false, await LanguageHelper.GetLanguageText("MESSAGEREPORT:NON_RECORD"));
            }

            return await Task.FromResult(result);
        }
        public async Task<GridModel<tbl_Lane_PC>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in tbl_Lane_PCRepository.Table
                        select n;




            var pageList = query.ToPagedList(pageNumber, pageSize);

            var model = GridModelHelper<tbl_Lane_PC>.GetPage(pageList.OrderByDescending(n => n.lane_ID).ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return await Task.FromResult(model);
        }

        public async Task<tbl_Lane_PC_Custom> GetByCustomId(string id)
        {
            var obj = await GetById(id);
            var model = new tbl_Lane_PC_Custom()
            {
                id = obj.id.ToString(),
                lane_ID = obj.lane_ID.ToLower(),
                pc_ID = obj.pc_ID.ToLower()

            };
            return model;
        }

        public async Task<tbl_Lane_PC> GetById(string id)
        {
            return await tbl_Lane_PCRepository.GetOneById(Guid.Parse(id));
        }

        public async Task<tbl_Lane_PC> getByPc_ID(string pc_ID)
        {
            var query = from n in tbl_Lane_PCRepository.Table
                        where n.pc_ID == pc_ID
                        select n;
            return await Task.FromResult(query.FirstOrDefault()) ;
        }

        public async Task<MessageReport> Update(tbl_Lane_PC obj)
        {
            return await tbl_Lane_PCRepository.Update(obj);
        }
    }
}
