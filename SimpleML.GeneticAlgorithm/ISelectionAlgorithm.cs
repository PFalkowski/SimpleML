using System.Collections.Generic;

namespace SimpleML.GeneticAlgorithm
{
    public interface ISelectionAlgorithm
    {
        List<Genotype> Select(SortedList<double, Genotype> organisms);
    }
}