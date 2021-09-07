namespace MyStrategyFactory.Abstractions
{
    public interface IStrategyFactory
    {
        T CreateStrategy<T>(string strategyCode) where T : class;
    }
}