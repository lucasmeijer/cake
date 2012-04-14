namespace cake
{
	public interface ITargetGeneratingAction
	{
		void Invoke(string target, TargetGenerateInstructions instructions);
		string GetActionHash();
	}
}