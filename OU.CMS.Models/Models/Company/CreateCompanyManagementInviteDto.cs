﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OU.CMS.Models.Models.Company
{
    public class CreateCompanyManagementInviteDto
    {
        public Guid CompanyId { get; set; }

        public string Email { get; set; }

        public bool IsInviteForAdmin { get; set; }
    }
}
