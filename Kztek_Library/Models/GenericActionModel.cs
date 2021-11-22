using System;
using System.Collections.Generic;
using System.Text;

namespace Kztek_Library.Models
{
    public class GenericActionModel<T>
        where T : class, new()
    {
        public T Model { get; set; } = new T();
        public string Controller { get; set; } = "";
        public string Action { get; set; } = "";
        public int Page { get; set; } = 1;
    }
}
