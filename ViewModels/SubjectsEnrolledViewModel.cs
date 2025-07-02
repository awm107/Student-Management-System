using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class SubjectsEnrolledViewModel
    {
        public List<SubjectType> Subjects { get; set; }
        public List<SubjectType> SelectedSubjects { get; set; } = new List<SubjectType>(); //need to make functions in controller
    }
}
