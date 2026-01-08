using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    public class EditUserDto
    {
        public int Id { get; set; }
        public string role { get; set; }
        public bool ActivityFlag { get; set; }
    }
}
