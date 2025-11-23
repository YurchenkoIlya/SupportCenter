using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    class folderResponsible
    {
        public folderResponsible(int id, string nameFolder, string responsibleUser, string wayFolder)
        {
            this.Id = id;
            this.nameFolder = nameFolder;
            this.responsibleUser = responsibleUser;
            this.wayFolder = wayFolder;

        }
        public int Id { get; set; }
        public string nameFolder { get; set; }
        public string responsibleUser { get; set; }
        public string wayFolder { get; set; }
    }
}
