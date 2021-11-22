using System.Collections.Generic;
using Kztek_Model.Models;

namespace Kztek_Library.Models
{
    public class BreadcrumbModel
    {
        public string ControllerName { get; set; } = "";

        public string ActionName { get; set; } = "";

        public MenuFunction CurrentView { get; set; } = null;

        public List<SelectListModel_Breadcrumb> Data { get; set; } = new List<SelectListModel_Breadcrumb>();

        public string Breadcrumb { get; set; } = "";

        public List<SelectListModel> MenuGroups { get; set; }

        public string AreaCode { get; set; }
    }
}