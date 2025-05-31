using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RottenPotatoes.Models;
using RottenPotatoes.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RottenPotatoes.DTO;

namespace RottenPotatoes.Controllers
{
    [Route("api/[controller]")]
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
        
        #endregion
    }
}
