using Kztek_Core.Models;
using Kztek_Library.Models;
using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Service.Admin
{
    public interface IServiceService
    {
        Task<GridModel<Service>> GetAllCustomPagingByFirst(string key, string pc, int page, int v);
        Task<MessageReport> DeleteById(string id);
        Task<Service> GetById(string id);
        Task<MessageReport> Update(Service oldObj);
        Task<MessageReport> Create(Service model);
        Task<List<Service>> GetAll();
        Task<Service> GetByName(string key);
        Task<SelectListModel_Chosen> SelectChoseService(string id = "", string placeholder = "", string selecteds = "");
        Task<SelectListModel_Multi> SelectColumn(string controller = "", string action = "",string id = "", string placeholder = "", string selecteds = "" , bool isMultipule = false);

        Task<SelectListModel_Multi> SelectColumnCoor(string controller = "", string action = "",string id = "", string placeholder = "", string selecteds = "", bool isMultipule = false);
        Task<SelectListModel_Chosen> SelectChoseParkingPosittion(string id = "", string placeholder = "", string selecteds = "");
        Task<SelectListModel_Multi> SelectColumnAssignment(string controller = "", string action = "", string id = "", string placeholder = "", string selecteds = "", bool isMultipule = false);
    }
}
