using SiteServer.Plugin;
using SS.Mail.Core;

namespace SS.Mail
{
    public class Plugin : PluginBase
    {
        public const string PluginId = "SS.Mail";

        internal static ConfigInfo GetConfigInfo()
        {
            return Context.ConfigApi.GetConfig<ConfigInfo>(PluginId, 0) ?? new ConfigInfo();
        }

        public override void Startup(IService service)
        {
            service
                .AddSystemMenu(() => new Menu
                {
                    Text = "邮件发送设置",
                    Href = "pages/settings.html"
                });
        }

        public bool IsReady
        {
            get
            {
                var config = GetConfigInfo();
                return config.IsEnabled && !string.IsNullOrEmpty(config.Address) && !string.IsNullOrEmpty(config.Password);
            }
        }

        public bool Send(string address, string displayName, string title, string body, out string errorMessage)
        {
            var config = GetConfigInfo();

            errorMessage = string.Empty;
            var isSuccess = MailManager.Send(config, address, displayName, title, body, out errorMessage);

            if (!isSuccess && string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = "后台邮件发送功能暂时无法使用，请联系管理员或稍后再试";
            }

            return isSuccess;
        }
    }
}