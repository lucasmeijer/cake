using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace bs
{
	public class DependencyGraph
	{
		readonly Dictionary<string, TargetBuildInstructions> graph = new Dictionary<string, TargetBuildInstructions>();
		public Action<string, TargetBuildInstructions> GenerateCallback = (s, i) => { };

		public void RequestTarget(string targetFile)
		{
			var instructions = graph[targetFile];
			
			if (instructions.SourceFiles.Any(sourceFile => !File.Exists(sourceFile)))
				throw new MissingDependencyException();

			if (File.Exists(targetFile))
				return;

			Generate(targetFile, instructions);
		}

		private void Generate(string targetFile, TargetBuildInstructions instructions)
		{
			GenerateCallback(targetFile, instructions);
			instructions.Action(targetFile, instructions.SourceFiles);
		}

		public void RegisterTarget(string targetFile, TargetBuildInstructions instructions)
		{
			graph.Add(targetFile,instructions);
		}
	}

	public class TargetBuildInstructions
	{
		public Action<string, IEnumerable<string>> Action;
		public IEnumerable<string> SourceFiles;
	}
}