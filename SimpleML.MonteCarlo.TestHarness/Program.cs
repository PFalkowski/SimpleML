namespace SimpleML.MonteCarlo.TestHarness;

public class Program
{
    public static void Main()
    {
        var approximator = new MonteCarloApproximator();

        // Generate some sample data (in a real scenario, this would be your observed data)
        var random = new Random(0);
        for (var i = 0; i < 1000; i++)
        {
            var x = random.NextDouble() * 10 - 5; // Range: -5 to 5
            var y = Math.Sin(x) * Math.Exp(-0.1 * Math.Pow(x, 2)) + random.NextDouble() * 0.1 - 0.05; // True function with some noise
            approximator.AddSample(x, y);
        }

        // Test the approximation
        Console.WriteLine("Approximation results:");
        for (double x = -5; x <= 5; x += 1)
        {
            var approximatedValue = approximator.Approximate(x);
            Console.WriteLine($"x: {x}, Approximated: {approximatedValue:F4}");
        }

        // Calculate cross-validation error
        var cvError = approximator.CalculateCrossValidationError();
        Console.WriteLine($"Cross-Validation Mean Squared Error: {cvError:F6}");
    }
}