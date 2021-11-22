using System.Collections.Generic;
using Kztek_Model.Models;

namespace Kztek_Library.Models
{
    public class SidebarModel
    {
        public string Id { get; set; } = "";

        public string ControllerName { get; set; } = "";

        public string ActionName { get; set; } = "";

        public List<MenuFunction> Data { get; set; } = new List<MenuFunction>();

        public List<MenuFunction> Data_Child { get; set; } = new List<MenuFunction>();

        public MenuFunction CurrentView { get; set; } = null;

        public string Breadcrumb { get; set; } = "";

        public string AreaCode { get; set; } = "";
    }
}