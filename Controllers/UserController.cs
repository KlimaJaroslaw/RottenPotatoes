using Data;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Login([Bind("Login_Hash,Password_Hash")] User user)
        {
            User u = ValidateUser(user);
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
        private User ValidateUser(User user)
        {
            //jesli user istnieje w bazie danych -> zwracamy user, jesli nie -> null
            user.User_ID = -1;
            user.Permission_ID = -1;
            user.Permission = new Permission() { Description="admin", Permission_ID = -1 };
            return user;
        }
        #endregion
    }
}
