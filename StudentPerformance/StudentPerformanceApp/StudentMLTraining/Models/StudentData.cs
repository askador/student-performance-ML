using Microsoft.ML.Data;

namespace StudentMLTraining.Models;

public class StudentData
{
    [ColumnName(@"SchoolCode")]
    public string SchoolCode { get; set; }

    [ColumnName(@"Sex")]
    public string Sex { get; set; }

    [ColumnName(@"Age")]
    public float Age { get; set; }

    [ColumnName(@"Address")]
    public string Address { get; set; }

    [ColumnName(@"FamilySize")]
    public string FamilySize { get; set; }

    [ColumnName(@"ParentStatus")]
    public string ParentStatus { get; set; }

    [ColumnName(@"MotherEducation")]
    public float MotherEducation { get; set; }

    [ColumnName(@"FatherEducation")]
    public float FatherEducation { get; set; }

    [ColumnName(@"MotherJob")]
    public string MotherJob { get; set; }

    [ColumnName(@"FatherJob")]
    public string FatherJob { get; set; }

    [ColumnName(@"SchoolReason")]
    public string SchoolReason { get; set; }

    [ColumnName(@"Guardian")]
    public string Guardian { get; set; }

    [ColumnName(@"TravelTime")]
    public float TravelTime { get; set; }

    [ColumnName(@"StudyTime")]
    public float StudyTime { get; set; }

    [ColumnName(@"PastFailures")]
    public float PastFailures { get; set; }

    [ColumnName(@"ExtraEducationalSupport")]
    public string ExtraEducationalSupport { get; set; }

    [ColumnName(@"FamilyEducationalSupport")]
    public string FamilyEducationalSupport { get; set; }

    [ColumnName(@"PaidClasses")]
    public string PaidClasses { get; set; }

    [ColumnName(@"ExtraCurricularActivities")]
    public string ExtraCurricularActivities { get; set; }

    [ColumnName(@"AttendedNursery")]
    public string AttendedNursery { get; set; }

    [ColumnName(@"HigherEducationAspirations")]
    public string HigherEducationAspirations { get; set; }

    [ColumnName(@"InternetAccess")]
    public string InternetAccess { get; set; }

    [ColumnName(@"RomanticRelationship")]
    public string RomanticRelationship { get; set; }

    [ColumnName(@"FamilyRelationshipQuality")]
    public float FamilyRelationshipQuality { get; set; }

    [ColumnName(@"FreeTime")]
    public float FreeTime { get; set; }

    [ColumnName(@"GoingOutWithFriends")]
    public float GoingOutWithFriends { get; set; }

    [ColumnName(@"WorkdayAlcoholConsumption")]
    public float WorkdayAlcoholConsumption { get; set; }

    [ColumnName(@"WeekendAlcoholConsumption")]
    public float WeekendAlcoholConsumption { get; set; }

    [ColumnName(@"HealthStatus")]
    public float HealthStatus { get; set; }

    [ColumnName(@"Absences")]
    public float Absences { get; set; }

    [ColumnName(@"Course")]
    public string Course { get; set; }

    [ColumnName(@"G1")]
    public float G1 { get; set; }

    [ColumnName(@"G2")]
    public float G2 { get; set; }

    [ColumnName(@"G3")]
    public float G3 { get; set; }
}