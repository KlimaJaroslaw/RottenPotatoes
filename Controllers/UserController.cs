using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RottenPotatoes.Models;
using RottenPotatoes.Services;

namespace RottenPotatoes.Controllers
{
    public class UserController : Controller
    {
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
            var userDb = await _context.Users
                .Include(u => u.Permission)
                .FirstOrDefaultAsync(u =>
                    u.Login_Hash == user.Login_Hash &&
                    u.Password_Hash == user.Password_Hash);
            return userDb;                   
        }
        #endregion
    }
}
