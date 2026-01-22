using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    public class ChatMessageDto
    {
        public string Login { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
