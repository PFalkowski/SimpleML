namespace SimpleML.MonteCarlo;

public interface IMonteCarloApproximator
{
    void AddSample(double x, double y);
    void AddSamples(IEnumerable<(double X, double Y)> samples);
    double Approximate(double x, int k = 5);
    double CalculateCrossValidationError(int folds = 5);
}