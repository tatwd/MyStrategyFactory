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
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_strategyTypeMapper =
				new Lazy<StrategyTypeMapper>(() => _serviceProvider.GetRequiredService<StrategyTypeMapper>());
		}

		public T CreateStrategy<T>(string strategyCode) where T : class
		{
			var strategyType = _strategyTypeMapper.Value.Get(strategyCode);
			if (!typeof(T).IsAssignableFrom(strategyType))
				throw new ArgumentException($"Not found strategy of type '{typeof(T).Name}' for '{strategyCode}' ");

			var strategyImpl = _serviceProvider.GetRequiredService(strategyType);
			return (T)strategyImpl;
		}
	}
}
