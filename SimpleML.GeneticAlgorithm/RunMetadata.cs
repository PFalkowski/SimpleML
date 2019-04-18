using System;
using System.Collections.Generic;

namespace SimpleML.GeneticAlgorithm
{
    public class RunMetadata
    {
        public ulong Epochs { get; set; }
        public double CurrentFitness { get; set; }
        public DateTime StartTime { get; set; }
        public RunStatus Status { get; set; }
        public List<double> LastNFitnesses { get; set; } = new List<double>();
        public override string ToString()
        {
            return $"Epoch: {Epochs}, CurrentFitness: {CurrentFitness}";
        }

        //internal void UpdateOnEpoch(Population thePopulation)
        //{
        //}
    }
}