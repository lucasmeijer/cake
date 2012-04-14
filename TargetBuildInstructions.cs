using System;
using System.Collections.Generic;

namespace bs
{
	public class TargetBuildInstructions
	{
		public Action<string, IEnumerable<string>> Action;
		public IEnumerable<string> SourceFiles;
	}
}