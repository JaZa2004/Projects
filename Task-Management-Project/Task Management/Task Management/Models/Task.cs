using System.ComponentModel.DataAnnotations;

namespace Task_Management.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Task Name is required.")]
        [StringLength(100)]
        public string TaskName { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Project is required.")]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Assigned To is required.")]

        public bool CompletedByEmployee { get; set; } // For Employee

        public string? ApprovedByAdmin { get; set; } = "waitinguser";// For Admin

        public string? Status { get; set; }

        [Required(ErrorMessage = "Created Date is required.")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage ="Deadline Date for this task is required")]
        [DataType(DataType.Date)]
        public DateTime? DeadlineDate { get; set; }


        public Project? Project { get; set; } = null!;


        public User? AssignedUser { get; set; }


        public int? AssignedUserId { get; set; }

        public ICollection<Comment>? Comments { get; set; }= new List<Comment>();
    }
}
