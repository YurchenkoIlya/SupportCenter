using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SupportCenter.Classes
{
    internal class appealFolder
    {
     
            public appealFolder(int idAppeal, string idApplicant, string idFolder, string idResponsible, string statusResponsible, string executor, 
                string statusExecutor, string comment)
            {
                this.idAppeal = idAppeal;
                this.idApplicant = idApplicant;
                this.idFolder = idFolder;
                this.idResponsible = idResponsible;
                this.statusResponsible = statusResponsible;
                this.executor = executor;
                this.statusExecutor = statusExecutor;
                this.comment = comment;
              


        }
            public int idAppeal { get; set; }
            public string idApplicant { get; set; }
            public string idFolder { get; set; }
            public string idResponsible { get; set; }
            public string statusResponsible { get; set; }
            public string executor { get; set; }
            public string statusExecutor { get; set; }
            public string comment { get; set; }



    }
}
