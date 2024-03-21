using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace StudentPerformanceApp.Data.Models
{
    public class Grades
    {
        [Key]
        public Guid StudentId { get; set; }

        [Required]
        [ForeignKey("StudentId")]
        public Student Student { get; set; }

        public float? G1 { get; set; }
        public float? G2 { get; set; }
        public float? G3 { get; set; }
    }
}