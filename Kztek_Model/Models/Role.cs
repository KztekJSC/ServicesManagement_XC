using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("Role")]
    public class Role
    {
        public string Id { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        [Column("Active", TypeName = "bit")]
        [DefaultValue(true)]
        public bool Active { get; set; }
    }

    public class Role_Submit
    {
        public string Id { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        public List<string> MenuFunctions { get; set; } = new List<string>();

        public string MenuFunctionIds { get; set; } = "";

        public bool Active { get; set; } = true;

        public List<MenuFunction> Data_Tree { get; set; }
    }
    public class Role_Custom
    {
        public string Id { get; set; }

        public string RoleName { get; set; }

        public string Description { get; set; }

        [Column("Active", TypeName = "bit")]
        [DefaultValue(true)]
        public bool Active { get; set; }
        public string UserId { get; set; }
    }
    //public class Role_Selected
    //{
    //    public List<string> Selected { get; set; } = new List<string>();

    //    public List<MenuFunction> Data_Tree { get; set; } = new List<MenuFunction>();

    //    public List<MenuFunction> Data_Child { get; set; } = new List<MenuFunction>();
    //}
}