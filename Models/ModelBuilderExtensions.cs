using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                   new Student
                   {
                       Id = 1,
                       Name = "Mary",
                       Class = 5,
                       Gender = GenderType.Female,
                       Email = "mary@gmail.com"
                   },
                   new Student
                   {
                       Id = 2,
                       Name = "John",
                       Class = 6,
                       Gender = GenderType.Male,
                       Email = "john@gmail.com"
                   },
                   new Student
                   {
                       Id = 3,
                       Name = "Ali",
                       Class = 3,
                       Gender = GenderType.Male,
                       Email = "ali@gmail.com"
                   }
               );
            modelBuilder.Entity<Teacher>().HasData(
                    new Teacher
                    {
                        Id = 1,
                        Name = "Maria",
                        Age = 35,
                        Class = 5,
                        Subject = SubjectType.English,
                        Gender = GenderType.Female,
                        Email = "maria@gmail.com"
                    },
                   new Teacher
                   {
                       Id = 2,
                       Name = "Liam",
                       Age = 46,
                       Class = 6,
                       Subject = SubjectType.Maths,
                       Gender = GenderType.Male,
                       Email = "liam@gmail.com"
                   },
                   new Teacher
                   {
                       Id = 3,
                       Name = "George",
                       Age = 32,
                       Class = 3,
                       Subject = SubjectType.Science,
                       Gender = GenderType.Male,
                       Email = "george@gmail.com"
                   }

                );
        }
    }
}
