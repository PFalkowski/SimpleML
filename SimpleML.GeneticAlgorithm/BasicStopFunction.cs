using System;
using System.Collections.Generic;

namespace SimpleML.GeneticAlgorithm
{
    public class BasicStopFunction : IStopFunction
    {
        public ulong MaxEpochs { get; set; } = 100000000;
        public double MinFitness { get; set; } = 100;
        public TimeSpan MaxDuration { get; set; } = TimeSpan.FromHours(10);
        public int DeltaNoChangeMaxEpochs { get; set; } = 100;
        private bool _stopRequested;
        public void ForceStop()
        {
            _stopRequested = true;
        }
        private double CalculateDeltaChange(List<double> deltas)
        {
            var absSum = 0.0;
            for (var i = deltas.Count -1; i > deltas.Count - 1 - DeltaNoChangeMaxEpochs; --i)
            {
                absSum += Math.Abs(deltas[i] - deltas[i - 1]);
            }

            return absSum;
        }
        public bool ShouldContinue(RunMetadata learningMetadata)
        {
            if (_stopRequested)
            {
                _stopRequested = false;

                return false;
            }

            if (DateTime.Now - learningMetadata.StartTime > MaxDuration ||
                learningMetadata.Epochs >= MaxEpochs ||
                learningMetadata.CurrentFitness >= MinFitness)
            {
                return false;
            }

            if (learningMetadata.LastNFitnesses.Count >= DeltaNoChangeMaxEpochs * 2)
            {
                if (CalculateDeltaChange(learningMetadata.LastNFitnesses) <= double.Epsilon)
                    return false;
            }

            return true;
        }
    }
}
