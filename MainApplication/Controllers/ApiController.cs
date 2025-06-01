using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using RottenPotatoes.Models;
using RottenPotatoes.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RottenPotatoes.DTO;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace RottenPotatoes.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly PotatoContext _context;
        private readonly SessionManager _session;

        public ApiController(PotatoContext context, SessionManager session)
        {
            _context = context;
            _session = session;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            {
                return userId;
            }
            throw new InvalidOperationException("User ID not found or invalid in token claims for an authenticated user.");
        }

        private string GetCurrentUsername()
        {
            var usernameClaim = User.FindFirst(ClaimTypes.Name);
            return usernameClaim?.Value ?? throw new InvalidOperationException("Username not found in token claims for an authenticated user.");
        }

        #region Movies
        [HttpGet("Movies")]
        public async Task<ActionResult<IEnumerable<MovieDTO>>> GetMovies()
        {
            if (_context.Movie == null)
                return NotFound();

            var dtos = await _context.Movie
                                     .Select(x => new MovieDTO(x))
                                     .ToListAsync();

            return Ok(dtos);
        }

        [HttpGet("Movies/{id}")]
        public async Task<ActionResult<MovieDTO>> GetMovies(int id)
        {
            if (_context.Movie == null)
            {
                return NotFound();
            }

            var dtos = await _context.Movie.FindAsync(id);
            if (dtos == null)
            {
                return NotFound();
            }

            return new MovieDTO(dtos);
        }
        #endregion

        #region Reviews
        [HttpGet("Reviews")]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviews()
        {
            if (_context.Reviews == null)
                return NotFound();

            var dtos = await _context.Reviews
                                     .Select(x => new ReviewDTO(x))
                                     .ToListAsync();

            return Ok(dtos);
        }

        [HttpGet("Reviews/{id}")]
        public async Task<ActionResult<ReviewDTO>> GetReviews(int id)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }

            var dtos = await _context.Reviews.FindAsync(id);
            if (dtos == null)
            {
                return NotFound();
            }

            return new ReviewDTO(dtos);
        }

        [HttpPut("Reviews")]
        public async Task<ActionResult<ReviewDTO>> PutReviews(ReviewDTO review)
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }

            try
            {
                Review r = ReviewDTO.GetReviewObject(review);
                _context.Reviews.Add(r);
                _context.SaveChangesAsync();
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("Reviews")]
        public async Task<ActionResult<ReviewDTO>> PatchReviews(ReviewDTO review)
        {
            if (_context.Reviews == null || review?.Review_ID == null || review?.Review_ID == 0)
            {
                return NotFound();
            }

            try
            {
                Review r = ReviewDTO.GetReviewObject(review);
                _context.Reviews.Update(r);
                _context.SaveChangesAsync();
                return Ok(review);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Watchlist
        [HttpGet("Watchlists")]
        public async Task<ActionResult<WatchlistDTO>> GetWatchlists()
        {
            if (_context.Watchlist == null) return NotFound();

            var dtos = await _context.Watchlist
                                     .Select(x => new WatchlistDTO(x))
                                     .ToListAsync();

            return Ok(dtos);
        }
        [HttpGet("Watchlists/{id}")]
        public async Task<ActionResult<WatchlistDTO>> GetWatchlists(int id)
        {
            if (_context.Watchlist == null) return NotFound();

            var dtos = await _context.Watchlist.FindAsync(id);
            if (dtos == null) return NotFound();
            return Ok(dtos);
        }
        [HttpPut("Watchlists")]
        public async Task<ActionResult<WatchlistDTO>> PutWatchlists(WatchlistDTO watchlist)
        {
            if (_context.Watchlist == null)
            {
                return NotFound();
            }

            try
            {
                Watchlist w = WatchlistDTO.GetWatchlistObject(watchlist);
                _context.Watchlist.Add(w);
                _context.SaveChangesAsync();
                return Ok(watchlist);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("MovieWatch")]
        public async Task<ActionResult<WatchlistDTO>> PostMovieToWatchlist(MovieTitleAndPriorityDTO movieData)
        {
            string title = movieData.Title;
            int priority = movieData.Priority; 
            Console.WriteLine(title);
            if (_context.Watchlist == null)
            {
                return NotFound();
            }

            try
            {
                
                Movie? m = await _context.Movie.FirstOrDefaultAsync(x => x.Title == title);

                if (m != null)
                {
                    Watchlist w = new Watchlist
                    {
                        User_ID = GetCurrentUserId(),
                        Movie_ID = m.Movie_ID,
                        Added_Date = DateTime.Now,
                        Priority = priority
                    };
                    
                    _context.Watchlist.Add(w);
                    await _context.SaveChangesAsync();
                    
                    return Ok(title);
                }
                return NotFound($"Movie '{title}' not found in database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Watchlists")]
        public async Task<ActionResult<WatchlistDTO>> DeleteMovieFromWatchlist(MovieTitleDTO movieData)
        {
            string title = movieData.Title;
            Console.WriteLine(title);
            if (_context.Watchlist == null)
            {
                return NotFound();
            }

            try
            {
                
                Movie? m = await _context.Movie.FirstOrDefaultAsync(x => x.Title == title);

                if (m != null)
                {
                    Watchlist? w = await _context.Watchlist.FirstOrDefaultAsync(x => x.Movie_ID == m.Movie_ID);
                    if (w != null)
                    {
                        _context.Watchlist.Remove(w);
                        await _context.SaveChangesAsync();
                        return Ok(title);
                    }
                    return NotFound($"Movie '{title}' not on your watchlist");
                }
                return NotFound($"Movie '{title}' not found in database");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        #endregion
    }
}
