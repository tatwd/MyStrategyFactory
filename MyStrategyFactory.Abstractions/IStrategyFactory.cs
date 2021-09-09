namespace MyStrategyFactory.Abstractions
{
    public interface IStrategyFactory
    {
        T CreateStrategy<T>(string strategyCode) where T : class;
        bool TryCreateStrategy<T>(string strategyCode, out T targetType) where T : class;
    }
}
