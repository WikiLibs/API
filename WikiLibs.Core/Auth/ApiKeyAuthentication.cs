using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using WikiLibs.Shared.Attributes;
using WikiLibs.Shared.Modules.Admin;

namespace WikiLibs.Core.Auth
{
    public class ApiKeyAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SCHEME = "ApiKey";
        private readonly IAdminManager _admin;
        public readonly int[] FLAGS = new[]
        {
            AuthorizeApiKey.FlagAuthBot,
            AuthorizeApiKey.FlagAuthentication,
            AuthorizeApiKey.FlagErrorReport,
            AuthorizeApiKey.FlagRegistration,
            AuthorizeApiKey.FlagSelfDestruct,
            AuthorizeApiKey.FlagStandard
        };

        public ApiKeyAuthentication(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAdminManager admin)
            : base(options, logger, encoder, clock)
        {
            _admin = admin;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string key = null;
            if (Request.Headers.ContainsKey("Authorization"))
                key = Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(key) || !_admin.ApiKeyManager.Exists(key))
                return AuthenticateResult.Fail("The given API Key does not exist!");
            var addr = Context.Connection.RemoteIpAddress;
            var mdl = await _admin.ApiKeyManager.GetAsync(key);
            if (mdl.Origin != null && addr.ToString() != mdl.Origin)
            {
                throw new Shared.Exceptions.InsuficientPermission()
                {
                    ResourceName = "ApiKey",
                    ResourceId = "ApiKey",
                    MissingPermission = "ApiKey",
                    ResourceType = typeof(Data.Models.ApiKey)
                };
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, mdl.Description),
                new Claim(ClaimTypes.NameIdentifier, mdl.Id)
            };
            foreach (var f in FLAGS)
            {
                if ((mdl.Flags & f) != 0)
                    claims.Add(new Claim(ClaimTypes.Role, AuthorizeApiKey.GetFlagName(f)));
            }
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(claims, SCHEME)), SCHEME);
            await _admin.ApiKeyManager.Use(key);
            return AuthenticateResult.Success(ticket);
        }
    }
}
