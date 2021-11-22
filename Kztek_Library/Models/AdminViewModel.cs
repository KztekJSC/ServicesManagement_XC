using System;
using System.Collections.Generic;
using Kztek_Model.Models;

namespace Kztek_Library.Models
{
    public class AdminViewModel
    {
        public tblSystemConfig Config { get; set; }

        public List<MenuFunctionConfig> Selecteds { get; set; }

        public List<MenuFunction> Menus { get; set; }

        public List<MenuFunction> Childs { get; set; }

    }
}
