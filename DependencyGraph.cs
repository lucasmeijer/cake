using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace bs
{
	public class DependencyGraph
	{
		readonly Dictionary<string, TargetGenerateInstructions> _graph = new Dictionary<string, TargetGenerateInstructions>();
		public Action<string, TargetGenerateInstructions> GenerateCallback = (s, i) => { };
		private readonly BuildHistory _buildHistory;

		public DependencyGraph() : this(new BuildHistory())
		{
		}

		public DependencyGraph(BuildHistory history)
		{
			_buildHistory = history;
		}

		public void RequestTarget(string targetFile)
		{
			var instructions = _graph[targetFile];

			if (instructions.Settings.InputFiles.Any(inputFile => !File.Exists(inputFile)))
				throw new MissingDependencyException();

			if (NeedToGenerate(targetFile, instructions.Settings))
				Generate(targetFile, instructions);
		}

		private bool NeedToGenerate(string targetFile, TargetGenerateSettings generateSettings)
		{
			var recordOfLastBuild = _buildHistory.FindRecordFor(targetFile);

			if (recordOfLastBuild == null)
				return true;

			if (!File.Exists(targetFile))
				return true;

			if (!generateSettings.Equals(recordOfLastBuild.Settings))
				return true;

			if (File.GetLastWriteTimeUtc(targetFile) < recordOfLastBuild.ModificationTimeOfTargetFile())
				return true;

			return generateSettings.InputFiles.Any(sourceFile => recordOfLastBuild.ModificationTimeOf(sourceFile) != File.GetLastWriteTimeUtc(sourceFile));
		}

		private void Generate(string targetFile, TargetGenerateInstructions instructions)
		{
			GenerateCallback(targetFile, instructions);
			instructions.Action.Invoke(targetFile, instructions.Settings);

			var record = new GenerationRecord(targetFile, instructions.Settings, "");
			_buildHistory.AddRecord(record);
		}

		public void RegisterTarget(string targetFile, TargetGenerateInstructions instructions)
		{
			_graph.Add(targetFile,instructions);
		}
	}
}