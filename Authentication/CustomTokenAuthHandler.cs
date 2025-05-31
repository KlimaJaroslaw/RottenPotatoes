using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace RottenPotatoes.Authentication
{
    public class CustomTokenAuthHandler : AuthenticationHandler<CustomTokenAuthOptions>
    {
        private readonly PotatoContext _context;

        public CustomTokenAuthHandler(
            IOptionsMonitor<CustomTokenAuthOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            PotatoContext context)
            : base(options, logger, encoder, clock)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("X-Username", out var usernameValue) ||
                !Request.Headers.TryGetValue("X-Auth-Token", out var tokenValue))
            {
                return AuthenticateResult.NoResult();
            }

            var username = usernameValue.ToString();
            var token = tokenValue.ToString();
            Console.WriteLine("###############");
            Console.WriteLine(username + " " + token);
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(token))
            {
                return AuthenticateResult.Fail("Username or Token not provided in headers.");
            }

            // Znajdź użytkownika w bazie danych
            var user = await _context.Users
                                 .FirstOrDefaultAsync(u => u.Login_Hash == username && u.ApiToken == token);

            if (user == null)
            {
                Logger.LogWarning($"Authentication failed for username: {username}. Invalid token or user not found.");
                return AuthenticateResult.Fail("Invalid username or token.");
            }

            // Użytkownik znaleziony i token pasuje, utwórz tożsamość
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.User_ID.ToString()),
                new Claim(ClaimTypes.Name, user.Login_Hash),
                // Możesz dodać inne claims, np. role
                // new Claim(ClaimTypes.Role, "UserRole"),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name); // Scheme.Name to "CustomToken"
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            Logger.LogInformation($"User {username} authenticated successfully.");
            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 401;
            Logger.LogDebug("HandleChallengeAsync called, returning 401.");
            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = 403;
            Logger.LogDebug("HandleForbiddenAsync called, returning 403.");
            return Task.CompletedTask;
        }
    }
}