using LoggerLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        public Population ThePopulation { get; protected set; }
        public GeneticAlgorithmSettings Settings { get; protected set; }
        public IStopFunction StopFunction { get; protected set; }
        public RunMetadata RunInfo { get; protected set; } = new RunMetadata();
        public ILoggerLite Logger { get; protected set; }

        public GeneticAlgorithm(GeneticAlgorithmSettings settings)
        {
            Settings = settings;
            ThePopulation = new Population(settings, RunInfo);
            ThePopulation.Initialize();
            ThePopulation.Randomize();
            StopFunction = settings.StopFunction;
            Logger = settings.Logger;
        }

        public async Task SaveWinnerAsync(FileInfo fileName)
        {
            if (ThePopulation?.BestFit != null)
            {
                await ThePopulation.BestFit.SaveToFileAsync(fileName);
            }
        }

        public async Task ReadWinnerAsync(FileInfo fileName)
        {
            if (ThePopulation.GenePool != null && ThePopulation.GenePool.Any())
            {
                await ThePopulation.GenePool[0].ReadFromFileAsync(fileName);
            }
            else
            {
                throw new ApplicationException("GenePool not initialized.");
            }
        }

        public async Task RunEpoch()
        {
            await ThePopulation.Evaluate();

            if (Settings.ContinueFile != null
                && ThePopulation.BestFit != null
                && ThePopulation.BestFit.Fitness > 0
                && ThePopulation.BestFit.Fitness > RunInfo.BestFitnessSoFar)
            {
                await SaveWinnerAsync(Settings.ContinueFile);
            }
            
            ++RunInfo.Epochs;
            RunInfo.LastNFitnesses.Add(RunInfo.CurrentFitness);

            ThePopulation.ApplySelection();
            ThePopulation.Breed();
        }

        public async Task Run()
        {
            try
            {
                RunInfo.StartTime = DateTime.Now;
                RunInfo.Status = RunStatus.Running;
                while (StopFunction.ShouldContinue(RunInfo))
                {
                    await RunEpoch();
                    Logger?.LogInfo(RunInfo.ToString());
                }
                RunInfo.Status = RunStatus.Finished;
            }
            catch (Exception ex)
            {
                Logger?.LogError(ex);
                RunInfo.Status = RunStatus.Faulted;
            }
        }
    }
}
