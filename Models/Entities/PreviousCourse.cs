using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlDawarat_W_AlEngazat.Models.Entities {
	public class PreviousCourse {
		public int ID { get; set; }
		[Required] public string CourseName { get; set; }
		[DataType(DataType.Date)] public DateTime CompletionDate { get; set; }
		[DataType(DataType.Date)] public DateTime StartDate { get; set; }
		public string Location { get; set; }
		[ForeignKey("EmployeeID")] public int EmployeeID { get; set; }
		public Employee? Employee { get; set; }
	}
}