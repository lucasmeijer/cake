namespace cake
{
	public interface ITargetGeneratingAction
	{
		void Invoke(TargetGenerateSettings settings);
		string GetActionHash();
	}
}