using Ardalis.Result;
using AS_2025.Domain.Common;
using Bogus;
using MediatR;

namespace AS_2025.Api.TableControlsPresentation.List;

public class ListTableControlsPresentationsHandler : IRequestHandler<ListTableControlsPresentationsRequest, Result<ListTableControlsPresentationsResponse>>
{
    public async Task<Result<ListTableControlsPresentationsResponse>> Handle(ListTableControlsPresentationsRequest request, CancellationToken cancellationToken)
    {
        var faker = new Faker<ListTableControlsPresentationsItem>("en");

        faker.RuleFor(x => x.Id, Guid.CreateVersion7);
        faker.RuleFor(x => x.Url, f => $"/test/{f.Random.String2(10)}");
        faker.RuleFor(x => x.String, f => f.Random.Words(f.Random.Int(1, 3)));
        faker.RuleFor(x => x.StringPattern, f => f.Random.String2(8, 15));
        faker.RuleFor(x => x.Text, f => f.Random.Words(f.Random.Int(15, 25)));
        faker.RuleFor(x => x.Integer, f => f.Random.Int());
        faker.RuleFor(x => x.Double, f => f.Random.Double());
        faker.RuleFor(x => x.Date, f => f.Date.PastDateOnly());
        faker.RuleFor(x => x.Checkbox, f => f.Random.Bool());
        faker.RuleFor(x => x.Link, f => $"https://google.com/?q={f.Company.CompanyName()}");
        faker.RuleFor(x => x.Percent, f => f.Random.Int(0, 120));
        faker.RuleFor(x => x.Tags, f => Enumerable.Range(1, f.Random.Int(1, 3)).Select(_ => f.PickRandom<EmployeeLevel>()).ToList());
        faker.RuleFor(x => x.ComplexTags, f => Enumerable.Range(1, f.Random.Int(1, 3)).Select(_ => new ComplexTag(f.PickRandom<EmployeeLevel>(), f.Random.Int(1, 5))).ToList());
        faker.RuleFor(x => x.Tag, f => f.PickRandom<TaskPriority>());

        return new ListTableControlsPresentationsResponse
        {
            Items = faker.Generate(50)
        };
    }
}
