using System.ComponentModel.DataAnnotations;

namespace Event.ViewModals
{
    public class Register
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter Email Address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage =" Password and confirmation password did not match ")]
          public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Please Enter valid Phone number")]
        [DataType(DataType.PhoneNumber)]
        public long phone { get; set; }
        [Required(ErrorMessage = "Please Enter Address")]

        public string Address { get; set; }
        [Required(ErrorMessage = "Please Enter your Gender")]
        public string Gender { get; set; }





    }
}
