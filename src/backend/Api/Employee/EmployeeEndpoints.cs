using Ardalis.Result.AspNetCore;
using AS_2025.Api.Employee.List;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AS_2025.Api.Employee;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/employee")
            .WithTags("Employee");

        group.MapGet("/list", async (IMediator mediator, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] ListEmployeesRequest? request) =>
        {
            var result = await mediator.Send(request ?? new ListEmployeesRequest());
            return result.ToMinimalApiResult();
        });
    }
}