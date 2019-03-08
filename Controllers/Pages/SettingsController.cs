using System;
using System.Web.Http;
using SiteServer.Plugin;

namespace SS.Mail.Controllers.Pages
{
    [RoutePrefix("pages/settings")]
    public class SettingsController : ApiController
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult GetConfig()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(MailPlugin.PluginId))
                {
                    return Unauthorized();
                }

                return Ok(new
                {
                    Value = MailPlugin.GetConfigInfo()
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Submit()
        {
            try
            {
                var request = Context.GetCurrentRequest();
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(MailPlugin.PluginId))
                {
                    return Unauthorized();
                }

                var config = MailPlugin.GetConfigInfo();

                config.IsEnabled = request.GetPostBool(nameof(config.IsEnabled));
                config.Provider = request.GetPostString(nameof(config.Provider));
                config.Host = request.GetPostString(nameof(config.Host));
                config.Port = request.GetPostInt(nameof(config.Port));
                config.IsEnableSsl = request.GetPostBool(nameof(config.IsEnableSsl));
                config.Address = request.GetPostString(nameof(config.Address));
                config.Password = request.GetPostString(nameof(config.Password));
                config.DisplayName = request.GetPostString(nameof(config.DisplayName));

                Context.ConfigApi.SetConfig(MailPlugin.PluginId, 0, config);

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
