using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPerformanceApp.Data.Models
{
    public class School
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string SchoolName { get; set; }
        public string SchoolCode { get; set; } // (e.g., 'GP' or 'MS')
        
        public ICollection<Student> Students { get; set; }
    }
}