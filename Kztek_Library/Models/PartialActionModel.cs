using System;
using System.Collections.Generic;

namespace Kztek_Library.Models
{
    public class PartialActionModel<T> where T: class
    {
        public T Model { get; set; }

        public string Title { get; set; } = "";

        public string Controller { get; set; } = "";
        public string Action { get; set; } = "";
        public int Page { get; set; } = 1;

        public SelectListModel_Chosen Data_Select_1 { get; set; } //List tham số 1 => kiểu chosen

        public SelectListModel_Chosen Data_Select_2 { get; set; } //List tham số 2 => kiểu chosen

        public SelectListModel_Chosen Data_Select_3 { get; set; } //List tham số 3 => kiểu chosen
        public SelectListModel_Chosen Data_Select_4 { get; set; } //List tham số 3 => kiểu chosen

        public SelectListModel_Multi Data_SelectMulti_1 { get; set; } //List tham số 1 => kiểu multi

        public List<SelectListModel> Dt_1 { get; set; } //List dữ liệu sử dụng
        public List<SelectListModel> Dt_2 { get; set; } //List dữ liệu sử dụng
        public List<SelectListModel_Communication> Commu_1 { get; set; } //List dữ liệu sử dụng
        public SelectListModel SelectListModel_1 { get; set; } //List dữ liệu sử dụng

        public string hidJSON_1 { get; set; } //model json => SerializeObject
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public bool IsCheck { get; set; }
    }
}
