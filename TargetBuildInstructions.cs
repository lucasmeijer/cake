using System;
using System.Collections.Generic;

namespace bs
{
	public class TargetBuildInstructions
	{
		public Action<string, TargetBuildSettings> Action;
		public TargetBuildSettings Settings;
	}

	public class TargetBuildSettings
	{
		public IEnumerable<string> InputFiles;
	}
}