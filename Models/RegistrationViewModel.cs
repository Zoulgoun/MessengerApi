using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Models
{
    public class RegistrationViewModel
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
