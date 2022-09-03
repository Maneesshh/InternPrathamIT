using System.ComponentModel.DataAnnotations;

namespace Event.ViewModals
{
    public class LoginVM
    {
        public string Email { get; set; }

        public string Password { get; set; }

        [Display(Name ="Remember Me")]
        public bool Cbox { get; set; }  

    }
}
