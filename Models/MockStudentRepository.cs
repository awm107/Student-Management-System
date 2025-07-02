using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class MockStudentRepository : IStudentRepository
    {
        private List<Student> _StudentList;
        public Student GetStudent(int Id)
        {
            return _StudentList.FirstOrDefault(e => e.Id == Id);
        }

        public IEnumerable<Student> GetAllStudents()
        {
            return _StudentList;
        }

        public IEnumerable<Student> GetAllStudentsByClass(uint classId)
        {
            return (IEnumerable<Student>)_StudentList.Where(e => e.Class == classId).ToList();
        }

        public Student Add(Student Student)
        {
           Student.Id = _StudentList.Max(e => e.Id) + 1;
            _StudentList.Add(Student);
            return Student;
        }

        public Student Update(Student StudentChanges)
        {
            Student Student = _StudentList.FirstOrDefault(e => e.Id == StudentChanges.Id);
            if (Student != null)
            {
                Student.Name= StudentChanges.Name;
                Student.Email = StudentChanges.Email;
                Student.Class = StudentChanges.Class;
                Student.Gender = StudentChanges.Gender;
            }
            return Student;
        }

        public Student Delete(int id)
        {
            Student Student = _StudentList.FirstOrDefault(e => e.Id == id);
            if(Student != null)
            {
                _StudentList.Remove(Student);
            }
            return Student;
        }

        public MockStudentRepository()
        {
            _StudentList = new List<Student>
            {
                new Student(){Id=1, Name="Mary", Class= 4, Email="mary@pragim", Gender  = GenderType.Female},
                new Student(){Id=2, Name="John", Class= 5, Email="john@pragim", Gender = GenderType.Male},
                new Student(){Id=3, Name="Sam", Class= 8, Email="sam@pragim", Gender = GenderType.Male }
            };
        }
    }
}
