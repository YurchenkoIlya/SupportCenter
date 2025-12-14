using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    public class EditUserDto
    {
        public int Id { get; set; }

        // "Пользователь", "Администратор" и т.п.
        public string role { get; set; }

        // true = активен, false = отключён
        public bool ActivityFlag { get; set; }
    }
}
