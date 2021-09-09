using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using MyStrategyFactory.Abstractions;

namespace MyStrategyFactory.Extensions
{
    public class ServiceStrategyFactory : IStrategyFactory
    {
        private readonly Lazy<StrategyTypeMapper> _strategyTypeMapper;
        private readonly IServiceProvider _serviceProvider;

        public ServiceStrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ??
                throw new ArgumentNullException(nameof(serviceProvider));
            _strategyTypeMapper =
                new Lazy<StrategyTypeMapper>(() => _serviceProvider.GetRequiredService<StrategyTypeMapper>());
        }

        public T CreateStrategy<T>(string strategyCode) where T : class
        {
            if (!TryCreateStrategy<T>(strategyCode, out T targetType))
            {
                throw new ArgumentException($"Not found a valid type to be '{typeof(T).Name}' by '{strategyCode}'");
            }

            return targetType;
        }

        public bool TryCreateStrategy<T>(string strategyCode, out T targetType) where T : class
        {
            targetType = default;

            if (!_strategyTypeMapper.Value.TryGet(strategyCode, out var strategyType))
            {
                return false;
            }

            if (!typeof(T).IsAssignableFrom(strategyType))
            {
                return false;
            }

            try
            {
                var strategyImpl = _serviceProvider.GetRequiredService(strategyType);
                targetType = (T) strategyImpl;
                return true;
            }
            catch { return false; }
        }
    }
}
