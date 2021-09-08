using System;

namespace MyStrategyFactory
{
	internal class StrategyType
	{
		public string Code { get; }
		public Type ImplType { get; }

		public StrategyType(string code, Type implType)
		{
			Code = code;
			ImplType = implType;
		}
	}
}
