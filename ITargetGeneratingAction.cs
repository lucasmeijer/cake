namespace bs
{
	public interface ITargetGeneratingAction
	{
		void Invoke(string target, TargetGenerateSettings instructions);
		string GetActionHash();
	}
}