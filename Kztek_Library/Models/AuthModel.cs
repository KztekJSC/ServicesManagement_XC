using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Kztek_Model.Models;

namespace Kztek_Library.Models
{
    public class AuthModel
    {
        [Required (ErrorMessage = "Nhập tài khoản")]
        public string Username { get; set; } = "";

        //[Required (ErrorMessage = "Nhập mật khẩu")]
        public string Password { get; set; } = "";

        public bool isRemember { get; set; } = true;

        public bool isAny { get; set; } = false;

        public string AreaCode { get; set; } = "";

        public List<SelectListModel> Data_Service { get; set; }
    }

    public class AuthModel_LowSecurity
    {
        [Required (ErrorMessage = "Nhập tài khoản")]
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public bool isRemember { get; set; } = true;

        public bool isAny { get; set; } = false;

        public string AreaCode { get; set; } = "";

        public List<SelectListModel> Data_Service { get; set; }
    }
}