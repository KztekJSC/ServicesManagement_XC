using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        //[Required]
        //[StringLength(250)]
        public string Email { get; set; }

        //[StringLength(500)]
        //[Column(TypeName = "varchar")]
        public string ImagePath { get; set; }

        [Required]
        //[StringLength(100)]
        public string Username { get; set; }

        //[StringLength(500)]
        //[Column(TypeName = "varchar")]
        public string Password { get; set; }

        //[StringLength(500)]
        //[Column(TypeName = "varchar")]
        public string PasswordSalat { get; set; }

        //[StringLength(500)]
        public string Address { get; set; }

        //[StringLength(150)]
        //[Column(TypeName = "varchar")]
        public string Phone { get; set; }

        [Column("Admin", TypeName = "bit")]
        [DefaultValue(false)]
        public bool Admin { get; set; }

        [Column("Active", TypeName = "bit")]
        [DefaultValue(true)]
        public bool Active { get; set; }

        [Column("IsDeleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool IsDeleted { get; set; }

        public string UserAvatar { get; set; }

        public Nullable<DateTime> DateCreated { get; set; }
    }

    public class User_Submit
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        public string OldPassword { get; set; }

        public string Password { get; set; }

        public string RePassword { get; set; }

        public bool isAdmin { get; set; }

        public bool Active { get; set; } = true;

        public List<string> Roles { get; set; } = new List<string>();

        public string RoleIds { get; set; }

        public List<Role> Data_Role { get; set; } = new List<Role>();

        public string Avatar { get; set; } = "";
    }

    //public class User_Selected
    //{
    //    public List<string> Selected { get; set; } = new List<string>();

    //    public List<Role> Data_Role { get; set; } = new List<Role>();

    //}
}