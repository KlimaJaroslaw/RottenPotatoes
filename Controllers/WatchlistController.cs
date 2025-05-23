using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;

namespace RottenPotatoes.Controllers
{
    public class WatchlistController : Controller
    {
        private readonly PotatoContext _context;

        public WatchlistController(PotatoContext context)
        {
            _context = context;
        }

        // GET: Watchlist
        public async Task<IActionResult> Index()
        {
              return _context.Watchlist != null ? 
                          View(await _context.Watchlist.ToListAsync()) :
                          Problem("Entity set 'PotatoContext.Watchlist'  is null.");
        }

        // GET: Watchlist/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Watchlist == null)
            {
                return NotFound();
            }

            var watchlist = await _context.Watchlist
                .FirstOrDefaultAsync(m => m.Watchlist_ID == id);
            if (watchlist == null)
            {
                return NotFound();
            }

            return View(watchlist);
        }

        // GET: Watchlist/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Watchlist/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Watchlist_ID,User_ID,Movie_ID,Added_Date,Priority")] Watchlist watchlist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(watchlist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(watchlist);
        }

        // GET: Watchlist/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
            if (id == null || _context.Watchlist == null)
            {
                return NotFound();
            }

            var watchlist = await _context.Watchlist
                .FirstOrDefaultAsync(m => m.Watchlist_ID == id);
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
            if (_context.Watchlist == null)
            {
                return Problem("Entity set 'PotatoContext.Watchlist'  is null.");
            }
            var watchlist = await _context.Watchlist.FindAsync(id);
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
