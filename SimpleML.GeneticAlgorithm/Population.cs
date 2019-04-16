using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public class Population
    {
        protected readonly object _syncRoot = new object();

        public Population(GeneticAlgorithmSettings settings)
        {
            Settings = settings;
            Rng = settings.Rng;
        }

        public Random Rng { get; protected set; }
        public List<Genotype> GenePool { get; protected set; }
        public SortedList<double, Genotype> Organisms { get; protected set; }
        public IFitnessFunction FitnessFunction { get; protected set; }
        public ISelectionAlgorithm FittestSelectionAlgorithm { get; protected set; }
        public ISelectionAlgorithm ParentSelectionAlgorithm { get; protected set; }


        public GeneticAlgorithmSettings Settings { get; protected set; }
        public void Initialize(GeneticAlgorithmSettings settings)
        {
            Organisms = new SortedList<double, Genotype>(Settings.PopulationSize);
            RandomizePopulation();
        }
        public void RandomizePopulation()
        {
            Organisms = new SortedList<double, Genotype>(Settings.PopulationSize);
            Parallel.For(0, Settings.PopulationSize, i =>
            {
                var genotype = new Genotype();
                var fitness = FitnessFunction.Evaluate(genotype);
                genotype.Fitness = fitness;
                lock (_syncRoot)
                { Organisms.Add(fitness, genotype); }
            });
        }
        public void NaturalSelection()
        {

        }
        public List<Genotype> GetFittest()
        {
            return FittestSelectionAlgorithm.Select(Organisms);
        }
        public List<Genotype> SelectParents()
        {
            return ParentSelectionAlgorithm.Select(Organisms);
        }
        public void Breed()
        {

        }
        public void Evaluate()
        {
            Parallel.ForEach(GenePool, genotype =>
            {
                genotype.Fitness = FitnessFunction.Evaluate(genotype);
            });
        }
    }
}
