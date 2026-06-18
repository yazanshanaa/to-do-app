using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class User
    {
        // EF Core generates this primary key value (Chapter 4)
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter a username.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please enter a password.")]
        public string Password { get; set; } = string.Empty;
    }
}
