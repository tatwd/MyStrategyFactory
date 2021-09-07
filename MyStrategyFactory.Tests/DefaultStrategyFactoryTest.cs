using System;
using System.Collections.Generic;
using MyStrategyFactory.Abstractions;
using Xunit;

namespace MyStrategyFactory.Tests
{
    public class DefaultStrategyFactoryTest
    {
        [Fact]
        public void CreateStrategy_Of_EmptyTestStrategy_Ok()
        {
            var defaultFactory = new DefaultStrategyFactory();
            var strategy = defaultFactory.CreateStrategy<EmptyTestStrategy>("test101");
            Assert.NotNull(strategy);
        }
        
        // [Fact]
        // public void LoadDefaultStrategyFactory_Of_RepeatEmptyTestStrategy_Throw()
        // {
        //     Assert.Throws<TypeInitializationException>(() =>
        //     {
        //         var defaultFactory = new DefaultStrategyFactory();
        //     });
        // }
        
        [Fact]
        public void CreateStrategy_Of_IStrategy_Ok()
        {
            var defaultFactory = new DefaultStrategyFactory();
            var strategy = defaultFactory.CreateStrategy<IStrategy>("test102");
            Assert.NotNull(strategy);
        }
        
        [Fact]
        public void CreateStrategy_Of_InternalTestStrategy_Ok()
        {
            var defaultFactory = new DefaultStrategyFactory();
            var strategy = defaultFactory.CreateStrategy<InternalTestStrategy>("test102");
            Assert.NotNull(strategy);
        }
        
        [Fact]
        public void CreateStrategy_Of_NotFoundStrategy_Throw()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var defaultFactory = new DefaultStrategyFactory();
                var strategy = defaultFactory.CreateStrategy<EmptyTestStrategy>("test102");
            });
        }
        
    }
}