using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cake
{
	public delegate bool WillFileBeGeneratedAtPath(string path);

	public class DependencyGraph
	{
		public readonly Dictionary<string, TargetGenerateSettings> _graph = new Dictionary<string, TargetGenerateSettings>();
		public Action<TargetGenerateSettings> GenerateCallback = s => { };
		public readonly BuildHistory _buildHistory;

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

			var c = new SchedulableActionCollector(this);
			var actions = c.CollectActionsToGenerate(targetFile);
			
			foreach(var action in actions)
				actionScheduler.Add(action);

			actionScheduler.VerifyAllInputFilesArePresentOrWillBeGenerated();

			while (actionScheduler.AnyJobsLeft)
			{
				var job = actionScheduler.FindJobToRun();
				if (job==null)
					throw new InvalidOperationException("No job is available to run, while there are still scheduled jobs left.");
				Generate(job);
				actionScheduler.JobFinished(job);
			}
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

		public bool IsTargetRegistered(string file)
		{
			return _graph.Keys.Contains(file);
		}
	}
}