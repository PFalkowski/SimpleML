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
        public List<Genotype> Select(IList<Genotype> organisms)
        {
            if (organisms.Count < 2) return organisms.ToList();
            var selected = new List<Genotype>(organisms.Count);
            Genotype alpha = organisms[0];
            int i = 0;
            while (selected.Count < organisms.Count)
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
            return selected;
        }
    }
}
