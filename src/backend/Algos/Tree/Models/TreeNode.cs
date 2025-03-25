namespace AS_2025.Algos.Tree.Models
{
    public record TreeNode<T>
    {
        public T Value { get; init; }
        public TreeNode<T>? Left { get; set; }
        public TreeNode<T>? Right { get; set; }

        public TreeNode(T value)
        {
            Value = value;
        }
    }
}
