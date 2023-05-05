using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPIWebApp.Models;

namespace MoviesAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private readonly MoviesAPIContext _context;

        public MoviesController(MoviesAPIContext context)
        {
            _context = context;
        }

        // GET: api/Movies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
          if (_context.Movies == null)
          {
              return NotFound();
          }
            return await _context.Movies
                .Include(mov => mov.Genre).AsNoTracking()
                .Include(mov => mov.DirectorMovies)
                .ThenInclude(dm => dm.Director)
                .ToListAsync();
            //return await _context.Movies.ToListAsync();
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
          if (_context.Movies == null)
          {
              return NotFound();
          }
            //var movie = await _context.Movies.FindAsync(id);
            var movie = await _context.Movies.Include(i => i.Genre)
    .FirstOrDefaultAsync(i => i.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            //return await _context.Entry(movie).Collection(i => i.Genre).LoadAsync();
            return movie;
        }

        // PUT: api/Movies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }
            //if (MovieDescrExists(movie.Description))
            //{
            //    ModelState.AddModelError("Description", "Фільм з таким описом вже існує.");
            //    return ValidationProblem(ModelState);
            //}
            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
          if (_context.Movies == null)
          {
              return Problem("Entity set 'MoviesAPIContext.Movies'  is null.");
          }
            if (MovieDescrExists(movie.Description))
            {
                ModelState.AddModelError("Description", "Фільм з таким описом вже існує.");
                return ValidationProblem(ModelState);
            }
            var existingGenre = await _context.Genres.FindAsync(movie.GenreId);
            if (existingGenre == null)
            {
                // Genre with the provided ID does not exist
                return NotFound("Такого жанру не існує.");
            }

            // Associate the existing genre with the movie
            movie.Genre = existingGenre;
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            if (_context.Movies == null)
            {
                return NotFound();
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MovieExists(int id)
        {
            return (_context.Movies?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool MovieDescrExists(string description)
        {
            return (_context.Movies?.Any(e => e.Description == description)).GetValueOrDefault();
        }
    }
}
