using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    internal class historyProgram
    {

        //Select u.id_log,u.date,u.object_action,u.action,o.user_name,u.name_pc,u.ip_pc
         public historyProgram(int id_log, string date, string object_action, string action, string user_name, string name_pc, string ip_pc)
        {
            this.id_log = id_log;
            this.date = date;
            this.object_action = object_action;
            this.action = action;
            this.user_name = user_name;
            this.name_pc = name_pc;
            this.ip_pc = ip_pc;

        }
        public int id_log { get; set; }
        public string date { get; set; }
        public string object_action { get; set; }
        public string action { get; set; }
        public string user_name { get; set; }
        public string name_pc { get; set; }
        public string ip_pc { get; set; }



    }
    
}
