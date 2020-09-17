using System;
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

        public string FullName { get; set; }

        public string ShortName { get; set; }

        public UserType UserType { get; set; }

        public DateTime? DateOfBirth { get; set; } 
    }
}
