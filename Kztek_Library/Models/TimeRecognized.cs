using System;
using System.Collections.Generic;
using System.Text;

namespace Kztek_Library.Models
{
    internal class TimeRecognized
    {
        public TimeRecognized()
        { }

        private int times = 0;
        public int Times
        {
            get { return times; }
            set { times = value; }
        }

        string countpoint = "";
        public string CountPoint
        {
            get { return countpoint; }
            set { countpoint = value; }
        }

        string haveoverpoint = "";//diem phai qua
        public string HaveOverPoint
        {
            get { return haveoverpoint; }
            set { haveoverpoint = value; }
        }
    }
}
