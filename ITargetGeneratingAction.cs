namespace cake
{
	public interface ITargetGeneratingAction
	{
		void Invoke(string target, TargetGenerateSettings settings);
		string GetActionHash();
	}
}