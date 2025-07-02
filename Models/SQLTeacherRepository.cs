using Microsoft.Extensions.Logging;
using StudentManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public class SQLTeacherRepository : ITeacherRepository
    {
        private readonly AppDbContext context;
        private readonly ILogger<SQLTeacherRepository> logger;

        public SQLTeacherRepository(AppDbContext context, ILogger<SQLTeacherRepository> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        Teacher ITeacherRepository.Add(Teacher teacher)
        {
            context.Teachers.Add(teacher);
            context.SaveChanges();
            return teacher;
        }

        Teacher ITeacherRepository.Delete(int id)
        {
            Teacher teacher = context.Teachers.Find(id);
            if (teacher != null)
            {
                context.Teachers.Remove(teacher);
                context.SaveChanges();
            }
            return teacher;

        }

        IEnumerable<Teacher> ITeacherRepository.GetAllTeachers()
        {
            logger.LogTrace("Trace Log");
            logger.LogDebug("Debug Log");
            logger.LogInformation("Information Log");
            logger.LogWarning("Warning Log");
            logger.LogError("Error Log");
            logger.LogCritical("Critical Log"); 
            return context.Teachers;
        }

        Teacher ITeacherRepository.GetTeacher(int Id)
        {
            return context.Teachers.Find(Id);
        }

        //Teacher ITeacherRepository.GetTeacherByClass(int classId)
        //{
        //    return context.Teachers.Find(classId);
        //}

        Teacher ITeacherRepository.Update(Teacher teacherChanges)
        {
            var teacher = context.Teachers.Attach(teacherChanges);
            teacher.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return teacherChanges;
        }
    }
}
