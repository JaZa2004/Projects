using System;
using System.ComponentModel.DataAnnotations;

namespace Task_Management.Models
{
    public class Comment
    {
        public int CommentId { get; set; }

        [Required(ErrorMessage = "Comment text is required.")]
        [StringLength(1000)]
        public string Text { get; set; } = null!;

        [Required(ErrorMessage = "Task is required.")]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Created Date is required.")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "User is required.")]
        public int UserId { get; set; } 

        public Task? Task { get; set; } = null!;

        public User? User { get; set; } = null!;
    }
}
