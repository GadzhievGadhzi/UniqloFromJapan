using System.ComponentModel.DataAnnotations;

namespace UniqloFromJapan.Models {
    public class Admin {
        [Required]
        public required string Login { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
