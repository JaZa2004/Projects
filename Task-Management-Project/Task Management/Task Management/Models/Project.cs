using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Task_Management.Models
{
    public class Project
    {
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Project Name is required.")]
        [StringLength(100)]
        public string ProjectName { get; set; } = null!;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Created Date is required.")]
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // Navigation property for tasks associated with this project
        public ICollection<Task>? Tasks { get; set; }
    }
}
