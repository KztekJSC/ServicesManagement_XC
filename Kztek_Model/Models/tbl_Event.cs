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

        public string Code { get; set; } //Mã đăng ký. Tạm thời không dùng

        public int EventType { get; set; } //Trạng thái công việc. 1 - Chờ xác nhận, 2 - Đã xác nhận/Chưa phân tổ, 3 - Đã phân tổ, 4 - Đang thực hiện, 5 - Chờ duyệt, 6 - Hoàn thành

        #region Xe VN
        public string PlateVN { get; set; } //Biển số xe Việt Nam

        public int VehicleStatusVN { get; set; } //Trạng thái xe VN. 0 - chưa vào, 1 - xe vào, 2 - xe ra

        public string ImageVN { get; set; } //ảnh xe Việt Nam

        public DateTime TimeInVN { get; set; } //Thời gian vào của xe VN
        public DateTime TimeOutVN { get; set; } //Thời gian ra của xe VN
        #endregion

        #region Xe CN
        public string PlateCN { get; set; } //Biển số xe Trung Quốc

        public int VehicleStatusCN { get; set; } //Trạng thái xe Trung Quốc. 0 - chưa vào, 1 - xe vào, 2 - xe ra

        public string ImageCN { get; set; } //Ảnh xe Trung Quốc

        public DateTime TimeInCN { get; set; } //Thời gian vào của xe Trung Quốc

        public DateTime TimeOutCN { get; set; } //Thời gian ra của xe Trung Quốc
        #endregion


        public decimal Weight { get; set; } //Khối lượng hàng

        public string ProductType { get; set; } //Loại hàng

        public string VehicleType { get; set; } //Loại xe

        public string ProductGroup { get; set; } //Nhóm hàng

        public string Description { get; set; } // ghi chú phát sinh

        public string Service { get; set; } //Dịch vụ id

        public decimal Price { get; set; } //Giá dịch vụ

        public decimal SubPrice { get; set; } //Phụ thu

        public string GroupId { get; set; } //Tổ bốc xếp    

        public string ServiceCode { get; set; } //Số trang (mã dịch vụ)

        public string ParkingPosition { get; set; }//Vị trí đỗ

        public bool IsDeleted { get; set; } //true là xóa

        public string PaymentStatus { get; set; } // 0: Chưa trả tiền, 1: Đã trả tiền.

        public decimal Cost { get; set; }//Chi phí cho công nhân

        public int PackageNumber { get; set; }//Số kiện

        public int Quantity { get; set; }//Số lượng

        public DateTime CreatedDate { get; set; } //Ngày tạo

        public DateTime ModifiedDate { get; set; } //Ngày cập nhật

        public DateTime StartDate { get; set; } //Ngày bắt đầu công việc

        public DateTime EndDate { get; set; } //Ngày kết thúc công việc

        public DateTime DivisionDate { get; set; } //Ngày phân tổ

        public DateTime ConfirmDate { get; set; } //Ngày xác nhận

        public string BB_Table { get; set; } //Tên bảng dịch vụ của Biển Bạc
        public string BB_Id { get; set; } //Id dịch vụ của Biển Bạc
    }

    public class tbl_Event_POST
    {
        public string service { get; set; } //Dịch vụ
        public string code { get; set; } //Mã đăng ký
        public string plateVN { get; set; } //Biển số xe Việt Nam
        public string imageVN { get; set; } //ảnh xe Việt Nam
        public string timeInVN { get; set; } //Thời gian vào của xe VN
        public string plateCN { get; set; } //Biển số xe Trung Quốc
        public string imageCN { get; set; } //ảnh xe Trung Quốc
        public string timeInCN { get; set; } //Thời gian vào của xe CN
        public string productType { get; set; } //Loại hàng
        public decimal weight { get; set; } //Khối lượng hàng    
        public string vehicleType { get; set; } //Loại xe
        public string productGroup { get; set; } //Nhóm hàng
        public decimal price { get; set; } //Giá dịch vụ
        public decimal subPrice { get; set; } //Phụ thu
        public string description { get; set; }
        public string serviceCode { get; set; } //Số trang (mã dịch vụ)
        public string paymentStatus { get; set; } // 0: Chưa trả tiền, 1: Đã trả tiền.
        public string bb_Table { get; set; } //Tên bảng dịch vụ của Biển Bạc
        public string bb_Id { get; set; } //Id dịch vụ của Biển Bạc

    }
    public class tbl_Event_Cus
    {
        public string Id { get; set; }

    
        public string service { get; set; } //Dịch vụ
        public string code { get; set; } //Mã đăng ký
        public string plateVN { get; set; } //Biển số xe Việt Nam
        public string imageVN { get; set; } //ảnh xe Việt Nam
        public string timeInVN { get; set; } //Thời gian vào của xe VN
        public string plateCN { get; set; } //Biển số xe Trung Quốc
        public string imageCN { get; set; } //ảnh xe Trung Quốc
        public string timeInCN { get; set; } //Thời gian vào của xe CN
        public string productType { get; set; } //Loại hàng
        public string weight { get; set; } //Khối lượng hàng    
        public string vehicleType { get; set; } //Loại xe
        public string productGroup { get; set; } //Nhóm hàng
        public string price { get; set; } //Giá dịch vụ
        public string subPrice { get; set; } //Phụ thu
        public string description { get; set; }
        public string serviceCode { get; set; } //Số trang (mã dịch vụ)
        public string paymentStatus { get; set; } // 0: Chưa trả tiền, 1: Đã trả tiền.
        public string bb_Table { get; set; } //Tên bảng dịch vụ của Biển Bạc
        public string bb_Id { get; set; } //Id dịch vụ của Biển Bạc
        public string GroupId { get; set; } //Tổ bốc xếp 
        public string serviceName { get; set; } //Dịch vụ
    }
    public class API_VehicleStatus
    {
        public string plate { get; set; } //Biển số
        public string type { get; set; } //VN hoặc CN
        public string time { get; set; } //Thời gian
        public string vehicleType { get; set; } //Loại xe
        public string image { get; set; }
    }

    public class Model_ServiceByGroup
    {
        public string GroupId { get; set; } //Biển số
        public int Number { get; set; } //VN hoặc CN
     
    }

    public class CountEventByType
    {
        public int EventType { get; set; } //trạng thái sự kiện
        public int Number { get; set; } //số sự kiện
        public string Icon { get; set; }
        public string ColorClass { get; set; }
        public string Title { get; set; }
    }
}
