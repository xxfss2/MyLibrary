using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Web;
namespace Xxf.Tool
{
    public class Mail
    {
        public void SendWebUnCatchErrorMail(string systemName)
        {
            try
            {
                MailAddress from = new MailAddress("jiuzh_admin@163.com", systemName);
                MailMessage mail = new MailMessage();

                mail.Subject = systemName;
                mail.From = from;
                string address = "164981897@qq.com";
                string displayName = "164981897@qq.com";

                mail.To.Add(new MailAddress(address, displayName));
                //设置邮件的抄送收件人
                mail.CC.Add(new MailAddress("164981897@qq.com", "管理员"));

                //设置邮件的内容
                string ErrorURL =HttpContext .Current . Request.Url.AbsoluteUri.ToString();
                DateTime ErrorDate = DateTime.Now;
                string ErrorMessage =HttpContext.Current. Server.GetLastError().InnerException.Message;
                string ErrorSource = HttpContext.Current.Server.GetLastError().InnerException.Source;
                string ErrorTrace = HttpContext.Current.Server.GetLastError().StackTrace;

                StringBuilder sb = new StringBuilder();
                sb.Append("页面URL：  ");
                sb.Append(ErrorURL);
                sb.Append("<br/>出错时间：  ");
                sb.Append(ErrorDate);
                sb.Append("<br/>错误信息：  ");
                sb.Append(ErrorMessage);
                sb.Append("<br/><br/>错误源：  ");
                sb.Append(ErrorSource);
                sb.Append("<br/><br/>调用堆栈：");
                sb.Append(ErrorTrace);
                mail.Body = sb.ToString();
                mail.BodyEncoding = System.Text.Encoding.UTF8;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;

                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                SmtpClient client = new SmtpClient();
                client.Host = "smtp.163.com";
                //client.Port = 25;
                client.UseDefaultCredentials = false;
                client.Credentials = new System.Net.NetworkCredential("jiuzh_admin", "jiuzhang");
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
            }
            catch
            {
            }
        }
    }
}
