using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Common.Models
{
    public class Response
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}
