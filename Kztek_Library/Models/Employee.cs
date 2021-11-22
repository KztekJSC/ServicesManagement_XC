using System;
using System.ComponentModel.DataAnnotations;

namespace Kztek_Library.Models
{
    public class Employee
    {
        // Constructor
        public Employee()
        {

        }

        // ID property
        private int id = 0;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        //public string code { get; set; } = ""

        // Code property
        private string code = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        // Name property
        private string name = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        // CardNumber property
        private string cardnumber = "0";
        public string CardNumber
        {
            get { return cardnumber; }
            set { cardnumber = value; }
        }

        // CardNumber1 property
        private string cardnumber1 = "0";
        public string CardNumber1
        {
            get { return cardnumber1; }
            set { cardnumber1 = value; }
        }

        // CardNumber2 property
        private string cardnumber2 = "0";
        public string CardNumber2
        {
            get { return cardnumber2; }
            set { cardnumber2 = value; }
        }

        // CardNumber3 property
        private string cardnumber3 = "0";
        public string CardNumber3
        {
            get { return cardnumber3; }
            set { cardnumber3 = value; }
        }

        // CardNumber4 property
        private string cardnumber4 = "0";
        public string CardNumber4
        {
            get { return cardnumber4; }
            set { cardnumber4 = value; }
        }

        // CardNumber5 property
        private string cardnumber5 = "0";
        public string CardNumber5
        {
            get { return cardnumber5; }
            set { cardnumber5 = value; }
        }

        // CardNumber6 property
        private string cardnumber6 = "0";
        public string CardNumber6
        {
            get { return cardnumber6; }
            set { cardnumber6 = value; }
        }

        // CardNumber7 property
        private string cardnumber7 = "0";
        public string CardNumber7
        {
            get { return cardnumber7; }
            set { cardnumber7 = value; }
        }

        // CardNumber8 property
        private string cardnumber8 = "0";
        public string CardNumber8
        {
            get { return cardnumber8; }
            set { cardnumber8 = value; }
        }

        // CardNumber9 property
        private string cardnumber9 = "0";
        public string CardNumber9
        {
            get { return cardnumber9; }
            set { cardnumber9 = value; }
        }

        // Passwords property
        private string passwords = "0000";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Passwords
        {
            get { return passwords; }
            set { passwords = value; }
        }

        // Finger property
        private string fingers1 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers1
        {
            get { return fingers1; }
            set { fingers1 = value; }
        }

        // Finger property
        private string fingers2 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers2
        {
            get { return fingers2; }
            set { fingers2 = value; }
        }

        // Finger property
        private string fingers3 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers3
        {
            get { return fingers3; }
            set { fingers3 = value; }
        }

        // Finger property
        private string fingers4 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers4
        {
            get { return fingers4; }
            set { fingers4 = value; }
        }

        // Finger property
        private string fingers5 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers5
        {
            get { return fingers5; }
            set { fingers5 = value; }
        }

        // Finger property
        private string fingers6 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers6
        {
            get { return fingers6; }
            set { fingers6 = value; }
        }

        // Finger property
        private string fingers7 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers7
        {
            get { return fingers7; }
            set { fingers7 = value; }
        }

        // Finger property
        private string fingers8 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers8
        {
            get { return fingers8; }
            set { fingers8 = value; }
        }

        // Finger property
        private string fingers9 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers9
        {
            get { return fingers9; }
            set { fingers9 = value; }
        }

        // Finger property
        private string fingers10 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Fingers10
        {
            get { return fingers10; }
            set { fingers10 = value; }
        }

        // 
        private int verifyTypeID = 4;
        public int VerifyTypeID
        {
            get { return verifyTypeID; }
            set { verifyTypeID = value; }
        }

        private int titleID = 0;
        public int TitleID
        {
            get { return titleID; }
            set { titleID = value; }
        }

        // DepartmentID property
        private int departmentID = 0;
        public int DepartmentID
        {
            get { return departmentID; }
            set { departmentID = value; }
        }

        // GroupID property
        private int groupID = 0;
        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        // SubGroupID property
        private int subgroupID = 0;
        public int SubGroupID
        {
            get { return subgroupID; }
            set { subgroupID = value; }
        }

        // Access LevelID property
        private string accessLevelID = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string AccessLevelID
        {
            get { return accessLevelID; }
            set { accessLevelID = value; }
        }

        // ShiftID property
        private int shiftID = 0;
        public int ShiftID
        {
            get { return shiftID; }
            set { shiftID = value; }
        }

