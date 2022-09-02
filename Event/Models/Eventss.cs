using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Models
{
    [Table("Event")]

    public class Eventss
    {

        public int Id { get; set; }
        [Required]
        public string? Venue { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public DateTime Time { get; set; }
        [Required]
        public string? Price { get; set; }
        public string? Image { get; set; }


    }
}
