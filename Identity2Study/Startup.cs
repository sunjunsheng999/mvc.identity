using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Mvc.Identity.BLL;
using Mvc.Identity.DAL;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Identity2Study
{
    public  partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

       

    }
}