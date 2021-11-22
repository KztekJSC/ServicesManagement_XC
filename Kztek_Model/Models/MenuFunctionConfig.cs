using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("MenuFunctionConfig")]
    public class MenuFunctionConfig
    {
        [Key]
        public string Id { get; set; }

        public string MenuFunctionId { get; set; }
    }
}
