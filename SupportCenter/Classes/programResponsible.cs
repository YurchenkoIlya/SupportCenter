using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    class programResponsible
    {
        public programResponsible(int id_program, string name_program, string responsible_program, string way_program)
        {
            this.id_program = id_program;
            this.name_program = name_program;
            this.responsible_program = responsible_program;
            this.way_program = way_program;

        }
        public int id_program { get; set; }
        public string name_program { get; set; }
        public string responsible_program { get; set; }
        public string way_program { get; set; }

    }
}
