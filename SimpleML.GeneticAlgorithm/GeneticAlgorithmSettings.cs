using System;

namespace SimpleML.GeneticAlgorithm
{
    public class GeneticAlgorithmSettings
    {
        public GeneticAlgorithmSettings(int problemSize)
        {
            GenotypeLength = problemSize;
        }
        public Random Rng { get; set; } = new Random();
        public int PopulationSize { get; set; } = 10000;
        public int GenotypeLength { get; }
        public double CrossoverRate { get; internal set; }
        IStopFunction StopFunction { get; set; } = new BasicStopFunction();
    }
}