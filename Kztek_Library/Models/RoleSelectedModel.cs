using System;
using System.Collections.Generic;
using Kztek_Model.Models;

namespace Kztek_Library.Models
{
    public class RoleSelectedModel
    {
        public List<string> Selected { get; set; } = new List<string>();

        public List<MenuFunction> Data_Tree { get; set; } = new List<MenuFunction>();

        public List<MenuFunction> Data_Child { get; set; } = new List<MenuFunction>();
    }
}
