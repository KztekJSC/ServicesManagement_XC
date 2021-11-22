using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("tbl_Lane")]
    public class tblLane
    {
        [Key]
        public Guid id { get; set; }

        public string lane_Name { get; set; }

        public string lane_Code { get; set; }

        public int direction { get; set; }

        public string vehicle_Types { get; set; }

        public string card_Types { get; set; }

        public bool auto_Mode { get; set; }

        public string camera_LPR_1 { get; set; }

        public string camera_LPR_2 { get; set; }

        public string camera_Panorama_1 { get; set; }

        public string camera_Panorama_2 { get; set; }

        public bool reversal { get; set; }

        public string description { get; set; }

       
    }

    public class tblLane_Submit
    {
        public string id { get; set; }

        public string lane_Name { get; set; }

        public string lane_Code { get; set; }

        public int direction { get; set; }

        public string vehicle_Types { get; set; }

        public string card_Types { get; set; }

        public bool auto_Mode { get; set; }

        public string camera_LPR_1_id { get; set; }

        public string camera_LPR_2_id { get; set; }

        public string camera_Panorama_1_id { get; set; }

        public string camera_Panorama_2_id { get; set; }

        public bool reversal { get; set; }

        public string description { get; set; }

    }

 
}
