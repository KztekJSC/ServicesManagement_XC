using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Model.Models
{
   public class ColumTable
    {
        public string Id { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public bool Active { get; set; }

        public string Columns { get; set; }

        public string ColumShows { get; set; }
    }
}
