using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Model.Models
{
    [Table("Service")]
    public class Service
    {
        [Key]
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; } //Ngày tạo

        public DateTime ModifiedDate { get; set; } //Ngày cập nhật
    }

    public class Service_Submit
    {
     
        public string Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

       

        public string Description { get; set; }

        public List<string> Groups { get; set; } = new List<string>();

        public List<Group> Data_Group { get; set; } = new List<Group>();

        public string CreatedDate { get; set; } //Ngày tạo

        public string ModifiedDate { get; set; } //Ngày cập nhật
    }

}
