using Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Repositories.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Services.Security
{
    public class TokenAuthenticationHandler : AuthenticationHandler<TokenAuthenticationOptions>
    {

        public IServiceProvider ServiceProvider { get; set; }
        public readonly AppSetting setting;

        public TokenAuthenticationHandler(IOptionsMonitor<TokenAuthenticationOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IServiceProvider serviceProvider, IOptions<AppSetting> configs)
            : base(options, logger, encoder, clock)
        {
            ServiceProvider = serviceProvider;
            setting = configs.Value;
        }



        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var headers = Request.Headers;
            var tokenHeader = headers[HeaderNames.Authorization];
            if (string.IsNullOrEmpty(tokenHeader)) return AuthenticateResult.NoResult();
            var token = tokenHeader.ToString().Replace("Bearer ", "");
            try
            {
                var information = TokenHelper.getInformationFromToken(token, setting.tokenKey, Options.tokenValidationParameters);
                var claim = information.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId);
                var user = new UserIdentity(claim.Value);
                user.email = "asdasdasd";
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(user), this.Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            catch (SecurityTokenInvalidSignatureException ex)
            {
                return AuthenticateResult.Fail("Invalid Token");
            }
        }
    }

    public class TokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public TokenValidationParameters tokenValidationParameters { get; set; }
        public bool saveToken { get; set; }

        public TokenAuthenticationOptions()
        {
            saveToken = false;
        }
    }
}
