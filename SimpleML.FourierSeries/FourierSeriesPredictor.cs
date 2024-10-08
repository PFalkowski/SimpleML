﻿using MathNet.Numerics.IntegralTransforms;
using System.Numerics;

namespace SimpleML.FourierSeries;

public class FourierSeriesPredictor(int numCoefficients)
{
    private List<Complex> _fourierCoefficients;
    private double _meanPrice;

    public void Fit(List<double> prices)
    {
        _meanPrice = prices.Average();
        var normalizedPrices = prices.Select(p => p - _meanPrice).ToList();

        // Pad the input to a power of 2 for FFT efficiency
        var n = 1;
        while (n < normalizedPrices.Count) n *= 2;
        var paddedPrices = new Complex[n];
        for (var i = 0; i < normalizedPrices.Count; i++)
            paddedPrices[i] = new Complex(normalizedPrices[i], 0);

        // Perform FFT
        Fourier.Forward(paddedPrices, FourierOptions.Default);

        // Store the most significant coefficients
        _fourierCoefficients = paddedPrices.Take(numCoefficients).ToList();
    }

    public List<double> Predict(int numDays)
    {
        var predictions = new List<double>();
        var originalLength = _fourierCoefficients.Count;

        for (var day = 0; day < numDays; day++)
        {
            var sum = new Complex(0, 0);
            for (var k = 0; k < numCoefficients; k++)
            {
                var angle = 2 * Math.PI * k * (originalLength + day) / originalLength;
                var exponential = new Complex(Math.Cos(angle), Math.Sin(angle));
                sum += _fourierCoefficients[k] * exponential;
            }
            predictions.Add((sum / originalLength).Real + _meanPrice);
        }

        return predictions;
    }
}