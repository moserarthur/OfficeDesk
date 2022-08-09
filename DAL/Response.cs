using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Response
    {
        public bool Executed { get; set; }
        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }
        public int ElementId { get; set; }
    }
}
