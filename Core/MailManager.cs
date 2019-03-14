using System;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

namespace SS.Mail.Core
{
    public static class MailManager
    {
        private static bool IsEmail(string val)
        {
            return Regex.IsMatch(val, @"^\w+([-_+.]\w+)*@\w+([-_.]\w+)*\.\w+([-_.]\w+)*$", RegexOptions.IgnoreCase);
        }

        public static bool Send(ConfigInfo config, string address, string displayName, string title, string body, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (!config.IsEnabled)
            {
                errorMessage = "邮件发送功能未启用";
                return false;
            }

            if (string.IsNullOrEmpty(address) || !IsEmail(address))
            {
                errorMessage = "邮件格式不正确";
                return false;
            }

            if (string.IsNullOrEmpty(displayName))
            {
                displayName = address;
            }

            string fromUserName;
            string fromDisplayName;
            string fromPassword;
            bool enableSsl;
            string host;
            var port = 25;

            if (string.Equals(config.Provider, MailProvider.Default.Value, StringComparison.OrdinalIgnoreCase))
            {
                fromUserName = "noreply@services.siteserver.cn";
                fromDisplayName = "SiteServer CMS";
                fromPassword = "1T3g8BMwzACa1z2";
                enableSsl = false;
                host = "smtpdm.aliyun.com";
                port = 80;

                //smtpClient = new SmtpClient
                //{
                //    EnableSsl = false,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Host = "smtpdm.aliyun.com",
                //    Port = 80,
                //    Credentials = new System.Net.NetworkCredential("noreply@services.siteserver.cn", "1T3g8BMwzACa1z2")
                //};
            }
            else if (string.Equals(config.Provider, MailProvider.QqMail.Value, StringComparison.OrdinalIgnoreCase))
            {
                fromUserName = config.Address;
                fromDisplayName = config.DisplayName;
                fromPassword = config.Password;
                enableSsl = true;
                host = "smtp.qq.com";

                //smtpClient = new SmtpClient
                //{
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Host = "smtp.qq.com",
                //    Credentials = new System.Net.NetworkCredential(config.Address, config.Password)
                //};
            }
            else if (string.Equals(config.Provider, MailProvider.QqExMail.Value, StringComparison.OrdinalIgnoreCase))
            {
                fromUserName = config.Address;
                fromDisplayName = config.DisplayName;
                fromPassword = config.Password;
                enableSsl = true;
                host = "smtp.exmail.qq.com";

                //smtpClient = new SmtpClient
                //{
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Host = "smtp.exmail.qq.com",
                //    Credentials = new System.Net.NetworkCredential(config.Address, config.Password)
                //};
            }
            else
            {
                fromUserName = config.Address;
                fromDisplayName = config.DisplayName;
                fromPassword = config.Password;
                enableSsl = config.IsEnableSsl;
                host = config.Host;
                port = config.Port;

                //smtpClient = new SmtpClient
                //{
                //    EnableSsl = config.IsEnableSsl,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    Host = config.Host,
                //    Port = config.Port,
                //    Credentials = new System.Net.NetworkCredential(config.Address, config.Password)
                //};
            }

            var smtpClient = new SmtpClient
            {
                EnableSsl = enableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Host = host,
                Port = port,
                Credentials = new System.Net.NetworkCredential(fromUserName, fromPassword)
            };

            var from = new MailAddress(fromUserName, fromDisplayName);
            var to = new MailAddress(address, displayName);
            var message = new MailMessage(from, to)
            {
                Subject = title,
                SubjectEncoding = Encoding.UTF8,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            try
            {
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
                return false;
            }
        }
    }
}