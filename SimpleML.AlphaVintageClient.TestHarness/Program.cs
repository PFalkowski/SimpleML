using Microsoft.Extensions.Configuration;
using SimpleML.AlphaVintageClient;
using SimpleML.Configuration;
using SimpleML.MonteCarlo;

var configuration = new ConfigurationFactory().GetConfiguration();

string apiKey = configuration["ApiSettings:AlphaVantageApiKey"];
var stockDataProvider = new StockDataProvider(configuration);

var approximator = new MonteCarloApproximator();

// Fetch Microsoft stock data
var stockData = await stockDataProvider.GetData();
// Split data into training and testing sets (80% training, 20% testing)
var splitIndex = (int)(stockData.Count * 0.8);
var trainingData = stockData.Take(splitIndex).ToList();
var testingData = stockData.Skip(splitIndex).ToList();

// Add training samples to the approximator
for (var i = 0; i < trainingData.Count; i++)
{
    approximator.AddSample(i, trainingData[i].Close);
}

// Evaluate on testing data
double totalError = 0;
Console.WriteLine("Evaluation on test data:");
for (var i = 0; i < testingData.Count; i++)
{
    var actualValue = testingData[i].Close;
    var approximatedValue = approximator.Approximate(splitIndex + i);
    var error = Math.Abs(actualValue - approximatedValue);
    totalError += error;

    Console.WriteLine($"Day: {splitIndex + i}, Actual: {actualValue:F2}, Predicted: {approximatedValue:F2}, Error: {error:F2}");
}

var meanAbsoluteError = totalError / testingData.Count;
Console.WriteLine($"\nMean Absolute Error on test data: {meanAbsoluteError:F2}");

// Calculate Mean Absolute Percentage Error (MAPE)
double totalPercentageError = 0;
for (var i = 0; i < testingData.Count; i++)
{
    var actualValue = testingData[i].Close;
    var approximatedValue = approximator.Approximate(splitIndex + i);
    totalPercentageError += Math.Abs((actualValue - approximatedValue) / actualValue);
}
var mape = (totalPercentageError / testingData.Count) * 100;
Console.WriteLine($"Mean Absolute Percentage Error (MAPE) on test data: {mape:F2}%");

// Predict the next day's closing price
var nextDayPrediction = approximator.Approximate(stockData.Count);
Console.WriteLine($"\nPredicted closing price for the next day: {nextDayPrediction:F2}");