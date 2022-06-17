using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(int movieGenre, string searchString, string sortBy)
        {
            IQueryable<Genre> genreQuery = _context.Genre.Select(m => m).OrderBy(m => m.Type);
            var movies = _context.Movie.Select(m => m);
            if(!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title!.ToLower().Contains(searchString.ToLower()));
            }
            if (movieGenre > 0)
            {
                movies = movies.Where(x => x.GenreId == movieGenre);
            }
            switch (sortBy)
            {
                case "Title":
                    movies = movies.OrderBy(m => m.Title);
                    break;
                case "Date":
                    movies = movies.OrderBy(m => m.ReleaseDate);
                    break;
                default:
                    break;
            }

            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.OrderBy(m => m.Type).ToListAsync(), "GenreId", "Type"),
                Movies = await movies.ToListAsync(),
                MovieGenre = movieGenre,
                SearchString = searchString
            };

            ViewData["SearchString"] = searchString;
            return View(movieGenreVM);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            IQueryable<Genre> genreQuery = _context.Genre.Select(m => m).Where(m => m.GenreId == movie.GenreId);
            var movieGenreVM = new MovieGenreViewModel
            {
                Genre = genreQuery.Select(m => m.Type).FirstOrDefault(),
                Movie = movie
            };
            return View(movieGenreVM);
        }

        // GET: Movies/Create
        public async Task<IActionResult> Create()
        {
            IQueryable<Genre> genreQuery = _context.Genre.Select(m => m).OrderBy(m => m.Type);
            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.OrderBy(m => m.Type).ToListAsync(), "GenreId", "Type")
            };

            return View(movieGenreVM);
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,GenreId,Price,Rating,ImagePath")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            IQueryable<Genre> genreQuery = _context.Genre.Select(m => m).OrderBy(m => m.Type);
            var movieGenreVM = new MovieGenreViewModel
            {
                Genres = new SelectList(await genreQuery.OrderBy(m => m.Type).ToListAsync(), "GenreId", "Type"),
                Movie = movie
            };
            return View(movieGenreVM);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,GenreId,Price,Rating,ImagePath")] Movie movie)
        {
            if (id != movie.Id)
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
                    if (!MovieExists(movie.Id))
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

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            IQueryable<Genre> genreQuery = _context.Genre.Select(m => m).Where(m => m.GenreId == movie.GenreId);
            var movieGenreVM = new MovieGenreViewModel
            {
                Genre = genreQuery.Select(m => m.Type).FirstOrDefault(),
                Movie = movie
            };
            return View(movieGenreVM);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
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
          return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
