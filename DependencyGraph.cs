using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cake
{
	public class DependencyGraph
	{
		readonly Dictionary<string, TargetGenerateSettings> _graph = new Dictionary<string, TargetGenerateSettings>();
		public Action<TargetGenerateSettings> GenerateCallback = s => { };
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
			var actionScheduler = new ActionScheduler();
			
			var action = ActionRequiredToGetTargetUpdated(targetFile);
			if (action == null)
				return;

			actionScheduler.Add(action);

			var job = actionScheduler.FindJobToRun();
			
			Generate(job);
		}

		SchedulableAction ActionRequiredToGetTargetUpdated(string target)
		{
			TargetGenerateSettings generateSettings;
			if (!_graph.TryGetValue(target, out generateSettings))
			{
				if (File.Exists(target))
					return null;
				throw new MissingDependencyException();
			}

			var inputFilesRequiringGeneration = generateSettings.InputFiles.Where(inputfile => ActionRequiredToGetTargetUpdated(inputfile) != null);

			if (inputFilesRequiringGeneration.Any())
				return new SchedulableAction(generateSettings, inputFilesRequiringGeneration);

			if (NeedToGenerate(target))
				return new SchedulableAction(generateSettings);

			return null;
		}

		private bool NeedToGenerate(string targetFile)
		{
			TargetGenerateSettings generateSettings = _graph[targetFile];

			var recordOfLastBuild = _buildHistory.FindRecordFor(targetFile);

			if (recordOfLastBuild == null)
				return true;

			if (!File.Exists(targetFile))
				return true;

			if (!generateSettings.Equals(recordOfLastBuild.Settings))
				return true;

			if (File.GetLastWriteTimeUtc(targetFile) < recordOfLastBuild.ModificationTimeOfTargetFile())
				return true;

			if (generateSettings.InputFiles.Any(inputFile => File.GetLastWriteTimeUtc(inputFile) > recordOfLastBuild.ModificationTimeOf(inputFile)))
				return true;

			return false;
		}

		private void Generate(TargetGenerateSettings settings)
		{
			GenerateCallback(settings);
			settings.Action.Invoke(settings);

			foreach (var outputFile in settings.OutputFiles)
			{
				var record = new GenerationRecord(outputFile, settings);
				_buildHistory.AddRecord(record);
			}
		}

		public void RegisterTarget(TargetGenerateSettings settings)
		{
			foreach(var targetFile in settings.OutputFiles)
				_graph.Add(targetFile,settings);
		}
	}
}