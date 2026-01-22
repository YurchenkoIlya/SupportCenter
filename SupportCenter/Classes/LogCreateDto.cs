using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    public class LogCreateDto
    {
        public DateTime Date { get; set; }
        public string Action { get; set; }
        public string ObjectAction { get; set; }
        public string NamePc { get; set; }
        public string IpPc { get; set; }
        public string LoginUser { get; set; }
    }
}
