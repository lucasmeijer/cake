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

			if (instructions.Settings.InputFiles.Any(inputFile => !File.Exists(inputFile)))
				throw new MissingDependencyException();

			if (NeedToGenerate(targetFile, instructions.Settings))
				Generate(targetFile, instructions);
		}

		private bool NeedToGenerate(string targetFile, TargetBuildSettings buildSettings)
		{
			var recordOfLastBuild = _buildHistory.FindRecordFor(targetFile);

			if (recordOfLastBuild == null)
				return true;

			if (!File.Exists(targetFile))
				return true;
			
			if (File.GetLastWriteTimeUtc(targetFile) < recordOfLastBuild.ModificationTimeOfTargetFile())
				return true;

			return buildSettings.InputFiles.Any(sourceFile => recordOfLastBuild.ModificationTimeOf(sourceFile) != File.GetLastWriteTimeUtc(sourceFile));
		}

		private void Generate(string targetFile, TargetBuildInstructions instructions)
		{
			GenerateCallback(targetFile, instructions);
			instructions.Action(targetFile, instructions.Settings);

			var record = new GenerationRecord(targetFile, instructions.Settings);
			_buildHistory.AddRecord(record);
		}

		public void RegisterTarget(string targetFile, TargetBuildInstructions instructions)
		{
			_graph.Add(targetFile,instructions);
		}
	}
}