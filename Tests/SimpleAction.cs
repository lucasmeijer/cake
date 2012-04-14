using System;

namespace bs.Tests
{
	class SimpleAction : ITargetGeneratingAction
	{
		private readonly Action<string, TargetGenerateSettings> _invoke;
		private readonly string _actionHash;

		public SimpleAction(Action<string, TargetGenerateSettings> invoke, string actionHash)
		{
			_invoke = invoke;
			_actionHash = actionHash;
		}

		public void Invoke(string target, TargetGenerateSettings instructions)
		{
			_invoke(target, instructions);
		}

		public string GetActionHash()
		{
			return _actionHash;
		}
	}
}