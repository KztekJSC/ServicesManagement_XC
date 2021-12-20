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
    public class GroupServieService : IGroupServieService
    {
        private IServiceRepository _ServiceRepository;
        public GroupServieService(IServiceRepository _ServiceRepository)
        {
            this._ServiceRepository = _ServiceRepository;
        }
        public Task<GridModel<Service>> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in _ServiceRepository.Table
                        select n;


            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }



            var pageList = query.ToPagedList(pageNumber, pageSize);

            var model = GridModelHelper<Service>.GetPage(pageList.OrderByDescending(n => n.Name).ToList(), pageNumber, pageSize, pageList.TotalItemCount, pageList.PageCount);

            return null;
            //return await Task.FromResult(model);
        }
    }
}
