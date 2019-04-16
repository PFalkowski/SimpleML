using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleML.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public Population ThePopulation { get; protected set; }
        public GeneticAlgorithmSettings Settings { get; protected set; }
        public IStopFunction StopFunction { get; protected set; }
        public GeneticAlgorithm(GeneticAlgorithmSettings settings)
        {
            Settings = settings;
            ThePopulation = new Population(settings);
            ThePopulation.RandomizePopulation();
        }
        public void RunEpoch()
        {
            ThePopulation.NaturalSelection();
            ThePopulation.Breed();
            ThePopulation.Evaluate();
        }
        public void Run()
        {
            while (StopFunction.ShouldContinue(ThePopulation))
            {
                RunEpoch();
            }
        }
    }
}
