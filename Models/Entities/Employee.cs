using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlDawarat_W_AlEngazat.Models.Entities
{
    public class Employee
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Rank { get; set; } = string.Empty;

        [Required]
        public string Number { get; set; } = string.Empty;

        [Required]
        public string Category { get; set; } = string.Empty;

        [Required]
        public string Department { get; set; } = string.Empty;

        [Required]
        public string Position { get; set; } = string.Empty;

        [Required]
        public string Certificate { get; set; } = string.Empty;

        public int? CourseID { get; set; }

        [ForeignKey("CourseID")]
        public Course? Course { get; set; }

        public ICollection<PreviousCourse>? PreviousCourses { get; set; }
    }
}
