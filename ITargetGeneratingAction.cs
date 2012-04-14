namespace cake
{
	public interface ITargetGeneratingAction
	{
		void Invoke(string target, TargetGenerateSettings instructions);
		string GetActionHash();
	}
}