using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TP4.Models
{
    public class Score
    {
        [Required]
        public int Id { get; set; }

        public float Temps { get; set; }

        public DateTime Date { get; set; }

        public bool IsVisible { get; set; }

        [JsonIgnore]
        public virtual User? User { get; set; }

        public Score()
        {

        }
    }
}
