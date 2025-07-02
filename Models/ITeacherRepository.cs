using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public interface ITeacherRepository
    {
        Teacher GetTeacher(int id);
        //Teacher GetTeacherByClass(int classId);
        //int GetTeacherClass(int id);
        IEnumerable<Teacher> GetAllTeachers();
        Teacher Add(Teacher teacher);
        Teacher Update(Teacher teacherChanges);
        Teacher Delete(int id);
    }
}
