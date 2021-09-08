using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MyStrategyFactory.Abstractions;

namespace MyStrategyFactory
{
    public class DefaultStrategyFactory : IStrategyFactory
    {
	    // Maybe this is not a safe but simple implement way
        private static readonly Lazy<StrategyTypeMapper> StrategyMap =
            new Lazy<StrategyTypeMapper>(LoadStrategyFromAssembly);

        private static StrategyTypeMapper LoadStrategyFromAssembly()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var strategyMapper = new StrategyTypeMapper();
            var types = Util.GetStrategyTypesFromAssemblies(assemblies);

            foreach (var type in types)
            {
                // Always add self type
                strategyMapper.Add(type.Code, type.ImplType);
            }

            return strategyMapper;
        }

        public T CreateStrategy<T>(string strategyCode) where T : class
        {
            var t = typeof(T);

            if (string.IsNullOrEmpty(t.FullName))
            {
                throw new ArgumentNullException(nameof(t.FullName));
            }

            // Only registry self type
            var strategyType = StrategyMap.Value.Get(strategyCode);

            if (!typeof(T).IsAssignableFrom(strategyType))
                throw new ArgumentException($"Not found strategy of type '{typeof(T).Name}' for '{strategyCode}' ");

            var strategyNewExpr = Expression.New(strategyType);

            var lambdaExpr = Expression.Lambda(typeof(Func<T>), strategyNewExpr);
            var ctorDelegate = (Func<T>)lambdaExpr.Compile();
            var strategy = ctorDelegate();
            return strategy;
        }


    }
}
