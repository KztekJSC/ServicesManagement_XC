using System;
using System.Collections.Generic;
using System.Text;

namespace Kztek_Library.Models
{
    public class tblPCCustomViewModel
    {
        public string PCID { get; set; }

        public string ComputerName { get; set; }

        public string GateID { get; set; }

        public string GateName { get; set; }

        public string IPAddress { get; set; }

        public string Description { get; set; }

        public bool Inactive { get; set; }

        public int SortOrder { get; set; }
    }
}
