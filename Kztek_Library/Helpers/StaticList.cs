using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Kztek_Library.Configs;
using Kztek_Library.Models;

namespace Kztek_Library.Helpers
{
    public class StaticList
    {
        /// <summary>
        /// Danh sách loại menu
        /// </summary>
        /// <returns>List<SelectListModel></returns>
        public static List<SelectListModel> MenuType()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "1", ItemText = "Menu"},
                                        new SelectListModel { ItemValue = "2", ItemText = "Function"}
                                    };
            return list;
        }

        public static List<SelectListModel> ListStatus()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = "Xe mới vào cổng"},
                                        new SelectListModel { ItemValue = "1", ItemText = "Xe đã làm thủ tục giấy tờ"},
                                        new SelectListModel { ItemValue = "2", ItemText = "Xe đã xuất hàng"}
                                    };
            return list;
        }

        public static List<SelectListModel_MenuGroup> GroupMenuList()
        {
            var list = new List<SelectListModel_MenuGroup> {
                                         new SelectListModel_MenuGroup { ItemValue = "12878956", ItemText = "Đếm xe", ItemIndex = 3, ItemCode = "PK_", Icon = "/Content/Image/sy-parking-icon.png", Color = "infobox-blue", AreaName = "Admin", Layout = "~/Areas/Admin/Views/Shared/_Layout.cshtml", Label = "label-success"}
                                    };

            return list;
        }

        public static List<SelectListModel> VehicleType()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = "Ô tô"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Xe máy"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Xe đạp"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Xe đạp điện"}
                                    };
            return list;
        }

        public static async Task<List<SelectListModel>> FormulationList()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:TURN") },
                                         new SelectListModel { ItemValue = "1", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:BLOCK")},
                                         new SelectListModel { ItemValue = "2", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:PERIODTIME")}
                                    };
            return list;
        }

        public static async Task<List<SelectListModel>> GetListLed_Function()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = "Dùng cho cổng"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Dùng cho Phòng Giao dịch"},
                                         new SelectListModel { ItemValue = "2", ItemText ="Dùng cho cửa kho"}
                                    };
            return await Task.FromResult( list);
        }

        public static async Task<List<SelectListModel>> GetCardType()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "0", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:MONTHLYTICKET")},
                                         new SelectListModel { ItemValue = "1", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:TICKETEACHTIME")},
                                         new SelectListModel { ItemValue = "2", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:FREETICKET")}
                                    };
            return list;

        }


        public static async Task<List<SelectListModel>> CardStatus()
        {

            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                                         new SelectListModel { ItemValue = "0", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:CARDACTIVE")},
                                         new SelectListModel { ItemValue = "1", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:CARDINACTIVE")},
                                    };
            return list;
        }

        public static List<SelectListModel_Communication> Communication1()
        {
            return new List<SelectListModel_Communication>
            {
                new SelectListModel_Communication {ItemValue = 1, ItemText = "TCP/IP"},
                new SelectListModel_Communication {ItemValue = 0, ItemText = "RS232/485/422"}
            };
        }

        public static List<SelectListModel> LineTypes2()
        {
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "", ItemText = "-- Chọn loại"},
                new  SelectListModel{ ItemValue = "0", ItemText = "IDTECK"},
                new  SelectListModel{ ItemValue = "1", ItemText = "Honeywell SY-MSA30/60L"},
                new  SelectListModel{ ItemValue = "2", ItemText = "Honeywell Nstar"},
                new  SelectListModel{ ItemValue = "3", ItemText = "Pegasus PP-3760"},
                new  SelectListModel{ ItemValue = "4", ItemText = "Pegasus PP-6750"},
                new  SelectListModel{ ItemValue = "5", ItemText = "Pegasus PFP-3700"},
                new  SelectListModel{ ItemValue = "6", ItemText = "FINGERTEC"},
                new  SelectListModel{ ItemValue = "7", ItemText = "DS3000"},
                new  SelectListModel{ ItemValue = "8", ItemText = "CS3000"},
                new  SelectListModel{ ItemValue = "9", ItemText = "RCP4000"},
                new  SelectListModel{ ItemValue = "10", ItemText = "PEGASUS PB7/PT3"},
                new  SelectListModel{ ItemValue = "11", ItemText = "PEGASUS PB5"},
                new  SelectListModel{ ItemValue = "12", ItemText = "IDTECK (006)"},
                new  SelectListModel{ ItemValue = "13", ItemText = "IDTECK (iTDC)"},
                new  SelectListModel{ ItemValue = "14", ItemText = "IDTECK (iMDC)"},
                new  SelectListModel{ ItemValue = "15", ItemText = "IDTECK (Elevator384)"},
                new  SelectListModel{ ItemValue = "16", ItemText = "Promax - FAT810W Kanteen"},
                new  SelectListModel{ ItemValue = "17", ItemText = "Promax - AC908"},
                new  SelectListModel{ ItemValue = "18", ItemText = "HAEIN S&amp;S"},
                new  SelectListModel{ ItemValue = "19", ItemText = "Promax - PCR310U"},
                new  SelectListModel{ ItemValue = "20", ItemText = "NetPOS Client MDB"},
                new  SelectListModel{ ItemValue = "21", ItemText = "NetPOS Client SERVER"},
                new  SelectListModel{ ItemValue = "22", ItemText = "Promax - FAT810W Parking"},
                new  SelectListModel{ ItemValue = "23", ItemText = "Promax - FAT810W Vending Machine"},
                new  SelectListModel{ ItemValue = "24", ItemText = "Pegasus - PP-110/PP-5210/PUA-310"},
                new  SelectListModel{ ItemValue = "25", ItemText = "Futech SC100"},
                new  SelectListModel{ ItemValue = "26", ItemText = "Honeywell HSR900"},
                new  SelectListModel{ ItemValue = "27", ItemText = "AC9xxPCR"},
                new  SelectListModel{ ItemValue = "28", ItemText = "E02.NET"},
                new  SelectListModel{ ItemValue = "29", ItemText = "Futech SC101"},
                new  SelectListModel{ ItemValue = "30", ItemText = "Futech SC100FPT"},
                new  SelectListModel{ ItemValue = "31", ItemText = "Futech SC100LANCASTER"},
                new  SelectListModel{ ItemValue = "32", ItemText = "Futech FUCM100"},
                new  SelectListModel{ ItemValue = "33", ItemText = "IDTECK 8 Number"},
                new  SelectListModel{ ItemValue = "34", ItemText = "E01 RS485"},
                new  SelectListModel{ ItemValue = "35", ItemText = "E02.NET Card Int"},
                new  SelectListModel{ ItemValue = "36", ItemText = "FUPC100"},
                new  SelectListModel{ ItemValue = "37", ItemText = "E02.NET Mifare"},
                new  SelectListModel{ ItemValue = "38", ItemText = "SOYAL"},
                new  SelectListModel{ ItemValue = "39", ItemText = "E02.NET Mifare SR30"},
                new  SelectListModel{ ItemValue = "40", ItemText = "Ingressus"},
                new  SelectListModel{ ItemValue = "41", ItemText = "E01 RS485 V2"},
                new  SelectListModel{ ItemValue = "42", ItemText = "Ingressus Mifare"},
                new  SelectListModel{ ItemValue = "43", ItemText = "FAT810WDispenser"},
                 new  SelectListModel{ ItemValue = "44", ItemText = "FUCMHID100"},
                new  SelectListModel{ ItemValue = "45", ItemText = "USB Mifare"},
                new  SelectListModel{ ItemValue = "46", ItemText = "USB Proximity"},

                new  SelectListModel{ ItemValue = "47", ItemText = "IDTECKSR30"},
                new  SelectListModel{ ItemValue = "48", ItemText = "E02QRCode"},
                new  SelectListModel{ ItemValue = "49", ItemText = "E04.NET"},
                new  SelectListModel{ ItemValue = "50", ItemText = "E04.NET Mifare"},
                new  SelectListModel{ ItemValue = "51", ItemText = "E05.NET"},
                new  SelectListModel{ ItemValue = "52", ItemText = "KZ-MFC01.NET"},
                new  SelectListModel{ ItemValue = "53", ItemText = "E02_FPT"},
                new  SelectListModel{ ItemValue = "54", ItemText = "E05.NET Mifare"},
                new  SelectListModel{ ItemValue = "55", ItemText = "IDTECK Mifare"},
                new  SelectListModel{ ItemValue = "56", ItemText = "FaceMQTT"},
                new  SelectListModel{ ItemValue = "57", ItemText = "E02Mifare_BTNMT"},
                new  SelectListModel{ ItemValue = "58", ItemText = "FaceMQTT_V2"},
                new  SelectListModel{ ItemValue = "59", ItemText = "E02_FirstSeri10"},
                new  SelectListModel{ ItemValue = "60", ItemText = "Ingress_FirstSeri10"}
            };
        }

        public static List<SelectListModel> TypeEventLocker()
        {
            var list = new List<SelectListModel> {
                                         //new SelectListModel { ItemValue = "t", ItemText = "--Lựa chọn--"},
                                         new  SelectListModel{ ItemValue = "1", ItemText = "Nạp cố định"},
                                         new  SelectListModel{ ItemValue = "2", ItemText = "Thẻ tức thời"},
                                         new  SelectListModel{ ItemValue = "3", ItemText = "Nhận dạng khuôn mặt"}

                                    };
            return list;
        }

        public static List<SelectListModel> ActionLockerProcess()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "t", ItemText = "--Lựa chọn--"},
                                         new SelectListModel { ItemValue = "0", ItemText = "Hủy" },
                                         new SelectListModel { ItemValue = "1", ItemText = "Nạp"},
                                         new SelectListModel { ItemValue = "2", ItemText = "Mở tủ thủ công"},
                                    };
            return list;
        }

        public static List<SelectListModel> TypeLockerProcess()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "t", ItemText = "--Lựa chọn--"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Cố định" },
                                         new SelectListModel { ItemValue = "2", ItemText = "Tức thời"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Nhận dạng"},
                                         new SelectListModel { ItemValue = "4", ItemText = "Mở tủ thủ công"},
                                    };
            return list;
        }

        public static List<SelectListModel> LockerEventCode()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "", ItemText = "--Lựa chọn--"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Check in" },
                                         new SelectListModel { ItemValue = "2", ItemText = "Check out"},
                                    };
            return list;
        }

        public static List<SelectListModel> LockerAlarmCode()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "", ItemText = "--Lựa chọn--"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Thẻ không tồn tại" },
                                         new SelectListModel { ItemValue = "2", ItemText = "Thẻ chưa đăng ký"},
                                         new SelectListModel { ItemValue = "3", ItemText = "Chưa gửi đồ"},
                                    };
            return list;
        }

        public static List<SelectListModel> TypeFee()
        {
            var list = new List<SelectListModel> {

                                         new SelectListModel { ItemValue = "1", ItemText = "Áp dụng từng thẻ" },
                                        new SelectListModel { ItemValue = "2", ItemText = "Tổng giá trị hóa đơn" },
                                         //new SelectListModel { ItemValue = "3", ItemText = "Áp dụng theo biểu phí"}
                                    };
            return list;
        }

        //public static List<SelectListModel> CustomerStatusType()
        //{
        //    var list = new List<SelectListModel> {
        //                                new SelectListModel { ItemValue = "0", ItemText = "Kích hoạt"},
        //                                 new SelectListModel { ItemValue = "1", ItemText = "Chưa kích hoạt"}
        //                            };
        //    return list;
        //}
        public static async Task<List<SelectListModel>> CustomerStatusType()
        {

            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                                         new SelectListModel { ItemValue = "0", ItemText = await LanguageHelper.GetLanguageText("BODY:TABLE:ACTIVE")},
                                         new SelectListModel { ItemValue = "1", ItemText = await LanguageHelper.GetLanguageText("BODY:TABLE:INACTIVE")},
                                    };
            return list;
        }
        public static async Task<List<SelectListModel>> AlarmCodes()
        {
            var list = new List<SelectListModel> {
                      // dự án hòa phát mandarin garden 15/7/2019 dungdt
                      //Sự kiện cảnh báo chưa có trường thông tin: "Không tồn tại trên hệ thống" => đổi tên 001 unknownCard <-> Not_exist_on_the_system
                                         new SelectListModel { ItemValue = "001", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Not_exist_on_the_system") },
                                         new SelectListModel { ItemValue = "002", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Lock")},
                                         new SelectListModel { ItemValue = "003", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Permissions")},
                                         new SelectListModel { ItemValue = "004", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:InParking")},
                                         new SelectListModel { ItemValue = "005", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:notInParking")},
                                         new SelectListModel { ItemValue = "006", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:OpenBarrieByComputer")},
                                         new SelectListModel { ItemValue = "007", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:OpenBarrieByButton")},
                                         new SelectListModel { ItemValue = "008", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:EventEscapeIn")},
                                         new SelectListModel { ItemValue = "009", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:eventEscapeOut")},

                                         new SelectListModel { ItemValue = "010", ItemText = await LanguageHelper.GetLanguageText("BODY:SEARCH:ExpiredCard")},

                                         new SelectListModel { ItemValue = "011", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Invalid_license_plates")},
                                         new SelectListModel { ItemValue = "012", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Black_list")},
                                         new SelectListModel { ItemValue = "013", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Lock_License_plate")},
                                         new SelectListModel { ItemValue = "014", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:License_plate_expired")},
                                         new SelectListModel { ItemValue = "015", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Locked_tag_group")},
                                         new SelectListModel { ItemValue = "016", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:License_plate_do_not_match")},
                                         new SelectListModel { ItemValue = "017", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Vehicles_beyond_the_magnetic_ring")},
                                    };
            return list;
        }

        public static async Task<List<SelectListModel>> Action()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                                         new  SelectListModel{ ItemValue = "ADD", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:ADD")},
                                         new SelectListModel { ItemValue = "RELEASE", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:RELEASE")},
                                         new  SelectListModel{ ItemValue = "RETURN", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:RETURN")},
                                         new SelectListModel { ItemValue = "CHANGE", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:CHANGE")},
                                         new  SelectListModel{ ItemValue = "DELETE", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DELETE")},
                                         new  SelectListModel{ ItemValue = "LOCK", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:LOCK")},
                                         new SelectListModel { ItemValue = "UNLOCK", ItemText =await LanguageHelper.GetLanguageText("STATICLIST:UNLOCK")},
                                         new  SelectListModel{ ItemValue = "ACTIVE", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:ACTIVE")}

                                    };
            return list;
        }
        public static async Task<List<SelectListModel>> ActionLog()
        {
            var list = new List<SelectListModel>();

            list.Add(new SelectListModel { ItemText = ActionConfig.Create, ItemValue = ActionConfig.Create });
            list.Add(new SelectListModel { ItemText = ActionConfig.Update, ItemValue = ActionConfig.Update });
            list.Add(new SelectListModel { ItemText = ActionConfig.Delete, ItemValue = ActionConfig.Delete });
            list.Add(new SelectListModel { ItemText = ActionConfig.Login, ItemValue = ActionConfig.Login });
           




            return await Task.FromResult(list);
        }

        public static async Task<List<SelectListModel>> TimePeriodType()
        {
            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Month") },
                                         new SelectListModel { ItemValue = "1", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Quarter") },
                                         new SelectListModel { ItemValue = "2", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Year") },
                                         new SelectListModel { ItemValue = "3", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Day") }
                                    };
            return list;
        }
        public static async Task<List<SelectListModel>> CameraTypes1()
        {

            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                                         new  SelectListModel{ ItemValue = "Geovision", ItemText = "Geovision"},
                                         new  SelectListModel{ ItemValue = "Panasonic i-Pro", ItemText = "Panasonic i-Pro"},
                                         new  SelectListModel{ ItemValue = "Axis", ItemText = "Axis"},
                                         new  SelectListModel{ ItemValue = "Secus", ItemText = "Secus"},
                                         new  SelectListModel{ ItemValue = "Shany-Stream1", ItemText = "Shany-Stream1"},
                                         new  SelectListModel{ ItemValue = "Shany-Stream21", ItemText = "Shany-Stream2"},
                                         new  SelectListModel{ ItemValue = "Vivotek", ItemText = "Vivotek"},
                                         new  SelectListModel{ ItemValue = "Lilin", ItemText = "Lilin"},
                                         new  SelectListModel{ ItemValue = "Messoa", ItemText = "Messoa"},
                                         new  SelectListModel{ ItemValue = "Entrovision", ItemText = "Entrovision"},
                                         new  SelectListModel{ ItemValue = "Sony", ItemText = "Sony"},
                                         new  SelectListModel{ ItemValue = "Bosch", ItemText = "Bosch"},
                                         new  SelectListModel{ ItemValue = "Vantech", ItemText = "Vantech"},
                                         new  SelectListModel{ ItemValue = "SC330", ItemText = "SC330"},
                                         new  SelectListModel{ ItemValue = "SecusFFMPEG", ItemText = "SecusFFMPEG"},
                                         new  SelectListModel{ ItemValue = "CNB", ItemText = "CNB"},//"CNB", "HIK", "Enster", "Afidus", "Dahua", "ITX"
                                         new  SelectListModel{ ItemValue = "HIK", ItemText = "HIK"},
                                         new  SelectListModel{ ItemValue = "Enster", ItemText = "Enster"},
                                         new  SelectListModel{ ItemValue = "Afidus", ItemText = "Afidus"},
                                         new  SelectListModel{ ItemValue = "Dahua", ItemText = "Dahua"},
                                         new  SelectListModel{ ItemValue = "ITX", ItemText = "ITX"},
                                         new  SelectListModel{ ItemValue = "Hanse", ItemText = "Hanse"},
                                          new  SelectListModel{ ItemValue = "Samsung", ItemText = "Samsung"}
                                    };
        }

        public static async Task<List<SelectListModel>> StreamTypes1()
        {

            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "", ItemText =  await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                                         new  SelectListModel{ ItemValue = "MJPEG", ItemText = "MJPEG"},
                                         new  SelectListModel{ ItemValue = "PlayFile", ItemText = "PlayFile"},
                                         new  SelectListModel{ ItemValue = "Local Video Capture Device", ItemText = "Local Video Capture Device"},
                                         new  SelectListModel{ ItemValue = "JPEG", ItemText = "JPEG"},
                                         new  SelectListModel{ ItemValue = "MPEG4", ItemText = "MPEG4"},
                                         new  SelectListModel{ ItemValue = "H264", ItemText = "H264"},
                                         new  SelectListModel{ ItemValue = "Onvif", ItemText = "Onvif"}
                                    };
        }

        public static async Task<List<SelectListModel>> SDKs1()
        {
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                new  SelectListModel{ ItemValue = "DahuaSDK", ItemText = "DahuaSDK"},
                new  SelectListModel{ ItemValue = "EmguSDK", ItemText = "EmguSDK"},
                 new  SelectListModel{ ItemValue = "KztekSDK2", ItemText = "KztekSDK2"}
            };
        }
        //dung cho iparking
        public static async Task<List<SelectListModel>> LineTypes1()
        {
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                new  SelectListModel{ ItemValue = "0", ItemText = "IDTECK"},
                new  SelectListModel{ ItemValue = "1", ItemText = "Honeywell SY-MSA30/60L"},
                new  SelectListModel{ ItemValue = "2", ItemText = "Honeywell Nstar"},
                new  SelectListModel{ ItemValue = "3", ItemText = "Pegasus PP-3760"},
                new  SelectListModel{ ItemValue = "4", ItemText = "Pegasus PP-6750"},
                new  SelectListModel{ ItemValue = "5", ItemText = "Pegasus PFP-3700"},
                new  SelectListModel{ ItemValue = "6", ItemText = "FINGERTEC"},
                new  SelectListModel{ ItemValue = "7", ItemText = "DS3000"},
                new  SelectListModel{ ItemValue = "8", ItemText = "CS3000"},
                new  SelectListModel{ ItemValue = "9", ItemText = "RCP4000"},
                new  SelectListModel{ ItemValue = "10", ItemText = "PEGASUS PB7/PT3"},
                new  SelectListModel{ ItemValue = "11", ItemText = "PEGASUS PB5"},
                new  SelectListModel{ ItemValue = "12", ItemText = "IDTECK (006)"},
                new  SelectListModel{ ItemValue = "13", ItemText = "IDTECK (iTDC)"},
                new  SelectListModel{ ItemValue = "14", ItemText = "IDTECK (iMDC)"},
                new  SelectListModel{ ItemValue = "15", ItemText = "IDTECK (Elevator384)"},
                new  SelectListModel{ ItemValue = "16", ItemText = "Promax - FAT810W Kanteen"},
                new  SelectListModel{ ItemValue = "17", ItemText = "Promax - AC908"},
                new  SelectListModel{ ItemValue = "18", ItemText = "HAEIN S&amp;S"},
                new  SelectListModel{ ItemValue = "19", ItemText = "Promax - PCR310U"},
                new  SelectListModel{ ItemValue = "20", ItemText = "NetPOS Client MDB"},
                new  SelectListModel{ ItemValue = "21", ItemText = "NetPOS Client SERVER"},
                new  SelectListModel{ ItemValue = "22", ItemText = "Promax - FAT810W Parking"},
                new  SelectListModel{ ItemValue = "23", ItemText = "Promax - FAT810W Vending Machine"},
                new  SelectListModel{ ItemValue = "24", ItemText = "Pegasus - PP-110/PP-5210/PUA-310"},
                new  SelectListModel{ ItemValue = "25", ItemText = "Futech SC100"},
                new  SelectListModel{ ItemValue = "26", ItemText = "Honeywell HSR900"},
                new  SelectListModel{ ItemValue = "27", ItemText = "AC9xxPCR"},
                new  SelectListModel{ ItemValue = "28", ItemText = "E02.NET"},
                new  SelectListModel{ ItemValue = "29", ItemText = "Futech SC101"},
                new  SelectListModel{ ItemValue = "30", ItemText = "Futech SC100FPT"},
                new  SelectListModel{ ItemValue = "31", ItemText = "Futech SC100LANCASTER"},
                new  SelectListModel{ ItemValue = "32", ItemText = "Futech FUCM100"},
                new  SelectListModel{ ItemValue = "33", ItemText = "IDTECK 8 Number"},
                new  SelectListModel{ ItemValue = "34", ItemText = "E01 RS485"},
                new  SelectListModel{ ItemValue = "35", ItemText = "E02.NET Card Int"},
                new  SelectListModel{ ItemValue = "36", ItemText = "FUPC100"},
                new  SelectListModel{ ItemValue = "37", ItemText = "E02.NET Mifare"},
                new  SelectListModel{ ItemValue = "38", ItemText = "SOYAL"},
                new  SelectListModel{ ItemValue = "39", ItemText = "E02.NET Mifare SR30"},

                new  SelectListModel{ ItemValue = "40", ItemText = "Ingressus"},
                new  SelectListModel{ ItemValue = "41", ItemText = "E01 RS485 V2"},
                new  SelectListModel{ ItemValue = "42", ItemText = "Ingressus Mifare"},
                new  SelectListModel{ ItemValue = "43", ItemText = "FAT810WDispenser"},
                new  SelectListModel{ ItemValue = "44", ItemText = "FUCMHID100"},
                new  SelectListModel{ ItemValue = "45", ItemText = "USB Mifare"},
                new  SelectListModel{ ItemValue = "46", ItemText = "USB Proximity"},

                new  SelectListModel{ ItemValue = "47", ItemText = "IDTECKSR30"},
                new  SelectListModel{ ItemValue = "48", ItemText = "E02QRCode"},
                new  SelectListModel{ ItemValue = "49", ItemText = "E04.NET"},
                new  SelectListModel{ ItemValue = "50", ItemText = "E04.NET Mifare"},
                new  SelectListModel{ ItemValue = "51", ItemText = "E05.NET"},
                new  SelectListModel{ ItemValue = "52", ItemText = "KZ-MFC01.NET"},
                new  SelectListModel{ ItemValue = "53", ItemText = "E02_FPT"},
                new  SelectListModel{ ItemValue = "54", ItemText = "E05.NET Mifare"},
                new  SelectListModel{ ItemValue = "55", ItemText = "IDTECK Mifare"},
                new  SelectListModel{ ItemValue = "56", ItemText = "FaceMQTT"},
                new  SelectListModel{ ItemValue = "57", ItemText = "E02Mifare_BTNMT"},
                new  SelectListModel{ ItemValue = "58", ItemText = "FaceMQTT_V2"},
                new  SelectListModel{ ItemValue = "59", ItemText = "E02_FirstSeri10"}

            };
        }

        public static List<SelectListModel> ReaderTypes1()
        {
            return new List<SelectListModel>
            {
                new SelectListModel{ItemValue = "0", ItemText = "1"},
                new SelectListModel{ItemValue = "1", ItemText = "2"}
            };
        }
        public static async Task<List<SelectListModel>> LaneTypes1()
        {
            return new List<SelectListModel>
            {
                new  SelectListModel{ ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                new  SelectListModel{ ItemValue = "0", ItemText = "0." + await LanguageHelper.GetLanguageText("BODY:TABLE:IN")},
                new  SelectListModel{ ItemValue = "1", ItemText = "1." + await LanguageHelper.GetLanguageText("BODY:TABLE:OUT")},
                new  SelectListModel{ ItemValue = "2", ItemText = "2." + await LanguageHelper.GetLanguageText("BODY:TABLE:IN") +"-" + await LanguageHelper.GetLanguageText("BODY:TABLE:OUT")},
                new  SelectListModel{ ItemValue = "3", ItemText = "3."+ await LanguageHelper.GetLanguageText("BODY:TABLE:IN") +"-" + await LanguageHelper.GetLanguageText("BODY:TABLE:IN")},
                new  SelectListModel{ ItemValue = "4", ItemText = "4."+ await LanguageHelper.GetLanguageText("BODY:TABLE:OUT") +"-" + await LanguageHelper.GetLanguageText("BODY:TABLE:OUT")},
                new  SelectListModel{ ItemValue = "5", ItemText = "5."+ await LanguageHelper.GetLanguageText("BODY:TABLE:IN") +"-" + await LanguageHelper.GetLanguageText("BODY:TABLE:OUT") +"2"},
                 new  SelectListModel{ ItemValue = "6", ItemText = "6. #"},
            };
        }
        public static async Task<List<SelectListModel_Communication>> CheckBSType()
        {

            var list = new List<SelectListModel_Communication> {
                                        new SelectListModel_Communication { ItemValue = 1, ItemText = await LanguageHelper.GetLanguageText("STATICLIST:CheckPlateLevelOuts:4char")},
                                        new SelectListModel_Communication { ItemValue = 2, ItemText = await LanguageHelper.GetLanguageText("STATICLIST:CheckPlateLevelOuts:all")},
                                        new SelectListModel_Communication { ItemValue = 0, ItemText = await LanguageHelper.GetLanguageText("STATICLIST:CheckPlateLevelOuts:noCheck")},
                                        new SelectListModel_Communication { ItemValue = 3, ItemText = await LanguageHelper.GetLanguageText("STATICLIST:CheckPlateLevelOuts:noOpen")}
                                    };
            return list;
        }
        public static async Task<List<SelectListModel>> HubList1()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                                         new SelectListModel { ItemValue = "0", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:HubList1:Left")},
                                         new SelectListModel { ItemValue = "1", ItemText =  await LanguageHelper.GetLanguageText("STATICLIST:HubList1:Right")},
                                         new SelectListModel { ItemValue = "2", ItemText =  await LanguageHelper.GetLanguageText("STATICLIST:HubList1:All")},
                                    };
            return list;
        }
        public static async Task<List<SelectListModel>> LEDType1()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                                         new SelectListModel { ItemValue = "1", ItemText = "DSP840"},
                                         new SelectListModel { ItemValue = "2", ItemText = "FUTECH"},
                                         new SelectListModel { ItemValue = "3", ItemText = "FAT810"},
                                         new SelectListModel { ItemValue = "4", ItemText = "FUTECH2"},
                                         new SelectListModel { ItemValue = "5", ItemText = "FUTECH2LINE"},
                                         new SelectListModel { ItemValue = "6", ItemText = "PGS_LED"},
                                         new SelectListModel { ItemValue = "7", ItemText = "ATPRO"}
                                    };
            return list;
        }

        public static async Task<List<SelectListModel>> LEDType2()
        {
            var list = new List<SelectListModel> {
                                        new SelectListModel { ItemValue = "", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:DEFAULT")},
                                         new SelectListModel { ItemValue = "DSP840", ItemText = "DSP840"},
                                         new SelectListModel { ItemValue = "FUTECH", ItemText = "FUTECH"},
                                         new SelectListModel { ItemValue = "FAT810", ItemText = "FAT810"},
                                         new SelectListModel { ItemValue = "FUTECH2", ItemText = "FUTECH2"},
                                         new SelectListModel { ItemValue = "FUTECH2LINE", ItemText = "FUTECH2LINE"},
                                         new SelectListModel { ItemValue = "PGS_LED", ItemText = "PGS_LED"},
                                         new SelectListModel { ItemValue = "ATPRO", ItemText = "ATPRO"}
                                    };
            return list;
        }

        public static async Task<List<SelectListModel>> ListNoteLock()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "mat the", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:LostCard")},
                                         new  SelectListModel{ ItemValue = "hong the", ItemText =  await LanguageHelper.GetLanguageText("STATICLIST:BrokenCard")},
                                         new  SelectListModel{ ItemValue = "chua dong phi", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:NotPaidYet")},
                                         new  SelectListModel{ ItemValue = "doi the", ItemText =  await LanguageHelper.GetLanguageText("STATICLIST:ExchangeCard")}
                                    };
        }
        public static async Task<List<SelectListModel>> ListNoteUnLock()
        {
            return new List<SelectListModel> {
                                         new  SelectListModel{ ItemValue = "tim thay the", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:CardFound")},
                                         new  SelectListModel{ ItemValue = "da dong phi", ItemText = await LanguageHelper.GetLanguageText("STATICLIST:Paid")}

                                    };
        }

        public static async Task<List<SelectListModel>> ListLanguage()
        {

            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "vi", ItemText = await LanguageHelper.GetLanguageText("LANGUAGE:VNA")},
                                         new SelectListModel { ItemValue = "en", ItemText = await LanguageHelper.GetLanguageText("LANGUAGE:English")},
                                    };
            return list;
        }

        /// <summary>
        /// dùng cho nhiệt điện vũng áng đếm xe
        /// </summary>
        /// <returns></returns>
        public static async Task<List<SelectListModel>> type_vehicle()
        {

            var list = new List<SelectListModel> {
                                         new SelectListModel { ItemValue = "0", ItemText = "Xe hở"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Xe kín"},
                                    };
            return list;
        }

        public static async Task<List<SelectListModel>> Turn()
        {

            var list = new List<SelectListModel> {
                 new SelectListModel { ItemValue = "", ItemText = "- Tất cả -"},
                                         new SelectListModel { ItemValue = "0", ItemText = "Chuyến"},
                                         new SelectListModel { ItemValue = "1", ItemText = "Thường"},
                                    };
            return list;
        }
    }
}