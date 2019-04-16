namespace SimpleML.GeneticAlgorithm
{
    public interface IFitnessFunction
    {
        double Evaluate(Genotype genotype);
    }
}