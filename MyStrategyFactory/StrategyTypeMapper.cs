using System;
using System.Collections.Generic;

namespace MyStrategyFactory
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

        public bool TryGet(string strategyCode, out Type type)
        {
            return _mapStrategy.TryGetValue(strategyCode, out type);
        }

    }
}
