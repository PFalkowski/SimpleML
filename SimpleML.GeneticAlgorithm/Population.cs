﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public class Population
    {
        private readonly RunMetadata _runInfo;
        private readonly object _syncRoot = new object();

        public Random Rng { get; protected set; }
        public List<Genotype> GenePool { get; protected set; }
        public Genotype BestFit { get; protected set; }
        public IFitnessFunction FitnessFunction { get; protected set; }
        public ISelectionAlgorithm FittestSelectionAlgorithm { get; protected set; }
        public GeneticAlgorithmSettings Settings { get; protected set; }

        public Population(GeneticAlgorithmSettings settings, RunMetadata runInfo)
        {
            _runInfo = runInfo;
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
                var options = new ParallelOptions
                {
                    MaxDegreeOfParallelism =  Settings.MaxDegreeOfParallelism
                };

                Parallel.ForEach(GenePool, 
                    options,
                    RunOneIteration);
            }
            else
            {
                var tasksAggregate = new List<Task>();
                foreach (var genotype in GenePool)
                {
                    tasksAggregate.Add(RunOneIterationAsync(genotype));
                }

                await Task.WhenAll(tasksAggregate);
            }
        }

        private void RunOneIteration(Genotype genotype)
        {
            genotype.Fitness = FitnessFunction.Evaluate(genotype);

            lock (_syncRoot)
            {
                ++_runInfo.SimulationsCount;
                _runInfo.PresentNewResult(genotype);
            }
        }

        private async Task RunOneIterationAsync(Genotype genotype)
        {
            genotype.Fitness = await FitnessFunction.EvaluateAsync(genotype);
            lock (_syncRoot)
            {
                ++_runInfo.SimulationsCount;
                _runInfo.PresentNewResult(genotype);
            }
        }

        public void ApplySelection()
        {
            var selectionResult = FittestSelectionAlgorithm.Select(GenePool, Settings.SurvivorsCount);
            GenePool = selectionResult.survivors;
            if (BestFit == null || selectionResult.best.Fitness > BestFit.Fitness)
            {
                BestFit = selectionResult.best;
            }
        }

        public void Breed()
        {
            if (Settings.PopulationSize < Settings.NewOrganismsCount + Settings.SurvivorsCount)
            {
                throw new ApplicationException("Incorrect state. Population over 100%.");
            }
            var parents = GenePool;
            GenePool = new List<Genotype>(Settings.PopulationSize);
            while (GenePool.Count < Settings.PopulationSize - Settings.NewOrganismsCount)
            {
                Genotype child;
                if (parents.Count >= 2)
                {
                    var parentA = parents[Rng.Next(0, parents.Count)];
                    Genotype parentB;
                    do
                    {
                        parentB = parents[Rng.Next(0, parents.Count)];
                    } while (parentA == parentB);

                    child = parentA.CrossoverWith(parentB);
                }
                else
                {
                    child = parents.First();
                }

                if (Rng.NextDouble() < Settings.MutationRate)
                {
                    child.Mutate();
                }

                GenePool.Add(child);
            }

            while (GenePool.Count < Settings.PopulationSize)
            {
                var newOrganism = new Genotype(Settings);
                newOrganism.Randomize();
                GenePool.Add(newOrganism);
            }
        }
    }
}
