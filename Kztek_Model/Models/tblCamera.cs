using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("tbl_Camera")]
    public class tblCamera
    {
        [Key]
        public Guid id { get; set; }

        public string camera_Name { get; set; }

        public string camera_Code { get; set; }

        public string video_Source { get; set; }

        public int http_Port { get; set; }

        public int server_Port { get; set; }

        public int chanel { get; set; }

        public string auth_Login { get; set; }

        public string auth_Password { get; set; }

        public string camera_Type { get; set; }

        public string stream_Type { get; set; }

        public string resolution { get; set; }

        public string using_Regions { get; set; }

        public string SDK { get; set; }

        public string description { get; set; }

     

    }

    public class tblCamera_View
    {
        
        public string CameraID { get; set; }

        public string CameraCode { get; set; }

        public string CameraName { get; set; }

        public string HttpURL { get; set; }

        public string HttpPort { get; set; }

        public string RtspPort { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Nullable<int> FrameRate { get; set; }

        public string Resolution { get; set; }

        public Nullable<int> Channel { get; set; }

        public string CameraType { get; set; }

        public string StreamType { get; set; }

        public string SDK { get; set; }

        public string Cgi { get; set; }

        public bool EnableRecording { get; set; }

        public string PCID { get; set; }

        public Nullable<int> PositionIndex { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public bool IsFaceRecognize { get; set; }
        public string MotionZone { get; set; }
        public string Config { get; set; }
    }

    public class tblCamera_POST
    {

        public string id { get; set; }

        public string camera_Name { get; set; }

        public string camera_Code { get; set; }

        public string video_Source { get; set; }

        public int http_Port { get; set; }

        public int server_Port { get; set; }

        public int chanel { get; set; }

        public string auth_Login { get; set; }

        public string auth_Password { get; set; }

        public string camera_Type { get; set; }

        public string stream_Type { get; set; }

        public string resolution { get; set; }

        public string using_Regions { get; set; }

        public string SDK { get; set; }

        public string description { get; set; }
    }

    public class tblCamera_PUT
    {
        public string Id { get; set; }

        public string CameraCode { get; set; }

        public string CameraName { get; set; }

        public string HttpURL { get; set; }

        public string HttpPort { get; set; }

        public string RtspPort { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Nullable<int> FrameRate { get; set; }

        public string Resolution { get; set; }

        public Nullable<int> Channel { get; set; }

        public string CameraType { get; set; }

        public string StreamType { get; set; }

        public string SDK { get; set; }

        public string Cgi { get; set; }

        public bool EnableRecording { get; set; }

        public string PCID { get; set; }

        public Nullable<int> PositionIndex { get; set; }

        public bool Inactive { get; set; }
        public string MotionZone { get; set; }
        public string Config { get; set; }

    }
}
