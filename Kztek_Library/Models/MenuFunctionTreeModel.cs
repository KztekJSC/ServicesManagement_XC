using System;
using System.Collections.Generic;
using Kztek_Model.Models;

namespace Kztek_Library.Models
{
    public class MenuFunctionTreeModel
    {
        public List<MenuFunction> Data_All { get; set; }

        public List<MenuFunction> Data_Child { get; set; }

        public string AreaCode { get; set; }
    }
}
