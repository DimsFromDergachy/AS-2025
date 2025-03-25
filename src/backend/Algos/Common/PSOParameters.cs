namespace AS_2025.Algos.Common
{
    public record PSOParameters(
    int PopulationSize,
    int Iterations,
    double CognitiveCoefficient,   // вероятность «подтягивания» к личному лучшему решению
    double SocialCoefficient,      // вероятность «подтягивания» к глобальному лучшему решению
    double RandomMoveProbability   // вероятность случайного изменения (рандомного шага)
);
}
