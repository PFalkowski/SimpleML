using System;
using System.Collections.Generic;

namespace SimpleML.GeneticAlgorithm
{
    public class BinaryTournamentSelection : ISelectionAlgorithm
    {
        private readonly Random _rng;
        public BinaryTournamentSelection(Random rng)
        {
            _rng = rng;
        }

        public (List<Genotype> survivors, Genotype best) Select(IList<Genotype> organisms, int size)
        {
            if (organisms.Count < 2)
            {
                throw new ArgumentException(null, nameof(organisms));
            }

            var selected = new List<Genotype>(size);
            var alpha = organisms[0];
            var i = 0;
            while (selected.Count < size)
            {
                var organismA = organisms[_rng.Next(0, organisms.Count)];
                Genotype organismB;

                do
                {
                    organismB = organisms[_rng.Next(0, organisms.Count)];
                }
                while (organismA == organismB);
                selected.Add(organismA.Fitness >= organismB.Fitness ? organismA : organismB);

                if (organisms[i].Fitness > alpha.Fitness)
                {
                    alpha = organisms[i];
                }

                ++i;
            }

            return (survivors: selected, best: alpha);
        }
    }
}
