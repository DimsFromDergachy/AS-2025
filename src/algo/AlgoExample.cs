namespace Algo;

public class AlgoExample
{
    public double PI = Math.PI;
}

public class AlgoExampleTest
{
    [Fact]
    internal void SomeTest()
    {
     // Assert.Equal(3,      new AlgoExample().PI, 0);
        Assert.Equal(3.1,    new AlgoExample().PI, 1);
        Assert.Equal(3.14,   new AlgoExample().PI, 2);
        Assert.Equal(3.142,  new AlgoExample().PI, 3);
        Assert.Equal(3.1416, new AlgoExample().PI, 4);
    }
}