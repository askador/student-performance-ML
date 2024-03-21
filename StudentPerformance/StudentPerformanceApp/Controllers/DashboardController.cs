using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using StudentPerformanceApp.Data;
using StudentPerformanceApp.Data.Models;
using StudentPerformanceApp.Models;
using System.Linq.Expressions;

namespace StudentPerformanceApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly StudentDbContext _context;
        public DashboardController(StudentDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var schools = await _context.Schools.ToListAsync();
            var courses = await _context.Courses.ToListAsync();

            var viewModel = new DashboardViewModel
            {
                TotalStudents = await _context.Students.CountAsync(),
                TotalSchools = schools.Count,
                TotalCourses = courses.Count,
                Schools = schools,
                Courses = courses,
                AverageG1 = await _context.Grades.Select(g => g.G1 ?? 0).AverageAsync(),
                AverageG2 = await _context.Grades.Select(g => g.G2 ?? 0).AverageAsync(),
                AverageG3 = await _context.Grades.Select(g => g.G3 ?? 0).AverageAsync(),
                GradesLabels = new[] { "F", "E", "D", "C", "B", "A" },
                G1GradesDistribution = await CalculateGradeDistribution("G1"),
                G2GradesDistribution = await CalculateGradeDistribution("G2"),
                G3GradesDistribution = await CalculateGradeDistribution("G3"),
            };

            return View(viewModel);
        }

        private async Task<int[]> CalculateGradeDistribution(
            string gradeColumn,
            List<Guid> selectedSchools = null,
            List<Guid> selectedCourses = null
            )
        {
            if (selectedSchools == null)
            {
                selectedSchools = new List<Guid>();
            }
            if (selectedCourses == null)
            {
                selectedCourses = new List<Guid>();
            }


            var query = _context.Grades
                .Where(g => g != null)
                .Where(g => g.G1 != null && g.G2 != null && g.G3 != null)
                .Include(g => g.Student)
                    .ThenInclude(s => s.School)
                .Include(g => g.Student)
                    .ThenInclude(s => s.Course)
                .Select(g => new { Grade = g, g.Student.SchoolId, g.Student.CourseId });

            if (selectedSchools.Any())
            {
                query = query.Where(x => selectedSchools.Contains(x.SchoolId));
            }
            if (selectedCourses.Any())
            {
                query = query.Where(x => selectedCourses.Contains(x.CourseId));
            }

            var grades = await query
                .Select(x => x.Grade.GetType().GetProperty(gradeColumn).GetValue(x.Grade))
                .ToListAsync();


            var gradeDistribution = new int[6]; // Grades are in the range A to F

            foreach (var grade in grades)
            {
                switch (grade)
                {
                    case float g when g >= 16: // A
                        gradeDistribution[5]++;
                        break;
                    case float g when g >= 14: // B
                        gradeDistribution[4]++;
                        break;
                    case float g when g >= 12: // C
                        gradeDistribution[3]++;
                        break;
                    case float g when g >= 10: // D
                        gradeDistribution[2]++;
                        break;
                    case float g when g >= 8: // E
                        gradeDistribution[1]++;
                        break;
                    default: // F
                        gradeDistribution[0]++;
                        break;
                }
            }

            return gradeDistribution;
        }

        [HttpPost]
        public async Task<IActionResult> GetFilteredChartData(List<Guid> selectedSchools, List<Guid> selectedCourses)
        {
            var g1Data = await CalculateGradeDistribution("G1", selectedSchools, selectedCourses);
            var g2Data = await CalculateGradeDistribution("G2", selectedSchools, selectedCourses);
            var g3Data = await CalculateGradeDistribution("G3", selectedSchools, selectedCourses);

            return Json(new { g1Data, g2Data, g3Data });
        }
    }
}
