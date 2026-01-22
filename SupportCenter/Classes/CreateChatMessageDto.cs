using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    public class CreateChatMessageDto
    {
        public int AppealId { get; set; }
        public int TypeAppeal { get; set; } // 1, 2 или 3
        public string Message { get; set; }
        public string Login { get; set; } 
    }
}
