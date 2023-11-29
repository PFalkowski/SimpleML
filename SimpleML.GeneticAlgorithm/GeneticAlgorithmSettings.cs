using LoggerLite;
using System;

namespace SimpleML.GeneticAlgorithm
{
    public class GeneticAlgorithmSettings
    {
        public GeneticAlgorithmSettings(IFitnessFunction fitnessFunction, int problemSize)
        {
            GenotypeLength = problemSize;
            Rng = new Random();
            FittestSelectionAlgorithm = new BinaryTournamentWithAlpha(Rng);
            FitnessFunction = fitnessFunction;
        }
        public Random Rng { get; set; } = new Random();
        public int PopulationSize { get; set; } = 10000;
        public int GenotypeLength { get; }
        public bool Parallel { get; set; } = true;
        public double CrossoverRate { get; protected set; } = 0.5;
        public IStopFunction StopFunction { get; set; } = new BasicStopFunction();
        public ISelectionAlgorithm FittestSelectionAlgorithm { get; set; }
        public IFitnessFunction FitnessFunction { get; protected set; }
        public ILogger Logger { get; protected set; } = new ConsoleLogger();
    }
}