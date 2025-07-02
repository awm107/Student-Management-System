using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class StudentAttendanceViewModel
    {
        public int Id { get; set; }
        public Teacher Teacher { get; set; }
        public List<Student> StudentClassList { get; set; }
        [Required]
        public AttendanceType AttendanceType { get; set; }
    }
}
