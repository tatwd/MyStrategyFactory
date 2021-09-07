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
        private static readonly Lazy<IDictionary<string, Type>> StrategyMap =
            new Lazy<IDictionary<string, Type>>(LoadStrategyFromAssembly);

        private static IDictionary<string, Type> LoadStrategyFromAssembly()
        {
            var map = new Dictionary<string, Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            foreach (var type in assemblies.SelectMany(x => x.DefinedTypes))
            {
                if (type.IsGenericType)
                    continue;
                
                // Now is not support abstract class or interface
                if (!type.IsClass)
                    continue;

                var hasStrategyAttr = type.CustomAttributes.Any(x => x.AttributeType == typeof(StrategyAttribute));
                if (!hasStrategyAttr)
                {
                    continue;
                }

                var strategyAttributes = type.GetCustomAttributes<StrategyAttribute>();
                var implInterfaces = type.GetInterfaces();

                foreach (var strategyAttribute in strategyAttributes)
                {
                    if (type.BaseType != null && type.BaseType != typeof(object))
                    {
                        map.Add($"{strategyAttribute.Code}__{type.BaseType.FullName}", type);
                    }
                    
                    // if (!implInterfaces.Any())
                    // {
                    //     // Add self type
                    //     StrategyMap.Add($"{strategyAttribute.Code}__{type.FullName}", type);
                    //     continue;
                    // }
                    
                    // Always add self type
                    map.Add(strategyAttribute.Code, type);

                    foreach (var implInterface in implInterfaces)
                    {
                        // var interfaceFullName = implInterface.FullName;
                        // if (interfaceFullName == null)
                        //     throw new ArgumentNullException(nameof(interfaceFullName),
                        //         $"The interface's full name of '{type.FullName}' implemented is null");
                        map.Add($"{strategyAttribute.Code}__{implInterface.FullName}", type);
                    }
                    
                    
                }
                
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

            var key = $"{strategyCode}__{t.FullName}";
            var strategyType = StrategyMap.Value.TryGetValue(key, out var cacheType)
                ? cacheType 
                : StrategyMap.Value[strategyCode]; // will return self type

            // System.Diagnostics.Debug.Assert(typeof(T).IsAssignableFrom(strategyType));
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