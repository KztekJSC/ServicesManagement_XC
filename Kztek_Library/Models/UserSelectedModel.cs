using System;
using System.Collections.Generic;
using Kztek_Model.Models;

namespace Kztek_Library.Models
{
    public class UserSelectedModel
    {
        public List<string> Selected { get; set; } = new List<string>();

        public List<Role> Data_Role { get; set; } = new List<Role>();
    }
}
