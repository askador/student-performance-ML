using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using StudentMLTraining.Models;
using StudentPerformanceApp.Data;

namespace StudentMLTraining;

public class Pogram
{
    public static void Main()
    {
        var dbContext = new StudentDbContext();

        var modelInputs = dbContext.Students
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
                .ToList();

        dbContext.Database.CloseConnection();



        var mlTrainer = new StudentMLModelTrainer();
        mlTrainer.TrainEvaluateSave(modelInputs);

        var predictions = mlTrainer.PredictG3(modelInputs[0]);
        Console.WriteLine(predictions.G3);

        Console.ReadKey();
    }
}