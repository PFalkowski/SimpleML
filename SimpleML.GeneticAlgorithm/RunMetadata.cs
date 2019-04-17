using System;

namespace SimpleML.GeneticAlgorithm
{
    public class RunMetadata
    {
        public long Epochs { get; internal set; }
        public double CurrentFitness { get; internal set; }
        public DateTime StartTime { get; internal set; }
    }
}