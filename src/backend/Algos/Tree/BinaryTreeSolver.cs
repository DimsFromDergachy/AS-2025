using AS_2025.Algos.Tree.Models;

namespace AS_2025.Algos.Tree
{
    public class BinaryTreeSolver<T>
    {
        private readonly IEnumerable<T> _elements;
        private readonly IComparer<T> _comparer;

        // Конструктор принимает последовательность элементов для построения дерева и (необязательно) компаратор.
        public BinaryTreeSolver(IEnumerable<T> elements, IComparer<T>? comparer = null)
        {
            _elements = elements;
            _comparer = comparer ?? Comparer<T>.Default;
        }

        // Метод Solve строит бинарное дерево поиска, вставляя все элементы, и возвращает результат в виде TreeSolution<T>
        public TreeSolution<T> Solve()
        {
            TreeNode<T>? root = null;

            // Последовательно вставляем каждый элемент из _elements в дерево
            foreach (var element in _elements)
            {
                root = Insert(root, element);
            }

            // Выполняем симметричный (in‑order) обход дерева для получения отсортированного списка значений
            var inOrderList = new List<T>();
            InOrderTraversal(root, inOrderList);

            if (root == null)
                throw new InvalidOperationException("Дерево не может быть пустым");

            return new TreeSolution<T>(root, inOrderList);
        }

        // Рекурсивная вставка нового значения в бинарное дерево поиска
        private TreeNode<T> Insert(TreeNode<T>? node, T value)
        {
            if (node == null)
                return new TreeNode<T>(value);

            // Используем _comparer для сравнения значений
            if (_comparer.Compare(value, node.Value) <= 0)
            {
                node.Left = Insert(node.Left, value);
            }
            else
            {
                node.Right = Insert(node.Right, value);
            }

            return node;
        }

        // Рекурсивный симметричный обход (in‑order) дерева
        private void InOrderTraversal(TreeNode<T>? node, List<T> result)
        {
            if (node == null)
                return;

            InOrderTraversal(node.Left, result);
            result.Add(node.Value);
            InOrderTraversal(node.Right, result);
        }
    }
}
