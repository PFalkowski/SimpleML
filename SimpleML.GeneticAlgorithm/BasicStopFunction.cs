using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleML.GeneticAlgorithm
{
    public class BasicStopFunction : IStopFunction
    {
        public long MaxEpochs { get; set; } = 10000000;
        public double MinFitness { get; set; } = 100;
        public TimeSpan MaxDuration { get; set; } = TimeSpan.FromHours(10);
        private bool _stopRequested;
        public void ForceStop()
        {
            _stopRequested = true;
        }
        public bool ShouldContinue(RunMetadata learningMetadata)
        {
            if (_stopRequested)
            {
                _stopRequested = false;
                return false;
            }
            else if (
                DateTime.Now - learningMetadata.StartTime > MaxDuration ||
                learningMetadata.Epochs >= MaxEpochs ||
                learningMetadata.CurrentFitness >= MinFitness)
            {
                return false;
            }
            return true;
        }
    }
}
