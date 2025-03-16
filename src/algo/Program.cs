using algo.Models;

namespace algo
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Пример входных данных:
            // N групп с коэффициентами эффективности
            List<Team> teams = new List<Team>
            {
                new Team(1, 2),
                new Team(2, 3),
                new Team(3, 1)
            };

            // M проектов
            List<Project> projects = new List<Project>
            {
                new Project(101, 10, 5000, 1000),
                new Project(102, 20, 8000, 1500),
                new Project(103, 5, 3000, 500),
                new Project(104, 15, 7000, 1200),
                new Project(105, 8, 4000, 800)
            };

            // Предположим, что квартал длится 90 дней
            int quarterDays = 90;

            SimulatedAnnealingScheduler scheduler = new SimulatedAnnealingScheduler(teams, projects, quarterDays);
            List<ProjectInWork> result = scheduler.Run();

            // Вывод результата
            Console.WriteLine("Результирующий план работ:");
            foreach (var assignment in result)
            {
                Console.WriteLine($"Проект {assignment.ProjectId} -> Команда {assignment.TeamId} с {assignment.Start} по {assignment.End} день");
            }
        }
    }
}
