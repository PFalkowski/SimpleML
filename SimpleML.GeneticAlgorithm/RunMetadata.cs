using System;
using System.Collections.Generic;

namespace SimpleML.GeneticAlgorithm
{
    public class RunMetadata
    {
        public ulong Epochs { get; set; }
        public ulong SimulationsCount { get; set; }
        public Genotype CurrentEpochBest { get; set; }
        public Genotype BestOverall { get; set; }
        public double CurrentFitness => CurrentEpochBest?.Fitness ?? 0;
        public double BestFitnessSoFar => BestOverall?.Fitness ?? 0;
        public DateTime StartTime { get; set; }
        public RunStatus Status { get; set; }
        public List<double> LastNFitnesses { get; set; } = new();

        public void PresentNewResult(Genotype genotype)
        {
            if (CurrentEpochBest == null)
            {
                CurrentEpochBest = genotype;
            }

            if (BestOverall == null)
            {
                BestOverall = genotype;
            }

            if (BestOverall.Fitness < genotype.Fitness)
            {
                BestOverall = genotype;
            }

            if (CurrentEpochBest.Fitness < genotype.Fitness)
            {
                CurrentEpochBest = genotype;
            }
        }
    
        public override string ToString()
        {
            return $"Epoch: {Epochs}, best fitness: {BestFitnessSoFar}, current fitness: {CurrentFitness}";
        }
    }
}