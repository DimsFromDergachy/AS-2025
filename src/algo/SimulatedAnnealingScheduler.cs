using algo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algo
{
    public class SimulatedAnnealingScheduler
    {
        // Параметры алгоритма
        private readonly double initialTemperature = 1000.0;
        private readonly double coolingRate = 0.995;
        private readonly int iterationsPerTemp = 100;
        private readonly int quarterDays; // длительность квартала (например, 90 дней)
        private readonly Random rnd = new Random();

        private List<Team> Teams;
        private List<Project> Projects;

        public SimulatedAnnealingScheduler(List<Team> teams, List<Project> projects, int quarterDays)
        {
            Teams = teams;
            Projects = projects;
            this.quarterDays = quarterDays;
        }

        // Основной метод – симулированный отжиг
        public List<ProjectInWork> Run()
        {
            // Инициализируем начальное решение: ни один проект не назначен
            ScheduleSolution currentSolution = new ScheduleSolution(Teams, Projects);
            ScheduleSolution bestSolution = currentSolution.DeepCopy();
            double bestScore = Evaluate(bestSolution);

            double temperature = initialTemperature;

            while (temperature > 1)
            {
                for (int i = 0; i < iterationsPerTemp; i++)
                {
                    ScheduleSolution newSolution = GetNeighbor(currentSolution);
                    double delta = Evaluate(newSolution) - Evaluate(currentSolution);

                    if (delta > 0 || Math.Exp(delta / temperature) > rnd.NextDouble())
                    {
                        currentSolution = newSolution;
                        double currentScore = Evaluate(currentSolution);
                        if (currentScore > bestScore)
                        {
                            bestSolution = currentSolution.DeepCopy();
                            bestScore = currentScore;
                        }
                    }
                }
                temperature *= coolingRate;
            }

            return ConvertToOutput(bestSolution);
        }

        // Функция оценки решения
        // Оценка = сумма (q_i + c_i) для всех проектов, завершённых до конца квартала.
        private double Evaluate(ScheduleSolution sol)
        {
            double score = 0;
            foreach (var team in Teams)
            {
                int currentTime = 0;
                // Если для команды назначены проекты
                if (sol.TeamSchedules.ContainsKey(team.Id))
                {
                    foreach (var proj in sol.TeamSchedules[team.Id])
                    {
                        // Время на «вникание» (3 дня) + выполнение (округляем вверх)
                        int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                        currentTime += duration;
                        if (currentTime <= quarterDays)
                        {
                            // Если проект успевают выполнить – прибавляем бонус
                            score += (proj.Q + proj.C);
                        }
                        else
                        {
                            // Если проект не укладывается – дальнейшие проекты той же команды также не выполнятся
                            break;
                        }
                    }
                }
            }
            return score;
        }

        // Генерация соседнего решения
        private ScheduleSolution GetNeighbor(ScheduleSolution sol)
        {
            ScheduleSolution neighbor = sol.DeepCopy();

            // Выбираем тип операции случайным образом: 0 - insert, 1 - remove, 2 - swap, 3 - move между командами
            int moveType = rnd.Next(4);

            switch (moveType)
            {
                case 0: // Insert: берем случайный проект из нераспределённых и вставляем в расписание случайной команды
                    if (neighbor.Unscheduled.Count > 0)
                    {
                        int projIndex = rnd.Next(neighbor.Unscheduled.Count);
                        Project proj = neighbor.Unscheduled[projIndex];
                        // Выбираем случайную команду
                        int teamIndex = rnd.Next(Teams.Count);
                        int teamId = Teams[teamIndex].Id;
                        List<Project> teamSchedule = neighbor.TeamSchedules[teamId];
                        // Случайная позиция вставки (от 0 до teamSchedule.Count включительно)
                        int pos = rnd.Next(teamSchedule.Count + 1);
                        teamSchedule.Insert(pos, proj);
                        neighbor.Unscheduled.RemoveAt(projIndex);
                    }
                    break;
                case 1: // Remove: удаляем случайный проект из расписания случайной команды и помещаем его в нераспределённые
                    {
                        // Выбираем случайную команду, у которой есть хотя бы один проект
                        var teamsWithProjects = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count > 0).Select(kvp => kvp.Key).ToList();
                        if (teamsWithProjects.Count > 0)
                        {
                            int teamId = teamsWithProjects[rnd.Next(teamsWithProjects.Count)];
                            List<Project> teamSchedule = neighbor.TeamSchedules[teamId];
                            int pos = rnd.Next(teamSchedule.Count);
                            Project proj = teamSchedule[pos];
                            teamSchedule.RemoveAt(pos);
                            neighbor.Unscheduled.Add(proj);
                        }
                    }
                    break;
                case 2: // Swap: меняем местами два проекта в расписании одной случайной команды
                    {
                        var teamsWithMultiple = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count >= 2).Select(kvp => kvp.Key).ToList();
                        if (teamsWithMultiple.Count > 0)
                        {
                            int teamId = teamsWithMultiple[rnd.Next(teamsWithMultiple.Count)];
                            List<Project> teamSchedule = neighbor.TeamSchedules[teamId];
                            int pos1 = rnd.Next(teamSchedule.Count);
                            int pos2 = rnd.Next(teamSchedule.Count);
                            // Меняем, если индексы различны
                            if (pos1 != pos2)
                            {
                                var temp = teamSchedule[pos1];
                                teamSchedule[pos1] = teamSchedule[pos2];
                                teamSchedule[pos2] = temp;
                            }
                        }
                    }
                    break;
                case 3: // Move между командами: переносим проект из расписания одной команды в расписание другой
                    {
                        var teamsWithProjects = neighbor.TeamSchedules.Where(kvp => kvp.Value.Count > 0).Select(kvp => kvp.Key).ToList();
                        if (teamsWithProjects.Count > 0)
                        {
                            int fromTeamId = teamsWithProjects[rnd.Next(teamsWithProjects.Count)];
                            List<Project> fromSchedule = neighbor.TeamSchedules[fromTeamId];
                            int pos = rnd.Next(fromSchedule.Count);
                            Project proj = fromSchedule[pos];
                            fromSchedule.RemoveAt(pos);
                            // Выбираем другую команду (или ту же, если других нет)
                            int toTeamId = fromTeamId;
                            if (Teams.Count > 1)
                            {
                                do
                                {
                                    toTeamId = Teams[rnd.Next(Teams.Count)].Id;
                                } while (toTeamId == fromTeamId);
                            }
                            List<Project> toSchedule = neighbor.TeamSchedules[toTeamId];
                            int insertPos = rnd.Next(toSchedule.Count + 1);
                            toSchedule.Insert(insertPos, proj);
                        }
                    }
                    break;
            }

            return neighbor;
        }

        // Преобразование решения в итоговый список назначений с вычислением start/end для каждого проекта, укладывающегося в квартал
        private List<ProjectInWork> ConvertToOutput(ScheduleSolution sol)
        {
            List<ProjectInWork> output = new List<ProjectInWork>();

            foreach (var team in Teams)
            {
                int currentTime = 0;
                if (sol.TeamSchedules.ContainsKey(team.Id))
                {
                    foreach (var proj in sol.TeamSchedules[team.Id])
                    {
                        int duration = 3 + (int)Math.Ceiling((double)proj.T / team.Efficiency);
                        int startTime = currentTime;
                        int endTime = currentTime + duration;
                        if (endTime <= quarterDays)
                        {
                            output.Add(new ProjectInWork
                            {
                                ProjectId = proj.Id,
                                TeamId = team.Id,
                                Start = startTime,
                                End = endTime
                            });
                            currentTime = endTime;
                        }
                        else
                        {
                            // Если проект не укладывается в сроки, дальнейшие в этой команде также не выполняются
                            break;
                        }
                    }
                }
            }

            return output;
        }
    }
}
