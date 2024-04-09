using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TP4.Models;

namespace TP4.Data
{
    public class ScoreService
    {
        protected readonly TP4Context _context;
        public ScoreService(TP4Context context)
        {
            _context = context;
        }

        public bool IsContextNull()
        {
            return _context == null || _context.Score == null;
        }

        public bool ScoreExists(int id)
        {
            return (_context.Score?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IEnumerable<Score>?> GetAllPublicScores()
        {
            if(IsContextNull()) return null;

            return await _context.Score
                              .Where(s => s.IsPublic == true)
                              .OrderByDescending(x => x.ScoreValue)
                              .Take(10)
                              .ToListAsync();
        }

        public async Task<IEnumerable<Score>?> GetMyScores(string userId)
        {
            if (IsContextNull()) return null;

            if (userId == null) return null;

            User? user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                return user.Scores;
            }

            return null;
        }

        public async Task<Score?> EditScore(int id, Score score)
        {
            if (IsContextNull()) return null; // Assuming IsContextNull() checks if _context is null

            var existingScore = await _context.Score.FindAsync(id);
            if (existingScore == null)
            {
                return null; // Score with the specified ID was not found
            }

            // Update the existing score properties
            existingScore.Pseudo = score.Pseudo; // Assuming Pseudo is a property of Score
            existingScore.Date = score.Date;
            existingScore.Temps = score.Temps;
            existingScore.ScoreValue = score.ScoreValue;
            existingScore.IsPublic = score.IsPublic;

            try
            {
                await _context.SaveChangesAsync();
                return existingScore; // Return the updated score
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle concurrency exception if necessary
                throw; // Rethrow the exception for now
            }
        }

        public async Task<User?> GetUser(string userId)
        {
            if (IsContextNull()) return null;

            User? user = await _context.Users.FindAsync(userId);
            return user;
        }

        public async Task<Score?> Add(Score score, string userId)
        {
            if (IsContextNull()) return null;

            User? user = await GetUser(userId);
            if (user != null)
            {
                Score newScore = new Score
                {
                    Pseudo = user.UserName,
                    Date = DateTime.Now.ToString(),
                    Temps = score.Temps,
                    ScoreValue = score.ScoreValue,
                    IsPublic = score.IsPublic,
                };

                newScore.User = user;

                user.Scores.Add(newScore);

                _context.Score.Add(score);
                await _context.SaveChangesAsync();

                return newScore;
            }

            return null;
        }
    }
}
