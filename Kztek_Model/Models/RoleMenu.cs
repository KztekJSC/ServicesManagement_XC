using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("RoleMenu")]
    public class RoleMenu
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string RoleId { get; set; }

        [Required]
        public string MenuId { get; set; }
    }
}