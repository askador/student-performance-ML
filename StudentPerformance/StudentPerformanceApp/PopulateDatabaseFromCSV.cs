using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceApp.Data;

namespace StudentPerformanceApp
{
    public class PopulateDatabaseFromCSV
    {
        public static void Main()
        {
            Console.WriteLine("Populating the students database");

            var csvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "data-student-performance", "students-math-and-por.csv");

            using (var context = new StudentDbContext())
            {
                StudentSchoolGradesCourses eduData = ParseCSV(csvPath);

                context.Schools.AddRange(eduData.Schools);
                context.Courses.AddRange(eduData.Courses);

                context.Students.AddRange(eduData.Students);
                context.Grades.AddRange(eduData.Grades);
                context.SaveChanges();
            }
            Console.WriteLine("Populating process is finished");
        }

        static StudentSchoolGradesCourses ParseCSV(string filePath)
        {
            StudentSchoolGradesCourses eduData = new StudentSchoolGradesCourses();

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CSVStudent>().ToList();

                foreach (var record in records)
                {
                    Guid schoolId;
                    Guid courseId;
                    var school = eduData.Schools.Find(s => s?.SchoolCode == record.SchoolCode);
                    var course = eduData.Courses.Find(c => c?.CourseName == record.Course);

                    if (school != null)
                    {
                        schoolId = school.Id;
                    }
                    else
                    {
                        schoolId = Guid.NewGuid();
                        eduData.Schools.Add(new Data.Models.School
                        {
                            Id = schoolId,
                            SchoolCode = record.SchoolCode,
                            SchoolName = record.SchoolName
                        });
                    }

                    if (course != null)
                    {
                        courseId = course.Id;
                    }
                    else
                    {
                        courseId = Guid.NewGuid();
                        eduData.Courses.Add(new Data.Models.Course
                        {
                            Id = courseId,
                            CourseName = record.Course,
                        });
                    }

                    Guid studentId = Guid.NewGuid();
                    var student = new Data.Models.Student
                    {
                        Id = studentId,
                        Name = record.Sex == "F" ? Faker.NameFaker.FemaleName() : Faker.NameFaker.MaleName(),
                        Sex = record.Sex,
                        Age = record.Age,
                        Address = record.Address,
                        FamilySize = record.FamilySize,
                        ParentStatus = record.ParentStatus,
                        MotherEducation = record.MotherEducation,
                        FatherEducation = record.FatherEducation,
                        MotherJob = record.MotherJob,
                        FatherJob = record.FatherJob,
                        SchoolReason = record.SchoolReason,
                        Guardian = record.Guardian,
                        TravelTime = record.TravelTime,
                        StudyTime = record.StudyTime,
                        PastFailures = record.PastFailures,
                        ExtraEducationalSupport = record.ExtraEducationalSupport,
                        FamilyEducationalSupport = record.FamilyEducationalSupport,
                        PaidClasses = record.PaidClasses,
                        ExtraCurricularActivities = record.ExtraCurricularActivities,
                        AttendedNursery = record.AttendedNursery,
                        HigherEducationAspirations = record.HigherEducationAspirations,
                        InternetAccess = record.InternetAccess,
                        RomanticRelationship = record.RomanticRelationship,
                        FamilyRelationshipQuality = record.FamilyRelationshipQuality,
                        FreeTime = record.FreeTime,
                        GoingOutWithFriends = record.GoingOutWithFriends,
                        WorkdayAlcoholConsumption = record.WorkdayAlcoholConsumption,
                        WeekendAlcoholConsumption = record.WeekendAlcoholConsumption,
                        HealthStatus = record.HealthStatus,
                        Absences = record.Absences,
                        SchoolId = schoolId,
                        CourseId = courseId
                    };

                    eduData.Students.Add(student);

                    var grades = new Data.Models.Grades
                    {
                        StudentId = studentId,
                        G1 = record.G1,
                        G2 = record.G2,
                        G3 = record.G3
                    };

                    eduData.Grades.Add(grades);
                }

                return eduData;
            }
        }

        private class StudentSchoolGradesCourses
        {
            public List<Data.Models.Student> Students = new List<Data.Models.Student>();
            public List<Data.Models.School> Schools = new List<Data.Models.School>();
            public List<Data.Models.Grades> Grades = new List<Data.Models.Grades>();
            public List<Data.Models.Course> Courses = new List<Data.Models.Course>();
        }

        class CSVStudent
        {
            // MUST HAVE THE SAME ORDER AS A CSV HEADERS
            public string SchoolCode { get; set; }
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
            public float G1 { get; set; }
            public float G2 { get; set; }
            public float G3 { get; set; }
            public string Course { get; set; }
            public string SchoolName { get; set; }
        }
    }
}