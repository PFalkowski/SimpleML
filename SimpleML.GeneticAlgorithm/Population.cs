using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public class Population
    {
        private readonly RunMetadata _runInfo;
        protected readonly object _syncRoot = new object();

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
                var countdown = new CountdownEvent(GenePool.Count);
                foreach (var genotype in GenePool)
                {
                    var thread = new Thread(() =>
                    {
                        genotype.Fitness = FitnessFunction.Evaluate(genotype);
                        lock (_syncRoot)
                        {
                            ++_runInfo.SimulationsCount;
                            _runInfo.CurrentFitness = Math.Max(_runInfo.CurrentFitness, genotype.Fitness);
                            countdown.Signal();
                        }
                    });
                    thread.Start();
                }

                countdown.Wait();
            }
            else
            {
                var tasksAggregate = new List<Task>();
                foreach (var genotype in GenePool)
                {
                    tasksAggregate.Add(RunOneIteration(genotype));
                }

                await Task.WhenAll(tasksAggregate);
            }
        }

        private async Task RunOneIteration(Genotype genotype)
        {
            genotype.Fitness = await FitnessFunction.EvaluateAsync(genotype);
            lock (_syncRoot)
            {
                ++_runInfo.SimulationsCount;
                _runInfo.CurrentFitness = Math.Max(_runInfo.CurrentFitness, genotype.Fitness);
            }
        }

        //private Task GenotypeFitnessAsync(Genotype genotype)
        //{
        //    return Task.Run(async () =>
        //    {
        //        genotype.Fitness = await FitnessFunction.EvaluateAsync(genotype);
        //    });
        //}

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
                GenePool.Add(newOrganism);
            }
        }
    }
}
