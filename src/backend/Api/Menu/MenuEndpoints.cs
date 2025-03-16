using AS_2025.Api.Menu.List;

namespace AS_2025.Api.Menu;

public static class MenuEndpoints
{
    public static void MapMenuEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/menu").WithTags("Menu");

        group.MapGet("/list", () => System.Threading.Tasks.Task.FromResult(new ListMenuResponse
        {
            Items = new List<ListMenuItem>
            {
                new()
                {
                    Title = "Departments",
                    Link = "/departments/list"
                },
                new()
                {
                    Title = "Teams",
                    Link = "/teams/list"
                },
                new()
                {
                    Title = "Employees",
                    Link = "/employees/list"
                }
            }
        }));
    }
}