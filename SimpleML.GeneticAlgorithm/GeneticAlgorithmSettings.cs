using System;

namespace SimpleML.GeneticAlgorithm
{
    public class GeneticAlgorithmSettings
    {
        public Random Rng { get; internal set; }
        public int PopulationSize { get; internal set; }
    }
}