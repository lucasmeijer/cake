using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cake
{
	public class DependencyGraph
	{
		readonly Dictionary<string, TargetGenerateSettings> _graph = new Dictionary<string, TargetGenerateSettings>();
		public Action<string, TargetGenerateSettings> GenerateCallback = (s, i) => { };
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

			if (instructions.InputFiles.Any(inputFile => !File.Exists(inputFile)))
				throw new MissingDependencyException();

			if (NeedToGenerate(targetFile, instructions))
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

		private void Generate(string targetFile, TargetGenerateSettings settings)
		{
			GenerateCallback(targetFile, settings);
			settings.Action.Invoke(targetFile, settings);

			var record = new GenerationRecord(targetFile, settings);
			_buildHistory.AddRecord(record);
		}

		public void RegisterTarget(TargetGenerateSettings settings)
		{
			foreach(var targetFile in settings.OutputFiles)
				_graph.Add(targetFile,settings);
		}
	}
}