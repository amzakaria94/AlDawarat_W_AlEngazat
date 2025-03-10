using System.ComponentModel.DataAnnotations;

namespace AlDawarat_W_AlEngazat.Models.Entities
{
    public class Course
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();  // Fix: Ensure it initializes as a List
    }

}