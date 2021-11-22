using System;
using System.Collections.Generic;
using System.Text;

namespace Kztek_Library.Models
{
    public class MN_License
    {
        public string Id { get; set; }

        public string ProjectName { get; set; }

        public bool IsExpire { get; set; }

        public string ExpireDate { get; set; }
    }
}
