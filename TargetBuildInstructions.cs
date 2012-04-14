using System;

namespace bs
{
	public class TargetBuildInstructions
	{
		public Action<string, TargetBuildSettings> Action;
		public TargetBuildSettings Settings;
	}
}