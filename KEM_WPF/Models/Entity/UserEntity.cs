using KEM_WPF.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEM_WPF.Models.Entity
{
    public class UserEntity : User
    {
        public UserEntity() { }
        public UserEntity(string user, string pw) { user_name = user; password = pw; }
    }
}
