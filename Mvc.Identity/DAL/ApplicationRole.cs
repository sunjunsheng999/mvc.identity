using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Mvc.Identity.DAL
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }

        /// <summary>
        /// 角色描述
        /// </summary>
        [DisplayName("角色描述")]
        public string Description { get; set; }
    }
}