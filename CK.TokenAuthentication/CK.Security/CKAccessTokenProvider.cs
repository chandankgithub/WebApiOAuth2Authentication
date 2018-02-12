using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Owin.Security;

namespace CK.TokenAuthentication.Security
{
    public class CKAccessTokenProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            await base.ValidateClientAuthentication(context);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //if no username is supplied then
            if(string.IsNullOrEmpty(context.UserName))
            {
                context.Rejected();
                return;
            }

            /*
             ****************
             * apply authentication code here
             */


            var guid = Guid.NewGuid().ToString();
            //create claim identity
            var claimsIdentity = new ClaimsIdentity(context.Options.AuthenticationType);
            claimsIdentity.AddClaims(new List<Claim>
            {
                new Claim(ClaimTypes.Name, guid),
                new Claim(ClaimTypes.NameIdentifier, context.UserName)
            });

            //put whatever metadata you wish to create token
            var authenticationMetadata = new AuthenticationProperties(new Dictionary<string, string>
            {
                {"guid", guid },
                {"isAdmin", "false" }
            });
            var ticket = new AuthenticationTicket(claimsIdentity, authenticationMetadata);
            context.Validated(ticket);
            //context.Validated(claimsIdentity);

            await base.GrantResourceOwnerCredentials(context);
        }

        public override async Task GrantRefreshToken(OAuthGrantRefreshTokenContext context)
        {
            var originalClientId = context.Ticket.Properties.Dictionary["as:clientId"];
            var requestedClientId = context.ClientId;

            if(originalClientId != requestedClientId)
            {
                context.Rejected();
                return;
            }

            var newClaimIdentity = new ClaimsIdentity(context.Ticket.Identity);
            newClaimIdentity.AddClaim(new Claim("type", "refresh_token"));

            var newTicket = new AuthenticationTicket(newClaimIdentity, context.Ticket.Properties);
            context.Validated(newTicket);

        }
    }
}