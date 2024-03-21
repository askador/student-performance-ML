using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.ML;
using StudentMLTraining;
using StudentMLTraining.Models;
using StudentPerformanceApp.Data;
using StudentPerformanceApp.Data.Models;
using StudentPerformanceApp.Models;
using StudentPerformanceApp.Models.misc;
using System.Net;
using System.Xml.Linq;

namespace StudentPerformanceApp.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StudentDbContext _context;

        public StudentsController(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new StudentViewModel();

            var students = await _context.Students
                .Include(s => s.Course)
                .Include(s => s.School)
                .Include(s => s.Grades)
                .ToListAsync();

            var topStudents = await _context.Students
                .Where(s => s.Grades != null)
                .Where(s => s.Grades.G1 != null && s.Grades.G2 != null && s.Grades.G3 != null)
                .OrderByDescending(s => (s.Grades.G1 + s.Grades.G2 + s.Grades.G3) / 3)
                .Take(5)
                .ToListAsync();

            viewModel.Students = students;
            viewModel.TopStudents = topStudents;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> GenerateG3([FromBody] Guid id)
        {

            try
            {
                var student = await _context.Students
                    .Include(s => s.School)
                    .Include(s => s.Course)
                    .Include(s => s.Grades)
                    .Where(s => s.Id == id)
                    .Where(s => s.Grades != null)
                    .Where(s => s.Grades.G1 != null && s.Grades.G2 != null && s.Grades.G3 != null)
                    .Select(s => new StudentData
                    {
                        SchoolCode = s.School.SchoolCode,
                        Sex = s.Sex,
                        Age = s.Age,
                        Address = s.Address,
                        FamilySize = s.FamilySize,
                        ParentStatus = s.ParentStatus,
                        MotherEducation = s.MotherEducation,
                        FatherEducation = s.FatherEducation,
                        MotherJob = s.MotherJob,
                        FatherJob = s.FatherJob,
                        SchoolReason = s.SchoolReason,
                        Guardian = s.Guardian,
                        TravelTime = s.TravelTime,
                        StudyTime = s.StudyTime,
                        PastFailures = s.PastFailures,
                        ExtraEducationalSupport = s.ExtraEducationalSupport,
                        FamilyEducationalSupport = s.FamilyEducationalSupport,
                        PaidClasses = s.PaidClasses,
                        ExtraCurricularActivities = s.ExtraCurricularActivities,
                        AttendedNursery = s.AttendedNursery,
                        HigherEducationAspirations = s.HigherEducationAspirations,
                        InternetAccess = s.InternetAccess,
                        RomanticRelationship = s.RomanticRelationship,
                        FamilyRelationshipQuality = s.FamilyRelationshipQuality,
                        FreeTime = s.FreeTime,
                        GoingOutWithFriends = s.GoingOutWithFriends,
                        WorkdayAlcoholConsumption = s.WorkdayAlcoholConsumption,
                        WeekendAlcoholConsumption = s.WeekendAlcoholConsumption,
                        HealthStatus = s.HealthStatus,
                        Absences = s.Absences,
                        Course = s.Course.CourseName,
                        G1 = (float)s.Grades.G1,
                        G2 = (float)s.Grades.G2,
                        G3 = (float)s.Grades.G3
                    })
                    .FirstAsync();

                var mlTrainer = new StudentMLModelTrainer();
                var prediction = mlTrainer.PredictG3(student);

                return Json(new { success = true, id, value = prediction.G3 });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSchools()
        {
            var schools = await _context.Schools.ToListAsync();
            return Json(schools);
        }

        [HttpGet]
        public JsonResult GetCourses()
        {
            var courses = _context.Courses.ToList();
            return Json(courses);
        }

        [HttpPost]
        public async Task<IActionResult> Create(StudentCreateForm formStudent)
        {
            ModelState.Remove("G3");
            if (ModelState.IsValid)
            {
                var studentId = Guid.NewGuid();
                var student = new Student() {
                    Id = studentId,
                    Name = formStudent.Name,
                    Sex = formStudent.Sex,
                    Age = formStudent.Age,
                    Address = formStudent.Address,
                    FamilySize = formStudent.FamilySize,
                    ParentStatus = formStudent.ParentStatus,
                    MotherEducation = formStudent.MotherEducation,
                    FatherEducation = formStudent.FatherEducation,
                    MotherJob = formStudent.MotherJob,
                    FatherJob = formStudent.FatherJob,
                    SchoolReason = formStudent.SchoolReason,
                    Guardian = formStudent.Guardian,
                    TravelTime = formStudent.TravelTime,
                    StudyTime = formStudent.StudyTime,
                    PastFailures = formStudent.PastFailures,
                    ExtraEducationalSupport = formStudent.ExtraEducationalSupport,
                    FamilyEducationalSupport = formStudent.FamilyEducationalSupport,
                    PaidClasses = formStudent.PaidClasses,
                    ExtraCurricularActivities = formStudent.ExtraCurricularActivities,
                    AttendedNursery = formStudent.AttendedNursery,
                    HigherEducationAspirations = formStudent.HigherEducationAspirations,
                    InternetAccess = formStudent.InternetAccess,
                    RomanticRelationship = formStudent.RomanticRelationship,
                    FamilyRelationshipQuality = formStudent.FamilyRelationshipQuality,
                    FreeTime = formStudent.FreeTime,
                    GoingOutWithFriends = formStudent.GoingOutWithFriends,
                    WorkdayAlcoholConsumption = formStudent.WorkdayAlcoholConsumption,
                    WeekendAlcoholConsumption = formStudent.WeekendAlcoholConsumption,
                    HealthStatus = formStudent.HealthStatus,
                    Absences = formStudent.Absences,
                    SchoolId = formStudent.SchoolId,
                    CourseId = formStudent.CourseId,
                };

                var grades = new Grades() { 
                    StudentId = studentId,
                    G1 = formStudent.G1,
                    G2 = formStudent.G2,
                    G3 = formStudent.G3,
                };

                _context.Add(student);
                _context.Add(grades);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            if (id == null || _context.Students == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var student = await _context.Students
                .Include(s => s.Course)
                .Include(s => s.School)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
