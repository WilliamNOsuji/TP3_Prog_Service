using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using TP4.Data;
using TP4.Models;

namespace TP4.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ScoresController : ControllerBase
    {
        private readonly TP4Context _context;

        public ScoresController(TP4Context context)
        {
            _context = context;
        }

        // GET: api/Scores
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Score>>> GetPublicScores()
        {
          if (_context.Score == null)
          {
              return NotFound();
          }
            return await _context.Score.ToListAsync();
        }

        // GET: api/Scores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Score>>> GetMyScores()
        {
            if (_context.Score == null)
            {
                return NotFound();
            }
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                return user.Scores;
            }

            return StatusCode(StatusCodes.Status400BadRequest, new {Message = "Utilisateur non trouvé."});
        }

        // PUT: api/Scores/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> ChangeScoreVisibility(int id, Score score)
        {
            if (id != score.Id)
            {
                return BadRequest();
            }

            _context.Entry(score).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoreExists(id))
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

        // POST: api/Scores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Score>> PostScore (Score score)
        {
            if (_context.Score == null)
            {
                return Problem("Entity set 'TP4Context.Score'  is null.");
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                score.User = user;
                user.Scores.Add(score);

                _context.Score.Add(score);
                await _context.SaveChangesAsync();
                return Ok(score);
            }

            return StatusCode(StatusCodes.Status400BadRequest, new { Message = "Utilisateur non trouvé." });
        }

        //// DELETE: api/Scores/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteScore(int id)
        //{
        //    if (_context.Score == null)
        //    {
        //        return NotFound();
        //    }
        //    var score = await _context.Score.FindAsync(id);
        //    if (score == null)
        //    {
        //        return NotFound();
        //    }
        //
        //    _context.Score.Remove(score);
        //    await _context.SaveChangesAsync();
        //
        //    return NoContent();
        //}

        private bool ScoreExists(int id)
        {
            return (_context.Score?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
