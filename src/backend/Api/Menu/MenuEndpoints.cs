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
                MenuItem.ClientPage("Стартовая панель", "appstore", "dashboard"),
                MenuItem.ClientPage("Проекты (static)", "project", "projects-static"),
                MenuItem.ClientPage("Заказчики (static)", "contacts", "customers-static"),
                MenuItem.ClientPage("Команды (static)", "team", "teams-static"),
                MenuItem.ModelPage("Презентация таблиц", "table", "tableControlsPresentation"),
                MenuItem.ModelPage("Отделы", "database", "department"),
                MenuItem.ModelPage("Команды", "team", "team"),
                MenuItem.ModelPage("Сотрудники", "user", "employee"),
                MenuItem.ModelPage("Проекты", "project", "project"),
                MenuItem.ModelPage("Задачи", "carry-out", "task"),
                MenuItem.ModelPage("Клиенты", "smile", "client"),
            }
        }));
    }
}