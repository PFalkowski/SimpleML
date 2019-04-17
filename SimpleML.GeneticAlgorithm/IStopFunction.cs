namespace SimpleML.GeneticAlgorithm
{
    public interface IStopFunction
    {
        bool ShouldContinue(RunMetadata learningMetadata);
    }
}