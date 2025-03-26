using System.ComponentModel.DataAnnotations;

namespace task6.Models
{
    public class Presentation
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public required string Title { get; set; }
        [Required]
        public required string CreatorNickname { get; set; }
        [Required]
        public Guid CreatorId { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        public List<Slide> Slides { get; set; } = new List<Slide>();
    }
}
