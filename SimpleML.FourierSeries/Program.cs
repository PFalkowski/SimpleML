using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using MathNet.Numerics.IntegralTransforms;
using Microsoft.Extensions.Configuration;
using SimpleML.AlphaVintageClient;
using SimpleML.Configuration;

public class FourierSeriesPredictor
{
    private List<Complex> _fourierCoefficients;
    private int _numCoefficients;
    private double _meanPrice;

    public FourierSeriesPredictor(int numCoefficients)
    {
        _numCoefficients = numCoefficients;
    }

    public void Fit(List<double> prices)
    {
        _meanPrice = prices.Average();
        var normalizedPrices = prices.Select(p => p - _meanPrice).ToList();

        // Pad the input to a power of 2 for FFT efficiency
        int n = 1;
        while (n < normalizedPrices.Count) n *= 2;
        var paddedPrices = new Complex[n];
        for (int i = 0; i < normalizedPrices.Count; i++)
            paddedPrices[i] = new Complex(normalizedPrices[i], 0);

        // Perform FFT
        Fourier.Forward(paddedPrices, FourierOptions.Default);

        // Store the most significant coefficients
        _fourierCoefficients = paddedPrices.Take(_numCoefficients).ToList();
    }

    public List<double> Predict(int numDays)
    {
        var predictions = new List<double>();
        int originalLength = _fourierCoefficients.Count;

        for (int day = 0; day < numDays; day++)
        {
            Complex sum = new Complex(0, 0);
            for (int k = 0; k < _numCoefficients; k++)
            {
                double angle = 2 * Math.PI * k * (originalLength + day) / originalLength;
                Complex exponential = new Complex(Math.Cos(angle), Math.Sin(angle));
                sum += _fourierCoefficients[k] * exponential;
            }
            predictions.Add((sum / originalLength).Real + _meanPrice);
        }

        return predictions;
    }
}

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

        int futureDays = 30;
        var predictions = predictor.Predict(futureDays);

        Console.WriteLine("Predictions for the next 30 days:");
        for (int i = 0; i < predictions.Count; i++)
        {
            Console.WriteLine($"Day {i + 1}: {predictions[i]:C2}");
        }
    }
}