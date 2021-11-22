using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("tbl_PC")]
    public class tblPC
    {
        [Key]
        public Guid id { get; set; }

        public string pc_Code { get; set; }

        public string pc_Name { get; set; }

        public string ip_Address { get; set; }

        public string description { get; set; }

    }
    public class tblPC_POST
    {
    
        public string id { get; set; }

        public string pc_Code { get; set; }

        public string pc_Name { get; set; }
     
        public string ip_Address { get; set; }

        public string description { get; set; }

    }
}
