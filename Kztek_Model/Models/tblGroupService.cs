using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Model.Models
{
    [Table("tblGroupService")]
    public class tblGroupService
    {    
            [Key]
            public string Id { get; set; }

            [Required]
            public string GroupId { get; set; }

            [Required]
            public string ServiceId { get; set; }
        }
    
}
