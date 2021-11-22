using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("tblLog")]
    public class tblLog
    {
        [Key]
        public string LogID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string UserName { get; set; }
        public string AppCode { get; set; }
        public string SubSystemCode { get; set; }
        public string ObjectName { get; set; }
        public string Actions { get; set; }
        public string Description { get; set; }
        public string IPAddress { get; set; }
        public string ComputerName { get; set; }
    }
}
