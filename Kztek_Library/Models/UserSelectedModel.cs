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

    public class ServiceSelectedModel
    {
        public List<string> Selected { get; set; } = new List<string>();

        public List<Group> Data_Group { get; set; } = new List<Group>();
    }
}
