using LoggerLite;
using System;

namespace SimpleML.GeneticAlgorithm
{
    public class GeneticAlgorithmSettings
    {
        public GeneticAlgorithmSettings(IFitnessFunction fitnessFunction, int problemSize)
        {
            GenotypeLength = problemSize;
            FittestSelectionAlgorithm = new EliteSelection();
            FitnessFunction = fitnessFunction;
        }
        public Random Rng { get; set; } = new Random();
        public int PopulationSize { get; set; } = 10000;
        public int SurvivorsCount => (int)(SurvivalRate * PopulationSize);
        public double SurvivalRate { get; set; } = 0.1;
        public double NewOrganismsRate { get; set; } = 0.1;
        public int NewOrganismsCount => (int)(NewOrganismsRate * PopulationSize);
        public int GenotypeLength { get; }
        public bool Parallel { get; set; } = true;
        public double CrossoverRate { get; set; } = 0.1;
        public double MutationRate { get; set; } = 0.1;
        public IStopFunction StopFunction { get; set; } = new BasicStopFunction();
        public ISelectionAlgorithm FittestSelectionAlgorithm { get; set; }
        public IFitnessFunction FitnessFunction { get; protected set; }
        public ILogger Logger { get; protected set; } = new ConsoleLogger();
    }
}