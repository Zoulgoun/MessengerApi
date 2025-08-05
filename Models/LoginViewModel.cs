using System.ComponentModel.DataAnnotations;

namespace MessengerApi.Models
{
    public class LoginViewModel
    {
        [Key]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
