using AS_2025.Algos.Common;
using AS_2025.Algos.TasksSchedule.Models;

namespace AS_2025.Algos.Backpack;

public class BackpackWithAnnealing
{
    private readonly int _capacity;
    private readonly IReadOnlyCollection<Item> _items;
    private readonly AnnealingParameters _parameters;

    public BackpackWithAnnealing(int capacity, IReadOnlyCollection<Item> items, AnnealingParameters parameters)
    {
        _capacity = capacity;
        _items = items;
        _parameters = parameters;
    }

    public SolutionResponse<Item> Solve()
    {
        var items = _items.Select(i => i with { }).ToList();
        var numItems = _items.Count;

        var temperature = _parameters.Temperature;

        Random rnd = new Random();

        // Инициализация начального решения (случайное заполнение)
        bool[] currentSolution = new bool[numItems];
        for (int i = 0; i < numItems; i++)
        {
            currentSolution[i] = rnd.NextDouble() > 0.5;
        }

        // Если решение недопустимо, восстанавливаем его (удаляем лишние предметы)
        currentSolution = RepairSolution(currentSolution, items, _capacity);

        // Сохраняем лучшее найденное решение
        bool[] bestSolution = (bool[])currentSolution.Clone();
        int bestValue = EvaluateSolution(bestSolution, items, _capacity);

        // Основной цикл алгоритма отжига
        while (temperature > 1)
        {
            for (int i = 0; i < _parameters.IterationsPerTemp; i++)
            {
                // Создаём соседнее решение: копия текущего с изменением одного случайного элемента
                bool[] newSolution = (bool[])currentSolution.Clone();
                int index = rnd.Next(numItems);
                newSolution[index] = !newSolution[index];

                // Если новое решение недопустимо, восстанавливаем его
                newSolution = RepairSolution(newSolution, items, _capacity);

                int currentVal = EvaluateSolution(currentSolution, items, _capacity);
                int newVal = EvaluateSolution(newSolution, items, _capacity);
                int delta = newVal - currentVal;

                // Принимаем новое решение, если оно лучше или с вероятностью, зависящей от температуры
                if (delta > 0 || Math.Exp(delta / temperature) > rnd.NextDouble())
                {
                    currentSolution = newSolution;
                    if (newVal > bestValue)
                    {
                        bestSolution = (bool[])newSolution.Clone();
                        bestValue = newVal;
                    }
                }
            }
            // Охлаждение: уменьшаем температуру
            temperature *= _parameters.CoolingRate;
        }

        var selectedItems = new List<Item>();
        for (int i = 0; i < numItems; i++)
        {
            if (!bestSolution[i])
            {
                continue;
            }

            selectedItems.Add(_items.ElementAt(i));
        }

        return new SolutionResponse<Item>(selectedItems, bestValue);
    }

    /*static void Evaluate()
    {
        // Пример набора предметов (вес, стоимость)
        List<Item> items = new List<Item>
        {
            new Item(12, 4),
            new Item(2, 2),
            new Item(1, 2),
            new Item(1, 1),
            new Item(4, 10)
        };

        // Вместимость рюкзака
        int capacity = 15;
        int numItems = items.Count;

        // Параметры алгоритма отжига
        double temperature = 1000.0;
        double coolingRate = 0.995;
        int iterationsPerTemp = 100;

        Random rnd = new Random();

        // Инициализация начального решения (случайное заполнение)
        bool[] currentSolution = new bool[numItems];
        for (int i = 0; i < numItems; i++)
        {
            currentSolution[i] = rnd.NextDouble() > 0.5;
        }
        // Если решение недопустимо, восстанавливаем его (удаляем лишние предметы)
        currentSolution = RepairSolution(currentSolution, items, capacity);

        // Сохраняем лучшее найденное решение
        bool[] bestSolution = (bool[])currentSolution.Clone();
        int bestValue = EvaluateSolution(bestSolution, items, capacity);

        // Основной цикл алгоритма отжига
        while (temperature > 1)
        {
            for (int i = 0; i < iterationsPerTemp; i++)
            {
                // Создаём соседнее решение: копия текущего с изменением одного случайного элемента
                bool[] newSolution = (bool[])currentSolution.Clone();
                int index = rnd.Next(numItems);
                newSolution[index] = !newSolution[index];

                // Если новое решение недопустимо, восстанавливаем его
                newSolution = RepairSolution(newSolution, items, capacity);

                int currentVal = EvaluateSolution(currentSolution, items, capacity);
                int newVal = EvaluateSolution(newSolution, items, capacity);
                int delta = newVal - currentVal;

                // Принимаем новое решение, если оно лучше или с вероятностью, зависящей от температуры
                if (delta > 0 || Math.Exp(delta / temperature) > rnd.NextDouble())
                {
                    currentSolution = newSolution;
                    if (newVal > bestValue)
                    {
                        bestSolution = (bool[])newSolution.Clone();
                        bestValue = newVal;
                    }
                }
            }
            // Охлаждение: уменьшаем температуру
            temperature *= coolingRate;
        }

        // Вывод результатов
        Console.WriteLine("Лучшее найденное значение: " + bestValue);
        Console.Write("Выбранные предметы (индексы): ");
        for (int i = 0; i < numItems; i++)
        {
            if (bestSolution[i])
                Console.Write(i + " ");
        }
        Console.WriteLine();
    }*/


    // Функция оценки: вычисляет суммарную стоимость решения, если оно допустимо (вес не превышает capacity), иначе возвращает 0
    private static int EvaluateSolution(bool[] solution, List<Item> items, int capacity)
    {
        int totalWeight = 0;
        int totalValue = 0;
        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i])
            {
                totalWeight += items[i].Weight;
                totalValue += items[i].Value;
            }
        }
        // Если вес превышает вместимость, решение недопустимо
        if (totalWeight > capacity)
            return 0;
        return totalValue;
    }

    // Функция восстановления решения: если общее значение веса превышает вместимость,
    // удаляет случайные выбранные предметы, пока решение не станет допустимым.
    private static bool[] RepairSolution(bool[] solution, List<Item> items, int capacity)
    {
        int totalWeight = 0;
        for (int i = 0; i < solution.Length; i++)
        {
            if (solution[i])
                totalWeight += items[i].Weight;
        }
        Random rnd = new Random();
        while (totalWeight > capacity)
        {
            // Формируем список индексов выбранных предметов
            List<int> selectedIndices = new List<int>();
            for (int i = 0; i < solution.Length; i++)
            {
                if (solution[i])
                    selectedIndices.Add(i);
            }
            if (selectedIndices.Count == 0)
                break;
            // Случайно удаляем один предмет
            int index = selectedIndices[rnd.Next(selectedIndices.Count)];
            solution[index] = false;
            totalWeight -= items[index].Weight;
        }
        return solution;
    }
}
