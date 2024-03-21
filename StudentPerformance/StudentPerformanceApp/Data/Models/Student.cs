using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentPerformanceApp.Data.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Sex { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public string FamilySize { get; set; }
        public string ParentStatus { get; set; }
        public int MotherEducation { get; set; }
        public int FatherEducation { get; set; }
        public string MotherJob { get; set; }
        public string FatherJob { get; set; }
        public string SchoolReason { get; set; }
        public string Guardian { get; set; }
        public int TravelTime { get; set; }
        public int StudyTime { get; set; }
        public int PastFailures { get; set; }
        public string ExtraEducationalSupport { get; set; }
        public string FamilyEducationalSupport { get; set; }
        public string PaidClasses { get; set; }
        public string ExtraCurricularActivities { get; set; }
        public string AttendedNursery { get; set; }
        public string HigherEducationAspirations { get; set; }
        public string InternetAccess { get; set; }
        public string RomanticRelationship { get; set; }
        public int FamilyRelationshipQuality { get; set; }
        public int FreeTime { get; set; }
        public int GoingOutWithFriends { get; set; }
        public int WorkdayAlcoholConsumption { get; set; }
        public int WeekendAlcoholConsumption { get; set; }
        public int HealthStatus { get; set; }
        public int Absences { get; set; }

        public Grades Grades { get; set; }
        
        [ForeignKey("School")]
        public Guid SchoolId { get; set; }
        public School School { get; set; }
        
        [ForeignKey("Course")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}