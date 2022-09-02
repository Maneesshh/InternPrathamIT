using Event.Models;
using System.ComponentModel.DataAnnotations;

namespace Event.ViewModals
{
    public class EventsVM
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
        public IFormFile? Image { get; set; }

    }
}
