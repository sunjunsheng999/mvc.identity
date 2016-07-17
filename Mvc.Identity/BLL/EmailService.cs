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
using Mvc.Identity.DAL;
using Microsoft.Owin;

namespace Mvc.Identity.BLL
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            var credentialUserName = "sunjunsheng999@163.com";
            var sentFrom = "sunjunsheng999@163.com";
            var pwd = "sjs@999";

            System.Net.Mail.SmtpClient client =
                new System.Net.Mail.SmtpClient("smtp.163.com");

            client.Port = 25;//smtp邮件服务器端口
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;

            System.Net.NetworkCredential credentials =
                new System.Net.NetworkCredential(credentialUserName, pwd);

            client.EnableSsl = true;
            client.Credentials = credentials;

            var mail =
                new System.Net.Mail.MailMessage(sentFrom, message.Destination);

            mail.Subject = message.Subject;
            mail.Body = message.Body;
            return client.SendMailAsync(mail);
        }
    }
}