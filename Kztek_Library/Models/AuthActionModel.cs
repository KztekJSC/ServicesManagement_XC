namespace Kztek_Library.Models
{
    public class AuthActionModel
    {
        public int Create_Auth { get; set; } = 0; // 0 - No, 1 - Yes

        public int Update_Auth { get; set; } = 0; // 0 - No, 1 - Yes

        public int Delete_Auth { get; set; } = 0; // 0 - No, 1 - Yes

        public int Remove_Auth { get; set; } = 0; // 0 - No, 1 - Yes //xóa không điều kiện
        public int LockCard_Auth { get; set; } = 0;
        public int OpenCard_Auth { get; set; } = 0;
        public int DeleteCard_Auth { get; set; } = 0;
    }
}