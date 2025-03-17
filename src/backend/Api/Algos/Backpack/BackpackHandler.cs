using Ardalis.Result;
using AS_2025.Algos.Backpack;
using AS_2025.Algos.Common;
using MediatR;

namespace AS_2025.Api.Algos.Backpack;

public class BackpackHandler : IRequestHandler<BackpackRequest, Result<BackpackResponse>>
{
    public Task<Result<BackpackResponse>> Handle(BackpackRequest request, CancellationToken cancellationToken)
    {
        var inputItems = new List<Item>
        {
            new(12, 4),
            new(2, 2),
            new(1, 2),
            new(1, 1),
            new(4, 10)
        };

        var parameters = new AnnealingParameters(request.Temperature, request.CoolingRate, request.IterationsPerTemp);
        var backpackWithAnnealing = new BackpackWithAnnealing(request.Capacity, inputItems, parameters);
        SolutionResponse<Item> algoResult = backpackWithAnnealing.Solve();

        var response = new BackpackResponse()
        {
            InputItems = inputItems,
            Parameters = parameters,
            BestValue = (int)algoResult.Score,
            SelectedItems = algoResult.Solution
        };

        return System.Threading.Tasks.Task.FromResult(new Result<BackpackResponse>(response));
    }
}