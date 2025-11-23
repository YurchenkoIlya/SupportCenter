using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportCenter.Classes
{
    class Users
    {
        public Users(int id, string login, string role, string name, string activity)
        {
            this.Id = id;
            this.login = login;
            this.role = role;
            this.name = name;
            this.activity = activity;

        }
        public int Id { get; set; }
        public string login { get; set; }
        public string role { get; set; }
        public string name { get; set; }
        public string activity { get; set; }

    }
}
