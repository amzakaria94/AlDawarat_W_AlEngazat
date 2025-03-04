using System.ComponentModel.DataAnnotations;

namespace AlDawarat_W_AlEngazat.Models.Entities
{
	public class Achievement
	{
		public int ID { get; set; }

		[Required]
		public string Title { get; set; }

		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }

		[DataType(DataType.Date)]
		public DateTime EndDate { get; set; }

		public int UserID { get; set; }
		public User? User { get; set; }
	}
}