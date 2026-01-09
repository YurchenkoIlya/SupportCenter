using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    public class HistoryAppealDto
    {
        public HistoryAppealDto(int idLog, string dateLog, int idAppeal, string action, int appealType, string userName)
        {
            this.idLog = idLog;
            this.dateLog = dateLog;
            this.idAppeal = idAppeal;
            this.action = action;
            this.action = action;
            this.appealType = appealType;
            this.userName = userName;

        }
        public int idLog { get; set; }
        public string dateLog { get; set; }
        public int idAppeal { get; set; }
        public string action { get; set; }
        public int appealType { get; set; }
        public string userName { get; set; }
    }
}
