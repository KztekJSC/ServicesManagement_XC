using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Kztek_Library.Models
{
    [Table("ReportModel")]
    public class ReportModel
    {

    }

    public class ReportEventOut
    {
        public long RowNumber { get; set; }
        public string Id { get; set; } //sự kiễn nhiễu là ko có biển số hoặc không có trong danh sách đăng ký

        public string EventCode { get; set; }

        public string PlateIn { get; set; }

        public string PicVehicleIn { get; set; } //ảnh xe

        public string PicAllIn { get; set; }//ảnh toàn cảnh

        public string GateIn { get; set; }

        public DateTime DateTimeIn { get; set; }

        public string PlateOut { get; set; }

        public string PicVehicleOut { get; set; } //ảnh xe

        public string PicAllOut { get; set; }//ảnh toàn cảnh

        public string GateOut { get; set; }

        public DateTime DateTimeOut { get; set; }

        public string GroupId { get; set; }

        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
        public bool IsDelete { get; set; }
        public string VehicleType { get; set; }

        public string Driver { get; set; }

        public string Plate { get; set; }

        public string Time { get; set; }
        public int Turn { get; set; }
        public int Weight { get; set; }
        public string RelativeEventId { get; set; }
    }



}
