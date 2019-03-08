using System;
using System.Web.Http;
using SiteServer.Plugin;
using SS.Mail.Core;

namespace SS.Mail.Controllers.Pages
{
    [RoutePrefix("pages/test")]
    public class TestController : ApiController
    {
        private const string Route = "";

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
                
                var address = request.GetPostString("address");
                var displayName = request.GetPostString("displayName");
                var title = request.GetPostString("title");
                var body = request.GetPostString("body");

                if (!MailManager.Send(config, address, displayName, title, body, out var errorMessage))
                {
                    return BadRequest(errorMessage);
                }

                return Ok(new { });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
