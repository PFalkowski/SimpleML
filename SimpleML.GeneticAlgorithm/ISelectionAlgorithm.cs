using System.Collections.Generic;

namespace SimpleML.GeneticAlgorithm
{
    public interface ISelectionAlgorithm
    {
        (List<Genotype> survivors, Genotype best) Select(IList<Genotype> organisms, int size);
    }
}