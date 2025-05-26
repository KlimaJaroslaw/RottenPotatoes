using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RottenPotatoes.Services;
using RottenPotatoes.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            User user = _session.Get<User>("user");
            if (user == null)                            
                return RedirectToAction("Login", "User");
            

            if (_context == null)
                return Problem("Database context is not available.");

            var allReviews = await _context.Reviews.ToListAsync();
            return View((allReviews, user));
        }

        //GET: Review/User
        public async Task<IActionResult> User()
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            if (_context == null)
                return Problem("Database context is not available.");

            var userReviews = await _context.Reviews.Where(x => x.User_ID == user.User_ID).ToListAsync();
            return View((userReviews, user));
        }

        public async Task<IActionResult> Create()
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            Review r = new Review() {User=user, User_ID=user.User_ID };

            ViewBag.MovieList = new SelectList(_context.Movie, "Movie_ID", "Title", 0);
            return View(r);
        }

        public async Task<IActionResult> Edit(int id)
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            ViewBag.MovieList = new SelectList(_context.Movie, "Movie_ID", "Title", review.Movie_ID);
            return View(review);
        }
    }
}