        // Date of Birth property
        //private DateTime dateOfBirth = DateTime.Now;
        //public DateTime DateOfBirth
        //{
        //    get { return dateOfBirth; }
        //    set { dateOfBirth = value; }
        //}

        // Sex property
        private int sex = 0;
        public int Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        // Telephone property
        private string telephone = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        // Mobilephone property
        private string mobile = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        // Email property
        private string email = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        // Place of birth property
        private string placeOfBirth = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string PlaceOfBirth
        {
            get { return placeOfBirth; }
            set { placeOfBirth = value; }
        }

        // Address property
        private string address = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private string idCard = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IDCard
        {
            get { return idCard; }
            set { idCard = value; }
        }

        // Picture property
        private byte[] picture = null;
        public byte[] Picture
        {
            get { return picture; }
            set { picture = value; }
        }

        // Description property
        private string description = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        // Register property
        private bool isRegister = false;
        public bool IsRegister
        {
            get { return isRegister; }
            set { isRegister = value; }
        }

        // Absent
        private bool isabsent = true;
        public bool IsAbsent
        {
            get { return isabsent; }
            set { isabsent = value; }
        }

        // Salary Scale
        private string salaryscale = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string SalaryScale
        {
            get { return salaryscale; }
            set { salaryscale = value; }
        }

        // DateOfWork
        //private DateTime dateofwork = DateTime.Now;
        //public DateTime DateOfWork
        //{
        //    get { return dateofwork; }
        //    set { dateofwork = value; }
        //}


        // NumOfInsurance
        private string numofinsurance = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string NumOfInsurance
        {
            get { return numofinsurance; }
            set { numofinsurance = value; }
        }


        // DateOfIDCard
        //private DateTime dateofidcard = DateTime.Now;
        //public DateTime DateOfIDCard
        //{
        //    get { return dateofidcard; }
        //    set { dateofidcard = value; }
        //}


        // Salary Scale
        private string education = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Education
        {
            get { return education; }
            set { education = value; }
        }


        // IsEmployee
        private string isemployee = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string IsEmployee
        {
            get { return isemployee; }
            set { isemployee = value; }
        }


        // Temp1
        private string temp1 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Temp1
        {
            get { return temp1; }
            set { temp1 = value; }
        }

        // Temp2
        private string temp2 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Temp2
        {
            get { return temp2; }
            set { temp2 = value; }
        }

        // Temp3
        private string temp3 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Temp3
        {
            get { return temp3; }
            set { temp3 = value; }
        }

        // Temp4
        private string temp4 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Temp4
        {
            get { return temp4; }
            set { temp4 = value; }
        }

        // Temp5
        private string temp5 = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Temp5
        {
            get { return temp5; }
            set { temp5 = value; }
        }

        //UseridOnDevice
        private int userIDofFinger = 0;
        public int UserIDofFinger
        {
            get { return userIDofFinger; }
            set { userIDofFinger = value; }
        }

        private int memoryid = 0;
        public int MemoryID
        {
            get { return memoryid; }
            set { memoryid = value; }
        }

        private int memoryid1 = 0;
        public int MemoryID1
        {
            get { return memoryid1; }
            set { memoryid1 = value; }
        }

        private int memoryid2 = 0;
        public int MemoryID2
        {
            get { return memoryid2; }
            set { memoryid2 = value; }
        }

        private int memoryid3 = 0;
        public int MemoryID3
        {
            get { return memoryid3; }
            set { memoryid3 = value; }
        }

        private int memoryid4 = 0;
        public int MemoryID4
        {
            get { return memoryid4; }
            set { memoryid4 = value; }
        }
        private int memoryid5 = 0;
        public int MemoryID5
        {
            get { return memoryid5; }
            set { memoryid5 = value; }
        }
        private int memoryid6 = 0;
        public int MemoryID6
        {
            get { return memoryid6; }
            set { memoryid6 = value; }
        }
        private int memoryid7 = 0;
        public int MemoryID7
        {
            get { return memoryid7; }
            set { memoryid7 = value; }
        }
        private int memoryid8 = 0;
        public int MemoryID8
        {
            get { return memoryid8; }
            set { memoryid8 = value; }
        }
        private int memoryid9 = 0;
        public int MemoryID9
        {
            get { return memoryid9; }
            set { memoryid9 = value; }
        }


        private string expiredate = "20991231";
        public string ExpireDate
        {
            get { return expiredate; }
            set { expiredate = value; }
        }

        private string controllerids = "";

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ControllerIDs
        {
            get { return controllerids; }
            set { controllerids = value; }
        }

        //user from web
        private string userid = "";
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }
    }
}
