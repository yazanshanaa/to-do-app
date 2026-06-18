using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Please enter a title.")]
        public string Title { get; set; } = string.Empty;

        public bool IsDone { get; set; }

        // foreign key to User (Chapter 4)
        public int UserId { get; set; }
    }
}
