using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public interface IFitnessFunction
    {
        Task<double> Evaluate(Genotype genotype);
    }
}