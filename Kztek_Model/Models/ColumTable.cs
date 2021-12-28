using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Model.Models
{
    [Table("ColumTable")]
    public class ColumTable
    {
        [Key]
        public string Id { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public bool Active { get; set; }

        public string Columns { get; set; }

        public string ColumShows { get; set; }
    }
}
