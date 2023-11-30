﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public class Population
    {
        protected readonly object _syncRoot = new object();

        public Random Rng { get; protected set; }
        public List<Genotype> GenePool { get; protected set; }
        public Genotype BestFit { get; protected set; }
        public IFitnessFunction FitnessFunction { get; protected set; }
        public ISelectionAlgorithm FittestSelectionAlgorithm { get; protected set; }
        public GeneticAlgorithmSettings Settings { get; protected set; }

        public Population(GeneticAlgorithmSettings settings)
        {
            Settings = settings;
            FitnessFunction = settings.FitnessFunction;
            FittestSelectionAlgorithm = settings.FittestSelectionAlgorithm;
            Rng = settings.Rng;
            Initialize();
            Randomize();
        }

        public void Initialize()
        {
            GenePool = new List<Genotype>(Settings.PopulationSize);
            for (var i = 0; i < Settings.PopulationSize; ++i)
            {
                GenePool.Add(new Genotype(Settings));
            }
        }

        public void Randomize()
        {
            foreach (var gene in GenePool)
            {
                gene.Randomize();
            }
        }

        public async Task Evaluate()
        {
            if (Settings.Parallel)
            {
                var tasksAggregate = GenePool
                    .Select(GenotypeFitnessAsync)
                    .ToList();

                await Task.WhenAll(tasksAggregate);
            }
            else
            {
                foreach (var genotype in GenePool)
                {
                    genotype.Fitness = await FitnessFunction.Evaluate(genotype);
                }
            }
        }

        private Task GenotypeFitnessAsync(Genotype genotype)
        {
            return Task.Run(async () =>
            {
                genotype.Fitness = await FitnessFunction.Evaluate(genotype);
            });
        }

        public void ApplySelection()
        {
            var selectionResult = FittestSelectionAlgorithm.Select(GenePool);
            GenePool = selectionResult.survivors;
            if (BestFit == null || selectionResult.best.Fitness > BestFit.Fitness)
            {
                BestFit = selectionResult.best;
            }
        }

        public void Breed()
        {
            var parents = GenePool;
            GenePool = new List<Genotype>(Settings.PopulationSize);
            while (GenePool.Count < Settings.PopulationSize)
            {
                var parentA = parents[Rng.Next(0, Settings.PopulationSize)];
                Genotype parentB;
                do
                {
                    parentB = parents[Rng.Next(0, Settings.PopulationSize)];
                } while (parentA == parentB);
                var child = parentA.CrossoverWith(parentB);
                child.Mutate();
                GenePool.Add(child);
            }
        }
    }
}
