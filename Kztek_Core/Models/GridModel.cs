using System.Collections.Generic;

namespace Kztek_Core.Models
{
    public class GridModel<T> where T : class
    {
        public List<T> Data { get; set; }

        public int TotalPage { get; set; }

        public int TotalIem { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public double TotalMoney { get; set; }
        public double TotalMoneyFree { get; set; }
    }
}