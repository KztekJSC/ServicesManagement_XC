using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kztek_Model.Models
{
    [Table("tblSystemConfig")]
    public class tblSystemConfig
    {
        [Key]
        public string SystemConfigID { get; set; }

        public string Company { get; set; }

        public string Address { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string FeeName { get; set; }

        public bool EnableDeleteCardFailed { get; set; }

        public string SystemCode { get; set; }

        public string KeyA { get; set; }

        public string KeyB { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public bool EnableSoundAlarm { get; set; }

        public string Logo { get; set; }

        public bool EnableAlarmMessageBox { get; set; }

        public bool EnableAlarmMessageBoxIn { get; set; }

        public string Tax { get; set; }

        public int DelayTime { get; set; }

        public string Para1 { get; set; }

        public string Para2 { get; set; }
        public bool isAuthInView { get; set; }
        public bool IsAutoCapture { get; set; }
        public string ImagePath { get; set; }
    }

    public class tblSystemConfig_POST
    {
        public string Company { get; set; } = "";

        public string Address { get; set; } = "";

        public string Tel { get; set; } = "";

        public string Fax { get; set; } = "";

        public string FeeName { get; set; } = "";

        public bool EnableDeleteCardFailed { get; set; } = false;

        public string SystemCode { get; set; } = "";

        public string KeyA { get; set; } = "";

        public string KeyB { get; set; } = "";

        public bool EnableSoundAlarm { get; set; } = false;

        public string Logo { get; set; } = "";

        public bool EnableAlarmMessageBox { get; set; } = false;

        public bool EnableAlarmMessageBoxIn { get; set; } = false;

        public string Tax { get; set; } = "";

        public int DelayTime { get; set; } = 0;

        public string Para1 { get; set; } = "";

        public string Para2 { get; set; } = "";

        public bool isAuthInView { get; set; } = false;
    }

    public class tblSystemConfig_PUT
    {
        public string Id { get; set; } = "";

        public string Company { get; set; } = "";

        public string Address { get; set; } = "";

        public string Tel { get; set; } = "";

        public string Fax { get; set; } = "";

        public string FeeName { get; set; } = "";

        public bool EnableDeleteCardFailed { get; set; } = false;

        public string SystemCode { get; set; } = "";

        public string KeyA { get; set; } = "";

        public string KeyB { get; set; } = "";

        public bool EnableSoundAlarm { get; set; } = false;

        public string Logo { get; set; } = "";

        public bool EnableAlarmMessageBox { get; set; } = false;

        public bool EnableAlarmMessageBoxIn { get; set; } = false;

        public string Tax { get; set; } = "";

        public int DelayTime { get; set; } = 0;

        public string Para1 { get; set; } = "";

        public string Para2 { get; set; } = "";

        public bool isAuthInView { get; set; } = false;
    }
}
