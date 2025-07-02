using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class SQLStudentRepository : IStudentRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQLStudentRepository> logger;

        public SQLStudentRepository(AppDbContext context, ILogger<SQLStudentRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }
        Student IStudentRepository.Add(Student student)
        {
            context.Students.Add(student);
            context.SaveChanges();
            return student;
        }

        Student IStudentRepository.Delete(int id)
        {
            Student student = context.Students.Find(id);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
            }
            return student;

        }

        IEnumerable<Student> IStudentRepository.GetAllStudents()
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log"); 
            return context.Students;
        }

        Student IStudentRepository.GetStudent(int Id)
        {
            return context.Students.Find(Id);
        }

        IEnumerable<Student> IStudentRepository.GetAllStudentsByClass(uint classId)
        {
            return context.Students.Where(s => s.Class == classId).ToList();
        }

        Student IStudentRepository.Update(Student studentChanges)
        {
            var student = context.Students.Attach(studentChanges);
            student.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return studentChanges;
        }
    }
}
