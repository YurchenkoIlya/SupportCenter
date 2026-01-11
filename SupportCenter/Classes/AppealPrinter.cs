using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SupportCenter.Classes
{
    public class AppealPrinter
    {
        public AppealPrinter() { }
        public AppealPrinter(int id_appeal, string ip_pc, string name_pc, string model_printer, string ip_printer, string executor,
            string status_execute, string comment, string applicant)
        {
            this.id_appeal = id_appeal;
            this.ip_pc = ip_pc;
            this.name_pc = name_pc;
            this.model_printer = model_printer;
            this.ip_printer = ip_printer;
            this.executor = executor;
            this.status_execute = status_execute;
            this.comment = comment;
            this.applicant = applicant;



        }
        public int id_appeal { get; set; }
        public string ip_pc { get; set; }
        public string name_pc { get; set; }
        public string model_printer { get; set; }
        public string ip_printer { get; set; }
        public string executor { get; set; }
        public string status_execute { get; set; }
        public string comment { get; set; }
        public string applicant { get; set; }



    }
}
