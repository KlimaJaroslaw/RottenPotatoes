using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RottenPotatoes.Models;
using RottenPotatoes.Services;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace RottenPotatoes.Controllers
{
    public class UserController : Controller
    {
        private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        #region Constructor
        private readonly PotatoContext _context;
        private readonly SessionManager _session;
        public UserController(PotatoContext context, SessionManager session)
        {
            _context = context;
            _session = session;
        }
        #endregion

        #region Http
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([Bind("Login_Hash,Password_Hash")] User user)
        {
            RegisterUser(user.Login_Hash, user.Password_Hash);
            User? u = await ValidateUser(user);
            if (u == null)
            {
                return View();
            }
            else
            {
                _session.Set("user", u);
                return RedirectToAction("Index", "Movie");
            }
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([Bind("Login_Hash,Password_Hash")] User user)
        {
            User? u = await ValidateUser(user);
            if (u == null)
            {
                return View();
            }
            else
            {
                _session.Set("user", u);
                return RedirectToAction("Index", "Movie");
            }
        }

        public IActionResult Logout()
        {
            _session.Remove("user");
            return RedirectToAction("Login");
        }
        #endregion

        #region Methods
        private async Task<User?> ValidateUser(User user)
        {
            //jesli user istnieje w bazie danych -> zwracamy user, jesli nie -> null
            var userDb = await _context.Users.Include(u => u.Permission).FirstOrDefaultAsync(u =>u.Login_Hash == user.Login_Hash);
            if (userDb == null)
                return null;
            if (VerifyPassword(userDb?.Login_Hash, user.Password_Hash, userDb.Password_Hash))
                return userDb;
            else
                return null;
        }

        public async void RegisterUser(string username, string plainPassword)
        {
            User usr = await _context.Users.Where(x => x.Login_Hash == username).FirstOrDefaultAsync();
            if (usr != default)            
                return;            

            string hashedPassword = _passwordHasher.HashPassword(username, plainPassword);
            RottenPotatoes.Models.User u = new User();
            u.Login_Hash = username;
            u.Password_Hash = hashedPassword;
            u.Email_Hash = username;
            var permission = await _context.Permissions.Where(x => x.Description == "Reviewer").FirstOrDefaultAsync();
            u.Permission = permission;
            u.Permission_ID = permission?.Permission_ID ?? 0;
            u.ApiToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
            Console.WriteLine("-----------------------------------------------");
            Console.WriteLine(u.ApiToken);
            await _context.Users.AddAsync(u);
            await _context.SaveChangesAsync();
            // Store username and hashedPassword in your database
        }

        public bool VerifyPassword(string username, string plainPassword, string storedHashedPassword)
        {
            try
            {
                var result = _passwordHasher.VerifyHashedPassword(username, storedHashedPassword, plainPassword);
                return result == PasswordVerificationResult.Success;
            }
            catch (Exception ex)
            {
                return false;
            }
            
        }
        #endregion
    }
}
