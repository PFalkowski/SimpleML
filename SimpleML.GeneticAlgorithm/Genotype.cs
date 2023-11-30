using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleML.GeneticAlgorithm
{
    public class Genotype
    {
        public double MutationRate { get; set; }
        public double CrossoverRate { get; set; }
        public Random Rng { get; set; }
        public bool[] Value { get; protected set; }
        public double Fitness { get; internal set; }

        public Genotype(GeneticAlgorithmSettings settings)
        {
            Rng = settings.Rng;
            Value = new bool[settings.GenotypeLength];
            MutationRate = 1.0 / settings.GenotypeLength;
            CrossoverRate = settings.CrossoverRate;
        }

        public Genotype(int genotypeLength, double crossoverRate, Random rng)
        {
            Value = new bool[genotypeLength];
            MutationRate = 1.0 / genotypeLength;
            CrossoverRate = crossoverRate;
            Rng = rng;
        }

        public bool this[int i]
        {
            get => Value[i];
            protected set => Value[i] = value;
        }

        public void Randomize()
        {
            for (var i = 0; i < Value.Length; ++i)
            {
                Value[i] = Rng.NextDouble() > 0.5;
            }
        }

        public void Mutate()
        {
            for (var i = 0; i < Value.Length; ++i)
            {
                if (Rng.NextDouble() < MutationRate)
                {
                    Value[i] = !Value[i];
                }
            }
        }

        public Genotype CrossoverWith(Genotype secondParent)
        {
            if (Value.Length < 2 || Rng.NextDouble() >= CrossoverRate) 
                return this;

            var splitPoint = Rng.Next(1, Value.Length);
            var child = new Genotype(Value.Length, CrossoverRate, Rng)
            {
                Value = new bool[Value.Length]
            };
            for (var i = 0; i < child.Value.Length; ++i)
            {
                child.Value[i] = i < splitPoint ? this[i] : secondParent[i];
            }

            return child;
        }

        public async Task SaveToFileAsync(FileInfo file)
        {
            var csvSerialized =
                string.Join(Environment.NewLine, Value.Select(x => x.ToString()));
            await File.WriteAllTextAsync(file.FullName, csvSerialized);
        }

        public async Task ReadFromFileAsync(FileInfo file)
        {
            var csvLines = await File.ReadAllLinesAsync(file.FullName);
            var listOfDeserializedValues = csvLines.Select(bool.Parse).ToList();

            if (listOfDeserializedValues.Count > 0)
            {
                Value = listOfDeserializedValues.ToArray();
            }
        }
    }
}
