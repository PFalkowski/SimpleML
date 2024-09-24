using Microsoft.ML;
using SimpleML.AlphaVintageClient;
using SimpleML.NeuralNetworks.Models;

namespace SimpleML.NeuralNetworks;

public class SdcaPredictor : ISdcaPredictor
{
    private MLContext _mlContext;
    private ITransformer _model;

    public SdcaPredictor()
    {
        _mlContext = new MLContext(seed: 0);
    }

    public void TrainModel(List<StockData> stockData)
    {
        // Prepare data
        var data = stockData.Select((s, i) => new StockPrice { DayIndex = i, Price = (float)s.Close }).ToList();
        var trainingData = _mlContext.Data.LoadFromEnumerable(data);

        // Define data preparation pipeline
        var pipeline = _mlContext.Transforms.Concatenate("Features", new[] { "DayIndex" })
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"))
            .Append(_mlContext.Transforms.CopyColumns("Label", "Price"))
            .Append(_mlContext.Regression.Trainers.Sdca(labelColumnName: "Label", featureColumnName: "Features"));

        // Train the model
        _model = pipeline.Fit(trainingData);
    }

    public float PredictPrice(float dayIndex)
    {
        var predictionEngine = _mlContext.Model.CreatePredictionEngine<StockPrice, StockPricePrediction>(_model);
        var prediction = predictionEngine.Predict(new StockPrice { DayIndex = dayIndex });
        return prediction.PredictedPrice;
    }

    public void EvaluateModel(List<StockData> testData)
    {
        var data = testData.Select((s, i) => new StockPrice { DayIndex = i, Price = (float)s.Close }).ToList();
        var testingData = _mlContext.Data.LoadFromEnumerable(data);

        var predictions = _model.Transform(testingData);
        var metrics = _mlContext.Regression.Evaluate(predictions);

        Console.WriteLine($"Root Mean Squared Error: {metrics.RootMeanSquaredError}");
        Console.WriteLine($"Mean Absolute Error: {metrics.MeanAbsoluteError}");
        Console.WriteLine($"R-Squared: {metrics.RSquared}");
    }
}