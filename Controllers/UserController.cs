using Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RottenPotatoes.Models;

namespace RottenPotatoes.Controllers
{
    public class UserController : Controller
    {
        #region Constructor
        private readonly PotatoContext _context;
        public UserController(PotatoContext context)
        {
            _context = context;
        }
        #endregion

        public User GetLoggedUser()
        {
            var userJson = HttpContext.Session.GetString("User");
            if (userJson == null)             
                return null;
            else
            {
                var user = JsonConvert.DeserializeObject<User>(userJson);
                return user;
            }                        
        }

        public void LogUser(User userData)
        {
            HttpContext.Session.SetString("User", JsonConvert.SerializeObject(userData));
            return;
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
