using BCrypt.Net;
namespace MessengerApi.Models

{
    public class User
    {
        public int id {  get; set; }
        public string UserName { get; set; }

        public string HashedPassword { get; set; }
    }
}
