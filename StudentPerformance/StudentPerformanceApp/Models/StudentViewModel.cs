using X.PagedList;
using StudentPerformanceApp.Data.Models;

namespace StudentPerformanceApp.Models
{
    public class StudentViewModel
    {
        public IEnumerable<Student> Students { get; set; }
        public IEnumerable<Student> TopStudents { get; set; }
    }
}
