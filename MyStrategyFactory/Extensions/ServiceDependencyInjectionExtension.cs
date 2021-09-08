using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyStrategyFactory.Abstractions;

namespace MyStrategyFactory.Extensions
{
	public static class ServiceDependencyInjectionExtension
	{
        /// <summary>
        /// Add current AppDomain strategy types automatically
        /// </summary>
        /// <param name="serviceCollection"></param>
        public static void AddStrategyAutomatically(this IServiceCollection serviceCollection)
        {
            AddStrategyAutomatically(serviceCollection, AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Add strategy types from passed assemblies
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="assemblies"></param>
        public static void AddStrategyAutomatically(this IServiceCollection serviceCollection,
            IEnumerable<Assembly> assemblies)
        {
            var strategyTypes = Util.GetStrategyTypesFromAssemblies(assemblies);

            var strategyTypeMapper = new StrategyTypeMapper();

            foreach (var strategyType in strategyTypes)
            {
                strategyTypeMapper.Add(strategyType.Code, strategyType.ImplType);
                serviceCollection.AddTransient(strategyType.ImplType);
            }

            serviceCollection.AddSingleton(strategyTypeMapper);
            serviceCollection.AddSingleton<IStrategyFactory, ServiceStrategyFactory>();
        }

    }
}
