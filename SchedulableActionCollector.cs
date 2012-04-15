using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cake
{
	public class SchedulableActionCollector
	{
		private readonly DependencyGraph _dependencyGraph;

		public SchedulableActionCollector(DependencyGraph dependencyGraph)
		{
			_dependencyGraph = dependencyGraph;
		}

		public IEnumerable<SchedulableAction> CollectActionsToGenerate(string targetFile)
		{
			var result = ActionRequiredToGetTargetUpdated(targetFile);
			if (result != null)
				yield return result;
		}

		SchedulableAction ActionRequiredToGetTargetUpdated(string target)
		{
			TargetGenerateSettings generateSettings;
			if (!_dependencyGraph._graph.TryGetValue(target, out generateSettings))
			{
				return null;
				//if (File.Exists(target))
				//	return null;
				//throw new MissingDependencyException();
			}

			var inputFilesRequiringGeneration = generateSettings.InputFiles.Where(inputfile => ActionRequiredToGetTargetUpdated(inputfile) != null).ToArray();

			if (inputFilesRequiringGeneration.Any())
				return new SchedulableAction(generateSettings, inputFilesRequiringGeneration);

			if (NeedToGenerate(target))
				return new SchedulableAction(generateSettings);

			return null;
		}

		private bool NeedToGenerate(string targetFile)
		{
			TargetGenerateSettings generateSettings = _dependencyGraph._graph[targetFile];

			var recordOfLastBuild = _dependencyGraph._buildHistory.FindRecordFor(targetFile);

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

	}
}