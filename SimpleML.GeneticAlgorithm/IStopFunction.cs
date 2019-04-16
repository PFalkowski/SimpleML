namespace SimpleML.GeneticAlgorithm
{
    public interface IStopFunction
    {
        bool ShouldContinue(Population thePopulation);
    }
}