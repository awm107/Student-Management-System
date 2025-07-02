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
    [Authorize(Roles = "Admin, Teacher, Class Teacher")]
    public class TeacherController : Controller
    {
        private readonly ITeacherRepository _teacherRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment hostingEnvironment;

        public TeacherController(ITeacherRepository teacherRepository, IStudentRepository studentRepository, IWebHostEnvironment hostingEnvironment)
        {
            _studentRepository = studentRepository;
            _teacherRepository = teacherRepository;
            this.hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        public ViewResult Index()
        {
            var model = _teacherRepository.GetAllTeachers();
            return View(model);
        }
        public ViewResult Details(int Id)
        {
            Teacher teacher = _teacherRepository.GetTeacher(Id);

            if (teacher == null)
            {
                Response.StatusCode = 404;
                return View("TeacherNotFound", Id);
            }

            //return View(teacher);
            Teacher model = _teacherRepository.GetTeacher(Id);
            ViewBag.PageTitle = "Teacher Details";
            //return View(model);
            TeacherDetailsViewModel teacherDetailsViewModel = new TeacherDetailsViewModel
            {
                teacher = _teacherRepository.GetTeacher(Id),
                PageTitle = "Teacher Details"
            };
            return View(teacherDetailsViewModel);
            
        }

        private string ProcessUploadFile(TeacherCreateViewModel model)
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
        public IActionResult Create(TeacherCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadFile(model);

                Teacher newTeacher = new Teacher
                {
                    Name = model.Name,
                    Email = model.Email,
                    Age = model.Age,
                    Gender = model.Gender,
                    Class = model.Class,
                    Subject = (SubjectType)model.Subject,
                    isClassTeacher = model.isClassTeacher,
                    Photopath = uniqueFileName
                };
                _teacherRepository.Add(newTeacher);
                return RedirectToAction("details", new { id = newTeacher.Id });
            }
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "AdminEditRolePolicy")]
        public ViewResult Edit(int id)
        {
            Teacher Teacher = _teacherRepository.GetTeacher(id);
            TeacherEditViewModel TeacherEditViewModel = new TeacherEditViewModel
            {
                Id = Teacher.Id,
                Name = Teacher.Name,
                Email = Teacher.Email,
                Age = Teacher.Age,
                Gender = Teacher.Gender,
                Class = Teacher.Class,
                Subject = Teacher.Subject,
                ExistingPhotoPath = Teacher.Photopath,
                isClassTeacher = Teacher.isClassTeacher
            };
            return View(TeacherEditViewModel);
        }

        // Through model binding, the action method parameter
        // TeacherEditViewModel receives the posted edit form data
        [HttpPost]
        [Authorize(Policy = "AdminEditRolePolicy")]
        public IActionResult Edit(TeacherEditViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the Teacher being edited from the database
                Teacher Teacher = _teacherRepository.GetTeacher(model.Id);
                // Update the Teacher object with the data in the model object
                Teacher.Name = model.Name;
                Teacher.Email = model.Email;
                Teacher.Age = model.Age;
                Teacher.Class = model.Class;
                Teacher.Subject = (SubjectType)model.Subject;
                Teacher.isClassTeacher = model.isClassTeacher;

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
                    // PhotoPath property of the Teacher object which will be
                    // eventually saved in the database
                    Teacher.Photopath = ProcessUploadFile(model);
                }

                // Call update method on the repository service passing it the
                // Teacher object to update the data in the database table
                Teacher updatedTeacher = _teacherRepository.Update(Teacher);

                return RedirectToAction("index");
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "AdminDeleteRolePolicy")]
        public IActionResult Delete(int id)
        {
            Teacher exTeacher = _teacherRepository.GetTeacher(id);

            if (exTeacher == null)
            {
                return View("TeacherNotFound");
            }
            TeacherDeleteViewModel teacherDeleteViewModel = new TeacherDeleteViewModel
            {
                Id = exTeacher.Id,
                teacher = exTeacher,
                PageTitle = "Teacher Delete"
            };
            return View(teacherDeleteViewModel);
        }

        [HttpPost]
        [Authorize(Policy = "AdminDeleteRolePolicy")]
        public IActionResult Delete(TeacherDeleteViewModel model)
        {
            Teacher exTeacher = _teacherRepository.GetTeacher(model.Id);
            if (ModelState.IsValid)
            {
                if (exTeacher == null)
                {
                    Response.StatusCode = 404;
                    return View("TeacherNotFound", exTeacher.Id);
                }

                // Delete the teacher photo if it exists
                if (exTeacher.Photopath != null)
                {
                    string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", exTeacher.Photopath);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                // Delete the teacher from the repository
                _teacherRepository.Delete(exTeacher.Id);

                return RedirectToAction("Index");

            }
            return View(model);
        }

        [Authorize(Policy = "ClassTeacherRolePolicy")]
        [HttpGet]
        public ViewResult Attendance(int id)
        {
            // Retrieve the teacher by ID
            Teacher classTeacher = _teacherRepository.GetTeacher(id);

            // Check if the teacher is null
            if (classTeacher == null)
            {
                return View("TeacherNotFound");
            }

            // Retrieve the list of students for the teacher's class
            uint classId = classTeacher.Class;
            List<Student> studentList = (List<Student>)_studentRepository.GetAllStudentsByClass(classId).ToList(); // Convert IQueryable or IEnumerable to List;

            // Prepare the view model
            StudentAttendanceViewModel studentAttendanceViewModel = new StudentAttendanceViewModel
            {
                Id = classTeacher.Id,
                Teacher = classTeacher,
                StudentClassList = studentList
            };

            // Return the view with the view model
            return View(studentAttendanceViewModel);
        }

    }
}
