namespace AlDawarat_W_AlEngazat.Models.Entities
{
	public class User
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string NationalID { get; set; }
		public string Category { get; set; }
		public string Department { get; set; }
		public string Position { get; set; }
		public string Certificate { get; set; }

		public List<Achievement>? Achievements { get; set; }
	}
}