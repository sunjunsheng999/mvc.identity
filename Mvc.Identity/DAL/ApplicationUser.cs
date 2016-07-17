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


namespace Mvc.Identity.DAL
{
  
    public class ApplicationUser : IdentityUser
    {
        //在这个类扩展自定义字段
        /// <summary>
        /// 头像URL
        /// </summary>
        public string headImage { get; set; }

        //添加一个方法 后面会用到
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}