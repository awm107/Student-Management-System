using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
            ErrorMessage = "Invalid email format")]
        [Display(Name = "Student Email")]
        public string Email { get; set; }
        [Required]
        [Range(3, 8, ErrorMessage = "Class must be between 3 and 8")]
        public uint Class { get; set; }
        [Required]
        public GenderType? Gender { get; set; }
        public string Photopath { get; set; }

    }
}
