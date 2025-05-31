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
    public class WatchlistController : Controller
    {
        private readonly PotatoContext _context;
        private readonly SessionManager _session;

        public WatchlistController(PotatoContext context, SessionManager session)
        {
            _context = context;
            _session = session;
        }

        // GET: Watchlist
        public async Task<IActionResult> Index()
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            var moviesOnWatchlist = _context.Watchlist.Where(watchlist => watchlist.User_ID == _session.Get<User>("user").User_ID).Select(x => x.Movie);


            return View(moviesOnWatchlist.ToList());
        }

        // GET: Watchlist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (id == null || _context.Watchlist == null)
            {
                return NotFound();
            }

            var watchlist = await _context.Watchlist
                .Include(watchlist => watchlist.Movie)
                .Include(watchlist => watchlist.User)
                .FirstOrDefaultAsync(m => m.Movie_ID == id);
            if (watchlist == null)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Movie",new { id });
            // return View(watchlist);
        }

        public IActionResult GetNextMovieFromWatchlist()
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            // Jeśli potrzebujesz warunku
            if (_context.Watchlist.Count() > 0)
            {
                int firstMovieID = _context.Watchlist
                                    .Where(x => x.User_ID == _session.Get<User>("user").User_ID)
                                    .OrderBy(x => x.Priority)
                                    .Select(x => x.Movie_ID).Last();
                Console.WriteLine(firstMovieID);
                return RedirectToAction("Details", "Movie", new { id = firstMovieID });
            }
            else
            {
                return RedirectToAction("Index", "Watchlist");
            }
            
        }

        // GET: Watchlist/Create
        public IActionResult Create()
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            return View();
        }

        // POST: Watchlist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Watchlist_ID,User_ID,Movie_ID,Added_Date,Priority")] Watchlist watchlist, int id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (ModelState.IsValid)
            {
                watchlist.User_ID = _session.Get<User>("user").User_ID;
                watchlist.Added_Date = DateTime.Now;
                Console.WriteLine("---------- ID FILMU DODANEGO DO WATCHLISTY!!! -------");
                Console.WriteLine(id);
                watchlist.Movie_ID = id;
                if (_context.Watchlist.Where(x => x.User_ID == _session.Get<User>("user").User_ID).Where(x => x.Movie_ID == id).Count() == 0)
                {
                    _context.Add(watchlist);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Movie");
            }
            return RedirectToAction("Index","Movie");
        }

        // GET: Watchlist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (id == null || _context.Watchlist == null)
            {
                return NotFound();
            }

            var watchlist = await _context.Watchlist.FindAsync(id);
            if (watchlist == null)
            {
                return NotFound();
            }
            return View(watchlist);
        }

        // POST: Watchlist/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Watchlist_ID,User_ID,Movie_ID,Added_Date,Priority")] Watchlist watchlist)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (id != watchlist.Watchlist_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(watchlist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WatchlistExists(watchlist.Watchlist_ID))
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
            return View(watchlist);
        }

        // GET: Watchlist/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (id == null || _context.Watchlist == null)
            {
                return NotFound();
            }
            Console.WriteLine("--------- " + id + " -----------");
            var watchlist = await _context.Watchlist
                .FirstOrDefaultAsync(m => m.Movie_ID == id);
            if (watchlist == null)
            {
                return NotFound();
            }

            return View(watchlist);
        }

        // POST: Watchlist/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_session.Get<User>("user") == null) return RedirectToAction("Login", "User");
            if (_context.Watchlist == null)
            {
                return Problem("Entity set 'PotatoContext.Watchlist'  is null.");
            }
            var watchlist = await _context.Watchlist.FirstOrDefaultAsync(m => m.Movie_ID == id);
            if (watchlist != null)
            {
                _context.Watchlist.Remove(watchlist);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WatchlistExists(int id)
        {
          return (_context.Watchlist?.Any(e => e.Watchlist_ID == id)).GetValueOrDefault();
        }
    }
}
