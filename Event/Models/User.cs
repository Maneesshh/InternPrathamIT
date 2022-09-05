using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Models
{
    [Table("Users")]
    public class User
    {
        
       public int Id { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }

        public long Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }


    }
}
