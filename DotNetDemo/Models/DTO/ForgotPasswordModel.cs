using System.ComponentModel.DataAnnotations;

namespace DotNetDemo.Models.DTO
{
    public class ForgotPasswordModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
