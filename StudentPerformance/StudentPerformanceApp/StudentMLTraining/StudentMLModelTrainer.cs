using Microsoft.ML;
using Microsoft.ML.Trainers.FastTree;
using Microsoft.ML.Transforms;
using Microsoft.ML.Data;
using StudentMLTraining.Models;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceApp.Data;
using StudentPerformanceApp.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace StudentMLTraining;

public class StudentMLModelTrainer
{

    public static readonly string ModelPath = Path.Combine(Directory.GetCurrentDirectory(), "StudentMLTraining", "TrainedMLModels", "student_performance_model.mlnet");


    public void TrainEvaluateSave(IEnumerable<StudentData> students)
    {
        var mlContext = new MLContext();
        var splitData = ToTrainTestData(students);

        var model = TrainMLModel(splitData.TrainSet);

        EvaluateMLModel(model, splitData.TestSet);

        SaveModel(model, splitData.TrainSet);
    }

    public DataOperationsCatalog.TrainTestData ToTrainTestData(IEnumerable<StudentData> students)
    {
        var mlContext = new MLContext();
        var mlData = mlContext.Data.LoadFromEnumerable(students);
        var splitData = mlContext.Data.TrainTestSplit(mlData, 0.3F);

        while (!IsSameSchema(mlContext, splitData.TrainSet, splitData.TestSet))
        {
            splitData = mlContext.Data.TrainTestSplit(mlData, 0.3F);
        }

        return splitData;
    }

    public IDataView ToDataView(IEnumerable<StudentData> students)
    {
        var mlContext = new MLContext();
        return mlContext.Data.LoadFromEnumerable(students);
    }

    public ITransformer TrainMLModel(IDataView trainData)
    {
        var mlContext = new MLContext();

        var dataPrepPipeline = PrepareDataPipeline(mlContext);

        var trainer = PrepareFastTreeRegressionTrainer(mlContext);
        var pipeline = dataPrepPipeline.Append(trainer);

        var model = pipeline.Fit(trainData);

        return model;
    }

    public RegressionMetrics EvaluateMLModel(ITransformer mlModel, IDataView testData)
    {
        var mlContext = new MLContext();

        var predictions = mlModel.Transform(testData);
        var metrics = mlContext.Regression.Evaluate(predictions, "G3", "Score");

        Console.WriteLine();
        Console.WriteLine($"*************************************************");
        Console.WriteLine($"*       Model quality metrics evaluation         ");
        Console.WriteLine($"*------------------------------------------------");
        Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
        Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
        Console.WriteLine($"*------------------------------------------------");

        return metrics;
    }

    public RegressionMetrics EvaluateCurrentMLModel(IDataView testData)
    {
        var mlContext = new MLContext();

        ITransformer model = mlContext.Model.Load(ModelPath, out _);

        var predictions = model.Transform(testData);
        var metrics = mlContext.Regression.Evaluate(predictions, "G3", "Score");

        return metrics;
    }


    public StudentPrediction PredictG3(StudentData student)
    {
        var mlContext = new MLContext();

        DataViewSchema predictionPipelineSchema;
        ITransformer predictionPipeline = mlContext.Model.Load(ModelPath, out predictionPipelineSchema);
        var predictionEngine = mlContext.Model.CreatePredictionEngine<StudentData, StudentPrediction>(predictionPipeline);
        var prediction = predictionEngine.Predict(student);
        return prediction;
    }

