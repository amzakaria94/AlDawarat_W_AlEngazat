using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlDawarat_W_AlEngazat.Models.Entities
{
	public class Employee
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Rank { get; set; }
		public string Number { get; set; }
		public string Category { get; set; }
		public string Department { get; set; }
		public string Position { get; set; }
		public string Certificate { get; set; }

		public int? CourseID { get; set; }
		public Course? Courses { get; set; }
	}
}