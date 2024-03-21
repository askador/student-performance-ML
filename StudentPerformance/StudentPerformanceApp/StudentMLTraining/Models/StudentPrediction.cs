using Microsoft.ML.Data;

namespace StudentMLTraining.Models;

public class StudentPrediction
{
    [ColumnName(@"Score")]
    public float G3 { get; set; }
}
