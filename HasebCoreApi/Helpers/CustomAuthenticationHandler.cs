//using HasebCoreApi.Models;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Text.Encodings.Web;
//using System.Threading.Tasks;

//namespace HasebCoreApi.Helpers
//{
//    public class BasicAuthenticationOptions : AuthenticationSchemeOptions
//    {
//    }

//    public class CustomAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
//    {
//        private readonly AppSettings _appSettings;
//        readonly IMongoRepository<UserInfo> _userRepo;

//        public CustomAuthenticationHandler(
//            IOptionsMonitor<BasicAuthenticationOptions> options,
//            ILoggerFactory logger,
//            UrlEncoder encoder,
//            ISystemClock clock,
//            IOptions<AppSettings> appSettings,
//            IMongoRepository<UserInfo> userRepo
//            )
//            : base(options, logger, encoder, clock)
//        {
//            _appSettings = appSettings.Value;
//            _userRepo = userRepo;
//        }

//        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
//        {
//            if (!Request.Headers.ContainsKey("Authorization"))
//                return AuthenticateResult.Fail("Unauthorized");

//            string authorizationHeader = Request.Headers["Authorization"];
//            if (string.IsNullOrEmpty(authorizationHeader))
//            {
//                return AuthenticateResult.NoResult();
//            }

//            if (!authorizationHeader.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
//            {
//                return AuthenticateResult.Fail("Unauthorized");
//            }

//            string token = authorizationHeader.Substring("bearer".Length).Trim();

//            if (string.IsNullOrEmpty(token))
//            {
//                return AuthenticateResult.Fail("Unauthorized");
//            }

        //    try
        //    {
        //        return await ValidateToken(token);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.ForegroundColor = ConsoleColor.Blue;
        //        Console.WriteLine("Auth Error : " + ex.Message);
        //        Console.WriteLine("Token : " + token);
        //        return AuthenticateResult.Fail(ex.Message);
        //    }
        //}

//        private async Task<AuthenticateResult> ValidateToken(string stream)
//        {
//            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            //var validationParameters = new TokenValidationParameters
            //{
            //    //RequireSignedTokens = true,
            //    //RequireExpirationTime = true,
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = new SymmetricSecurityKey(key),
            //    ValidateIssuer = false,
            //    ValidateAudience = false,
            //};


//            var handler = new JwtSecurityTokenHandler();

//            var token = handler.ValidateToken(stream, validationParameters, out SecurityToken securityToken);

//            var nameid = token.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

//            var user = await _userRepo.FindOneAsync(x => x.Id == nameid && x.TokenExpireDate >= DateTime.Now);

//            var valid = BCrypt.Net.BCrypt.EnhancedVerify(stream, user.Token, BCrypt.Net.HashType.SHA512);

//            if (user == null || !valid)
//            {
//                throw new Exception("Token is not valid.");
//            }

//            user.TokenExpireDate = DateTime.Now.AddDays(1);
//            await _userRepo.ReplaceOneAsync(user);


//            var claims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.NameIdentifier, nameid),
//                };

//            var identity = new ClaimsIdentity(claims, Scheme.Name);
//            var principal = new System.Security.Principal.GenericPrincipal(identity, null);
//            var ticket = new AuthenticationTicket(principal, Scheme.Name);
//            return AuthenticateResult.Success(ticket);
//        }
//    }
//}
