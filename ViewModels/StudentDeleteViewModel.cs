using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class StudentDeleteViewModel
    {
        public Student student { get; set; }

        public int Id { get; set; }
        public string PageTitle { get; set; }
    }
}
