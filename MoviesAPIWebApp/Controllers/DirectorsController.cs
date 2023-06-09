﻿using System;
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
    //[Route("api")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        private readonly MoviesAPIContext _context;

        public DirectorsController(MoviesAPIContext context)
        {
            _context = context;
        }

        // GET: api/Directors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Director>>> GetDirectors()
        {
          if (_context.Directors == null)
          {
              return NotFound();
          }
            return await _context.Directors
                .Include(d => d.DirectorMovies)
                .ThenInclude(dm => dm.Movie)
                //.ThenInclude(m => m.Genre)
                .ToListAsync();
            //return await _context.Directors.ToListAsync();
        }

        // GET: api/Directors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Director>> GetDirector(int id)
        {
          if (_context.Directors == null)
          {
              return NotFound();
          }
            var director = await _context.Directors.FindAsync(id);

            if (director == null)
            {
                return NotFound();
            }

            return director;
        }

        // PUT: api/Directors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDirector(int id, Director director)
        {
            if (id != director.Id)
            {
                return BadRequest();
            }
            //if (DirectorNameExists(director.Name))
            //{
            //    ModelState.AddModelError("Name", "Режисер з таким ім'ям вже існує");
            //    return ValidationProblem(ModelState);
            //}
            _context.Entry(director).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DirectorExists(id))
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

        // POST: api/Directors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Director>> PostDirector(/*int movieId, */Director director)
        {
          if (_context.Directors == null)
          {
              return Problem("Entity set 'MoviesAPIContext.Directors'  is null.");
          }
            if (DirectorNameExists(director.Name))
            {
                ModelState.AddModelError("Name", "Режисер з таким ім'ям вже існує");
                return ValidationProblem(ModelState);
            }
            //var existingMovie = await _context.Movies.FindAsync(movieId);
            //if (existingMovie == null)
            //{
            //    // Genre with the provided ID does not exist
            //    return NotFound("Movie not found.");
            //}
            //DirectorMovie newDirectorMovie = new DirectorMovie();
            //// Associate the existing genre with the movie
            //newDirectorMovie.Movie = existingMovie;
            //newDirectorMovie.Director = director;
            _context.Directors.Add(director);
            //_context.DirectorMovies.Add(newDirectorMovie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDirector", new { id = director.Id }, director);
        }

        // DELETE: api/Directors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDirector(int id)
        {
            if (_context.Directors == null)
            {
                return NotFound();
            }
            var director = await _context.Directors.FindAsync(id);
            if (director == null)
            {
                return NotFound();
            }

            _context.Directors.Remove(director);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DirectorExists(int id)
        {
            return (_context.Directors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool DirectorNameExists(string name)
        {
            return (_context.Directors?.Any(e => e.Name == name)).GetValueOrDefault();
        }
    }
}
