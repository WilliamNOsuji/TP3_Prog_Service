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
        private readonly ScoreService _scoreService;

        public ScoresController(ScoreService scoreService)
        {
            _scoreService = scoreService;
        }

        // GET: api/Scores
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Score>>> GetPublicScores()
        {
          IEnumerable<Score>? publicScores = await _scoreService.GetAllPublicScores();
          if( publicScores == null ) return NotFound();
          return Ok(publicScores);
        }

        // GET: api/Scores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Score>>> GetMyScores()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var scores = await _scoreService.GetMyScores(userId);

            if (scores == null)
            {
                return BadRequest("Utilisateur non trouvé.");
            }

            return Ok(scores);
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

            Score? updatedScore = await _scoreService.EditScore(id, score);
            if (updatedScore == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { Message = "Le Score a ete supprime ou modifie. Veillez reesayer" });
            }

            return Ok(updatedScore);
        }

        // POST: api/Scores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Score>> PostScore (Score score)
        {
            if (_scoreService.IsContextNull() == null)
            {
                return Problem("Entity set 'TP4Context.Score'  is null.");
            }

            if (score == null) return null;

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest,
                new { Message = "Utilisateur non trouvé." });
            }

            Score? newScore = await _scoreService.Add(score, userId);

            if(newScore == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                new { Message = "Echec lors de la creation du nouveau score." });
            }
            return newScore;
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
    }
}
