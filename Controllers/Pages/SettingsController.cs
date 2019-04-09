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
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Plugin.PluginId))
                {
                    return Unauthorized();
                }

                return Ok(new
                {
                    Value = Plugin.GetConfigInfo()
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
                var request = Context.AuthenticatedRequest;
                if (!request.IsAdminLoggin || !request.AdminPermissions.HasSystemPermissions(Plugin.PluginId))
                {
                    return Unauthorized();
                }

                var config = Plugin.GetConfigInfo();

                config.IsEnabled = request.GetPostBool(nameof(config.IsEnabled));
                config.Provider = request.GetPostString(nameof(config.Provider));
                config.Host = request.GetPostString(nameof(config.Host));
                config.Port = request.GetPostInt(nameof(config.Port));
                config.IsEnableSsl = request.GetPostBool(nameof(config.IsEnableSsl));
                config.Address = request.GetPostString(nameof(config.Address));
                config.Password = request.GetPostString(nameof(config.Password));
                config.DisplayName = request.GetPostString(nameof(config.DisplayName));

                Context.ConfigApi.SetConfig(Plugin.PluginId, 0, config);

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
