using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.Infrastructure;
using System.Collections.Concurrent;
using Microsoft.Owin.Security;

namespace CK.TokenAuthentication.Security
{
    public class CKRefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly ConcurrentDictionary<string, AuthenticationTicket> concurrent;
        public CKRefreshTokenProvider()
        {
            this.concurrent = new ConcurrentDictionary<string, AuthenticationTicket>();
        }

        public void Create(AuthenticationTokenCreateContext context)
        {
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            //creating our own custom token
            var guid = Guid.NewGuid().ToString();

            //adding refresh ticket properties
            var refreshTicketProperties = new AuthenticationProperties
            {
                IssuedUtc = context.Ticket.Properties.IssuedUtc,
                ExpiresUtc = DateTime.Now.AddYears(1)
            };

            //creating refresh ticket with our modified properties
            var refreshTicket = new AuthenticationTicket(context.Ticket.Identity, refreshTicketProperties);

            //set the token and the corresponding ticket in memory
            this.concurrent.TryAdd(guid, context.Ticket);

            //set our custom guid token
            context.SetToken(guid);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            AuthenticationTicket ticket;

            //if our custom token found then set the corresponding ticket in the context
            if(this.concurrent.TryRemove(context.Token, out ticket))
            {
                context.SetTicket(ticket);
            }
        }
    }
}