using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class MockTeacherRepository : ITeacherRepository
    {
        private List<Teacher> _TeacherList;
        public Teacher GetTeacher(int Id)
        {
            return _TeacherList.FirstOrDefault(e => e.Id == Id);
        }

        //public Teacher GetTeacherByClass(int classId)
        //{
        //    return _TeacherList.FirstOrDefault(e => e.Class == classId);
        //}
        //public int GetTeacherClass(int Id)
        //{

        //}

        public IEnumerable<Teacher> GetAllTeachers()
        {
            return _TeacherList;
        }

        public Teacher Add(Teacher teacher)
        {
           teacher.Id = _TeacherList.Max(e => e.Id) + 1;
            _TeacherList.Add(teacher);
            return teacher;
        }

        public Teacher Update(Teacher teacherChanges)
        {
            Teacher teacher = _TeacherList.FirstOrDefault(e => e.Id == teacherChanges.Id);
            if (teacher != null)
            {
                teacher.Name= teacherChanges.Name;
                teacher.Email = teacherChanges.Email;
                teacher.Age = teacherChanges.Age;
                teacher.Class = teacherChanges.Class;
                teacher.Subject = teacherChanges.Subject;
                teacher.Gender = teacherChanges.Gender;
            }
            return teacher;
        }

        public Teacher Delete(int id)
        {
            Teacher teacher = _TeacherList.FirstOrDefault(e => e.Id == id);
            if(teacher != null)
            {
                _TeacherList.Remove(teacher);
            }
            return teacher;
        }

        public MockTeacherRepository()
        {
            _TeacherList = new List<Teacher>
            {
                new Teacher(){Id=1, Name="Sarah", Age= 34, Class = 5, Subject = SubjectType.English, Email="sarah@pragim", Gender  = GenderType.Female},
                new Teacher(){Id=2, Name="James", Age= 45, Class = 6, Subject = SubjectType.Maths, Email="james@pragim", Gender = GenderType.Male},
                new Teacher(){Id=3, Name="Peter", Age= 38, Class = 3, Subject = SubjectType.Science, Email="peter@pragim", Gender = GenderType.Male }
            };
        }
    }
}
