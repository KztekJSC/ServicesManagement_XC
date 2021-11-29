using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek_Model.Models
{
    [Table("tbl_Event")]
    public class tbl_Event
    {
        [Key]
        public string Id { get; set; }

        public string Code { get; set; } //Mã đăng ký

        public int EventType { get; set; } //Trạng thái

        public string PlateVN { get; set; } //Biển số xe Việt Nam

        public string PlateCN { get; set; } //Biển số xe Trung Quốc

        public int Weight { get; set; } //Khối lượng hàng

        public string ProductType { get; set; } //Loại hàng

        public string VehicleType { get; set; } //Loại xe

        public string ProductGroup { get; set; } //Nhóm hàng

        public string Description { get; set; }

        public string Service { get; set; } //Dịch vụ

        public decimal Price { get; set; } //Giá dịch vụ

        public decimal SubPrice { get; set; } //Phụ thu

        public string GroupId { get; set; } //Tổ bốc xếp

        public DateTime CreatedDate { get; set; } //Ngày tạo

        public DateTime ModifiedDate { get; set; } //Ngày cập nhật

        public DateTime StartDate { get; set; } //Ngày bắt đầu công việc
        public DateTime EndDate { get; set; } //Ngày kết thúc công việc
        public DateTime DivisionDate { get; set; } //Ngày phân tổ
        public string ImageVN { get; set; } //ảnh xe Việt Nam
        public string ImageCN { get; set; } //ảnh xe Trung Quốc
        public string ServiceCode { get; set; } //Số trang (mã dịch vụ)
        public string ParkingPosition { get; set; }//Vị trí đỗ
        public bool IsDeleted { get; set; } //true là xóa
        public string PaymentStatus { get; set; } // 0: Chưa trả tiền, 1: Đã trả tiền.
        public decimal Cost { get; set; }//Giá trả cho công nhân
        public string PackageNumber { get; set; }//Số kiện
        public int Quantity { get; set; }//Số lượng
    }

    public class tbl_Event_POST
    {
        public string ID { get; set; } //id sự kiện
        public string Service { get; set; } //Dịch vụ
        public string Code { get; set; } //Mã đăng ký
        public int EventType { get; set; } //Trạng thái
        public string PlateVN { get; set; } //Biển số xe Việt Nam
        public string ImageVN { get; set; } //ảnh xe Việt Nam
        public string PlateCN { get; set; } //Biển số xe Trung Quốc
        public string ImageCN { get; set; } //ảnh xe Trung Quốc
        public string ProductType { get; set; } //Loại hàng
        public int Weight { get; set; } //Khối lượng hàng    
        public string VehicleType { get; set; } //Loại xe
        public string ProductGroup { get; set; } //Nhóm hàng
        public decimal Price { get; set; } //Giá dịch vụ
        public decimal SubPrice { get; set; } //Phụ thu
        public string Description { get; set; }
        public string ServiceCode { get; set; } //Số trang (mã dịch vụ)
        public string ParkingPosition { get; set; }//Vị trí đỗ
        public string PaymentStatus { get; set; } // 0: Chưa trả tiền, 1: Đã trả tiền.
        public decimal Cost { get; set; }//Giá trả cho công nhân
    }
}
