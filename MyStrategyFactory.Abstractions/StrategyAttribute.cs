using System;
using System.Diagnostics.CodeAnalysis;

namespace MyStrategyFactory.Abstractions
{
    public class StrategyAttribute : Attribute
    {
        /// <summary>
        /// This is the strategy identity code.
        /// </summary>
        public string Code { get; private set; }
        
        public StrategyAttribute([NotNull] string code)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }
    }
}