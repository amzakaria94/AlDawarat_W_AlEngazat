using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlDawarat_W_AlEngazat.Models.Entities
{
	public class Achievement
	{
		public int ID { get; set; }

		[Required]
		public string Title { get; set; }

		[DataType(DataType.Date)]
		public DateTime StartDate { get; set; }
		
		public string Description { get; set; }
	}
}