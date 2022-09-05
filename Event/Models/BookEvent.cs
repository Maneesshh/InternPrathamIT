using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Models
{
    [Table("Event_User")]
    public class BookEvent
    {
        public Guid Id { get; set; }
        public int Eid { get; set; }
        public int Uid { get; set; }

    }
}
