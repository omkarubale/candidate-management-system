using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(OU.CMS.Web.API.Startup))]

namespace OU.CMS.Web.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
