using System;
using System.Collections.Generic;

namespace MyStrategyFactory.Extensions
{
	internal class StrategyTypeMapper
	{
		private readonly IDictionary<string, Type> _mapStrategy;

		public StrategyTypeMapper()
		{
			_mapStrategy = new Dictionary<string, Type>();
		}

		public void Add(string strategyCode, Type strategyType)
		{
			_mapStrategy.Add(strategyCode, strategyType);
		}

		public Type Get(string strategyCode)
		{
			return _mapStrategy[strategyCode];
		}

	}
}