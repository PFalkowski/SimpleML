using LoggerLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleML.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public Population ThePopulation { get; protected set; }
        public GeneticAlgorithmSettings Settings { get; protected set; }
        public IStopFunction StopFunction { get; protected set; }
        public RunMetadata RunInfo { get; protected set; } = new RunMetadata();
        public ILogger Logger { get; protected set; }
        public GeneticAlgorithm(GeneticAlgorithmSettings settings)
        {
            Settings = settings;
            ThePopulation = new Population(settings);
            ThePopulation.Initialize();
            ThePopulation.Randomize();
            StopFunction = settings.StopFunction;
            Logger = settings.Logger;
        }
        public void RunEpoch()
        {
            ThePopulation.ApplySelection();
            ThePopulation.Breed();
            ThePopulation.Evaluate();

            ++RunInfo.Epochs;
            RunInfo.CurrentFitness = ThePopulation.BestFit.Fitness;
            RunInfo.LastNFitnesses.Add(ThePopulation.BestFit.Fitness);
        }
        public void Run()
        {
            try
            {
                RunInfo.StartTime = DateTime.Now;
                RunInfo.Status = RunStatus.Running;
                while (StopFunction.ShouldContinue(RunInfo))
                {
                    RunEpoch();
                    Logger?.LogInfo(RunInfo.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex);
                RunInfo.Status = RunStatus.Faulted;
            }
        }
    }
}
