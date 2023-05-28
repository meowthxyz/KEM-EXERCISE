﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KEM_WPF.Data
{
    public class User
    {
        [Key]
        public int user_id { get; set; }
        public string? user_type { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email_address { get; set; }
        public string? user_name { get; set; }
        public string? password { get; set; }
    }
}