    private EstimatorChain<ColumnConcatenatingTransformer> PrepareDataPipeline(MLContext mlContext)
    {
        var dataPrepPipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "G3")
            .Append(mlContext.Transforms.Categorical.OneHotEncoding(new[] {
                new InputOutputColumnPair(@"SchoolCode", @"SchoolCode"),
                new InputOutputColumnPair(@"Sex", @"Sex"),
                new InputOutputColumnPair(@"Address", @"Address"),
                new InputOutputColumnPair(@"FamilySize", @"FamilySize"),
                new InputOutputColumnPair(@"ParentStatus", @"ParentStatus"),
                new InputOutputColumnPair(@"MotherJob", @"MotherJob"),
                new InputOutputColumnPair(@"FatherJob", @"FatherJob"),
                new InputOutputColumnPair(@"MotherEducation", @"MotherEducation"),
                new InputOutputColumnPair(@"FatherEducation", @"FatherEducation"),
                new InputOutputColumnPair(@"SchoolReason", @"SchoolReason"),
                new InputOutputColumnPair(@"Guardian", @"Guardian"),
                new InputOutputColumnPair(@"TravelTime", @"TravelTime"),
                new InputOutputColumnPair(@"StudyTime", @"StudyTime"),
                new InputOutputColumnPair(@"PastFailures", @"PastFailures"),
                new InputOutputColumnPair(@"ExtraEducationalSupport", @"ExtraEducationalSupport"),
                new InputOutputColumnPair(@"FamilyEducationalSupport", @"FamilyEducationalSupport"),
                new InputOutputColumnPair(@"PaidClasses", @"PaidClasses"),
                new InputOutputColumnPair(@"ExtraCurricularActivities", @"ExtraCurricularActivities"),
                new InputOutputColumnPair(@"AttendedNursery", @"AttendedNursery"),
                new InputOutputColumnPair(@"HigherEducationAspirations", @"HigherEducationAspirations"),
                new InputOutputColumnPair(@"InternetAccess", @"InternetAccess"),
                new InputOutputColumnPair(@"RomanticRelationship", @"RomanticRelationship"),
                new InputOutputColumnPair(@"FamilyRelationshipQuality", @"FamilyRelationshipQuality"),
                new InputOutputColumnPair(@"FreeTime", @"FreeTime"),
                new InputOutputColumnPair(@"GoingOutWithFriends", @"GoingOutWithFriends"),
                new InputOutputColumnPair(@"WorkdayAlcoholConsumption", @"WorkdayAlcoholConsumption"),
                new InputOutputColumnPair(@"WeekendAlcoholConsumption", @"WeekendAlcoholConsumption"),
                new InputOutputColumnPair(@"HealthStatus", @"HealthStatus"),
                new InputOutputColumnPair(@"Course", @"Course")
            }, outputKind: OneHotEncodingEstimator.OutputKind.Indicator))
            .Append(mlContext.Transforms.Concatenate(@"Features", new[] { 
                @"SchoolCode", @"Sex", @"Address", @"FamilySize", @"ParentStatus", @"MotherJob", 
                @"FatherJob", @"SchoolReason", @"Guardian", @"ExtraEducationalSupport", 
                @"FamilyEducationalSupport", @"PaidClasses", @"ExtraCurricularActivities", 
                @"AttendedNursery", @"HigherEducationAspirations", @"InternetAccess", @"RomanticRelationship", 
                @"Course", @"Age", @"MotherEducation", @"FatherEducation", @"TravelTime", @"StudyTime", 
                @"PastFailures", @"FamilyRelationshipQuality", @"FreeTime", @"GoingOutWithFriends", 
                @"WorkdayAlcoholConsumption", @"WeekendAlcoholConsumption", @"HealthStatus", @"Absences", @"G1", @"G2" 
            }));

        return dataPrepPipeline;
    }

    private FastTreeRegressionTrainer PrepareFastTreeRegressionTrainer(MLContext mlContext)
    {

        var trainer = mlContext.Regression.Trainers.FastTree(
            new FastTreeRegressionTrainer.Options()
            {
                NumberOfLeaves = 8,
                MinimumExampleCountPerLeaf = 14,
                NumberOfTrees = 645,
                MaximumBinCountPerFeature = 899,
                FeatureFraction = 0.93,
                LearningRate = 0.00840364998277254,
                LabelColumnName = "G3",
                FeatureColumnName = "Features"
            }
        );

        return trainer;
    }

    public void SaveModel(ITransformer model, IDataView data)
    {
        var mlContext = new MLContext();
        DataViewSchema dataViewSchema = data.Schema;

        using (var fs = File.Create(ModelPath))
        {
            Console.WriteLine();
            Console.WriteLine($"Saving the model to: {ModelPath}");
            mlContext.Model.Save(model, dataViewSchema, fs);
        }
    }

    private IEnumerable<string> GetUniqueValues(MLContext mlContext, IDataView dataView, string featureColumnName)
    {
        return mlContext.Data.CreateEnumerable<StudentData>(dataView, reuseRowObject: false)
            .Select(data => data.GetType().GetProperty(featureColumnName).GetValue(data).ToString())
            .Distinct();
    }

    private bool IsSameSchema(MLContext mlContext, IDataView trainData, IDataView testData)
    {
        var trainSchema = trainData.Schema;

        foreach (var trainColumn in trainSchema)
        {
            var featureColumnName = trainColumn.Name;

            string[] notToCheck = { "Age", "Absences", "G1", "G2", "G3" };

            if (notToCheck.Contains(featureColumnName))
            {
                continue;
            }

            var trainUniqueValues = GetUniqueValues(mlContext, trainData, featureColumnName);
            var testUniqueValues = GetUniqueValues(mlContext, testData, featureColumnName);

            if (trainUniqueValues.Count() != testUniqueValues.Count())
            {
                Console.WriteLine($"{featureColumnName} Values Count Train:{trainUniqueValues.Count()} != Test:{testUniqueValues.Count()}");
                Console.WriteLine($"Train: [{string.Join(",", trainUniqueValues.ToArray())}]");
                Console.WriteLine($"Test: [{string.Join(",", testUniqueValues.ToArray())}]");
                Console.WriteLine();
                return false;
            }
        }

        return true;
    }
}
