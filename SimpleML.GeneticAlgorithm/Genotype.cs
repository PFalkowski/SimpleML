using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleML.GeneticAlgorithm
{
    public class Genotype
    {
        public double MutationRate { get; set; }
        public double CrossoverRate { get; set; }
        public Random Rng { get; set; }
        public Genotype(GeneticAlgorithmSettings settings)
        {
            Rng = settings.Rng;
            Value = new bool[settings.GenotypeLength];
            MutationRate = 1.0 / settings.GenotypeLength;
            CrossoverRate = settings.CrossoverRate;
        }
        public Genotype(int genotypeLength, double crossoverRate)
        {
            Value = new bool[genotypeLength];
            MutationRate = 1.0 / genotypeLength;
            CrossoverRate = crossoverRate;
        }
        public bool this[int i]
        {
            get { return Value[i]; }
            protected set { Value[i] = value; }
        }
        public bool[] Value { get; protected set; }
        public double Fitness { get; internal set; }

        public void Randomize()
        {
            for (int i = 0; i < Value.Length; ++i)
            {
                Value[i] = Rng.NextDouble() > 0.5;
            }
        }

        public void Mutate()
        {
            for (int i = 0; i < Value.Length; ++i)
            {
                if (Rng.NextDouble() < MutationRate)
                    Value[i] = !Value[i];
            }
        }

        public Genotype CrossoverWith(Genotype secondParent)
        {
            if (Value.Length < 2 || Rng.NextDouble() >= CrossoverRate) return this;
            var splitPoint = Rng.Next(1, Value.Length);
            var child = new Genotype(Value.Length, CrossoverRate);
            child.Value = new bool[Value.Length];
            for (int i = 0; i < child.Value.Length; ++i)
            {
                child.Value[i] = i < splitPoint ? this[i] : secondParent[i];
            }
            return child;
        }
    }
}
