using System.Collections.Generic;
using System.Linq;

namespace SimpleML.GeneticAlgorithm
{
    public class EliteSelection : ISelectionAlgorithm
    {
        public (List<Genotype> survivors, Genotype best) Select(IList<Genotype> organisms, int size)
        {
            var ordered = organisms
                .OrderByDescending(x => x.Fitness)
                .Take(size)
                .ToList();

            return (ordered, ordered.First());
        }
    }
}
