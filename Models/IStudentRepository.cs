﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Models
{
    public interface IStudentRepository
    {
        Student GetStudent(int id);
        IEnumerable<Student> GetAllStudents();
        IEnumerable<Student> GetAllStudentsByClass(uint classId);

        Student Add(Student student);
        Student Update(Student studentChanges);
        Student Delete(int id);
    }
}
