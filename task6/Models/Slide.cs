using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace task6.Models
{
    public class Slide
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public int Order { get; set; }
        [AllowNull]
        public string? Content { get; set; }
        [Required]
        public Guid PresentationId { get; set; }
    }
}
