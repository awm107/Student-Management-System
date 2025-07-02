using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class TeacherDeleteViewModel
    {
        public Teacher teacher { get; set; }

        public int Id { get; set; }
        public string PageTitle { get; set; }
    }
}
