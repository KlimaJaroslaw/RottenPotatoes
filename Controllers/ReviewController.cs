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

            var allReviews = await _context.Reviews
                .Include(r => r.Movie)
                .Include(r => r.User)
                .ToListAsync();
            return View((allReviews, user));
        }

        //GET: Review/User
        [HttpGet("Review/User")]
        public async Task<IActionResult> User()
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            if (_context == null)
                return Problem("Database context is not available.");

            var userReviews = await _context.Reviews.Where(x => x.User_ID == user.User_ID).Include(r => r.Movie)
                .Include(r => r.User).ToListAsync();
            return View((userReviews, user, user));
        }

        [HttpGet("Review/User/{id}")]
        public async Task<IActionResult> User(int id)
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            if (_context == null)
                return Problem("Database context is not available.");

            var u = await _context.Users.Where(u => u.User_ID == id).Include(u=>u.Permission).FirstOrDefaultAsync();
            var userReviews = await _context.Reviews.Where(x => x.User_ID == id).Include(r => r.Movie)
                .Include(r => r.User).ToListAsync();

            if (u == null)
                return NotFound();
            return View((userReviews, user, u));
        }

        
        public async Task<IActionResult> Movie(int id)
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            if (_context == null)
                return Problem("Database context is not available.");

            var movie = await _context.Movie.Include(movie => movie.Reviews)
                .FirstOrDefaultAsync(m => m.Movie_ID == id);
            var movieReviews = await _context.Reviews.Where(x => x.Movie_ID == id).Include(r => r.Movie)
                .Include(r => r.User).ToListAsync();
            
            return View((movieReviews, user, movie));
        }

        public async Task<IActionResult> Details(int id)
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            if (_context == null)
                return Problem("Database context is not available.");

            var review = await _context.Reviews.Where(x => x.Review_ID == id).Include(r => r.Movie)
                .Include(r => r.User).Include(p=>p.User.Permission).FirstOrDefaultAsync();

            if (review == null)
                return NotFound();
            return View(review);

        }

        public async Task<IActionResult> Create()
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            Review r = new Review();

            ViewBag.MovieList = new SelectList(_context.Movie, "Movie_ID", "Title", 0);
            return View(r);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Review review)
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            if (ModelState.IsValid)
            {                
                review.User_ID = user.User_ID;
                _context.Reviews.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("All");
            }
            else
            {
                ViewBag.MovieList = new SelectList(_context.Movie, "Movie_ID", "Title", 0);
                return View(review);
            }    
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

        [HttpPost]
        public async Task<IActionResult> Edit(Review review)
        {
            User user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            if (ModelState.IsValid)
            {                
                _context.Reviews.Update(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("All");
            }
            else
            {
                ViewBag.MovieList = new SelectList(_context.Movie, "Movie_ID", "Title", 0);
                return View(review);
            }
        }

        [HttpPost]        
        public async Task<IActionResult> Delete(int id)
        {
            var user = _session.Get<User>("user");
            if (user == null)
                return RedirectToAction("Login", "User");

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return NotFound();


            if(user?.Permission?.Description == "System Admin")
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("All");
            }

            
            if (review.User_ID == user.User_ID)
            {
                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();
                return RedirectToAction("All");
            }

            return Forbid();
        }

    }
}
