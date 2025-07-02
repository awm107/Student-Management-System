using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Models;
using StudentManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Controllers
{
    [Authorize(Roles = "Admin, Student")]
    public class StudentController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public StudentController(IStudentRepository studentRepository, IWebHostEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        public ViewResult Index()
        {
            var model = _studentRepository.GetAllStudents();
            return View(model);
        }
        //[Authorize(Policy ="AdminRolePolicy")]
        public ViewResult Details(int Id)
        {
            Student student = _studentRepository.GetStudent(Id);

            if (student == null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound", Id);
            }
            //Student model = _studentRepository.GetStudent(Id);
            //ViewBag.PageTitle = "Student Details";
            //return View(model);
            StudentDetailsViewModel studentDetailsViewModel = new StudentDetailsViewModel
            {
                student = _studentRepository.GetStudent(Id),
                PageTitle = "Student Details"
            };
            return View(studentDetailsViewModel);
            //return View(student);
        }

        private string ProcessUploadFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var filesStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(filesStream);
                }
            }

            return uniqueFileName;
        }

        [HttpGet]
        [Authorize(Policy = "AdminCreateRolePolicy")]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Policy = "AdminCreateRolePolicy")]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadFile(model);

                Student newStudent = new Student
                {
                    Name = model.Name,
                    Email = model.Email,
                    Class = model.Class,
                    Gender = model.Gender,
                    Photopath = uniqueFileName
                };
                _studentRepository.Add(newStudent);
                return RedirectToAction("details", new { id = newStudent.Id });
            }
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "AdminEditRolePolicy")]
        public ViewResult Edit(int id)
        {
            Student Student = _studentRepository.GetStudent(id);
            StudentEditViewModel StudentEditViewModel = new StudentEditViewModel
            {
                Id = Student.Id,
                Name = Student.Name,
                Email = Student.Email,
                Class = Student.Class,
                Gender = Student.Gender,
                ExistingPhotoPath = Student.Photopath
            };
            return View(StudentEditViewModel);
        }

        // Through model binding, the action method parameter
        // StudentEditViewModel receives the posted edit form data
        [HttpPost]
        [Authorize(Policy = "AdminEditRolePolicy")]
        public IActionResult Edit(StudentEditViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the Student being edited from the database
                Student Student = _studentRepository.GetStudent(model.Id);
                // Update the Student object with the data in the model object
                Student.Name = model.Name;
                Student.Email = model.Email;
                Student.Class = model.Class;

                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.Photo != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath,
                            "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the Student object which will be
                    // eventually saved in the database
                    Student.Photopath = ProcessUploadFile(model);
                }

                // Call update method on the repository service passing it the
                // Student object to update the data in the database table
                Student updatedStudent = _studentRepository.Update(Student);

                return RedirectToAction("index");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "AdminDeleteRolePolicy")]
        public IActionResult Delete(int id)
        {
            Student exStudent = _studentRepository.GetStudent(id);

            if (exStudent == null)
            {
                return View("StudentNotFound");
            }
            StudentDeleteViewModel studentDeleteViewModel = new StudentDeleteViewModel
            {
                Id = exStudent.Id,
                student = exStudent,
                PageTitle = "Student Delete"
            };
            return View(studentDeleteViewModel);
        }

        [HttpPost]
        [Authorize(Policy = "AdminDeleteRolePolicy")]
        public IActionResult Delete(StudentDeleteViewModel model)
        {
            Student exStudent = _studentRepository.GetStudent(model.Id);
            if (ModelState.IsValid)
            {
                if (exStudent == null)
                {
                    Response.StatusCode = 404;
                    return View("StudentNotFound", exStudent.Id);
                }

                // Delete the student's photo if it exists
                if (exStudent.Photopath != null)
                {
                    string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", exStudent.Photopath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Delete the student from the repository
                _studentRepository.Delete(exStudent.Id);

                return RedirectToAction("Index");

            }
            return View(model);
        }

        public IActionResult Enroll(int Id)
        {
            var model = new SubjectsEnrolledViewModel
            {
                Subjects = Enum.GetValues(typeof(SubjectType)).Cast<SubjectType>().ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Enroll(SubjectsEnrolledViewModel model)
        {
            // Retrieve the selected subjects from the model
            var selectedSubjects = model.SelectedSubjects;

            // Example processing: count the number of selected subjects
            var count = selectedSubjects.Count;

            // Example processing: do something with the selected subjects
            // For example, you could save them to a database or perform some calculations.

            // Add a message to the view to show the number of selected subjects
            ViewBag.Message = $"You have selected {count} subject(s).";

            // Re-populate the model with the list of subjects (if you need to re-render the form)
            model.Subjects = Enum.GetValues(typeof(SubjectType)).Cast<SubjectType>().ToList();

            // Return the view with the model, including the message
            return View(model);
        }

    }
}
