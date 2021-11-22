using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Kztek_Model.Models
{
    [Table("User_AuthGroup")]
    public class User_AuthGroup
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string CardGroupIds { get; set; }
    }
}
