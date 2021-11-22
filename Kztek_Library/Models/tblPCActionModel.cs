using Kztek_Model.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kztek_Library.Models
{
    public class tblPCActionModel
    {
        public tblPC Model { get; set; } = new tblPC();
        public string Controller { get; set; } = "";
        public string Action { get; set; } = "";
        public int Page { get; set; } = 1;
    }
}
