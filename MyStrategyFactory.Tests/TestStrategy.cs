using MyStrategyFactory.Abstractions;

namespace MyStrategyFactory.Tests
{
    /// <summary>
    /// will register EmptyTestStrategy by 'test101' code
    /// </summary>
    [Strategy("test101")]
    public class EmptyTestStrategy
    {
    }
    
    // /// <summary>
    // /// will register RepeatEmptyTestStrategy by 'test101' code
    // /// </summary>
    // [Strategy("test101")]
    // public class RepeatEmptyTestStrategy
    // {
    // }
    
    /// <summary>
    /// will register IStrategy by 'test102' code
    /// </summary>
    [Strategy("test102")]
    internal class InternalTestStrategy : IStrategy
    {
        public void Apply()
        {
        }
    }
    
}