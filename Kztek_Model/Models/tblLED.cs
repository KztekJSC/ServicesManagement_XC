using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Model.Models
{
    [Table("tbl_LED")]
    public  class tblLED
    {
        [Key]
        public Guid id { get; set; }

        public string led_Name { get; set; }

        public string led_Code { get; set; }

        public int led_Function { get; set; }

        public string controller_Type { get; set; }

        public string ip_Address { get; set; }

        public int port { get; set; }

        public string description { get; set; }
    }
    public class tblLED_Submit
    {

        public string id { get; set; }

        public string led_Name { get; set; }

        public string led_Code { get; set; }

        public string FunctionLed { get; set; }

        public string led_Function1 { get; set; }

        public int led_Function { get; set; }

        public string controller_Type { get; set; }

        public string ip_Address { get; set; }

        public int port { get; set; }

        public string description { get; set; }



    }
}
