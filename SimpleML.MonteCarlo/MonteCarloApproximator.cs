namespace SimpleML.MonteCarlo;

public class MonteCarloApproximator(int seed = 0)
{
    private readonly List<(double X, double Y)> _samples = [];
    private readonly Random _random = new(seed);

    public void AddSample(double x, double y)
    {
        _samples.Add((x, y));
    }

    public void AddSamples(IEnumerable<(double X, double Y)> samples)
    {
        _samples.AddRange(samples);
    }

    public double Approximate(double x, int k = 5)
    {
        if (_samples.Count == 0)
        {
            throw new InvalidOperationException("No samples available for approximation.");
        }

        var nearestNeighbors = _samples
            .OrderBy(s => Math.Abs(s.X - x))
            .Take(k)
            .ToList();

        return nearestNeighbors.Average(s => s.Y);
    }

    public double CalculateCrossValidationError(int folds = 5)
    {
        if (_samples.Count < folds)
        {
            throw new InvalidOperationException("Not enough samples for cross-validation.");
        }

        var shuffledSamples = _samples.OrderBy(x => _random.Next()).ToList();
        var foldSize = _samples.Count / folds;
        var totalSquaredError = 0.0;
        var totalSamples = 0;

        for (var i = 0; i < folds; i++)
        {
            var testSet = shuffledSamples.Skip(i * foldSize).Take(foldSize).ToList();
            var trainingSet = shuffledSamples.Except(testSet).ToList();

            var tempApproximator = new MonteCarloApproximator();
            tempApproximator.AddSamples(trainingSet);

            foreach (var (x, y) in testSet)
            {
                var approximatedY = tempApproximator.Approximate(x);
                totalSquaredError += Math.Pow(y - approximatedY, 2);
                totalSamples++;
            }
        }

        return totalSquaredError / totalSamples;
    }
}