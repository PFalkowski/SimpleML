namespace SimpleML.GeneticAlgorithm
{
    public interface IStopFunction
    {
        ulong MaxEpochs { get; set; }
        bool ShouldContinue(RunMetadata learningMetadata);
    }
}