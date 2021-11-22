using System.Collections.Generic;

namespace Kztek_Library.Models
{
    public class SelectListModel
    {
        public string ItemValue { get; set; }

        public string ItemText { get; set; }
    }

    public class SelectListModel_Breadcrumb
    {
        public string MenuName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool isFolder { get; set; }
    }

    public class SelectListModel_Print_Header
    {
        public string ItemText { get; set; }
    }

    public class SelectListModel_Print_Column_Header
    {
        public string ItemText { get; set; }
    }

    public class SelectListModel_Chosen
    {
        public string IdSelectList { get; set; }

        public List<SelectListModel> Data { get; set; }

        public string Selecteds { get; set; }

        public bool isMultiSelect { get; set; }

        public string Placeholder { get; set; }
    }
    public class SelectListModelCommunication_Chosen
    {
        public string IdSelectList { get; set; }

        public List<SelectListModel_Communication> Data { get; set; }

        public int Selecteds { get; set; }

        public bool isMultiSelect { get; set; }

        public string Placeholder { get; set; }
    }
    public class SelectListModel_Multi
    {
        public string IdSelectList { get; set; }

        public List<SelectListModel> Data { get; set; }

        public string Selecteds { get; set; }

        public string Placeholder { get; set; }
    }

    public class SelectListModel_MenuGroup
    {
        public string ItemValue { get; set; }

        public string ItemText { get; set; }

        public int ItemIndex { get; set; }

        public string ItemCode { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }

        public string AreaName { get; set; }

        public string Layout { get; set; }

        public string Label { get; set; } = "";
    }


    public class SelectListModel_Communication
    {
        public int ItemValue { get; set; }

        public string ItemText { get; set; }
    }

    public class SelectListModelAutocomplete
    {
        //Id
        public string id { get; set; }

        //Tên tìm thêm
        public string name { get; set; }

        //Tên hiển thị
        public string value { get; set; }
    }

    public class SelectListModel_FileUpload
    {
        public string FileUploadName { get; set; }

        public string BoxRenderId { get; set; }

        public string Base64String { get; set; }

        public string FilePath { get; set; }

        public string CustomerId { get; set; }
    }

    public class SelectListModel_Date
    {
        public string fromdate { get; set; }

        public string todate { get; set; }
    }
}