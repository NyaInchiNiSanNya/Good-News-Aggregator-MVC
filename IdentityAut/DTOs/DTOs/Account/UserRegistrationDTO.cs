﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities_Context.Entities.UserNews;

namespace Core.DTOs.Account
{
    public class UserRegistrationDTO
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
