using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public class RouletteWheelSelection : ISelectionAlgorithm
    {
        private readonly Random _rng;
        public RouletteWheelSelection(Random rng)
        {
            _rng = rng;
        }

        public (List<Genotype> survivors, Genotype best) Select(IList<Genotype> organisms, int size)
        {
            if (organisms.Count < 2)
            {
                throw new ArgumentException(null, nameof(organisms));
            }

            var selected = new List<Genotype>(organisms.Count);
            var alpha = organisms[0];
            // TODO
            return (survivors: selected, best: alpha);
        }
    }
}
