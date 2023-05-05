using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPIWebApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesAPIWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly MoviesAPIContext _context;

        public GenresController(MoviesAPIContext context)
        {
            _context = context;
        }

        // GET: api/Genres
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
          if (_context.Genres == null)
          {
              return NotFound();
          }
            return await _context.Genres
                .Include(g => g.Movies)
                .ToListAsync();
            //return await _context.Genres.ToListAsync();
        }

        // GET: api/Genres/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Genre>> GetGenre(int id)
        {
          if (_context.Genres == null)
          {
              return NotFound();
          }
            //var genre = await _context.Genres.FindAsync(id);
            var genre = await _context.Genres.Include(i => i.Movies)
    .FirstOrDefaultAsync(i => i.Id == id);
            if (genre == null)
            {
                return NotFound();
            }

            return genre;
        }

        // PUT: api/Genres/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGenre(int id, Genre genre)
        {
            if (id != genre.Id)
            {
                return BadRequest();
            }
            //if (GenreNameExists(genre.Name))
            //{
            //    ModelState.AddModelError("Name", "Жанр з такою назвою вже існує");
            //    return ValidationProblem(ModelState);
            //}
            _context.Entry(genre).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenreExists(id))
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

        // POST: api/Genres
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
          if (_context.Genres == null)
          {
              return Problem("Entity set 'MoviesAPIContext.Genres'  is null.");
          }
            if (GenreNameExists(genre.Name))
            {
                ModelState.AddModelError("Name", "Жанр з такою назвою вже існує");
                return ValidationProblem(ModelState);
            }
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGenre", new { id = genre.Id }, genre);
        }

        // DELETE: api/Genres/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGenre(int id)
        {
            if (_context.Genres == null)
            {
                return NotFound();
            }
            var genre = await _context.Genres.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }
            var movies = await _context.Movies.Where(m => m.GenreId == id).ToListAsync();
            foreach(Movie m in movies)
            {
                m.GenreId = null;
            }
            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GenreExists(int id)
        {
            return (_context.Genres?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool GenreNameExists(string name)
        {
            return (_context.Genres?.Any(e => e.Name == name)).GetValueOrDefault();
        }
    }
}
