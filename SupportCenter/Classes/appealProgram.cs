using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SupportCenter.Classes
{
    internal class appealProgram
    {
     
            public appealProgram(int idAppeal, string idProgram, string pcName, string ipPc, string applicant, string oibResponsible, string oibStatusResponsible, string oitResponsible,
               string oitStatusResponsible,string otpExecutor, string otpStatusExecutor)
            {
                this.idAppeal = idAppeal;
                this.idProgram = idProgram;
                this.pcName = pcName;
                this.ipPc = ipPc;
                this.applicant = applicant;
                this.oibResponsible = oibResponsible;
                this.oibStatusResponsible = oibStatusResponsible;
                this.oitResponsible = oitResponsible;
                this.oitStatusResponsible = oitStatusResponsible;
                this.otpExecutor = otpExecutor;
                this.otpStatusExecutor = otpStatusExecutor;


        }
            public int idAppeal { get; set; }
            public string idProgram { get; set; }
            public string pcName { get; set; }
            public string ipPc { get; set; }
            public string applicant { get; set; }
            public string oibResponsible { get; set; }
            public string oibStatusResponsible { get; set; }
            public string oitResponsible { get; set; }
            public string oitStatusResponsible { get; set; }
            public string otpExecutor { get; set; }
            public string otpStatusExecutor { get; set; }


    }
}
