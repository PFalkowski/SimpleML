using MathNet.Numerics.LinearAlgebra;

namespace SimpleML.PolynomialRegression.TestHarness;

public class PolynomialRegression
{
    public static double[] Fit(double[] x, double[] y, int degree)
    {
        if (x.Length != y.Length)
            throw new ArgumentException("Input arrays must have the same length.");

        var design = new double[x.Length, degree + 1];
        for (var i = 0; i < x.Length; i++)
        {
            for (var j = 0; j <= degree; j++)
            {
                design[i, j] = Math.Pow(x[i], j);
            }
        }

        var designMatrix = Matrix<double>.Build.DenseOfArray(design);
        var yVector = Vector<double>.Build.Dense(y);

        var coefficients = designMatrix.Solve(yVector);
        return coefficients.ToArray();
    }

    public static double Predict(double[] coefficients, double x)
    {
        return coefficients.Select((c, i) => c * Math.Pow(x, i)).Sum();
    }
}

// Usage example
public class Program
{
    public static void Main()
    {
        double[] x = { 10, 11, 10, 13, 9, 15 };
        double[] y = { 0, 1, 2, 3, 4, 5 };
        var degree = 4;

        var coefficients = PolynomialRegression.Fit(x, y, degree);

        Console.WriteLine("Coefficients: " + string.Join(", ", coefficients));

        // Predict for a new x value
        var newX = 6;
        var prediction = PolynomialRegression.Predict(coefficients, newX);
        Console.WriteLine($"Prediction for x = {newX}: {prediction}");
    }
}