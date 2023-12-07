using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public interface IFitnessFunction
    {
        double Evaluate(Genotype genotype);
        Task<double> EvaluateAsync(Genotype genotype);
    }
}