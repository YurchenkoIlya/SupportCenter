using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    public static class Session
    {
        public static string CurrentUserLogin { get; set; }
        public static string CurrentUserName { get; set; }
        public static string CurrentPcName { get; set; }
        public static string CurrenLoginUserId { get; set; }
    }
}
