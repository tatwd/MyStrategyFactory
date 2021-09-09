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
            if (!TryCreateStrategy<T>(strategyCode, out T targetType))
            {
                throw new ArgumentException($"Not found a valid type to be '{typeof(T).Name}' by '{strategyCode}'");
            }

            return targetType;
        }

        public bool TryCreateStrategy<T>(string strategyCode, out T targetType) where T : class
        {
            targetType = default;
            var t = typeof(T);

            if (string.IsNullOrEmpty(t.FullName))
            {
                return false;
            }

            if (!StrategyMap.Value.TryGet(strategyCode, out var strategyType))
            {
                return false;
            }

            if (!typeof(T).IsAssignableFrom(strategyType))
            {
                return false;
            }

            try
            {
                var strategyNewExpr = Expression.New(strategyType);
                var lambdaExpr = Expression.Lambda(typeof(Func<T>), strategyNewExpr);
                var ctorDelegate = (Func<T>)lambdaExpr.Compile();
                var strategy = ctorDelegate();
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
