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

            SmtpClient smtpClient;

            if (string.Equals(config.Provider, MailProvider.QqMail.Value, StringComparison.OrdinalIgnoreCase))
            {
                smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = "smtp.qq.com",
                    Credentials = new System.Net.NetworkCredential(config.Address, config.Password)
                };
            }
            else if (string.Equals(config.Provider, MailProvider.QqExMail.Value, StringComparison.OrdinalIgnoreCase))
            {
                smtpClient = new SmtpClient
                {
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = "smtp.exmail.qq.com",
                    Credentials = new System.Net.NetworkCredential(config.Address, config.Password)
                };
            }
            else
            {
                smtpClient = new SmtpClient
                {
                    EnableSsl = config.IsEnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Host = config.Host,
                    Port = config.Port,
                    Credentials = new System.Net.NetworkCredential(config.Address, config.Password)
                };
            }

            var from = new MailAddress(config.Address, config.DisplayName);
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