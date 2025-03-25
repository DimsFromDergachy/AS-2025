namespace AS_2025.Algos.Common
{
    public record GeneticParameters(
    int PopulationSize,
    int Generations,
    double CrossoverRate,
    double MutationRate,
    int TournamentSize);
}
