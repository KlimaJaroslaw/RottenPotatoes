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

        public IActionResult Login()
        {
            return View();
        }
    }
}
