using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyStrategyFactory.Abstractions;

namespace MyStrategyFactory
{
	internal static class Util
	{
		internal static IList<StrategyType> GetStrategyTypesFromAssemblies(IEnumerable<Assembly> assemblies)
		{
			var list = new List<StrategyType>(16);

			foreach (var type in assemblies.SelectMany(x => x.DefinedTypes))
			{
				var hasStrategyAttr = type.CustomAttributes.Any(x => x.AttributeType == typeof(StrategyAttribute));
				if (!hasStrategyAttr)
				{
					continue;
				}

                if (type.IsGenericType)
                    throw new NotSupportedException("Not support impl strategy on a generic type");

                if (type.IsAbstract || type.IsInterface)
                    throw new NotSupportedException("Not support impl strategy on a abstract class or interface");

                if (!type.IsClass)
                    throw new NotSupportedException("Must impl strategy on a class type");


				var strategyAttributes = type.GetCustomAttributes<StrategyAttribute>();

				foreach (var strategyAttribute in strategyAttributes)
				{
					// Always add self type
					var strategyTypeInfo = new StrategyType(strategyAttribute.Code, type);
					list.Add(strategyTypeInfo);
				}

			}

			return list;
		}
	}
}
