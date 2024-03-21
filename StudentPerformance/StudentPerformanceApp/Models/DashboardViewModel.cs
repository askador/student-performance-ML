using StudentPerformanceApp.Data.Models;

namespace StudentPerformanceApp.Models
{
    public class DashboardViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalSchools { get; set; }
        public List<School> Schools { get; set; }
        public int TotalCourses { get; set; }
        public List<Course> Courses { get; set; }
        public float AverageG1 { get; set; }
        public float AverageG2 { get; set; }
        public float AverageG3 { get; set; }
        public string[] GradesLabels { get; set; }
        public int[] G1GradesDistribution { get; set; }
        public int[] G2GradesDistribution { get; set; }
        public int[] G3GradesDistribution { get; set; }
    }
}
