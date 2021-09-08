using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyStrategyFactory.Abstractions;
using MyStrategyFactory.Extensions;
using Xunit;

namespace MyStrategyFactory.Tests
{
	public class ServiceDependencyInjectionExtension
	{
		private IStrategyFactory CreateStrategyFactory()
		{
			var serviceCollection = new ServiceCollection();
            var assemblies = new[]
            {
                Assembly.Load(new AssemblyName("MyStrategyFactory.Tests"))
            };
			serviceCollection.AddStrategyAutomatically(assemblies);
			var serviceProvider = serviceCollection.BuildServiceProvider();
			return serviceProvider.GetService<IStrategyFactory>();
		}

        [Fact]
        public void CreateStrategyFactory_DoubleCall()
        {
            var factory1 = CreateStrategyFactory();
            var factory2 = CreateStrategyFactory();
            Assert.NotEqual(factory1, factory2);
        }

		[Fact]
		public void CreateStrategy_Of_EmptyTestStrategy_Ok()
		{
			var factory = CreateStrategyFactory();
            var strategy = factory.CreateStrategy<EmptyTestStrategy>("test101");
			Assert.NotNull(factory);
		}

        [Fact]
        public void CreateStrategy_Of_IStrategy_Ok()
        {
            var defaultFactory = CreateStrategyFactory();
            var strategy = defaultFactory.CreateStrategy<IStrategy>("test102");
            Assert.NotNull(strategy);
        }

        [Fact]
        public void CreateStrategy_Of_InternalTestStrategy_Ok()
        {
            var defaultFactory = CreateStrategyFactory();
            var strategy = defaultFactory.CreateStrategy<InternalTestStrategy>("test102");
            Assert.NotNull(strategy);
        }

        [Fact]
        public void CreateStrategy_Of_NotFoundStrategy_Throw()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var defaultFactory = CreateStrategyFactory();
                var strategy = defaultFactory.CreateStrategy<EmptyTestStrategy>("test102");
            });
        }

	}
}
