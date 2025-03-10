using System.ComponentModel.DataAnnotations;

namespace AlDawarat_W_AlEngazat.Models.Entities
{
    public class Achievement
    {
        public int ID { get; set; }

        [Required]
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
