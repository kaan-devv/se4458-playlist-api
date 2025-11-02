using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly TodoContext _context;

        public SongsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Songs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs([FromQuery] string? search)
    {
        if (_context.Songs == null)
        {
                return NotFound();
        }

        var query = _context.Songs.AsQueryable();

        if (!string.IsNullOrEmpty(search))
        {
            string lowerSearch = search.ToLower(); 

        
            query = query.Where(s => (s.Title != null && s.Title.ToLower().Contains(lowerSearch)) ||
                                 (s.Artist != null && s.Artist.ToLower().Contains(lowerSearch)));
        }

        return await query.ToListAsync();
    }

        // GET: api/Songs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSong(long id)
        {
          if (_context.Songs == null)
          {
              return NotFound();
          }
            var Song = await _context.Songs.FindAsync(id);

            if (Song == null)
            {
                return NotFound();
            }

            return Song;
        }

        // PUT: api/Songs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSong(long id, Song Song)
        {
            if (id != Song.Id)
            {
                return BadRequest();
            }

            _context.Entry(Song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(id))
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

        // POST: api/Songs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Song>> PostSong(Song song)
        {
            if (_context.Songs == null)
            {
                return Problem("Entity set 'TodoContext.Songs'  is null.");
            }
            if (!ModelState.IsValid) {
                return Problem("Invalid json");
            }
            if (song.Title   == null || song.Title.Length == 0)
            {
                return Problem("Need description for Song");
            }
                _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSong", new { id = song.Id }, song);
        }

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        // [Authorize]
        public async Task<IActionResult> DeleteSong(long id)
        {
            if (_context.Songs == null)
            {
                return NotFound();
            }
            var Song = await _context.Songs.FindAsync(id);
            if (Song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(Song);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("Search")]
        public List<Song> SearchSong(String search)
        {
            if (_context.Songs == null)
            {
                return new List<Song>();
            }
            List<Song> ret = _context.Songs.Where(t => t.Title != null && t.Title.StartsWith(search)).ToList();

            return ret;
        }


        private bool SongExists(long id)
        {
            return (_context.Songs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
