using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.OAuth;
using System.Web.Http;
using CK.TokenAuthentication.Security;

//[assembly: OwinStartup(typeof(CK.TokenAuthentication.Startup))]

namespace CK.TokenAuthentication
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //setup configuration for web api
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();
            RouteConfig.RegisterRoutes(configuration);

            //generate token
            app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(1),
                Provider= new CKAccessTokenProvider(),
                RefreshTokenProvider= new CKRefreshTokenProvider()
            });

            //consume token
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            //use web api with configuration
            app.UseWebApi(configuration);
            
        }
    }
}
