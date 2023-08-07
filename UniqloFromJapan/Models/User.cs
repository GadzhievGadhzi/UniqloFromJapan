using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace UniqloFromJapan.Models {
    public class User {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserStatus Status { get; set; }
        public int[] WishList { get; set; }

        public User() {
            WishList = new int[] { };
        }
        public User(int id, string firstName, string lastName, string patronymic, string email, string password, UserStatus userStatus) {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Patronymic = patronymic;
            Email = email;
            Password = password;
            Status = userStatus; 
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserStatus {
        Default,
        Admin
    }
}