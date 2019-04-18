using System;
using System.Collections.Generic;

namespace SimpleML.GeneticAlgorithm.TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = new GeneticAlgorithmSettings(new BasicStockTrading(), BasicStockTrading.Prices.Count);
            settings.StopFunction = new BasicStopFunction { MinFitness = 144 };
            var algorithm = new GeneticAlgorithm(settings);
            algorithm.Run();
        }
        private class BasicStockTrading : IFitnessFunction
        {
            public double Evaluate(Genotype genotype)
            {
                bool alreadyBought = false;
                var sum = 0.0;
                for (int i = 0; i < Prices.Count; i++)
                {
                    if (genotype[i] && !alreadyBought)
                    {
                        sum -= Prices[i];
                        alreadyBought = true;
                    }
                    else if (!genotype[i] && alreadyBought)
                    {
                        sum += Prices[i];
                        alreadyBought = false;
                    }
                }
                if (alreadyBought)
                {
                    sum -= Prices[Prices.Count - 1];
                }
                return sum;
            }
            public static List<double> Prices { get; set; } = new List<double>
        {
            165.075 ,
            173.7   ,
            177.9   ,
            177.3   ,
            179.875 ,
            178.35  ,
            182.15  ,
            187.5   ,
            194.65  ,
            198.025 ,
            205.325 ,
            203.725 ,
            201.525 ,
            205.45  ,
            207 ,
            208.9   ,
            213 ,
            215.65  ,
            216.6   ,
            205.45  ,
            197.55  ,
            199.7   ,
            198.625 ,
            199.2   ,
            203.125 ,
            211.2   ,
            213.5   ,
            208.15  ,
            205.625 ,
            211.5   ,
            208.6   ,
            205.85  ,
            207.4   ,
            212.45  ,
            208.35  ,
            207.3   ,
            207.75  ,
            210.1   ,
            219.4   ,
            218.5   ,
            214.9   ,
            206.8   ,
            203.725 ,
            198.575 ,
            189.65  ,
            183.65  ,
            179.9   ,
            174.875 ,
            183.225 ,
            181.575 ,
            179.525 ,
            181.725 ,
            182.1   ,
            185.7   ,
            191.725 ,
            190.275 ,
            193.6   ,
            191.35  ,
            186.425 ,
            186.9   ,
            189.1   ,
            190.25  ,
            190.5   ,
            189.675 ,
            191.55  ,
            186.4   ,
            180.825 ,
            171.6   ,
            162.45  ,
            159.05  ,
            150.725 ,
            159.125 ,
            162.3   ,
            162.025 ,
            165.425 ,
            159.85  ,
            156.95  ,
            157.5   ,
            148.925 ,
            145.35  ,
            143.775 ,
            146.275 ,
            147.325 ,
            149.55  ,
            157 ,
            165.3   ,
            163.75  ,
            162.95  ,
            164.475 ,
            161.95  ,
            152.6   ,
            147.6   ,
            143.575 ,
            142.625 ,
            136.875 ,
            133.425 ,
            129.875 ,
            134.1   ,
            137.7   ,
            139.5   ,
            140 ,
            136.7   ,
            137.05  ,
            141.05  ,
            140.325 ,
            152 ,
            153.3   ,
            148.575 ,
            144.925 ,
            145.6   ,
            142.05  ,
            141.5   ,
            142.1   ,
            143.4   ,
            143.3   ,
            141.65  ,
            139.875 ,
            147.5   ,
            150.075 ,
            151.725 ,
            151.775 ,
            145.175 ,
            147.45  ,
            149.15  ,
            149.375 ,
            152.95  ,
            154.2   ,
            159.525 ,
            166.95  ,
            169.8   ,
            161.575 ,
            161.625 ,
            165.175 ,
            168.175 ,
            173.1   ,
            172.775 ,
            173.875 ,
            186.625 ,
            193.05  ,
            190.75  ,
            188.225 ,
            189 ,
            193.95  ,
            194.25  ,
            190.05  ,
            189.775 ,
            191.275 ,
            186.4   ,
            179.875 ,
            178.65  ,
            182.15  ,
            180.5   ,
            177.675 ,
            177.675 ,
            179.85  ,
            182.6   ,
            179.275 ,
            179.625 ,
            180.75  ,
            179 ,
            180.35  ,
            183.325 ,
            186.775 ,
            191.075 ,
            191.9   ,
            190.7   ,
            188.325 ,
            188.275 ,
            186.85  ,
            184.475 ,
            182.45  ,
            181.4   ,
            181.55  ,
            183.7   ,
            184.75  ,
            187 ,
            187.9   ,
            188.35  ,
            185.25  ,
            186.65  ,
            185.075 ,
            185.175 ,
            191.5   ,
            198.725 ,
            200.675 ,
            203.45  ,
            208.925 ,
            217.325
        };
        }
    }
}
