using AS_2025.Api.Menu.List;
using AS_2025.Menu;

namespace AS_2025.Api.Menu;

public static class MenuEndpoints
{
    public static void MapMenuEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/menu").WithTags("Menu");

        group.MapGet("/list", () => System.Threading.Tasks.Task.FromResult(new ListMenuResponse
        {
            Items = new List<MenuItem>
            {
                MenuItem.ClientPage("Стартовая панель", "appstore", "dashboard", "/dashboard"),
                MenuItem.ClientPage("Проекты (static)", "project", "projects-static", "/client-static/projects"),
                MenuItem.ClientPage("Заказчики (static)", "contacts", "customers-static", "/client-static/customers"),
                MenuItem.ClientPage("Команды (static)", "team", "teams-static", "/client-static/teams"),
                MenuItem.ModelPage("Презентация таблиц", "table", "tableControlsPresentation", "/table-controls-presentation"),
                MenuItem.ModelPage("Отделы", "database", "department", "/departments"),
                MenuItem.ModelPage("Команды", "team", "team", "/teams"),
                MenuItem.ModelPage("Сотрудники", "user", "employee", "/employees"),
                MenuItem.ModelPage("Проекты", "project", "project", "/projects"),
                MenuItem.ModelPage("Задачи", "carry-out", "task", "/tasks"),
                MenuItem.ModelPage("Клиенты", "smile", "client", "/clients"),
            }
        }));
    }
}