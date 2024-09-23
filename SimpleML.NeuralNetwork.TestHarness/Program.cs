using SimpleML.AlphaVintageClient;
using SimpleML.Configuration;
using SimpleML.NeuralNetworks;


public class Program
{
    public static async Task Main()
    {
        var configFactory = new ConfigurationFactory();
        var dataProvider = new StockDataProvider(configFactory.GetConfiguration());
        var stockData = await dataProvider.GetData();

        var predictor = new SdcaPredictor();

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