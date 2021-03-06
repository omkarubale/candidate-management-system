﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OU.CMS.Domain.Lookups;

namespace OU.CMS.Models.Models.Account
{
    public class SignUpDto
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsCandidateLogin { get; set; }

        public string CompanyName { get; set; } 
    }
}
