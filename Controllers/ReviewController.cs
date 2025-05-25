using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RottenPotatoes.Services;
using RottenPotatoes.Models;

namespace RottenPotatoes.Controllers
{
    public class ReviewController : Controller
    {
        #region Constructor
        private readonly PotatoContext _context;
        private readonly SessionManager _session;

        public ReviewController(PotatoContext context, SessionManager session)
        {
            _context = context;
            _session = session;
        }
        #endregion

        // GET: Review/All
        public async Task<IActionResult> All()
        {           
            if (_context == null)
                return Problem("Database context is not available.");

            var allReviews = await _context.Reviews.ToListAsync();
            return View(allReviews);
        }

        //GET: Review/User
        public async Task<IActionResult> User()
        {            
            if (_context == null)
                return Problem("Database context is not available.");

            var userReviews = await _context.Reviews.Where(x => x.User_ID == 1).ToListAsync();
            return View(userReviews);
        }
    }
}
