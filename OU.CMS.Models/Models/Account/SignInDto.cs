﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Lookups;

namespace OU.CMS.Models.Models.Account
{
    public class SignInDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public int UserType { get; set; }
    }
}
