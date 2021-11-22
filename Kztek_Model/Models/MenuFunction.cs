using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Kztek_Model.Models
{
    [Table("MenuFunction")]
    public class MenuFunction
    {
        public string Id { get; set; }

        public string MenuName { get; set; }

        [StringLength(150)]
        public string ControllerName { get; set; }

        [StringLength(10)]
        public string MenuType { get; set; }

        [StringLength(150)]
        public string ActionName { get; set; }

        [StringLength(1000)]
        public string Url { get; set; }

        [StringLength(100)]
        public string Icon { get; set; }

        [StringLength(100)]
        public string ParentId { get; set; }

        [Column("Active", TypeName = "bit")]
        [DefaultValue(true)]
        public bool Active { get; set; }

        [Column("Deleted", TypeName = "bit")]
        [DefaultValue(false)]
        public bool Deleted { get; set; }

        public Nullable<int> OrderNumber { get; set; }

        public string Breadcrumb { get; set; }

        public Nullable<int> Dept { get; set; }

        [Column("isSystem", TypeName = "bit")]
        [DefaultValue(false)]
        public bool isSystem { get; set; } = false;

        public string MenuGroupListId { get; set; } = "";
    }

    public class MenuFunction_Submit
    {
        public string Id { get; set; }

        public string MenuName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string MenuType { get; set; }

        public string Icon { get; set; }

        public bool Active { get; set; }

        public int SortOrder { get; set; }

        public string ParentId { get; set; }

        public bool isSystem { get; set; } = false;

        public string MenuGroupListId { get; set; } = "";

        public Nullable<int> OrderNumber { get; set; }
    }

    //public class MenuFunction_Tree
    //{
    //    public List<MenuFunction> Data_All { get; set; }

    //    public List<MenuFunction> Data_Child { get; set; }

    //    public string AreaCode { get; set; }
    //}
}