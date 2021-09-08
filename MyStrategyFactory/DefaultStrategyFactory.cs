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
        private static readonly Lazy<IDictionary<string, Type>> StrategyMap =
            new Lazy<IDictionary<string, Type>>(LoadStrategyFromAssembly);

        private static IDictionary<string, Type> LoadStrategyFromAssembly()
        {
            var map = new Dictionary<string, Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var types = Util.GetStrategyTypesFromAssemblies(assemblies);

            foreach (var type in types)
            {
                // Always add self type
                map.Add(type.Code, type.ImplType);
            }

            return map;
        }

        public T CreateStrategy<T>(string strategyCode) where T : class
        {
            var t = typeof(T);

            if (string.IsNullOrEmpty(t.FullName))
            {
                throw new ArgumentNullException(nameof(t.FullName));
            }

            // Only registry self type
            var strategyType = StrategyMap.Value[strategyCode];

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
