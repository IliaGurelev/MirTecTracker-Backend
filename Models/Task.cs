using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace tracker.Models
{
    public class Task
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateOnly CreatedAt { get; set; }

        [Required]
        public DateOnly DueDate { get; set; }

        [ForeignKey("BriefcaseId")]
        public virtual Briefcase Briefcase { get; set; }

        public int BriefcaseId { get; set; }

        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }

        [Required]
        public int StatusId { get; set; }

        public virtual ICollection<User> Workers { get; set; }
    }
}