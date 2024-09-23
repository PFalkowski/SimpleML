using SimpleML.AlphaVintageClient;
using SimpleML.Configuration;
using SimpleML.FourierSeries;


public class Program
{
    public static async Task Main()
    {
        // Assume we have a method to load stock data
        var config = new ConfigurationFactory().GetConfiguration();
        var stockData = await new StockDataProvider(config).GetData();
        var prices = stockData.Select(d => d.Close).ToList();

        var predictor = new FourierSeriesPredictor(numCoefficients: 10);
        predictor.Fit(prices);

        var futureDays = 30;
        var predictions = predictor.Predict(futureDays);

        Console.WriteLine("Predictions for the next 30 days:");
        for (var i = 0; i < predictions.Count; i++)
        {
            Console.WriteLine($"Day {i + 1}: {predictions[i]:C2}");
        }
    }
}