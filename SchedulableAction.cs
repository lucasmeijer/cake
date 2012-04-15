using System.Collections.Generic;

namespace cake
{
	public class SchedulableAction
	{
		public TargetGenerateSettings Settings;
		public IEnumerable<string> InputFilesRequiringGeneration;

		public SchedulableAction(TargetGenerateSettings generateSettings)
			:this(generateSettings,new List<string>())
		{
		}

		public SchedulableAction(TargetGenerateSettings generateSettings, IEnumerable<string> inputFilesRequiringGeneration)
		{
			Settings = generateSettings;
			InputFilesRequiringGeneration = inputFilesRequiringGeneration;
		}
	}
}