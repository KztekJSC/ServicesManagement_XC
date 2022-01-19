using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Model.Models
{
    [Table("EventIn")]
    public class EventIn
    {
        [Key]
        public string Id { get; set; }

        public string Plate { get; set; }
        public string PlateUnsign { get; set; }
        public DateTime TimeIn { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
