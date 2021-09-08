using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MyStrategyFactory.Extensions
{
	public static class ServiceDependencyInjectionExtension
	{


		/// <summary>
		/// Add all strategies from assembly
		/// <example>
		///		services.AddStrategyAutomatically("foo")
		/// </example>
		/// </summary>
		/// <param name="serviceCollection"></param>
		/// <param name="assemblyName"></param>
		public static void AddStrategyAutomatically(this IServiceCollection serviceCollection,
			string assemblyName)
		{
			var assembly = Assembly.Load(new AssemblyName(assemblyName));
			var strategyTypes = Util.GetStrategyTypesFromAssemblies(new[] { assembly });

			foreach (var strategyType in strategyTypes)
			{
				serviceCollection.AddTransient(strategyType.ImplType);

			}


		}




	}
}
