using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Transforms;
using Microsoft.ML.Trainers;
using SimpleML.AlphaVintageClient;
using Microsoft.Extensions.Configuration;
using SimpleML.Configuration;

public class StockPrice
{
    [LoadColumn(0)]
    public float DayIndex { get; set; }

    [LoadColumn(1)]
    public float Price { get; set; }
}

public class StockPricePrediction
{
    [ColumnName("Score")]
    public float PredictedPrice { get; set; }
}

public class NeuralNetworkStockPredictor
{
    private MLContext _mlContext;
    private ITransformer _model;

    public NeuralNetworkStockPredictor()
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

public class Program
{
    public static async Task Main()
    {
        var configFactory = new ConfigurationFactory();
        var dataProvider = new StockDataProvider(configFactory.GetConfiguration());
        var stockData = await dataProvider.GetData();

        var predictor = new NeuralNetworkStockPredictor();

        // Split data into training (80%) and testing (20%) sets
        var splitIndex = (int)(stockData.Count * 0.8);
        var trainingData = stockData.Take(splitIndex).ToList();
        var testingData = stockData.Skip(splitIndex).ToList();

        // Train the model
        Console.WriteLine("Training the neural network model...");
        predictor.TrainModel(trainingData);

        // Evaluate the model
        Console.WriteLine("Evaluating the model on test data...");
        predictor.EvaluateModel(testingData);

        // Make predictions
        Console.WriteLine("\nPredictions for the next 5 days:");
        for (var i = 1; i <= 5; i++)
        {
            var predictedPrice = predictor.PredictPrice(stockData.Count + i);
            Console.WriteLine($"Day {stockData.Count + i}: Predicted Price = {predictedPrice:C2}");
        }
    }
}