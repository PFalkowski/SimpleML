using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleML.GeneticAlgorithm
{
    public class BinaryTournamentWithAlpha : ISelectionAlgorithm
    {
        private Random _rng;
        public BinaryTournamentWithAlpha(Random rng)
        {
            _rng = rng;
        }
        public (List<Genotype> survivors, Genotype best)  Select(IList<Genotype> organisms)
        {
            if (organisms.Count < 3) throw new ArgumentException(nameof(organisms));
            var selected = new List<Genotype>(organisms.Count);
            Genotype alpha = organisms[0];
            int i = 0;
            while (selected.Count < organisms.Count - 1)
            {
                var organismA = organisms[_rng.Next(0, organisms.Count)];
                Genotype organismB;
                do
                { organismB = organisms[_rng.Next(0, organisms.Count)]; }
                while (organismA == organismB);
                selected.Add(organismA.Fitness >= organismB.Fitness ? organismA : organismB);
                if (organisms[i].Fitness > alpha.Fitness)
                {
                    alpha = organisms[i];
                }
                ++i;
            }
            selected.Add(alpha);
            return (survivors: selected, best: alpha);
        }
    }
}
