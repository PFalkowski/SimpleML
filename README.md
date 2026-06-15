# SimpleML.GeneticAlgorithm

[![CI](https://github.com/PFalkowski/SimpleML/actions/workflows/ci.yml/badge.svg)](https://github.com/PFalkowski/SimpleML/actions/workflows/ci.yml)
[![NuGet version](https://img.shields.io/nuget/v/SimpleML.GeneticAlgorithm.svg)](https://www.nuget.org/packages/SimpleML.GeneticAlgorithm/)
[![NuGet downloads](https://img.shields.io/nuget/dt/SimpleML.GeneticAlgorithm.svg)](https://www.nuget.org/packages/SimpleML.GeneticAlgorithm/)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=PFalkowski_SimpleML&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=PFalkowski_SimpleML)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://choosealicense.com/licenses/mit/)
[![Buy Me a Coffee](https://img.shields.io/badge/Buy%20Me%20a%20Coffee-support-yellow.svg)](https://www.buymeacoffee.com/piotrfalkowski)

A simple, extensible Genetic Algorithm implementation for .NET 8.

## Install

```bash
dotnet add package SimpleML.GeneticAlgorithm
```

## Usage

```csharp
// Implement IFitnessFunction for your problem
public class MyFitness : IFitnessFunction
{
    public double Evaluate(Genotype genotype) => /* compute fitness */;
    public Task<double> EvaluateAsync(Genotype genotype) => Task.FromResult(Evaluate(genotype));
}

// Configure and run
var settings = new GeneticAlgorithmSettings(new MyFitness(), problemSize: 100)
{
    PopulationSize = 5000,
    SurvivalRate = 0.1,
    MutationRate = 0.05,
    StopFunction = new BasicStopFunction { MaxEpochs = 500, MinFitness = 95 }
};

var ga = new GeneticAlgorithm(settings);
await ga.Run();

Console.WriteLine($"Best fitness: {ga.RunInfo.BestFitnessSoFar}");
```

## Key types

| Type | Description |
|------|-------------|
| `GeneticAlgorithm` | Orchestrates the evolution loop |
| `GeneticAlgorithmSettings` | Population size, survival/mutation rates, stop condition |
| `Population` | Gene pool management, selection, and breeding |
| `Genotype` | Individual solution encoded as a boolean array |
| `BasicStopFunction` | Stops after max epochs, target fitness, or fitness plateau |
| `EliteSelection` | Keeps the top N organisms |
| `BinaryTournamentSelection` | Random pairwise tournament selection |

---

The idea of the project is to implement various popular ML algorithms in a simple yet efficient way, conforming to best patterns and practices of modern OO programming. PRs are very welcome.
