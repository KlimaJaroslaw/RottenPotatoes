using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using RottenPotatoes.Services;
using RottenPotatoes.Models;

namespace RottenPotatoes.Controllers
{
    public class MovieController : Controller
    {
        private readonly PotatoContext _context;
        private readonly SessionManager _session;

        public MovieController(PotatoContext context, SessionManager session)
        {
            _context = context;
            _session = session;
        }

        // GET: Movie
        public async Task<IActionResult> Index()
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            return _context.Movie != null ?
                        View(await _context.Movie.ToListAsync()) :
                        Problem("Entity set 'PotatoContext.Movie'  is null.");
        }
        public async Task<IActionResult> Top5()
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            return View(await _context.Reviews.Include(x => x.Movie).GroupBy(x => x.Movie).Select(x => new { Movie = x.Key, AvgRating = x.Average(y => y.Rating) }).OrderByDescending(x => x.AvgRating).Take(5).Select(x => x.Movie).ToListAsync());
        }

        public async Task<IActionResult> Fresh()
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            return View(await _context.Watchlist.Include(x => x.Movie).GroupBy(x => x.Movie).Select(x => new { Movie = x.Key, number = x.Count() }).OrderByDescending(x => x.number).Take(10).Select(x => x.Movie).ToListAsync());
        }

        // GET: Movie/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.Include(movie => movie.Reviews)
                .FirstOrDefaultAsync(m => m.Movie_ID == id);
            if (movie == null)
            {
                return NotFound();
            }


            return View(movie);
        }

        public IActionResult AddToWatchlist(int? id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            Console.WriteLine("####### ID to:");
            Console.WriteLine(id);
            return RedirectToAction("Create", "Watchlist", new { id = id });
        }
        // GET: Movie/Create
        public IActionResult Create()
        {
            if (_session.Get<User>("User") == null) return RedirectToAction("Login", "User");
            return View();
        }

        // POST: Movie/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Movie_ID,Title,Production_Date,Director,Producer,ScreenWriter,Synopsis")] RottenPotatoes.Models.Movie movie)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (_session.Get<User>("User") == null) return RedirectToAction("Login", "User");
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Movie_ID,Title,Production_Date,Director,Producer,ScreenWriter,Synopsis")] RottenPotatoes.Models.Movie movie)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (id != movie.Movie_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Movie_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Movie_ID == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (_context.Movie == null)
            {
                return Problem("Entity set 'PotatoContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.Movie_ID == id)).GetValueOrDefault();
        }
    }
}
