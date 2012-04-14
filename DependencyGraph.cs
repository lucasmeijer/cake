using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace bs
{
	public class DependencyGraph
	{
		readonly Dictionary<string, TargetBuildInstructions> _graph = new Dictionary<string, TargetBuildInstructions>();
		public Action<string, TargetBuildInstructions> GenerateCallback = (s, i) => { };
		private readonly BuildHistory _buildHistory = new BuildHistory();

		public void RequestTarget(string targetFile)
		{
			var instructions = _graph[targetFile];

			if (instructions.SourceFiles.Any(sourceFile => !File.Exists(sourceFile)))
				throw new MissingDependencyException();

			if (NeedToGenerate(targetFile, instructions))
				Generate(targetFile, instructions);
		}

		private bool NeedToGenerate(string targetFile, TargetBuildInstructions instructions)
		{
			if (!File.Exists(targetFile))
				return true;

			var recordOfLastBuild = _buildHistory.FindRecordFor(targetFile);

			return instructions.SourceFiles.Any(sourceFile => recordOfLastBuild.ModificationTimeOf(sourceFile) != File.GetLastWriteTimeUtc(sourceFile));
		}

		private void Generate(string targetFile, TargetBuildInstructions instructions)
		{
			GenerateCallback(targetFile, instructions);
			instructions.Action(targetFile, instructions.SourceFiles);

			var record = new GenerationRecord(targetFile, instructions.SourceFiles);
			_buildHistory.AddRecord(record);
		}

		public void RegisterTarget(string targetFile, TargetBuildInstructions instructions)
		{
			_graph.Add(targetFile,instructions);
		}
	}
}