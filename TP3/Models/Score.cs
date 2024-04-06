using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TP4.Models
{
    public class Score
    {
        [Required]
        public int Id { get; set; }

        public string Pseudo { get; set; }

        public string Date { get; set; }

        public float Temps { get; set; }

        public int ScoreValue { get; set; }

        public bool IsPublic { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
