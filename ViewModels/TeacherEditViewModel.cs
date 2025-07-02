using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.ViewModels
{
    public class TeacherEditViewModel : TeacherCreateViewModel
    {
        public int Id { get; set; }
        //public bool? isClassTeacher { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}
