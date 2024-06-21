using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace tracker.Models
{
    public class Briefcase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ColorId { get; set;}

        [ForeignKey("ColorId")]
        public Color Color { get; set;}

        public ICollection<Models.Task> Task { get; set; }
    }
}