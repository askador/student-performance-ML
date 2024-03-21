using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using StudentMLTraining;
using StudentMLTraining.Models;
using StudentPerformanceApp.Data;
using StudentPerformanceApp.Data.Models;
using StudentPerformanceApp.Models;
using System.Web.Helpers;

namespace StudentPerformanceApp.Controllers
{
    public class MLModelController : Controller
    {
        private readonly StudentDbContext _context;

        public MLModelController(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var mlTrainer = new StudentMLModelTrainer();

            var modelInputs = await GetTrainStudents();

            var splitData = mlTrainer.ToTrainTestData(modelInputs);
            var metrics = mlTrainer.EvaluateCurrentMLModel(splitData.TestSet);

            var viewModel = new MLModelViewModel() {
                CurrentMetrics = metrics,
            };

            return View(viewModel);
        }

        private async Task<List<StudentData>> GetTrainStudents()
        {
            return await _context.Students
                .Include(s => s.School)
                .Include(s => s.Course)
                .Include(s => s.Grades)
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
                .ToListAsync();
        }

        [HttpPost]
        public async Task<JsonResult> RetrainModel()
        {
            var modelInputs = await GetTrainStudents();
            var mlTrainer = new StudentMLModelTrainer();
            var splitData = mlTrainer.ToTrainTestData(modelInputs);

            try
            {
                var newModel = mlTrainer.TrainMLModel(splitData.TrainSet);
                mlTrainer.SaveModel(newModel, splitData.TrainSet);
            } catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

            RedirectToAction(nameof(Index));
            return Json(new { success = true });

        }
    }
}
