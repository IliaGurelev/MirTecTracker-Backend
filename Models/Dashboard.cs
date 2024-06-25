using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace tracker.Models
{
    public class Dashboard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        public string Invite { get; set; }

        public ICollection<Task> Tasks { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Briefcase> Briefcase { get; set; }
    }
}