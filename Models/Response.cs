using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Response<T> where T:class
    {
        public T Result { get; set; }
        public string Source { get; set; }
    }
}
