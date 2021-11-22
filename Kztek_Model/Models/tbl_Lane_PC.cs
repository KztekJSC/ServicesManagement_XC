using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Model.Models
{
    [Table("tbl_Lane_PC")]
   public class tbl_Lane_PC
    {
        [Key]
        public Guid id { get; set; }
        public string lane_ID { get; set; }
        public string pc_ID { get; set; }
       
    }

    public class tbl_Lane_PC_Custom
    {
     
        public string id { get; set; }
        public string lane_ID { get; set; }

        public string pc_ID { get; set; }

        public string LaneIds { get; set; }

    }
}
