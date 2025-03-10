namespace AlDawarat_W_AlEngazat.Models.Entities
{
	public class CourseEmployee
	{
		public int CourseID { get; set; }
		public Course Course { get; set; }

		public int EmployeeID { get; set; }
		public Employee Employee { get; set; }
	}
}