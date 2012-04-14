using System;

namespace cake.Tests
{
	class SimpleAction : ITargetGeneratingAction
	{
		private readonly Action<string, TargetGenerateInstructions> _invoke;
		private readonly string _actionHash;

		public SimpleAction(Action<string, TargetGenerateInstructions> invoke, string actionHash)
		{
			_invoke = invoke;
			_actionHash = actionHash;
		}

		public void Invoke(string target, TargetGenerateInstructions instructions)
		{
			_invoke(target, instructions);
		}

		public string GetActionHash()
		{
			return _actionHash;
		}
	}
}